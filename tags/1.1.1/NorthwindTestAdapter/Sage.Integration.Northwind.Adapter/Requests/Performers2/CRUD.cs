using System;
using System.Collections.Generic;
using System.Text;
using Sage.Integration.Messaging.Model;
using Sage.Common.Syndication;
using Sage.Integration.Northwind.Adapter.Data;
using Sage.Integration.Northwind.Adapter.Common;
using System.Reflection;
using Sage.Integration.Northwind.Application.Base;
using Sage.Integration.Northwind.Adapter.Requests;

namespace Sage.Integration.Northwind.Adapter.Common.Handler
{
    class CRUD
    {
        private RequestContext _context;
        IFeedEntryEntityWrapper _wrapper;
        IRequest _request;
        private const int DEFAULT_COUNT = 10;

        public CRUD(IRequest request)
        {
            _context = new RequestContext(request.Uri);
            _wrapper = FeedEntryWrapperFactory.Create(_context.ResourceKind, _context);
            _request = request;
        }

        public void Create(FeedEntry entry)
        {
            

            if (IsValid(entry))
            {
                IFeedEntryEntityWrapper cWrapper = FeedEntryWrapperFactory.Create(_context.ResourceKind, _context);
                SdataTransactionResult trResult = cWrapper.Add(entry);
                if (trResult.HttpStatus == System.Net.HttpStatusCode.Created ||
                    trResult.HttpStatus == System.Net.HttpStatusCode.OK)
                {

                    if(string.IsNullOrEmpty(entry.Id))
                        entry.Id = trResult.Location;

                    _request.Response.FeedEntry = entry;
                    _request.Response.Protocol.SendKnownResponseHeader(System.Net.HttpResponseHeader.Location, entry.Id);
                }
                else
                {
                    _request.Response.StatusCode = trResult.HttpStatus;
                    _request.Response.Diagnoses = new Diagnoses();
                    _request.Response.Diagnoses.Add(GetDiagnosis(trResult));
                    //throw new InvalidDataException(trResult.HttpStatus + ": " + trResult.HttpMessage);
                }
            }
            else
            {
                throw new RequestException("Please use valid single resource url");
            }
        }

        public void Read(string id)
        {
            Feed<FeedEntry> result = new Feed<FeedEntry>();
            //result.Updated = GetRandomDate();
            result.Url = _request.Uri.ToString();
            result.Author.Name = "Northwind Adapter";


            string resource = TrimApostophes(id);

            if (resource != null)
            {
                _request.Response.FeedEntry = _wrapper.GetFeedEntry(resource);
                if (_request.Response.FeedEntry != null && !_request.Response.FeedEntry.IsDeleted && isActive(_request.Response.FeedEntry))
                    _request.Response.FeedEntry.Title = _request.Response.FeedEntry.ToString();

                else
                    throw new DiagnosesException(Severity.Error, resource + " not found", DiagnosisCode.DatasetNotFound);
                
            }
            else
            {
                result.Category.Scheme = "http://schemas.sage.com/sdata/categories";
                result.Category.Term = "collection";
                result.Category.Label = "Resource Collection";

                string[] ids = _wrapper.GetFeed();
                long start = Math.Max(0, _request.Uri.StartIndex - 1); //Startindex is 1-based
                long max = _request.Uri.Count == null ? ids.Length : Math.Min((long)_request.Uri.Count + start, (long)ids.Length);
                long entryCount = _request.Uri.Count == null ? DEFAULT_COUNT : (long)_request.Uri.Count;
                _request.Uri.Count = entryCount;
                for (long i = start; result.Entries.Count < entryCount && i < ids.Length; i++)
                {
                    string entryId = ids[i];
                    FeedEntry entry = _wrapper.GetFeedEntry(entryId);
                    if (entry != null)
                    {
                        entry.Title = entry.ToString();
                        entry.Links.AddRange(LinkFactory.CreateEntryLinks(_context, entry));
                        entry.Updated = DateTime.Now;
                        result.Entries.Add(entry);
                    }
                }

                result.Title = result.Entries.Count + " " + _context.ResourceKind.ToString();
                HandlePaging(_request, result, ids);
                //FeedLink link = new FeedLink(_request.Uri.AppendPath("$schema").ToString(), LinkType.Schema, MediaType.Xml, "Schema");
                FeedLink[] links = LinkFactory.CreateFeedLinks(_context, _request.Uri.ToString());
                result.Links.AddRange(links);
                result.Updated = DateTime.Now;
                _request.Response.Feed = result;
            }
        }

        private bool isActive(FeedEntry feedEntry)
        {
            PropertyInfo propInfo = feedEntry.GetType().GetProperty("active");
            if (propInfo == null)
                return true;
            bool prop = (bool)propInfo.GetValue(feedEntry, null);

            return prop;
        }

        public void Update(FeedEntry entry, string id)
        {
            string resource = TrimApostophes(id);
            if (!string.IsNullOrEmpty(resource) && string.IsNullOrEmpty(entry.Key))
                entry.Key = resource;

            if (IsValid(entry))
            {
                SdataTransactionResult trResult = _wrapper.Update(entry);
                if (trResult.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    Read(resource);
                }
                else
                {
                    _request.Response.StatusCode = trResult.HttpStatus;
                    _request.Response.Diagnoses = new Diagnoses();
                    _request.Response.Diagnoses.Add(GetDiagnosis(trResult));
                }
            }
            else
            {
                throw new RequestException("Please use valid single resource url");
            }
        }

        public void Delete(string id)
        {
            if (id == null)
                // The resource key must exists in url
                throw new RequestException("Please use single resource url");

            SdataTransactionResult sdTrResult = _wrapper.Delete(_context.ResourceKey);

            if (sdTrResult == null)
                throw new RequestException("Entity does not exists");

            if (sdTrResult.HttpStatus == System.Net.HttpStatusCode.OK)
            {
                _request.Response.StatusCode = System.Net.HttpStatusCode.OK;
            }
            else
            {
                throw new RequestException(sdTrResult.HttpMessage);
            }
        }

        #region Helper Methods
        /// <summary>
        /// Helper Method, because the Framework can't handle an ID-Template with apostophes
        /// </summary>
        /// <param name="resource"></param>
        /// <returns></returns>
        protected string TrimApostophes(string resource)
        {
            if (resource == null)
                return null;
            return resource.Trim(new char[] { ' ', '\'', '"' });
        }

        /// <summary>
        /// Helper Method, because the Framework handles emtpy entry elements (<entry/>) as...well...empty entries (and not null)
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        private bool IsValid(FeedEntry newEntry)
        {
            return newEntry.ContainsPayload;
        }

        private void HandlePaging(IRequest request, Feed<FeedEntry> result, string[] ids)
        {
            SDataUri baseUri = request.Uri;
            int max = ids.Length;
            long startIndex = baseUri.StartIndex == 0 ? 1 : baseUri.StartIndex;
            result.TotalResults = max;
            result.StartIndex = request.Uri.StartIndex == 0 ? 1 : request.Uri.StartIndex;
            result.ItemsPerPage = (int?)request.Uri.Count ?? max;

            FeedLink link;

            if (request.Uri.Count != null)
            {
                long count = (long)request.Uri.Count;
                if (count > 0)
                {
                    SDataUri uri = new SDataUri(baseUri);
                    uri.StartIndex = 1;
                    link = new FeedLink(uri.ToString(), LinkType.First, MediaType.Atom, "First Page");
                    result.Links.Add(link);

                    uri = new SDataUri(baseUri);
                    uri.StartIndex = max % count == 0 ? max - count + 1 : ((max / count) * count) + 1;
                    link = new FeedLink(uri.ToString(), LinkType.Last, MediaType.Atom, "Last Page");
                    result.Links.Add(link);


                    if (startIndex + count < max)
                    {
                        uri = new SDataUri(baseUri);
                        uri.StartIndex = startIndex + count;
                        link = new FeedLink(uri.ToString(), LinkType.Next, MediaType.Atom, "Next Page");
                        result.Links.Add(link);
                    }
                    if (startIndex > 1) //Startindex is 1-based
                    {
                        uri = new SDataUri(baseUri);
                        uri.StartIndex = Math.Max(1, startIndex - count);
                        link = new FeedLink(uri.ToString(), LinkType.Previous, MediaType.Atom, "Previous Page");
                        result.Links.Add(link);
                    }
                }
            }
        }

        private Diagnosis GetDiagnosis(SdataTransactionResult result)
        {
            Diagnosis d = result.Diagnosis;
            if(d == null)
            {
                d= new Diagnosis();
                d.Message = result.HttpMessage;
            }
            return d;
        }
        #endregion

    }
}

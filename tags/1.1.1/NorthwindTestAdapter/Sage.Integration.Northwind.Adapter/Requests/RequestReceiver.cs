#region Usings

using System;
//using Sage.Integration.Northwind.Adapter.Services;
using System.IO;
using Sage.Common.Syndication;
using Sage.Integration.Messaging.Model;
using Sage.Integration.Northwind.Adapter.Common.Performers;
using Sage.Integration.Northwind.Adapter.Requests;
using Sage.Integration.Northwind.Adapter.Common;

#endregion

namespace Sage.Integration.Northwind.Adapter.Common
{
    /// <summary>
    /// Entrypoints for adapter requests.
    /// </summary>
    public class RequestReceiver
    {
        public static NorthwindAdapter NorthwindAdapter;

        #region Ctor.

        /// <summary>
        /// Initialises a new instance of the <see cref="RequestReceiver"/> class.
        /// </summary>
        /// <param name="adapter"></param>
        public RequestReceiver(NorthwindAdapter adapter)
        {
            NorthwindAdapter = adapter;
        }

        #endregion

        #region Request Handlers

        /*[GetRequestTarget("*")]
        [SupportedAccept(MediaType.Atom, true)]
        [SupportedContentType(MediaType.Atom)]
        public void GetRequest(IRequest request)
        {
                string requestGuid = Guid.NewGuid().ToString();
                try
                {
                    LogRequest(request, requestGuid);
                RequestContext context;
                IRequestPerformer requestPerformer = null;
                RequestPerformerLocator requestPerformerLocator;


                requestPerformerLocator = NorthwindAdapter.RequestPerformerLocator;

                context = new RequestContext(request.Uri);
                if (!String.IsNullOrEmpty(context.ErrorMessage))
                    throw new ApplicationException(context.ErrorMessage);

                System.Runtime.Remoting.Messaging.CallContext.LogicalSetData("queryParam_Precedence", GetPrecedenceQuery(context));
                System.Runtime.Remoting.Messaging.CallContext.LogicalSetData("queryParam_Select", this.GetSelectQueryParts(context));

                switch (context.RequestType)
                {
                    case RequestType.Resource:
                        //requestPerformer = requestPerformerLocator.Resolve<GetResourceRequestPerformer>(context);
                        break;
                    case RequestType.SyncDigest:
                        requestPerformer = requestPerformerLocator.Resolve<GetSyncDigestRequestPerformer>(context);
                        break;
                    case RequestType.SyncSource:
                        //requestPerformer = requestPerformerLocator.Resolve<GetSyncSourceRequestPerformer>(context);
                        break;
                    case RequestType.SyncTarget:
                        //requestPerformer = requestPerformerLocator.Resolve<GetSyncTargetRequestPerformer>(context);
                        break;
                    case RequestType.ResourceCollectionSchema:
                        //requestPerformer = requestPerformerLocator.Resolve<GetSchemaRequestPerformer>(context);
                        break;
                    case RequestType.ResourceSchema:
                        requestPerformer = requestPerformerLocator.Resolve<GetResourceSchemaRequestPerformer>(context);
                        break;
                    case RequestType.ImportSchema:
                        //requestPerformer = requestPerformerLocator.Resolve<GetSchemaImportRequestPerformer>(context);
                        break;
                    case RequestType.Contract:
                        //requestPerformer = requestPerformerLocator.Resolve<GetContractRequestPerformer>(context);
                        break;
                    case RequestType.Dataset:
                        //requestPerformer = requestPerformerLocator.Resolve<GetDatasetRequestPerformer>(context);
                        break;
                    case RequestType.ResourceCollection:
                        //requestPerformer = requestPerformerLocator.Resolve<GetResourceCollectionRequestPerformer>(context);
                        break;
                    case RequestType.Linked:
                        //requestPerformer = requestPerformerLocator.Resolve<GetLinkingRequestPerformer>(context);
                        break;
                    case RequestType.ServiceSchema:
                        requestPerformer = requestPerformerLocator.Resolve<GetServiceSchemaRequestPerformer>(context);
                        break;
                    case RequestType.Template:
                        //requestPerformer = requestPerformerLocator.Resolve<GetTemplateRequestPerformer>(context);
                        break;
                }

                if (null != requestPerformer)
                    requestPerformer.DoWork(request);
                else
                    throw new RequestException("The request is not supported.");

                LogResponse(request, requestGuid);
            }
            catch(Exception e)
            {

                LogException(e, requestGuid);
                throw; 
            }
        }*/


       


        [DeleteRequestTarget("*")]
        public void DeleteRequest(IRequest request)
        {
            string requestGuid = Guid.NewGuid().ToString();
            try
            {
                LogRequest(request, requestGuid);
                RequestPerformerLocator requestPerformerLocator = NorthwindAdapter.RequestPerformerLocator; ;
                IRequestPerformer requestPerformer = null;
                RequestContext context = new RequestContext(request.Uri);
                switch (context.RequestType)
                {
                    case RequestType.Resource:
                        //requestPerformer = requestPerformerLocator.Resolve<DeleteResourceRequestPerformer>(context);
                        if (null != requestPerformer)
                            requestPerformer.DoWork(request);
                        else
                            throw new RequestException("The request is not supported.");
                        break;
                    case RequestType.SyncSource:
                    case RequestType.SyncTarget:
                        requestPerformerLocator = NorthwindAdapter.RequestPerformerLocator;
                        Exception error;
                        if (requestPerformerLocator.Delete(context, out error))
                            request.Response.StatusCode = System.Net.HttpStatusCode.OK;
                        else
                        {
                            if (error == null)
                                request.Response.StatusCode = System.Net.HttpStatusCode.NotFound;
                            else
                                throw error;
                        }
                        break;
                    case RequestType.Linked:
                        //requestPerformer = requestPerformerLocator.Resolve<DeleteLinkingRequestPerformer>(context);
                        if (null == requestPerformer)
                            throw new RequestException("The request is not supported.");

                        requestPerformer.DoWork(request);

                        break;
                    default:
                        throw new NotSupportedException("This Request was not supported");

                }
                LogResponse(request, requestGuid);
            }
            catch (Exception e)
            {

                LogException(e, requestGuid);
                throw;
            }

        }
        
        
        #endregion

        #region Private Helpers

        private int? GetPrecedenceQuery(RequestContext context)
        {
            return context.SdataUri.Precedence;
        }

        private string[] GetSelectQueryParts(RequestContext context)
        {
            string selectStatement;

            if (!context.SdataUri.QueryArgs.TryGetValue("select", out selectStatement))
                return new string[] { "*" };    // no select means select=*

            string[] parts;

            string[] selectArgs;
            selectArgs = selectStatement.Split(new char[]{','}, StringSplitOptions.RemoveEmptyEntries);

            parts = new string[selectArgs.Length];

            for (int i = 0; i < selectArgs.Length; i++)
            {
                parts[i] = selectArgs[i].Trim();   // remove white-space characters
            }

            return parts;
        }

        #endregion

        #region Logging

        private string logPath = @"c:\temp\Northwind";

        private void LogRequest(IRequest request, string requestUuid)
        {
            if (!Directory.Exists(logPath))
                return;
            try
            {
                string fileName = logPath + @"\" + requestUuid + "_request.txt";
                string content = "";
                content += request.Uri.ToString();
                content += Environment.NewLine;
                content += "Verb: " + request.Verb.ToString();
                content += Environment.NewLine;
                content += "ContentType: " + request.ContentType.ToString();
                content += Environment.NewLine;
                File.WriteAllText(fileName, content);

            }
            catch { }
        }

        private void LogResponse(IRequest request, string requestUuid)
        {
            if (!Directory.Exists(logPath))
                return;
            try
            {
                string fileName = logPath + @"\" + requestUuid + "_response.txt";
                string content = "";
                content += "HttpStatus: " + request.Response.StatusCode.ToString();
                content += Environment.NewLine;
                content += "ContentType: " + request.Response.ContentType.ToString() ;
                content += Environment.NewLine;
                if (request.Response.ContentType == MediaType.Xml)
                {
                    content += "XML (only for Contenttype XML): " + request.Response.Xml.ToString();
                    content += Environment.NewLine;
                }
                File.WriteAllText(fileName, content);
            }
            catch { }
        }

        private void LogException(Exception exception, string requestUuid)
        {
            if (!Directory.Exists(logPath))
                return;
            try
            {
                string fileName = logPath + @"\" + requestUuid + "_error.txt";

                string content = "";
                content += exception.ToString();
                File.WriteAllText(fileName, content);
            }
            catch { }
        }



        #endregion

    }
}

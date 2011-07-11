#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Sage.Sis.Common.Data.OleDb;
using System.Data.OleDb;
using Sage.Sis.Sdata.Sync.Storage.Jet.Syndication;
using System.Data;
using Sage.Sis.Sdata.Sync.Storage.Jet.TableAdapters;
using Sage.Sis.Sdata.Sync.Storage.Jet.Tables;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Xml;
using Sage.Sis.Sdata.Sync.Context;
using System.Reflection;

#endregion

namespace Sage.Sis.Sdata.Sync.Storage.Jet
{
    public class AppBookmarkInfoStoreProvider : IAppBookmarkInfoStoreProvider
    {
        #region Class Variables

        private readonly IJetConnectionProvider _jetConnectionProvider;
        private readonly IAppBookmarkSerializer _serializer;
        private readonly SdataContext _context;

        #endregion

        #region Ctor.

        public AppBookmarkInfoStoreProvider(IJetConnectionProvider jetConnectionProvider, IAppBookmarkSerializer serializer, SdataContext context)
        {
            _jetConnectionProvider = jetConnectionProvider;
            _serializer = serializer;
            _context = context;
            StoreEnvironment.Initialize(jetConnectionProvider, context);
        }

        #endregion

        #region IAppBookmarkInfoStoreProvider Members

        public bool Get(string resourceKind, Type type, out object applicationBookmark)
        {
            bool result = false;
            applicationBookmark = null;
            string assemblyQualifiedName = type.AssemblyQualifiedName;

            IAppBookmarkTableAdapter appBookmarkTableAdapter = StoreEnvironment.Resolve<IAppBookmarkTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                byte[] blob;
                string receivedAssemblyQualifiedName;
                if (appBookmarkTableAdapter.Get(resourceKindInfo.Id, out blob, out receivedAssemblyQualifiedName, jetTransaction))
                {
                    applicationBookmark = this.DeserializeBlob(blob, type);    // use given type not the one stored
                    result = true;
                }

                jetTransaction.Commit();
            }

            return result;
        }

        public void Add(string resourceKind, object applicationBookmark)
        {
            string assemblyQualifiedName = applicationBookmark.GetType().AssemblyQualifiedName;
            object blob = this.SerializeApplicationBookmark(applicationBookmark);

            IAppBookmarkTableAdapter appBookmarkTableAdapter = StoreEnvironment.Resolve<IAppBookmarkTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                try
                {
                    appBookmarkTableAdapter.Insert(resourceKindInfo.Id, blob, assemblyQualifiedName, jetTransaction);
                }
                catch (OleDbException exception)
                {
                    if (exception.Errors.Count == 1 && exception.Errors[0].SQLState == "3022")
                        throw new StoreException(string.Format("An error occured while adding a new application bookmark. An application bookmark already exists for the resource kind '{0}'.", resourceKind), exception);

                    throw;
                }

                jetTransaction.Commit();
            }
        }

        public void Update(string resourceKind, object applicationBookmark)
        {
            string assemblyQualifiedName = applicationBookmark.GetType().AssemblyQualifiedName;
            object blob = this.SerializeApplicationBookmark(applicationBookmark);

            IAppBookmarkTableAdapter appBookmarkTableAdapter = StoreEnvironment.Resolve<IAppBookmarkTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                if (!appBookmarkTableAdapter.Update(resourceKindInfo.Id, blob, assemblyQualifiedName, jetTransaction))
                    throw new StoreException(string.Format("No application bookmark exists for the resource kind '{0}' that can be updated.", resourceKind));

                jetTransaction.Commit();
            }
        }

        public void Delete(string resourceKind)
        {
            IAppBookmarkTableAdapter appBookmarkTableAdapter = StoreEnvironment.Resolve<IAppBookmarkTableAdapter>(_context);
            IResourceKindTableAdapter resourceKindTableAdapter = StoreEnvironment.Resolve<IResourceKindTableAdapter>(_context);

            using (IJetTransaction jetTransaction = _jetConnectionProvider.GetTransaction(false))
            {
                ResourceKindInfo resourceKindInfo = resourceKindTableAdapter.GetOrCreate(resourceKind, jetTransaction);

                try
                {
                    appBookmarkTableAdapter.Delete(resourceKindInfo.Id, jetTransaction);
                }
                catch
                {
                    return;
                }

                jetTransaction.Commit();
            }
        }

        #endregion

        #region Private Helpers

        private object DeserializeBlob(byte[] blob, Type type)
        {
            object resultObj;

            try
            {
                AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolveEventHandler;

                resultObj = _serializer.Deserialize(Encoding.Unicode.GetString(blob), type); 
                
            }
            finally
            {
                AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolveEventHandler;
            }
            return resultObj;
        }

        private object SerializeApplicationBookmark(object applicationBookmark)
        {
            object blob = null;
            
                try
                {
                    AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolveEventHandler;

                    blob = _serializer.Serialize(applicationBookmark);
                }
                finally
                {
                    AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomain_AssemblyResolveEventHandler;
                }
            
            return blob;
        }


        private Assembly CurrentDomain_AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly result = null;
            //string shortAssemblyName = args.Name.Split(',')[0];
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                //if (shortAssemblyName == assembly.FullName.Split(',')[0])
                if (args.Name == assembly.FullName)
                {
                    result = assembly;
                    break;
                }
            }

            return result;

        }


        #endregion
    }
}

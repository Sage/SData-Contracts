using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Sage.Common.Metadata;
using Sage.Common.Syndication;

namespace Sage.Integration.Northwind.Feeds
{
    public class SyncFeedSerializer : MediaTypeSerializer, IFeedSerializer
    {
      #region Constants
		
		private const string NullName	= "null";
		
		#endregion
		
		#region Fields
		
		private static MethodInfo _oSaveToStream;
		
		static SyncFeedSerializer()
		{
			foreach (MethodInfo method in typeof(FeedSerializer).GetMethods())
			{
				// We're looking for the specific SaveToStream generic 
				// method with the correct number of parameters
				if (method.Name != "SaveToStream")
					continue;
			
				if (!method.IsGenericMethod)		
					continue;

				if (method.GetParameters().Length != 4)
					continue;
					
				_oSaveToStream = method;
				break;
			}
		}
		
		#endregion
		

		public SyncFeedSerializer() :
			this(MediaType.Atom)
		{
		}
		

        public SyncFeedSerializer(MediaType mediaType) :
			base(mediaType)
		{
		}
		
		#region Methods
	

		public void SaveToStream(IFeed feed, Stream stream)
		{
			SaveToStream(feed, stream, null, null);
		}

	
		public void SaveToStream(IFeed feed, Stream stream, ISerializationSettings settings)
		{
			SaveToStream(feed, stream, settings);
		}


        public void SaveToStream(IFeed feed, Stream stream, ISerializationSettings settings, InclusionNode inclusionTree)
        {
            if (feed is SyncFeed)
            {
                XmlTextWriter xmlWriter = new XmlTextWriter(stream, Encoding.UTF8);

                xmlWriter.WriteStartDocument();

                // TODO: HANDLE AS ERROR(404) IF FEEDTYPE IS 'ENTRY' AND NO ENTRY EXISTS.
                ((SyncFeed)feed).WriteXml(xmlWriter);

                xmlWriter.WriteEndDocument();
                xmlWriter.Flush();
            }
        }
		

		public void LoadFromStream<T>(Feed<T> feed, Stream stream) where T : FeedEntry, new()
		{
            throw new NotImplementedException();
		}


		public void SaveToStream<T>(T feedEntry, Stream stream, ISerializationSettings settings) where T : FeedEntry, new()
		{
			SaveToStream<T>(feedEntry, stream, settings, null);
		}
		
	
		public void SaveToStream<T>(T feedEntry, Stream stream, ISerializationSettings settings, InclusionNode inclusionTree) where T : FeedEntry, new()
		{
            throw new NotImplementedException();
		}
		
		public void LoadFromStream<T>(T feedEntry, Stream stream) where T : FeedEntry
		{
            throw new NotImplementedException();
		}
		
		#endregion

	}
}

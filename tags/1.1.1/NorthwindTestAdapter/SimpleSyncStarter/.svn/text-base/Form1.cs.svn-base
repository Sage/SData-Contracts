using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.IO;

namespace SimpleSyncStarter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        #region Ui Handler

        private void btn_1to2_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEndpoint1.Text) || string.IsNullOrEmpty(txtEndpoint2.Text))
                {
                    MessageBox.Show("Please enter an endpoint", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                string endpoint1 = txtEndpoint1.Text.TrimEnd('/');
                string endpoint2 = txtEndpoint2.Text.TrimEnd('/');

                this.StartSync(endpoint1, endpoint2);
                
            }
            catch (Exception exception)
            {
                MessageBox.Show("An error occurred during synchronization. " + exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_2to1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtEndpoint1.Text) || string.IsNullOrEmpty(txtEndpoint2.Text))
                {
                    MessageBox.Show("Please enter an endpoint", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                string endpoint1 = txtEndpoint1.Text.TrimEnd('/');
                string endpoint2 = txtEndpoint2.Text.TrimEnd('/');

                this.StartSync(endpoint2, endpoint1);

                
            }
            catch (Exception exception)
            {
                MessageBox.Show("An error occurred during synchronization. " + exception.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private void StartSync(string endpointSource, string endpointTarget)
        {

            string strStartDate = DateTime.Now.ToString("yyyyMMdd_hhmmss");
            string runDirPath = Path.Combine(Environment.CurrentDirectory, @"SimpleSyncStarter\Run_" + strStartDate);
            if (!Directory.Exists(runDirPath))
                Directory.CreateDirectory(runDirPath);

            string trackingId = Guid.NewGuid().ToString();

            string responsePayload;
            string requestPayload;
            HttpStatusCode httpStatusCode;

            // GET SyncDigest
            httpStatusCode = this.GetSyncDigest(endpointTarget, trackingId, out responsePayload);
            this.SaveResponse(responsePayload, Path.Combine(runDirPath, "GET_SyncDigest.xml"));

            // POST: SyncSource
            requestPayload = responsePayload;
            httpStatusCode = this.PostSyncSource(endpointSource, trackingId, requestPayload, out responsePayload);

            // 3. GET SyncSource
            do
            {
                // wait a little time period 
                System.Threading.Thread.Sleep(this.GetPollingMillis(responsePayload));

                responsePayload = "";
                httpStatusCode = this.GetSyncSource(endpointSource, trackingId, out responsePayload);

            } while (httpStatusCode == HttpStatusCode.Accepted);

            string getSyncSourceResponse = responsePayload;
            string nextSyncSourcePagingUrl;
            bool nextSyncSourcePageExists = false;
            int syncSourcePageCounter = 1;
            // paging through GET syncsource
            do
            {
                this.SaveResponse(getSyncSourceResponse, Path.Combine(runDirPath, "GET_SyncSource_page" + syncSourcePageCounter + ".xml"));

                // 4. POST SyncTarget
                string trackingId2 = Guid.NewGuid().ToString(); // !!!!!!!
                requestPayload = getSyncSourceResponse;
                
                httpStatusCode = this.PostSyncTarget(endpointTarget, trackingId2, requestPayload, out responsePayload);

                // 5. GET SyncTarget
                do
                {
                    // wait a little time period
                    System.Threading.Thread.Sleep(this.GetPollingMillis(responsePayload));

                    responsePayload = "";
                    httpStatusCode = this.GetSyncTarget(endpointTarget, trackingId2, out responsePayload);

                } while (httpStatusCode == HttpStatusCode.Accepted);

                // paging through GET SyncTarget 
                string getSyncTargetResponse;
                int syncTargetPageCounter = 1;
                string nextSyncTargetPagingUrl;
                bool nextSyncTargetPageExists = false;
                do
                {
                    getSyncTargetResponse = responsePayload;

                    // 5.2 POST SyncResults
                    httpStatusCode = this.PostSyncResults(endpointSource, getSyncTargetResponse);

                    this.SaveResponse(getSyncTargetResponse, Path.Combine(runDirPath, "GET_SyncTarget" + syncSourcePageCounter + "_page" + syncTargetPageCounter + ".xml"));

                    // next SyncSource page
                    syncTargetPageCounter++;
                    nextSyncTargetPagingUrl = this.GetNextPagingUrl(getSyncTargetResponse);
                    nextSyncTargetPageExists = (nextSyncTargetPagingUrl != null);
                    if (nextSyncTargetPageExists)
                        httpStatusCode = this.GetPage(nextSyncTargetPagingUrl, out getSyncTargetResponse);
                }
                while (nextSyncTargetPageExists);

                // DELETE SyncTarget
                this.DeleteSyncTarget(endpointTarget, trackingId2);

                // next SyncSource page
                syncSourcePageCounter++;
                nextSyncSourcePagingUrl = this.GetNextPagingUrl(getSyncSourceResponse);
                nextSyncSourcePageExists = (nextSyncSourcePagingUrl != null);
                if (nextSyncSourcePageExists)
                    httpStatusCode = this.GetPage(nextSyncSourcePagingUrl, out getSyncSourceResponse);
            }
            while (nextSyncSourcePageExists);

            // DELETE SyncSource
            this.DeleteSyncSource(endpointSource, trackingId);
        }

        private HttpStatusCode DeleteSyncSource(string endpoint, string trackingId)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;
            string responsePayload;

            url = url = string.Format("{0}/$syncsource?trackingid={1}", endpoint, trackingId);
            httpMethod = HttpMethod.DELETE;
            contentType = "HTTP/1.1";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, false);
        }

        private HttpStatusCode DeleteSyncTarget(string endpoint, string trackingId)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;
            string responsePayload;

            url = string.Format("{0}/$synctarget?trackingid={1}", endpoint, trackingId);
            httpMethod = HttpMethod.DELETE;
            contentType = "HTTP/1.1";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, false);
        }


        private HttpStatusCode GetSyncSource(string endpoint, string trackingId, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;

            url = string.Format("{0}/$syncsource('{1}')", endpoint, trackingId);
            httpMethod = HttpMethod.GET;
            contentType = "";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }

        private HttpStatusCode GetSyncDigest(string endpoint, string trackingId, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;

            url = endpoint + "/$syncdigest";
            httpMethod = HttpMethod.GET;
            contentType = "";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }

        private HttpStatusCode PostSyncSource(string endpoint, string trackingId, string requestPayload, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";

            url = string.Format("{0}/$syncsource?trackingid={1}", endpoint, trackingId);
            httpMethod = HttpMethod.POST;
            contentType = "application/atom+xml;type=entry";
            responsePayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }

        private HttpStatusCode GetSyncTarget(string endpoint, string trackingId, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;

            url = string.Format("{0}/$synctarget('{1}')", endpoint,  trackingId);
            httpMethod = HttpMethod.GET;
            contentType = "";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }

        private HttpStatusCode PostSyncTarget(string endpoint, string trackingId, string requestPayload, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";

            url = string.Format("{0}/$synctarget?trackingid={1}", endpoint, trackingId);
            httpMethod = HttpMethod.POST;
            contentType = "application/atom+xml;type=feed";
            responsePayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }

        private HttpStatusCode PostSyncResults(string endpoint, string requestPayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string responsePayload = "";

            url = string.Format("{0}/$syncresults", endpoint);
            httpMethod = HttpMethod.POST;
            contentType = "application/atom+xml;type=feed";
            

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, false);
        }

        private HttpStatusCode GetPage(string pageUrl, out string responsePayload)
        {
            HttpMethod httpMethod;
            string url;
            string contentType;
            string userName = "";
            string pwd = "";
            string requestPayload;

            url = pageUrl;
            httpMethod = HttpMethod.GET;
            contentType = "";
            requestPayload = "";

            return doRequest(httpMethod, url, contentType, userName, pwd, requestPayload, out responsePayload, true);
        }


        private string GetNextPagingUrl(string responseFeed)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseFeed);

            XmlNamespaceManager nsmgr = NameSpaceHelper.CreateNamespaceManager(xmlDocument.NameTable);

            XmlElement xmlNode = (XmlElement)xmlDocument.DocumentElement.SelectSingleNode("atom:link[@rel='next']", nsmgr);
            if (null == xmlNode)
                return null;

            if (!xmlNode.HasAttribute("href"))
                return null;

            string href = xmlNode.Attributes["href"].Value;

            if (string.IsNullOrEmpty(href))
                return null;

            return href;
            

            //return xmlDocument.SelectSingleNode("/sdata:tracking/sdata:pollingMillis", nsmgr).InnerText;
        }

        private int GetPollingMillis(string responseFeed)
        {
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.LoadXml(responseFeed);

                XmlNamespaceManager nsmgr = NameSpaceHelper.CreateNamespaceManager(xmlDocument.NameTable);
                int pollingMillis = int.Parse(xmlDocument.SelectSingleNode("/sdata:tracking/sdata:pollingMillis", nsmgr).InnerText);

                if (pollingMillis <= 0)
                    return 1000;

                return pollingMillis;
            }
            catch
            {
                return 1000;
            }
        }
        
        private void SaveResponse(string xml, string fileName)
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                xmlDoc.Save(fileName);
            }
            catch(Exception e)
            {
                File.CreateText(fileName + ".txt").Write(xml);
                throw new Exception(String.Format("Could not parse and write xml to: {0}", fileName), e);
            }
        }






        #region doRequest

        private HttpStatusCode doRequest(HttpMethod httpMethod,
            string url,
            string contentType,
            string userName,
            string password,
            string requestPayload,
            out string responsePayload,
            bool responseStreamExpected)
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] buffer = encoding.GetBytes(requestPayload);
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(url);
            myRequest.Method = httpMethod.ToString();
            if (!String.IsNullOrEmpty(userName))
                myRequest.Credentials = new NetworkCredential(userName, password);


            if (httpMethod != HttpMethod.GET)
            {
                if (requestPayload.Length > 0)
                {
                    myRequest.ContentType = contentType;
                    myRequest.ContentLength = buffer.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    newStream.Write(buffer, 0, buffer.Length);
                    newStream.Close();
                }
            }

            HttpWebResponse myHttpWebResponse = null;
            responsePayload = "";

            try
            {
                myHttpWebResponse = (HttpWebResponse)myRequest.GetResponse();
                statusCode = myHttpWebResponse.StatusCode;
                if (responseStreamExpected)
                {
                    Stream streamResponse = myHttpWebResponse.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    Char[] readBuffer = new Char[256];
                    int count = streamRead.Read(readBuffer, 0, 256);
                    if (count == 0)
                        throw new Exception("Response stream is empty.");
                    while (count > 0)
                    {
                        responsePayload += new String(readBuffer, 0, count);
                        count = streamRead.Read(readBuffer, 0, 256);
                    }
                    streamRead.Close();
                    streamResponse.Close();
                }
                myHttpWebResponse.Close();
            }
            catch (WebException webEx)
            {
                responsePayload = "";
                if (webEx.Response != null)
                {
                    Stream streamResponse = webEx.Response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    Char[] readBuffer = new Char[256];
                    int count = streamRead.Read(readBuffer, 0, 256);
                    while (count > 0)
                    {
                        responsePayload += new String(readBuffer, 0, count);
                        count = streamRead.Read(readBuffer, 0, 256);
                    }
                    streamRead.Close();
                    streamResponse.Close();
                }
            }
            catch (Exception e)
            {
                statusCode = HttpStatusCode.InternalServerError;
                responsePayload = e.Message;
            }

            return statusCode;
        }

        
        #endregion

        #region ENUM: HttpMethod

        enum HttpMethod
        {
            PUT, POST, GET, DELETE
        }

        #endregion

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

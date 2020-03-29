using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace HttpProxyServer
{
    public class ProxyRecord
    {
        public bool IsSccuess = false;
        public bool IsComplated = false;
        public string Message = "";
        public Uri Uri;
        public String ClientIP;
        public HttpRequestContent Request;
        public HttpResponseContent ServerResponse;
        public HttpResponseContent CacheResponse;
        public HttpResponseContent ProxyResponse;
        public DateTime AccessTime; 
        public bool HasLocalCache
        {
            get {
                if (!CacheChecked)
                {
                    this.loadCache();
                }
                return CacheResponse != null && CacheResponse.LocalCachePath != null; 
            }
        }
        public bool IsDisableLocalCache
        {
            get
            {
                if (!CacheChecked)
                {
                    this.loadCache();
                }
                return CacheResponse != null && CacheResponse.IsDisableCache; 
            }
        }
        public object Control;
        public bool CacheChecked;
        public bool RemoteChecked;

        public string StatusText
        {
            get
            {
                if (IsComplated)
                {
                    if (IsSccuess)
                    {
                        return "成功";
                    }
                    else
                    {
                        return "失败";
                    }
                }
                else
                {
                    return "处理中";
                }
            }
        }

        public ProxyRecord(Uri uri)
        {
            this.Uri = uri;
        }

        public bool loadServerResponse()
        {
            if (this.RemoteChecked) return ServerResponse != null;
            this.RemoteChecked = true;
            HttpWebResponse httpResponse = null;
            try
            {
                httpResponse = getRemoteResponse(this.Uri,this.Request.Body);
            }
            catch (WebException ex)
            {
				//TODO需要处理Load失败
                httpResponse = (HttpWebResponse)ex.Response;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                httpResponse = null;
               
            }
            if (httpResponse == null)
            {
                return false;
            }
            this.ServerResponse = new HttpResponseContent(httpResponse);
            httpResponse.Close();
            return true;
        }
         
        private HttpWebResponse getRemoteResponse(Uri url, byte[] requestBody)
        {
            HttpWebRequest httpRequest = WebRequest.Create(url) as HttpWebRequest;
            this.Request.CopyHeaderTo(httpRequest);
            httpRequest.ContentLength = requestBody.Length;

            if (requestBody != null && requestBody.Length > 0)
            {
                Stream newStream = httpRequest.GetRequestStream();
                newStream.Write(requestBody, 0, requestBody.Length);
                newStream.Close();
            }

            return (HttpWebResponse)httpRequest.GetResponse();
        }

        public bool loadCache()
        {
            if (this.CacheChecked) return HasLocalCache;
            this.CacheChecked = true;
            CacheResponse = new HttpResponseContent(this.Uri);
            return this.HasLocalCache;
        }

        public void SaveCache()
        {
            CacheHelper.Save(this.Uri, this.ProxyResponse);
        }


        public void DeleteCache()
        {
            CacheHelper.Delete(this.Uri);
        }
    }
}

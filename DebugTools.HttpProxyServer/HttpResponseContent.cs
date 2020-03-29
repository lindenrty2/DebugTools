using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace HttpProxyServer
{
    public class HttpResponseContent : HttpContent
    {
        private const string HEADER_SERVER = "server";
        private const string HEADER_CONTENT_LENGTH = "Content-Length";
        private const string HEADER_CONTENT_TYPE = "Content-Type";
        private const string HEADER_CONTENT_ENCODING = "Content-Encoding";
        private const string HEADER_CACHE_CONTROL = "Cache-Control";

        public String Server
        {
            get { return _headers[HEADER_SERVER]; }
            set { _headers[HEADER_SERVER] = value; }
        }

        public long ContentLength
        {
            get {  
                long result = 0;
                long.TryParse( _headers[HEADER_CONTENT_LENGTH],out result);
                return result;
            }
            set { _headers[HEADER_CONTENT_LENGTH] = value.ToString(); }
        }

        public String ContentType
        {
            get { return _headers[HEADER_CONTENT_TYPE]; }
            set { _headers[HEADER_CONTENT_TYPE] = value; }
        }

        public String ContentEncoding
        {
            get { return _headers[HEADER_CONTENT_ENCODING]; }
            set { _headers[HEADER_CONTENT_ENCODING] = value; }
        }

        public String CacheControl
        {
            get { return _headers[HEADER_CACHE_CONTROL]; }
            set { _headers[HEADER_CACHE_CONTROL] = value; }
        }
        
        public override bool IsGzip
        {
            get{
                String encoding = this.ContentEncoding;
                return encoding == null ? false : encoding.IndexOf("gzip") >= 0;
            }
        }

        private int _stateCode = 200;
        public int StateCode
        {
            get { return _stateCode; }
            set { this._stateCode = value; }
        }

        internal HttpResponseContent()
        {
        }

        public HttpResponseContent(Uri uri)
        {
            this._isDisableCache = CacheHelper.IsDisableLocalCache(uri);
            if (!_isDisableCache)
            {
                this._localCachePath = CacheHelper.FindLocalCache(uri);
                if (this._localCachePath != null)
                {
                    this._body = CacheHelper.loadCache(this._localCachePath);
                    this._localHeadersPath = CacheHelper.FindLocalHeaders(uri);
                    this._headers = CacheHelper.loadHeaders(this._localHeadersPath);
                    this._localCookiesPath = CacheHelper.FindLocalCookies(uri);
                    this._cookies = CacheHelper.loadCookies(this._localCookiesPath);
                }
            }
        }
             
        public HttpResponseContent(HttpListenerResponse response)
        {
            this._headers = response.Headers;
            this._cookies = response.Cookies;
            bool isThunk = string.Equals( response.Headers["Transfer-Encoding"], "chunked");
            this._body = readAllBytes(response.OutputStream, this.ContentLength,isThunk);
            this.StateCode = response.StatusCode;
        }

        public HttpResponseContent(HttpWebResponse response)
        {
            this._headers = response.Headers;
            this._cookies = response.Cookies;
            bool isThunk = string.Equals( response.Headers["Transfer-Encoding"] , "chunked");
            this._body = readAllBytes(response.GetResponseStream(), this.ContentLength,isThunk);
            this.StateCode = (int)response.StatusCode;
            
        }


        internal void CopyHeaderTo(HttpListenerResponse response)
        {
            for (int i = 0; i < this.Headers.Count; i++)
            {
                String key = this.Headers.GetKey(i);
                if (key == "Content-Length")
                {
                }
                else if (key == "Transfer-Encoding")
                {
                }
                else
                {
                    try
                    {
                        response.Headers[key] = this.Headers[key];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }

            }
            //response.StatusCode = (int)httpResponse.StatusCode; ;
            //response.ContentType = httpResponse.ContentType;
            response.ContentLength64 = _body == null ? -1 : _body.Length;
            response.StatusCode = this.StateCode;
        }


        public HttpResponseContent Copy()
        {
            HttpResponseContent content = new HttpResponseContent();
            this.CopyTo(content);
            content.StateCode = this.StateCode;
            content.LocalCachePath = this.LocalCachePath;
            content.LocalCookiesPath = this.LocalCookiesPath;
            content.LocalHeadersPath = this.LocalHeadersPath;
            return content;
        } 
    }



    public class HttpNoResponseContent : HttpResponseContent
    {

        public HttpNoResponseContent(Uri uri)
        {
            this._isDisableCache = true;
            this.StateCode = 503;
            this._body = new byte[0];
            this._headers = new NameValueCollection();
            this._cookies = new CookieCollection();
        }
    }
}
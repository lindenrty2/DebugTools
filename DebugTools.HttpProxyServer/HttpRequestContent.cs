using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace HttpProxyServer
{
    public class HttpRequestContent : HttpContent
    {
        private const string HEADER_ACCEPT = "Accept";
        private const string HEADER_ACCEPT_ENCODING = "Accept-Encoding";
        private const string HEADER_ACCEPT_LANGUAGE = "Accept-Language";
        private const string HEADER_CONNECTION = "Connection";
        private const string HEADER_HOST = "Host";
        private const string HEADER_REFERER = "Referer";
        private const string HEADER_USERAGENT = "User-Agent"; 

        public String Accept
        {
            get { return _headers[HEADER_ACCEPT]; }
            set { _headers[HEADER_ACCEPT] = value; }
        }

        public String AcceptEncoding
        {
            get { return _headers[HEADER_ACCEPT_ENCODING]; }
            set { _headers[HEADER_ACCEPT_ENCODING] = value; }
        }

        public String AcceptLanguage
        {
            get { return _headers[HEADER_ACCEPT_LANGUAGE]; }
            set { _headers[HEADER_ACCEPT_LANGUAGE] = value; }
        }

        public String Connection
        {
            get { return _headers[HEADER_CONNECTION]; }
            set { _headers[HEADER_CONNECTION] = value; }
        }

        public String Host
        {
            get { return _headers[HEADER_HOST]; }
            set { _headers[HEADER_HOST] = value; }
        }

        public String Referer
        {
            get { return _headers[HEADER_REFERER]; }
            set { _headers[HEADER_REFERER] = value; }
        }

        public String UserAgent
        {
            get { return _headers[HEADER_USERAGENT]; }
            set { _headers[HEADER_USERAGENT] = value; }
        }

        public String HttpMethod
        {
            //TODO
            get { return _headers[HEADER_USERAGENT]; }
            set { _headers[HEADER_USERAGENT] = value; }
        }

        private String _method = "Get";
        public String Method
        {
            get { return _method; }
        }

        public HttpRequestContent(HttpListenerRequest request)
        {
            this._headers = request.Headers;
            this._cookies = request.Cookies;
            this._method = request.HttpMethod;
            bool isThunk = request.Headers["Transfer-Encoding"] == "chunked";
            this._body = readAllBytes(request.InputStream,0, isThunk);
        }

        public void CopyHeaderTo(HttpWebRequest httpRequest)
        {
            httpRequest.Method = this._method;
            for (int i = 0; i < this.Headers.Count; i++)
            {
                String key = this.Headers.GetKey(i);
                if (key == "User-Agent")
                {
                    httpRequest.UserAgent = this.UserAgent;
                }
                else if (key == "Accept")
                {
                    httpRequest.Accept = this.Headers[key];
                }
                else if (key == "Content-Type")
                {
                    httpRequest.ContentType = this.Headers[key];
                }
                else if (key == "Content-Length")
                {
                    httpRequest.ContentLength = long.Parse( this.Headers[key] );
                }
                else if (key == "Host")
                {
                    httpRequest.Host = this.Headers[key];
                }
                else if (key == "Referer")
                {
                    //httpRequest.Referer = request.UrlReferrer.ToString();
                }
                else if (key == "Connection")
                {
                    //httpRequest.Connection = request.Headers[key];
                }
                else if (key == "Proxy-Connection")
                {
                }
                else if (key == "If-Modified-Since")
                {
                }
                else
                {
                    try
                    {
                        httpRequest.Headers[key] = this.Headers[key];
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
            }
        }

        public HttpRequestContent Copy()
        {
            throw new NotImplementedException();
        }

    }
}

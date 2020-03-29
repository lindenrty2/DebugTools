using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Specialized;

namespace HttpProxyServer
{
    public abstract class HttpContent
    {
        protected NameValueCollection _headers = null;
        public NameValueCollection Headers
        {
            get { return _headers; }
        }

        protected CookieCollection _cookies = null;
        public CookieCollection Cookies
        {
            get { return _cookies; }
        }

        public virtual bool IsGzip
        {
            get
            {
                return false;
            }
        }

        protected byte[] _body = null; 
        public byte[] Body
        {
            get { return _body; }
        }

        public string StringBody {
            get {
                if (_body == null) return "";
                if (IsGzip)
                {
                    return UTF8Encoding.UTF8.GetString(GZipHelper.Decompress(_body));
                }
                else
                {
                    return UTF8Encoding.UTF8.GetString(_body);
                }
            }
            set {
                if (value == null)
                {
                    _body = null;
                }
                else
                {
                    _body = UTF8Encoding.UTF8.GetBytes(value);
                }
            }
        }

        protected Boolean _isDisableCache = false;
        public Boolean IsDisableCache
        {
            get { return _isDisableCache; }
            set { _isDisableCache = value; }
        }


        protected String _localCachePath = null;
        public String LocalCachePath
        {
            get { return _localCachePath; }
            set { _localCachePath = value; }
        }

        protected String _localHeadersPath = null;
        public String LocalHeadersPath
        {
            get { return _localHeadersPath; }
            set { _localHeadersPath = value; }
        }

        protected String _localCookiesPath = null;
        public String LocalCookiesPath
        {
            get { return _localCookiesPath; }
            set { _localCookiesPath = value; }
        }

        protected HttpContent(){
        }
         
        private static byte[] trunkEndFlag = { 0x0a, 0x0b, 0x30, 0x0a, 0x0b, 0x0a, 0x0b };
        public static byte[] readAllBytes(Stream stream,long contentLength,bool isTrunk)
        {
            MemoryStream resultStream = new MemoryStream();

            byte[] result = new byte[256];
            int count = stream.Read(result, 0, 256);
            resultStream.Write(result, 0, count);
            while (count > 0) //IsTrunk的时候是否妥当需要确认
            {
                try
                {
                    count = stream.Read(result, 0, 256);
                    resultStream.Write(result, 0, count);
                    if (isTrunk)
                    {

                    }
                    else if (contentLength == 0)
                    {
                        if (count < 256)
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (contentLength <= resultStream.Length)
                        {
                            break;
                        }
                    }
                }
                catch
                {

                }
                
            }
            return resultStream.ToArray();
        }

        public NameValueCollection CopyHeaders()
        {
            NameValueCollection newHeaders = new NameValueCollection();
            for (int i = 0; i < this.Headers.Count; i++)
            {
                newHeaders.Add(this.Headers.GetKey(i), this.Headers.Get(i));
            }
            return newHeaders;
        }

        public CookieCollection CopyCookies()
        {
            CookieCollection newCookies = new CookieCollection();
            for (int i = 0; i < this.Cookies.Count; i++)
            {
                Cookie oc = this.Cookies[i];
                Cookie c = new Cookie(oc.Name,oc.Value,oc.Path,oc.Domain);
                c.CommentUri = oc.CommentUri;
                c.Comment = oc.Comment;
                c.Discard = oc.Discard;
                c.Expired = oc.Expired;
                c.HttpOnly = oc.HttpOnly;
                c.Port = oc.Port;
                c.Secure = oc.Secure;
                c.Version = oc.Version;
                newCookies.Add(c);
            }
            return newCookies;
        }

        public void CopyTo(HttpContent content)
        { 
            content._headers = this.CopyHeaders();
            content._cookies = this.CopyCookies();
            if (this._body == null)
            {
                content._body = null;
            }
            else
            {
                Byte[] newByte = new byte[this._body.Length];
                Array.Copy(this._body, newByte, this._body.Length);
                content._body = newByte;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Collections.Specialized;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;

namespace HttpProxyServer
{
    public class CacheHelper
    {
        public static String TYPE_CACHE_CONTENT = ".cache";
        public static String TYPE_HEADERS_CONTENT = ".headers";
        public static String TYPE_COOKIES_CONTENT = ".cookies";
        public static String TYPE_DISABLE = ".disable";

        public static String PATH_CACHEBASE = System.Environment.CurrentDirectory + @"\Data\";

        public static string getLocalCacheName(Uri uri)
        {
            String name = uri.ToString().Replace("?", @"\").Replace("&", @"\").Replace("=", "_").Replace(".", "_").Replace("://", "__").Replace(":", "_");
            if (!name.EndsWith(@"\"))
            {
                name += @"\";
            }
            return name;
           
        }
        public static string getLocalCachePath(Uri uri)
        {
            return getVaildPath(PATH_CACHEBASE + getLocalCacheName(uri)) + TYPE_CACHE_CONTENT;
        }

        public static string getLocalHeadersPath(Uri uri)
        {
            return getVaildPath(PATH_CACHEBASE + getLocalCacheName(uri)) + TYPE_HEADERS_CONTENT;
        }

        public static string getLocalCookiesPath(Uri uri)
        {
            return getVaildPath(PATH_CACHEBASE + getLocalCacheName(uri)) + TYPE_COOKIES_CONTENT;
        }

        private static string getVaildPath(String path)
        {
            if (path.Length > 240)
            {
                return path.Substring(0, 240);
            }
            return path;
        }

        public static bool IsDisableLocalCache(Uri uri)
        {
            String path = getVaildPath(System.Environment.CurrentDirectory + @"\Data\" + getLocalCacheName(uri)) + TYPE_DISABLE;
            return FindFile(path, TYPE_DISABLE) != null;
        }

        public static String FindLocalCache(Uri uri)
        {
            String path = getLocalCachePath(uri);
            return FindFile(path,TYPE_CACHE_CONTENT);
        }

        public static String FindLocalHeaders(Uri uri)
        {
            String path = getLocalHeadersPath(uri);
            return FindFile(path, TYPE_HEADERS_CONTENT);
        }

        public static String FindLocalCookies(Uri uri)
        {
            String path = getLocalCookiesPath(uri);
            return FindFile(path, TYPE_COOKIES_CONTENT);
        }

        private static String FindFile(String path,String type)
        { 
            if (!File.Exists(path))
            {
                DirectoryInfo currentDir = new DirectoryInfo(Path.GetDirectoryName(path));
                while (currentDir != null)
                {
                    String fileName = currentDir.FullName + @"\" + type;
                    if (currentDir.FullName == System.Environment.CurrentDirectory)
                    {
                        return null;
                    }
                    else if (currentDir.Exists && File.Exists(fileName))
                    {
                        return fileName;
                    }
                    currentDir = currentDir.Parent;
                }
                return null;
            }
            else
            {
                return path;
            }
        }

        public static bool Delete(Uri uri)
        {
            try
            {
                String path = getLocalCachePath(uri);
                if (!File.Exists(path))
                {
                    return true;
                }
                File.Delete(path);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public static byte[] loadCache(Uri uri)
        {
            String path = FindLocalCache(uri);
            return File.ReadAllBytes(path);
        }

        public static byte[] loadCache(String path)
        { 
            if(path == null){
                return new byte[0];
            }
            return File.ReadAllBytes(path);
        }

        public static CookieCollection loadCookies(Uri uri)
        {
            String path = FindLocalCookies(uri);
            return loadCookies(path);
        }

        public static CookieCollection loadCookies(String path)
        {
            CookieCollection cookies = null; 
            if (path == null)
            {
                return new CookieCollection();
            }
            try
            { 
                SoapFormatter serializer = new SoapFormatter();
                FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read,FileShare.Read);
                cookies = (CookieCollection)serializer.Deserialize(fileStream);
                fileStream.Close();
                return cookies; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new CookieCollection();
            }  
        }

        public static NameValueCollection loadHeaders(Uri uri)
        {
            String path = FindLocalHeaders(uri);
            return loadHeaders(path);
        }
        
        public static NameValueCollection loadHeaders(String path)
        {
            NameValueCollection headers = null;
            if (path == null)
            {
                return new NameValueCollection();
            }
            try
            {
                SoapFormatter serializer = new SoapFormatter();
                FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                headers = (NameValueCollection)serializer.Deserialize(fileStream);
                fileStream.Close();
                return headers;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new NameValueCollection();
            } 
        }

        private static Object _saveLock = new Object();
        public static bool Save(Uri uri,HttpResponseContent response)
        {
            lock (_saveLock)
            {
                if (!SaveCacheBody(uri, response))
                {
                    return false;
                }
                if (!SaveCacheHeaders(uri, response))
                {
                    return false;
                }
                if (!SaveCacheCookies(uri, response))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool SaveCacheBody(Uri uri, HttpResponseContent response)
        {
            String path = response.LocalCachePath;
            if (path == null) { path = getLocalCachePath(uri); }
            return SaveFile(path, response.Body);
        }
        public static bool SaveCacheHeaders(Uri uri, HttpResponseContent response)
        {
            String path = response.LocalHeadersPath;
            if (path == null) { path = getLocalHeadersPath(uri); }

            try
            {
                SoapFormatter serializer = new SoapFormatter();
                FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                serializer.Serialize(fileStream, response.Headers);
                fileStream.Close();
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
        public static bool SaveCacheCookies(Uri uri, HttpResponseContent response)
        {
            String path = response.LocalCookiesPath;
            if (path == null) { path = getLocalCookiesPath(uri); }
            try
            {
                SoapFormatter serializer = new SoapFormatter();
                FileStream fileStream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
                serializer.Serialize(fileStream, response.Cookies);
                fileStream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private static bool SaveFile(String path,byte[] data)
        {
            FileStream stream = null;
            try
            {
                if (File.Exists(path))
                {
                    stream = File.OpenWrite(path);
                }
                else
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(path));
                    stream = File.Create(path);
                }

                stream.Write(data, 0, data.Length);
                stream.Flush(true);
                stream.Close();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace HttpProxyServer
{
    public delegate void ProxyEventHandler(ProxyRecord record);

    public class ProxyServer
    {
        public event ProxyEventHandler OnProxyStarted;
        public event ProxyEventHandler OnProxyFinished;

        private HttpListener _listener = null;
        private Thread _listenerThread = null;

        public bool IsRecordData = false;
        public bool IsDebug = false;
        public bool IsOnlyNoData = true;
        public ProxyModes ProxyMode = ProxyModes.Auto;

        public bool IsRuning()
        {
            return _listener != null && _listener.IsListening;
        }

        public void Start(int port)
        {
            String address = String.Format( "http://+:{0}/",port );
            //AddAddress(address, Environment.UserDomainName, Environment.UserName);
            _listener = new HttpListener();
            _listener.Prefixes.Add(address);
            _listener.Start();

            _listenerThread = new Thread(new ThreadStart(Server_Process));
            _listenerThread.Start();
        }

        private void Server_Process()
        {
            while (true)
            {
                // 注意: GetContext 方法将阻塞线程，直到请求到达
                HttpListenerContext context = _listener.GetContext();

                Thread thread = new Thread(new ParameterizedThreadStart(Connect_Process));
                thread.Start(context);
                
            }
        }

        private void Connect_Process(object state)
        {
            HttpListenerContext context = (HttpListenerContext)state;
            // 取得请求对象
            HttpListenerRequest request = context.Request;
            // 取得回应对象
            HttpListenerResponse response = context.Response;  
             
            ProxyRecord record = new ProxyRecord(getTargetUrl(request));
            record.AccessTime = DateTime.Now; 
            record.ClientIP = request.RemoteEndPoint.Address.ToString();
            record.Request = new HttpRequestContent(request); 
            if (OnProxyStarted != null)
            {
                OnProxyStarted(record);
            } 
              
            if (this.ProxyMode == ProxyModes.Proxy)
            {
                if (record.loadServerResponse())
                {
                    record.ProxyResponse = record.ServerResponse.Copy();
                }
                else
                {
                    //服务器无响应失败
                    record.ProxyResponse = new HttpNoResponseContent(record.Uri);
                }
            }
            else if (this.ProxyMode == ProxyModes.Local)
            {
                record.loadCache();
                record.ProxyResponse = record.CacheResponse.Copy();
            }
            else
            {
                if (record.loadCache() && !record.IsDisableLocalCache )
                {
                    record.ProxyResponse = record.CacheResponse.Copy();
                }
                else
                {
                    if (record.loadServerResponse())
                    {
                        record.ProxyResponse = record.ServerResponse.Copy();
                    }
                }
            }
            bool saved = false;
            if (IsDebug && !(this.IsOnlyNoData && record.HasLocalCache) && !record.IsDisableLocalCache)
            {
                DebugForm debugForm = new DebugForm();
                debugForm.setData(record);
                debugForm.ShowDialog(); 
                if (debugForm.DialogResult == DialogResult.Yes)
                {
                    record.SaveCache();
                    saved = true;
                }
            }
            if (!saved && IsRecordData && !(this.IsOnlyNoData && record.HasLocalCache) && !record.IsDisableLocalCache)
            {
                record.SaveCache();
            }
            if(record.ProxyResponse == null)
            {
                record.IsComplated = true;
                if (OnProxyFinished != null)
                {
                    OnProxyFinished(record);
                }
                return;
            }
            record.ProxyResponse.CopyHeaderTo(response);

            if (response.ContentLength64 > 0)
            {
                try
                {
                    // 输出回应内容
                    System.IO.Stream output = response.OutputStream;
                    System.IO.BinaryWriter writer = new System.IO.BinaryWriter(output);
                    writer.Write(record.ProxyResponse.Body, 0, record.ProxyResponse.Body.Length);
                    writer.Close();
                    record.IsSccuess = true;
                }
                catch (Exception ex)
                {
                    record.IsSccuess = false;
                    Console.WriteLine(ex.ToString());
                }
            }
            record.IsComplated = true;
            if (OnProxyFinished != null)
            {
                OnProxyFinished(record);
            }
            // 必须关闭输出流
        }
         
        public void Stop()
        {
            if (_listenerThread != null)
            {
                _listenerThread.Abort();
            }
            if (_listener != null)
            {
                _listener.Close();
                _listener = null;
            }
        }

        public static Uri getTargetUrl(HttpListenerRequest request)
        {
            String scheme = request.Url.Scheme;
            String host = request.Url.Host;
            if (!request.RawUrl.StartsWith(scheme) && !request.RawUrl.StartsWith(host))
            {
                String newURL = String.Format("{0}://{1}/{2}",scheme, request.Headers["Host"],request.RawUrl);
                return new Uri(newURL);
            }
            return new Uri(request.RawUrl);
        }

         

        public static void AddAddress(string address, string domain, string user)
        {
            //string argsDll = String.Format(@"http delete urlacl url={0}", address);
            //string args = string.Format(@"http add urlacl url={0} user={1}\{2}", address, domain, user);
            //ProcessStartInfo psi = new ProcessStartInfo("netsh", argsDll);
            //psi.Verb = "runas";
            //psi.CreateNoWindow = true;
            //psi.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.UseShellExecute = false;
            //Process.Start(psi).WaitForExit();//删除urlacl
            //psi = new ProcessStartInfo("netsh", args);
            //psi.Verb = "runas";
            //Process.Start(psi).WaitForExit();//添加urlacl
            //psi.CreateNoWindow = true;
            //psi.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.UseShellExecute = false;
        }


       

    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Collections;

namespace DebugTools.Common.Net.Server
{
    public class HttpProcessor
    {

        private System.Net.Sockets.Socket s;
        private BufferedStream bs;
        private StreamReader sr;
        private StreamWriter sw;
        private String method;
        private String url;
        private String protocol;
        private Hashtable hashTable;

        public HttpProcessor(System.Net.Sockets.Socket s)
        {
            this.s = s;
            hashTable = new Hashtable();
        }

        public void process()
        {
            NetworkStream ns = new NetworkStream(s, FileAccess.ReadWrite);
            bs = new BufferedStream(ns);
            sr = new StreamReader(bs);
            sw = new StreamWriter(bs);
            parseRequest();
            readHeaders();
            writeURL();
            s.Shutdown(SocketShutdown.Both);
            ns.Close();
        }

        public void parseRequest()
        {
            String request = sr.ReadLine();
            string[] tokens = request.Split(new char[] { ' ' });
            method = tokens[0];
            url = tokens[1];
            protocol = tokens[2];
        }

        public void readHeaders()
        {
            String line;
            while ((line = sr.ReadLine()) != null && line != "")
            {
                string[] tokens = line.Split(new char[] { ':' });
                String name = tokens[0];
                String value = "";
                for (int i = 1; i < tokens.Length; i++)
                {
                    value += tokens[i];
                    if (i < tokens.Length - 1) tokens[i] += ":";
                }
                hashTable[name] = value;
            }
        }

        public void writeURL()
        {
            try
            {
                FileStream fs = new FileStream(url.Substring(1), FileMode.Open, FileAccess.Read);
                writeSuccess();
                BufferedStream bs2 = new BufferedStream(fs);
                byte[] bytes = new byte[4096];
                //int read;
                //while ((read = bs2.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //    bs.Write(bytes, 0, read);
                //}

                byte[] sent = UTF8Encoding.UTF8.GetBytes("<Html>Test</Html>");
                bs2.Write(sent, 0, sent.Length);
                bs2.Close();
            }
            catch (FileNotFoundException)
            {
                writeFailure();
                sw.WriteLine("File not found: " + url);
            }
            sw.Flush();
        }

        public void writeSuccess()
        {
            sw.WriteLine("HTTP/1.0 200 OK");
            sw.WriteLine("Connection: close");
            sw.WriteLine();
        }

        public void writeFailure()
        {
            sw.WriteLine("HTTP/1.0 404 File not found");
            sw.WriteLine("Connection: close");
            sw.WriteLine();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace MathieuJaumain.Tools.Networking
{

    public delegate void HttpResponseDelegate(byte[] response, HttpEndPoint sender);

    public class HttpServer
    {
        private HttpListener _HttpListener = new HttpListener();
        private bool _isListening = false;
        private object _Lock = new object();
        private List<HttpEndPoint> _Clients = new List<HttpEndPoint>();

        public event HttpResponseDelegate OnNewResponseIncoming;

        private void Listening()
        {
            IsListening = true;
            try
            {
                while (IsListening && _HttpListener.IsListening)
                {
                    ThreadPool.QueueUserWorkItem((c) =>
                    {
                        var ctx = c as HttpListenerContext;
                        try
                        {
                            IPEndPoint endPoint = ctx.Request.RemoteEndPoint;
                            if (Clients.Where(x => x.Endpoint.Address == endPoint.Address).ToArray().Length == 0)
                                Clients.Add(new HttpEndPoint(endPoint));

                            string requestString = new StreamReader(ctx.Request.InputStream).ReadToEnd();
                            OnNewResponseIncoming(Encoding.UTF8.GetBytes(requestString), Clients.First(x => x.Endpoint.Address == endPoint.Address));
                        }
                        catch { }
                        finally
                        {
                            ctx.Response.OutputStream.Close();
                        }
                    }, _HttpListener.GetContext());
                }
            }
            catch { } // suppress any exception
        }

        public bool CanSendRequest
        {
            get { return true; }
        }

        public void StartListening()
        {
            new Thread(Listening).Start();
        }

        public void StopListening()
        {
            IsListening = false;

        }


        public void SendToClient(byte[] request, int  index, string url, string contentType, string method)
        {
            HttpEndPoint client = null;
            try
            {
                client = Clients[index];
            }
            catch { throw new Exception("Couldn't find a client at index = " + index); }

            var httpReq = (HttpWebRequest)WebRequest.Create("http://" + client.Endpoint.Address.ToString() + ":9080//" + url);
            httpReq.Method = method;
            httpReq.ContentType = contentType;
            httpReq.ContentLength = request.Length;
            using (var requestStream = httpReq.GetRequestStream())
            {
                requestStream.Write(request, 0, request.Length);
            }
        }


        public void PostToClient(byte[] request, int index, string url, string contentType)
        {
            SendToClient(request, index, url, contentType, "POST");
        }

        public void GetToClient(byte[] request, int index, string url, string contentType)
        {
            SendToClient(request, index, url, contentType, "GET");
        }

        public void PostToClients(byte[] request, string url, string contentType)
        {
            int count = Clients.Count;
            if( count > 0)
                for (int i = 0; i < count; i++)
                    PostToClient(request, i, url, contentType);
        }

        public void GetToClients(byte[] request, string url, string contentType)
        {
            int count = Clients.Count;
            if (count > 0)
                for (int i = 0; i < count; i++)
                    GetToClient(request, i, url, contentType);
                
        }

        public bool IsListening
        {
            get { lock (_Lock) return _isListening; }
            set { lock (_Lock) _isListening = value; }
        }

        public HttpListener HttpListener
        {
            get { lock (_Lock) return _HttpListener; }
            set { lock (_Lock) _HttpListener = value; }
        }



        public List<HttpEndPoint> Clients
        {
            get { lock (_Lock) return _Clients; }
            set { lock (_Lock) _Clients = value; }
        }
    }

    public class HttpEndPoint 
    {
        private IPEndPoint _Endpoint;

        public HttpEndPoint(IPEndPoint endpoint)
        {
            _Endpoint = endpoint;
        }

        public IPEndPoint Endpoint
        {
            get { return _Endpoint; }
            set { _Endpoint = value; }
        }
    }
}
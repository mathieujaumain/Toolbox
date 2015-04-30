using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net.Sockets;

namespace MathieuJaumain.Tools.Networking
{
    public delegate void TcpResponseDelegate(byte[] response, ConnectedClient sender);

    public class TCPServer
    {

        private TcpListener _Listener;
        private static object _Lock = new object();
        private bool _isListening = false;
        private List<ConnectedClient> _Clients = new List<ConnectedClient>();
        public event TcpResponseDelegate OnNewResponseIncoming;

        public TCPServer(int port)
        {
            _Listener = new TcpListener(System.Net.IPAddress.Any, port);
        }

        public void StartListening()
        {
            Listener.Start(5);
            new Thread(Listening).Start();
        }

        private void Listening()
        {
            IsListening = true;
            while (IsListening && Listener != null)
            {
                bool pending = false;
                try
                {
                    pending = Listener.Pending();
                }
                catch
                {
                    pending = false;
                }


                if (pending)
                {
                    TcpClient client = null;
                    try
                    {
                        client = Listener.AcceptTcpClient();
                    }
                    catch
                    {
                        break;
                    }

                    if (client != null)
                    {
                        ConnectedClient newClient = new ConnectedClient(client);
                        Clients.Add(newClient);
                        newClient.StartListening();
                        newClient.OnNewResponseFromClient += newClient_OnNewResponseFromClient;
                    }
                }

                // detecter les déco :
                List<ConnectedClient> clients2BeRemoved = new List<ConnectedClient>(Clients.Where(x => !x.TcpClient.Connected).ToList());
                foreach (ConnectedClient client in clients2BeRemoved)
                {
                    Clients.Remove(client);
                }
            }
        }

        public void SendToClients(byte[] request, object argument)
        {
            SendToClients(request);
        }

        public void SendToClient(byte[] request, int index)
        {
            Clients[index].Send(request);
        }


        private void newClient_OnNewResponseFromClient(byte[] response, object sender)
        {
            try
            {
                OnNewResponseIncoming(response, sender as ConnectedClient);
            }
            catch (Exception e)
            {
                string mess = Encoding.UTF8.GetString(response);
            }
        }

        public void StopListening()
        {
            IsListening = false;
            Listener.Stop();
            foreach (ConnectedClient client in Clients)
                client.StopListening();

            Clients.Clear();
        }

        public void SendToClients(byte[] request)
        {
            foreach (ConnectedClient client in Clients)
                client.Send(request); // ???
        }


        public List<ConnectedClient> Clients
        {
            get { lock (_Lock) return _Clients; }
            set { lock (_Lock) _Clients = value; }
        }

        public bool IsListening
        {
            get { lock (_Lock) return _isListening; }
            set { lock (_Lock) _isListening = value; }
        }

        public TcpListener Listener
        {
            get { lock (_Lock) return _Listener; }
            set { lock (_Lock) _Listener = value; }
        }
    }


    /// <summary>
    /// Classe client !
    /// </summary>
    public class ConnectedClient
    {
        private TcpClient _Client;
        private bool _IsListening = false;
        private object _Lock;

        public event TcpResponseDelegate OnNewResponseFromClient;

        public ConnectedClient(TcpClient client)
        {
            _Client = client;
            client.SendBufferSize = 12000;
            client.SendTimeout = 500;
            _Lock = new object();
        }


        public void StartListening()
        {
            new Thread(Listening).Start();
        }

        private void Listening()
        {
            _IsListening = true;
            byte[] buffer = new byte[12000]; // Yeah....
            int read = 0;
            NetworkStream stream;

            while (IsListening)
            {
                if (!TcpClient.Connected)
                {
                    IsListening = false;
                    continue;
                }
                try
                {
                    stream = _Client.GetStream();
                    read = stream.Read(buffer, 0, buffer.Length);


                    if (read > 0)
                    {
                        if (this != null)
                            OnNewResponseFromClient(buffer.Take(read).ToArray(), this);
                    }
                }
                catch
                {
                }
            }

            StopListening();
        }

        public void StopListening()
        {
            _IsListening = false;
            _Client.Close();
        }

        public void Send(byte[] request)
        {
            if (_Client.Connected)
            {
                try
                {
                    _Client.GetStream().Write(request, 0, request.Length);
                }
                catch
                {
                }
            }
        }

        public TcpClient TcpClient
        {
            get { lock (_Lock) return _Client; }
            set { lock (_Lock) _Client = value; }
        }

        public bool IsListening
        {
            get { lock (_Lock) return _IsListening; }
            set { lock (_Lock) _IsListening = value; }
        }
    }

}

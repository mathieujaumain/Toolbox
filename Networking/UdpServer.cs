using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

namespace MathieuJaumain.Tools.Networking
{
    public delegate void UdpResponseDelegate(byte[] response, UDPEndPoint sender);

    public class UDPServer
    {
        private UdpClient _UdpListener; // reception = envoi ?
        private bool _isListening = false;
        private object _Lock = new object();
        private List<UDPEndPoint> _Clients = new List<UDPEndPoint>();

        private int _Port;

        public UDPServer(int port)
        {
            _UdpListener = new UdpClient();
            _UdpListener.DontFragment = true;
            _Port = port;

        }

        private void Listening()
        {
            IsListening = true;
            while (IsListening && _UdpListener != null)
            {
                if (UdpListener.Available > 0)
                {
                    IPEndPoint endpoint = new IPEndPoint(IPAddress.Any, _Port);
                    byte[] received = UdpListener.Receive(ref endpoint);

                    //On regarde si un client avec un tel end point existe déjà et si non, on l'ajoute à la liste des clients
                    if (Clients.Where(x => x.Endpoint.Address == endpoint.Address).ToArray().Length == 0)
                        Clients.Add(new UDPEndPoint(endpoint));

                    OnNewResponseIncoming(received, Clients.First(x => x.Endpoint.Address == endpoint.Address));
                }
            }
        }

        public event UdpResponseDelegate OnNewResponseIncoming;

        public bool CanSendRequest
        {
            get { return true; }
        }

        public void StartListening()
        {
            _UdpListener.Connect(new IPEndPoint(IPAddress.Any, _Port));
            new Thread(Listening).Start();
        }

        public void StopListening()
        {
            IsListening = false;
            UdpListener.Close();
            Clients.Clear();
        }

        public void Broadcast(byte[] request)
        {
            UdpListener.Send(request, request.Length, new IPEndPoint(IPAddress.Any, _Port));
        }

        public void SendToClient(byte[] request, int index)
        {
            UdpListener.Send(request, request.Length, Clients[index].Endpoint);
        }

        public void SendToAllClients(byte[] request)
        {
            foreach( UDPEndPoint client in Clients )
                UdpListener.Send(request, request.Length, client.Endpoint);
        }
        

        public bool IsListening
        {
            get { lock (_Lock) return _isListening; }
            set { lock (_Lock) _isListening = value; }
        }

        public UdpClient UdpListener
        {
            get { lock (_Lock) return _UdpListener; }
            set { lock (_Lock) _UdpListener = value; }
        }

        public List<UDPEndPoint> Clients
        {
            get { lock (_Lock) return _Clients; }
            set { lock (_Lock) _Clients = value; }
        }
    }

    public class UDPEndPoint
    {
        private IPEndPoint _Endpoint;

        public UDPEndPoint(IPEndPoint endpoint)
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

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace IGMPSnooping.IGMP
{
    public class IGMPController
    {
        String IGMP_address;
        int IGMP_port;

        public IGMPController(String ip, int port)
        {
            this.IGMP_address = ip;
            this.IGMP_port = port;
        }

        private Thread _IGMPConsumer;
        private Socket _multicastSocket = null;

        public void Start()
        {
            if (_IGMPConsumer == null)
            {
                _IGMPConsumer = new Thread(new ThreadStart(IGMPListener))
                {
                    IsBackground = true
                };
                _IGMPConsumer.Start();
            }
        }

        public void Stop()
        {
            if (_IGMPConsumer != null)
            {
                try
                {
                    _IGMPConsumer.Abort();
                    _IGMPConsumer = null;
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                _multicastSocket.Close();
                _multicastSocket = null;
            }
        }

        public void JoinMulticastGroup()
        {
            _multicastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram,
                    ProtocolType.Udp);
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, this.IGMP_port);
            _multicastSocket.Bind(ipep);
            IPAddress ip = IPAddress.Parse(this.IGMP_address);
            _multicastSocket.SetSocketOption(SocketOptionLevel.IP,
            SocketOptionName.AddMembership,
            new MulticastOption(ip, IPAddress.Any));
        }

        public void IGMPListener()
        {
            try
            {
                string receivedMsg;
                if (_multicastSocket == null)
                {
                    JoinMulticastGroup();
                }
                byte[] messageReceivedIGMP = new byte[1024];
                int nmbBytesReceived = -1;
                while (true)
                {
                    Array.Clear(messageReceivedIGMP, 0, 1024);
                    nmbBytesReceived = _multicastSocket.Receive(messageReceivedIGMP);
                    if (nmbBytesReceived > 0)
                    {
                        receivedMsg = System.Text.Encoding.UTF8.GetString(messageReceivedIGMP);
                        if (!String.IsNullOrEmpty(receivedMsg))
                        {
                            Console.WriteLine("Received ( " + nmbBytesReceived.ToString() + " bytes): " + receivedMsg);
                        }
                        else
                        {
                            Console.WriteLine("Received NULL message");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error on receive...");
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine("Error on socket...");
                _multicastSocket.Close();
                _multicastSocket = null;
            }
            catch (ThreadAbortException)
            {
                Console.WriteLine("Error on Thread...");
                Thread.ResetAbort();
            }
            catch (Exception exp)
            {
                Console.WriteLine("General Error");
            }
        }
    }
}

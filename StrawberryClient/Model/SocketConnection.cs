using StrawberryClient.Model.Enumerate;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StrawberryClient.Model
{

    class SocketConnection
    {
        public delegate void Receive(int cmd, string data);
        public event Receive ChatRecv;
        public event Receive HomeRecv;
        public event Receive LoginRecv;
        public event Receive JoinRecv;
        public event Receive AuthRecv;

        public delegate void ImageReceive(string id, byte[] image);
        public event ImageReceive imageRecv;

        public static SocketConnection Instance;
        private Socket socket;
        private bool isRun = false;

        public SocketConnection()
        {

        }

        public static SocketConnection GetInstance()
        {
            if(Instance == null)
            {
                Instance = new SocketConnection();
            }

            return Instance;

        }

        public Socket GetSocket()
        {
            if (socket == null)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SendBufferSize = 0;
            }

            return socket;
        }

        public bool Connect()
        {
            if(!GetSocket().Connected)
            {
                try
                {
                    GetSocket().Connect(new IPEndPoint(IPAddress.Parse("172.30.1.26"), 3000));
                }
                

                catch(SocketException)
                {
                    return false;
                }
                
            }

            return true;
        }

        public void DisConnect()
        {
            if(GetSocket().Connected)
            {
                byte[] send = BitConverter.GetBytes((int)PacketType.Close);
                byte[] size = BitConverter.GetBytes(send.Length);

                GetSocket().Send(size, 0, 4, SocketFlags.None);
                GetSocket().Send(send, 0, send.Length, SocketFlags.None);

                GetSocket().Close();
            }

        }


        public void StartRecv()
        {
            if(!isRun)
            {
                Thread thread = new Thread(Run);
                thread.Start();

                isRun = true;
            }
        }


        private void Run()
        {
            byte[] recvSize;
            byte[] recv;

            while (GetSocket().Connected)
            {
                try
                {
                    recvSize = new byte[4];
                    GetSocket().Receive(recvSize, 0, 4, SocketFlags.None);

                    int dataSize = BitConverter.ToInt32(recvSize, 0);

                    recv = new byte[dataSize];

                    GetSocket().Receive(recv, 0, dataSize, SocketFlags.None);
                    
                    int dataType = BitConverter.ToInt32(recv, 0);
                    int viewModel = BitConverter.ToInt32(recv, 4);


                    if(dataType == (int)PacketType.Text)
                    {
                        int cmd = BitConverter.ToInt32(recv, 8);
                        string data = Encoding.UTF8.GetString(recv, 12, recv.Length - 12);

                        switch (viewModel)
                        {
                            case (int)Destination.Login:
                                LoginRecv(cmd, data);
                                break;
                            case (int)Destination.Join:
                                JoinRecv(cmd, data);
                                break;
                            case (int)Destination.Auth:
                                AuthRecv(cmd, data);
                                break;
                            case (int)Destination.Home:
                                HomeRecv(cmd, data);
                                break;
                            case (int)Destination.ChatRoom:
                                ChatRecv(cmd, data);
                                break;
                            case (int)Destination.Both:
                                HomeRecv(cmd, data);
                                ChatRecv?.Invoke(cmd, data);
                                break;
                            default:
                                break;
                        }
                       
                    }
                    
                    else
                    {
                        string userName = Encoding.UTF8.GetString(recv, 4, 10);
                        imageRecv(userName, recv);
                    }
                    
                }

                catch(TimeoutException)
                {
                    Console.WriteLine("timeout");
                    break;
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }


            }
        }

       
        public void Send(string request, params string[] queryString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(request);
            sb.Append("/");

            foreach (string i in queryString)
            {
                sb.Append(i);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            byte[] packetType = BitConverter.GetBytes((int)PacketType.Text);
            byte[] text = Encoding.UTF8.GetBytes(sb.ToString());
            byte[] send = new byte[packetType.Length + text.Length];

            packetType.CopyTo(send, 0);
            text.CopyTo(send, 4);

            byte[] size;

            size = BitConverter.GetBytes(send.Length);
            GetSocket().Send(size, 0, 4, SocketFlags.None);

            GetSocket().Send(send, 0, send.Length, SocketFlags.None);

        }

        public void Send(byte[] image)
        {

            byte[] packetType = BitConverter.GetBytes((int)PacketType.Image);
            byte[] text = Encoding.UTF8.GetBytes("MyImage");
            byte[] send = new byte[packetType.Length + text.Length + image.Length];

            packetType.CopyTo(send, 0);
            text.CopyTo(send, 4);
            image.CopyTo(send, 11);

            byte[] size;

            size = BitConverter.GetBytes(send.Length);

            GetSocket().Send(size, 0, 4, SocketFlags.None);

            GetSocket().Send(send, 0, send.Length, SocketFlags.None);
        }


    }
}

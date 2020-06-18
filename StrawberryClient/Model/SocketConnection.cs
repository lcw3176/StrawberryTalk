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
        public delegate void Receive(string param);
        public delegate void ImageReceive(byte[] image);
        public event Receive Recv;
        public event ImageReceive imageRecv;

        enum PacketType { Text, Image };
        public static SocketConnection Instance;
        private Socket socket;
        public string data;
        byte[] recv = new byte[2048 * 100];
        private object lockObject = new object();

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
            }

            return socket;
        }

        public void Connect()
        {
            if(!GetSocket().Connected)
            {
                GetSocket().Connect(new IPEndPoint(IPAddress.Loopback, 3000)); 
            }            
        }

        public void DisConnect()
        {
            GetSocket().Close();
        }


        public void StartRecv()
        {
            Thread thread = new Thread(Run);
            thread.Start();
        }


        // 로그인 성공 시 시작됨, 지속적으로 받음
        private void Run()
        {

            while (GetSocket().Connected)
            {
                try
                {

                    byte[] recvSize = new byte[4];
                    GetSocket().Receive(recvSize, 0, 4, SocketFlags.None);

                    int dataSize = BitConverter.ToInt32(recvSize, 0);

                    byte[] recv = new byte[dataSize];

                    GetSocket().Receive(recv, 0, dataSize, SocketFlags.None);

                    
                    int dataType = BitConverter.ToInt32(recv, 0);
                    
                    if(dataType == (int)PacketType.Text)
                    {
                        data = Encoding.UTF8.GetString(recv, 4, recv.Length - 4);
                        Recv(data);                    
                    }
                    
                    else
                    {
                        imageRecv(recv);
                    }
                    
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    continue;
                }


            }
        }

        // 로그인 시 잠깐 씀
        public string LoginRecv()
        {

            byte[] recv;
               
            try
            {
                byte[] recvSize = new byte[4];
                socket.Receive(recvSize, 0, 4, SocketFlags.None);

                int dataSize = BitConverter.ToInt32(recvSize, 0);
                recv = new byte[dataSize];

                data = null;

                int bytesRec = GetSocket().Receive(recv);
                data = Encoding.UTF8.GetString(recv, 4, bytesRec - 4);
                
                
                return data;
          
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                GetSocket().Close();
                return "false";
            }
                
        }

        public void ImageSend(byte[] image)
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

            Thread.Sleep(10);

            GetSocket().Send(send, 0, send.Length, SocketFlags.None);

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

    }
}

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
            byte[] recv = new byte[2048 * 100];
            int dataType;

            while (true)
            {
                try
                { 
                    int bytesRec = GetSocket().Receive(recv);
                    
                    dataType = BitConverter.ToInt32(recv, 0);
                    
                    if(dataType == (int)PacketType.Text)
                    {
                        data = Encoding.UTF8.GetString(recv, 4, bytesRec - 4);
                        Recv(data);
                    
                    }
                    
                    else
                    {
                        imageRecv(recv);
                    }
                    
                    Array.Clear(recv, 0, recv.Length);
                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    GetSocket().Close();
                    break;
                }

            }
        }

        // 로그인 시 잠깐 씀
        public string LoginRecv()
        {
            
            byte[] recv = new byte[1024];
               
            try
            {
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
            int sendLen = 0;

            byte[] packetType = BitConverter.GetBytes((int)PacketType.Image);
            byte[] text = Encoding.UTF8.GetBytes("MyImage");

            byte[] send = new byte[packetType.Length + text.Length + image.Length];

            packetType.CopyTo(send, 0);
            text.CopyTo(send, 4);
            image.CopyTo(send, 11);

            while (send.Length / 2 >= sendLen)
            {
                sendLen += GetSocket().Send(send);
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

            int sendLen = 0;
            

            while (send.Length / 2 >= sendLen)
            {
                sendLen += GetInstance().GetSocket().Send(send);
            }

            Thread.Sleep(15);

        }

    }
}

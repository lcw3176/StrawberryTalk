using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StrawberryClient.Model
{

    class SocketConnection
    {
        public delegate void Receive(string param);
        public event Receive Recv;


        public static SocketConnection Instance;
        private Socket socket;
        public string data;
        public bool isRun { get; set; }

        public SocketConnection()
        {
            isRun = false;
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

            byte[] recv = new byte[4096];

            while (true)
            {
                try
                { 
                    data = null;

                    while (isRun)
                    {
                        int bytesRec = GetSocket().Receive(recv);
                        data += Encoding.UTF8.GetString(recv, 0, bytesRec);

                        Array.Clear(recv, 0, recv.Length);

                        if (data.IndexOf("<EOF>") > -1)
                        {
                            Recv(data.Replace("<EOF>", string.Empty));
                            break;
                        }
                    }
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
            
                while (true)
                {
                    int bytesRec = GetSocket().Receive(recv);
                    data += Encoding.UTF8.GetString(recv, 0, bytesRec);
            
                    if (data.IndexOf("<EOF>") > -1)
                    {                            
                        break;
                    }            
                }

                return data.Substring(0, data.Length - 5);
            }
            
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                GetSocket().Close();
                return "false";
            }
                
        }

        public byte[] ImageReceive()
        {
            // 이미지 크기 먼저 받아옴
            byte[] recv = new byte[8192 * 100];

            try
            {
                GetSocket().Receive(recv);

                return recv;
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex);
                GetSocket().Close();
                return null;
            }
        }

        public void ImageSend(byte[] image, string userId)
        {
            int sendLen = 0;

            byte[] preSend = Encoding.UTF8.GetBytes(string.Format("SetImage?{0},{1}<EOF>", userId, image.Length));
            byte[] send = image;

            while (preSend.Length / 2 >= sendLen)
            {
                sendLen += GetSocket().Send(preSend);
            }

            // 딜레이 넣어서 서버에서 준비할 시간 주기
            Thread.Sleep(100);
            sendLen = 0;

            while(send.Length / 2 >= sendLen)
            {
                sendLen += GetSocket().Send(send);
            }
        }

        public void Send(string requset, params string[] queryString)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(requset);
            sb.Append("?");

            foreach (string i in queryString)
            {
                sb.Append(i);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);

            int sendLen = 0;
            byte[] send = Encoding.UTF8.GetBytes(sb.ToString() + "<EOF>");

            while (send.Length / 2 >= sendLen)
            {
                sendLen += GetInstance().GetSocket().Send(send);
            }

        }

    }
}

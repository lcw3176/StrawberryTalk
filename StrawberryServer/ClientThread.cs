using StrawberryServer.routes;
using System;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using System.Threading;

namespace StrawberryServer
{
    class ClientThread
    {
        private Socket socket;
        Index index;
        enum PacketType { Text, Image };

        public ClientThread()
        {
            Console.WriteLine("유저 연결됨");
        }

        public void SetInfo(Socket socket)
        {
            this.socket = socket;
            index = new Index(this.socket);
        }
       
        //Receive 계속 받기
        public void Start()
        {
            byte[] recv;
            string router;
            string param;
            int dataType;
            byte[] byteData;


            while (true)
            {
                try
                {
                    byte[] recvSize = new byte[4];
                    socket.Receive(recvSize, 0, 4, SocketFlags.None);

                    int dataSize = BitConverter.ToInt32(recvSize, 0);
                    recv = new byte[dataSize];

                    int recvLen = socket.Receive(recv, 0, dataSize, SocketFlags.None);
                    dataType = BitConverter.ToInt32(recv, 0);

                    // 텍스트 전송
                    if (dataType == (int)PacketType.Text)
                    {
                        string data = Encoding.UTF8.GetString(recv, 4, recvLen - 4);
                        router = data.Split('/')[0];
                        param = data.Split('/')[1];

                        Console.WriteLine(data);

                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { param });
                    }

                    // 이미지 전송
                    else
                    {
                        router = Encoding.UTF8.GetString(recv, 4, 7);

                        Console.WriteLine(router);
                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { recv });
                    }

                    byte[] sendSize;

                    sendSize = BitConverter.GetBytes(byteData.Length);
                    socket.Send(sendSize, 0, 4, SocketFlags.None);

                    Thread.Sleep(10);

                    socket.Send(byteData, 0, byteData.Length, SocketFlags.None);

                    Array.Clear(recv, 0, recv.Length);
                    Array.Clear(byteData, 0, byteData.Length);
                }

                catch
                {
                    Console.WriteLine("유저 연결 종료");
                    RoomManager.GetInstance().RemoveUser(socket);
                    socket.Close();
                    break;
                }
            }
        }
    }
}

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
        enum destination { Login, Join, Auth, Home, ChatRoom, Both };

        public ClientThread()
        {
            Console.WriteLine("유저 연결됨");
        }

        public void SetInfo(Socket socket)
        {
            this.socket = socket;
            socket.SendBufferSize = 0;
            index = new Index(socket);

            // https://support.microsoft.com/eu-es/help/214397/design-issues-sending-small-data-segments-over-tcp-with-winsock

        }

        //Receive 계속 받기
        public void Start()
        {
            string router;
            string param;
            int dataType;

            while (true)
            {
                try
                {
                    byte[] byteData; 

                    byte[] recvSize = new byte[4];
                    socket.Receive(recvSize, 0, 4, SocketFlags.None);

                    int dataSize = BitConverter.ToInt32(recvSize, 0);
                    byte[] recv = new byte[dataSize];

                    int recvLen = socket.Receive(recv, 0, dataSize, SocketFlags.None);
                    dataType = BitConverter.ToInt32(recv, 0);

                    // 텍스트 전송
                    if (dataType == (int)PacketType.Text)
                    {
                        string data = Encoding.UTF8.GetString(recv, 4, recvLen - 4);
                        Console.WriteLine(data);

                        router = data.Split('/')[0];
                        param = data.Replace(router + "/", string.Empty);

                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { param });
                    }

                    // 이미지 전송
                    else
                    {
                        router = Encoding.UTF8.GetString(recv, 4, 7);

                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { recv });
                    }

                    byte[] sendSize;

                    sendSize = BitConverter.GetBytes(byteData.Length);
                    socket.Send(sendSize, 0, 4, SocketFlags.None);

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

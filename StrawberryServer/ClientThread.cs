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
        enum PacketType { Text, Image};
        public ClientThread()
        {
            Console.WriteLine("유저 연결됨");
        }

        public void SetInfo(Socket socket)
        {
            this.socket = socket;
            index = new Index(this.socket);
        }

        // Receive 계속 받기
        public void Start()
        {
            byte[] recv = new byte[2048 * 100];
            //string data;
            string router;
            string param;
            int dataType = 0;
            byte[] byteData;


            while (true)
            {
                try
                {
                    //data = null;

                    int recvLen = socket.Receive(recv);
                    //data = Encoding.UTF8.GetString(recv, 0, bytesRec);

                    //Console.WriteLine(data);
                    dataType = BitConverter.ToInt32(recv, 0);

                    // 텍스트 전송
                    if(dataType == (int)PacketType.Text)
                    {
                        string data = Encoding.UTF8.GetString(recv, 4, recvLen - 4);
                        router = data.Split('/')[0];
                        param = data.Split('/')[1];

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

                    int sendLen = 0;

                    while (byteData.Length / 2 >= sendLen)
                    {
                        sendLen += socket.Send(byteData);
                    }

                    Array.Clear(recv, 0, recvLen);

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

using StrawberryServer.routes;
using System;
using System.Net.Sockets;
using System.Text;
using System.Reflection;

namespace StrawberryServer
{
    class ClientThread
    {
        private Socket socket;
        Index index;

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
            byte[] recv = new byte[1024];
            string data;
            string router;
            string param;

            while (true)
            {
                try
                {
                    data = null;

                    int bytesRec = socket.Receive(recv);
                    data = Encoding.UTF8.GetString(recv, 0, bytesRec);

                    Array.Clear(recv, 0, bytesRec);

                    Console.WriteLine(data);
                    
                    router = data.Split('/')[0];
                    param = data.Split('/')[1];

                    Type type = index.GetType();
                    MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                    byte[] byteData = (byte[])routes.Invoke(index, new object[] { param });

                    int sendLen = 0;

                    while(byteData.Length / 2 >= sendLen)
                    {
                        sendLen += socket.Send(byteData);
                    }

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

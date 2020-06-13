using StrawberryServer.routes;
using System;
using System.Net.Sockets;
using System.Text;
using System.Reflection;
using System.Linq;

namespace StrawberryServer
{
    class ClientInfo
    {
        private Socket socket;
        Index index;

        public ClientInfo()
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

            while (true)
            {
                try
                {
                    data = null;

                    while (true)
                    {
                        int bytesRec = socket.Receive(recv);
                        data += Encoding.UTF8.GetString(recv, 0, bytesRec);

                        Array.Clear(recv, 0, recv.Length);
                        if (data.IndexOf("<EOF>") > -1)
                        {
                            break;
                        }
                    }

                    Console.WriteLine(data);

                    Type type = index.GetType();
                    MethodInfo routes = type.GetMethod(data.Split('?')[0], BindingFlags.Instance | BindingFlags.Public);
                    byte[] byteData = (byte[])routes.Invoke(index, new object[] { data.Split('?')[1].Replace("<EOF>", string.Empty) });
                    //string path = "StrawberryServer.routes." + data.Split('?')[0];
                    //string queryString = data.Replace(data.Split('?')[0] + "?", string.Empty);

                    //Assembly assembly = Assembly.GetExecutingAssembly();

                    //Type router = assembly.GetType(path);

                    //IRoutes routes = (IRoutes)Activator.CreateInstance(router);
                    //routes.SetInfo(queryString.Replace("<EOF>", string.Empty), socket);

                    //byte[] byteData = routes.Process();
                    byte[] eof = Encoding.UTF8.GetBytes("<EOF>");

                    byte[] send = new byte[byteData.Length + eof.Length];

                    Array.Copy(byteData, 0, send, 0, byteData.Length);
                    Array.Copy(eof, 0, send, byteData.Length, eof.Length);

                    int sendLen = 0;

                    while(send.Length / 2 >= sendLen)
                    {
                        sendLen += socket.Send(send);
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

﻿using StrawberryServer.routes;
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
        enum PacketType { Text, Image, Close };
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
        }

        //Receive 계속 받기
        public void Start()
        {
            string router;
            string param;
            int dataType;

            while (socket.Connected)
            {
                try
                {
                    byte[] byteData = null; 

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

                        router = data.Split('/')[0];
                        param = data.Replace(router + "/", string.Empty);

                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { param });
                    }

                    // 이미지 전송
                    else if(dataType == (int)PacketType.Image)
                    {
                        router = Encoding.UTF8.GetString(recv, 4, 7);

                        Type type = index.GetType();
                        MethodInfo routes = type.GetMethod(router, BindingFlags.Instance | BindingFlags.Public);
                        byteData = (byte[])routes.Invoke(index, new object[] { recv });
                    }

                    // 종료 신호
                    else
                    {
                        Close();
                        break;
                    }

                    byte[] sendSize;
                    sendSize = BitConverter.GetBytes(byteData.Length);

                    socket.Send(sendSize, 0, 4, SocketFlags.None);
                    socket.Send(byteData, 0, byteData.Length, SocketFlags.None);
                }

                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                    Close();
                    break;
                }
            }
        }

        private void Close()
        {
            RoomManager.GetInstance().RemoveUser(socket);
            socket.Close();
        }
    }
}

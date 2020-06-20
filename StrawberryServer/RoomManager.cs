using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace StrawberryServer
{
    class RoomManager
    {
        public static RoomManager instance;
        Dictionary<string, Socket> userDic = new Dictionary<string, Socket>();
        enum PacketType { Text, Image };
        public static RoomManager GetInstance()
        {
            if(instance == null)
            {
                instance = new RoomManager();
            }

            return instance;
        }

        public void AddUser(string userId, Socket socket)
        {
            userDic.Add(userId, socket);
            Console.WriteLine(userId + " 접속함  " + socket.RemoteEndPoint.ToString());
        }

        public void RemoveUser(Socket socket)
        {
            try
            {
                string userId = userDic.FirstOrDefault(x => x.Value == socket).Key;
                userDic.Remove(userId);
                Console.WriteLine(userId + " 접속 종료");
            }

            catch
            {
                Console.WriteLine("로그인에서 접속 종료");
                return;
            }

        }

        //public void RemoveUser(string userId)
        //{
        //    userDic.Remove(userId);
        //}
        
        // 중복 로그인 체크
        public bool CheckUser(string userId)
        {
            bool isConnect = userDic.TryGetValue(userId, out Socket temp);
            if(isConnect == true)
            {
                return false;
            }

            return true;
        }


        // 메세지 뿌리기
        public void EchoRoomUsers(string roomName, string fromUserName, string sendData)
        {
            List<string> key = new List<string>();

            foreach (string i in roomName.Split('&'))
            {
                if(i != fromUserName)
                {
                    key.Add(i);
                }
            }

            byte[] type = BitConverter.GetBytes((int)PacketType.Text);
            byte[] text = Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);

            byte[] send = new byte[type.Length + text.Length];

            byte[] size;

            size = BitConverter.GetBytes(send.Length);

            type.CopyTo(send, 0);
            text.CopyTo(send, 4);

            foreach (string i in key)
            {
                // 유저 접속 여부 체크
                if (userDic.TryGetValue(i, out Socket sockTemp))
                {
                    userDic[i].Send(size);
                    Thread.Sleep(10);
                    userDic[i].Send(send);
                }

                else
                {
                    continue;
                }
                   
            }
  
        }
    }
}

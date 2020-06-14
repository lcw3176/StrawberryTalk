using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer
{
    class RoomManager
    {
        public static RoomManager instance;
        Dictionary<string, Socket> userDic = new Dictionary<string, Socket>();

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
        }

        public void RemoveUser(Socket socket)
        {
            try
            {
                string userId = userDic.FirstOrDefault(x => x.Value == socket).Key;
                userDic.Remove(userId);
            }

            catch(Exception ex)
            {
                Console.WriteLine(ex);
                return;
            }

        }

        public void RemoveUser(string userId)
        {
            userDic.Remove(userId);
        }
        
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
            string temp = roomName.Replace(fromUserName, string.Empty);
            string[] key = temp.Replace("&&", "&").Trim().Split('&');

            foreach (string i in key)
            {
                // 유저 접속 여부 체크
                if (userDic.TryGetValue(i, out Socket sockTemp))
                {
                    userDic[i].Send(Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData + "<EOF>"));
                }

                else
                {
                    continue;
                }
                   
            }
  
        }
    }
}

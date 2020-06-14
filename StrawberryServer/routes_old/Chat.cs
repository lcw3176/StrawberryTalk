using StrawberryServer.DataBase;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class Chat : IRoutes
    {
        bool isExist = false;
        private string roomName;
        private string fromUserName = string.Empty;
        private string msg = string.Empty;
        private string sendData = string.Empty;

        public byte[] Process()
        {
            if(isExist)
            {
                // 방 이름을 앞에 달아서 보내주기
                return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
            }
            else
            {
                return Encoding.UTF8.GetBytes("<FIRST>");
            }
        }

        public void SetInfo(string param, Socket socket)
        {   
            roomName = param.Split(',')[0];
            roomName = roomName.Replace("@", "&");

            // 채팅 중일 때
            try
            {
                fromUserName = param.Split(',')[1];
                msg = param.Split(',')[2];

                Query.GetInstance().SetMessage(roomName, fromUserName, msg);
                sendData = string.Join("&", fromUserName, msg) + "<CHAT>";
                isExist = true;

                RoomManager.GetInstance().EchoRoomUsers(roomName, fromUserName, sendData); 
            }

            // 채팅방 처음 띄울 때
            catch (IndexOutOfRangeException)
            {
                // 채팅방 없으면 만들기
                if (string.IsNullOrEmpty(Query.GetInstance().GetNameFromRoom(roomName)))
                {
                    Query.GetInstance().SetRoom(roomName);
                    isExist = false;
                }

                // 존재하는 채팅방 메세지 불러오기
                else
                {
                    // [0] fromUserName [1] msg
                    List<string> data = Query.GetInstance().GetMessage(roomName);
                    sendData = string.Join("&", data) + "<MADE>";
                    isExist = true;
                }
            }
          
            
        }
    }
}

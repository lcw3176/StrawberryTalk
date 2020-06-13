using StrawberryServer.DataBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryServer.routes
{
    class Index
    {
        
        Socket socket;
        
        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Chat(string param)
        {
            bool isExist;
            string roomName;
            string fromUserName;
            string msg;
            string sendData = string.Empty;

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

            if (isExist)
            {
                // 방 이름을 앞에 달아서 보내주기
                return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
            }
            else
            {
                return Encoding.UTF8.GetBytes("<FIRST>");
            }
        }

        public byte[] Close(string param)
        {
            string userId = param;
            RoomManager.GetInstance().RemoveUser(userId);

            return Encoding.UTF8.GetBytes("raise Exception");
        }

        public byte[] GetImage(string param)
        {            
            string userId = param.Split(',')[0];

            string imagePath = Query.GetInstance().GetImageFromUser(userId);

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = Image.FromFile(imagePath);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();
                return ms.ToArray();
            }
        }

        public byte[] GetUser(string param)
        {
            string userId = param.Split(',')[0];
            string friendsName = param.Split(',')[1];

            string result = Query.GetInstance().GetPersonalNameFromUser(friendsName);

            if (result != "None")
            {
                Query.GetInstance().SetFriend(userId, friendsName);
            }

            return Encoding.UTF8.GetBytes("<FIND>" + result);
        }

        public byte[] Join(string param)
        {
            string userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            if (Query.GetInstance().SetUser(userId, userPw))
            {
                return Encoding.UTF8.GetBytes("true");
            }

            else
            {
                return Encoding.UTF8.GetBytes("false");
            }
        }

        public byte[] Login(string param)
        {
            Console.WriteLine(param + "login 메소드");
            string userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            string[] userList = Query.GetInstance().GetUserLogin().Split(',');

            // [0]: 아이디, [1]: 비밀번호
            for (int i = 0; i < userList.Length; i += 2)
            {
                if (userList[i] == userId && userList[i + 1] == userPw)
                {
                    if (RoomManager.GetInstance().CheckUser(userId))
                    {
                        string userInfo = Query.GetInstance().GetUserLoginSuccess(userId);
                        RoomManager.GetInstance().AddUser(userId, socket);
                        return Encoding.UTF8.GetBytes(userInfo);
                    }

                    else
                    {
                        return Encoding.UTF8.GetBytes("already");
                    }

                }
            }

            return Encoding.UTF8.GetBytes("false");
        }

        public byte[] MoreChat(string param)
        {
            int pageNation;
            string roomName = param.Split(',')[0];
            string fromUserName = param.Split(',')[1];
            int.TryParse(param.Split(',')[2], out pageNation);

            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data) + "<PLUS>";

            return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);

        }

        public byte[] SetImage(string param)
        {
            string userId = param.Split(',')[0];
            int len = int.Parse(param.Split(',')[1]);

            byte[] byteImage = new byte[len];

            // len == 0일땐 상태 메세지만 변경

            string path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\" + userId + ".jpg";

            socket.Receive(byteImage);

            using (MemoryStream ms = new MemoryStream(byteImage))
            {
                Image image = Image.FromStream(ms);
                image.Save(path, ImageFormat.Jpeg);
                image.Dispose();
            }

            Query.GetInstance().SetImage(userId, path);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }

        public byte[] SetImageDefault(string param)
        {
            string userId = param.Split(',')[0];

            Query.GetInstance().SetImage(userId, null);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }
    }
}

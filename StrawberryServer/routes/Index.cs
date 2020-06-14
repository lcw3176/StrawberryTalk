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
        private string userId { get; set; }
        private Socket socket;

        
        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Login(string param)
        {
            userId = param.Split(',')[0];
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

        public byte[] Join(string param)
        {
            userId = param.Split(',')[0];
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

        public byte[] User(string param)
        {
            string findUser = param;
            string result = Query.GetInstance().GetPersonalNameFromUser(findUser);

            if (result != "None")
            {
                Query.GetInstance().SetFriend(userId, findUser);
            }

            return Encoding.UTF8.GetBytes("<FIND>" + result);
        }

        public byte[] Room(string param)
        {
            string roomName = param;
            
            // 채팅방 없으면 만들기
            if (string.IsNullOrEmpty(Query.GetInstance().GetNameFromRoom(roomName)))
            {
                Query.GetInstance().SetRoom(roomName);

                return Encoding.UTF8.GetBytes("<FIRST>");
            }

            // 존재하는 채팅방 메세지 불러오기
            else
            {
                List<string> data = Query.GetInstance().GetMessage(roomName);
                string sendData = string.Join("&", data);
                // 방 이름을 앞에 달아서 보내주기
                return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
            }
        }

        public byte[] Chat(string param)
        {
            string roomName = param.Split(',')[0];

            string fromUserName = param.Split(',')[1];
            string msg = param.Split(',')[2];
            
            Query.GetInstance().SetMessage(roomName, fromUserName, msg);
            string sendData = string.Join("&", fromUserName, msg) + "<CHAT>";
            
            RoomManager.GetInstance().EchoRoomUsers(roomName, fromUserName, sendData);
            return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
        }

        public byte[] Message(string param)
        {
            string roomName = param.Split(',')[0];
            int pageNation = int.Parse(param.Split(',')[1]);

            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data) + "<PLUS>";

            return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
        }


        public byte[] Image(string param)
        {            
            string userId = param.Split(',')[0];

            string imagePath = Query.GetInstance().GetImageFromUser(userId);

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = System.Drawing.Image.FromFile(imagePath);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();
                return ms.ToArray();
            }
        }
     

        public byte[] MyImage(string param)
        {
            int len = int.Parse(param);

            byte[] byteImage = new byte[len];

            string path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\" + userId + ".jpg";

            socket.Receive(byteImage);

            using (MemoryStream ms = new MemoryStream(byteImage))
            {
                Image image = System.Drawing.Image.FromStream(ms);
                image.Save(path, ImageFormat.Jpeg);
                image.Dispose();
            }

            Query.GetInstance().SetImage(userId, path);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }

        public byte[] DefaultImage(string param)
        {
            Query.GetInstance().SetImage(userId, null);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }
    }
}

using StrawberryServer.DataBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class Index
    {
        private string userId { get; set; }
        private string userNickname { get; set; }
        private Socket socket;
        private Auth auth;
        enum packetType { Text,Image};
        
        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Login(string param)
        {
            userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            userNickname = Query.GetInstance().GetNickname(userId, userPw);
            bool isAuth = Query.GetInstance().GetAuth(userId);

            if (!string.IsNullOrEmpty(userNickname) && isAuth)
            {
                if (RoomManager.GetInstance().CheckUser(userId))
                {
                    string userInfo = Query.GetInstance().GetUserInfo(userNickname);
                    RoomManager.GetInstance().AddUser(userNickname, socket);

                    return Process(userInfo);
                }
            
                else
                {
                    return Process("already");
                }
            
            }

            else if(!string.IsNullOrEmpty(userNickname) && !isAuth)
            {
                return Process("auth");
            }

            else
            {
                return Process("false");
            }
        }

        public byte[] Join(string param)
        {
            userId = param.Split(',')[0];
            userNickname = param.Split(',')[1];
            string userPw = param.Split(',')[2];

            string isExist = Query.GetInstance().GetNickname(userId);

            if(string.IsNullOrEmpty(isExist))
            {
                Query.GetInstance().SetUser(userId, userNickname, userPw);
                return Process("true");
            }

            else
            {
                return Process("false");
            }
        }

        public byte[] Auth(string param)
        {
            string request = param;

            if(request == "set")
            {
                auth = new Auth();
                auth.SetAuthNumber();
                auth.SendMail(userId);

                return Process("true");
            }


            if (auth.CompareAuthNumber(int.Parse(request)))
            {
                Query.GetInstance().SetAuth(userId);
                return Process("true");
            }

            else
            {
                return Process("false");
            }
        }

        public byte[] User(string param)
        {
            string findUser = param;
            string result = Query.GetInstance().GetNickname(findUser);

            if (!string.IsNullOrEmpty(result))
            {
                Query.GetInstance().SetFriend(userNickname, findUser);
            }

            else
            {
                result = "None";
            }

            return Process("<FIND>" + result);
        }

        public byte[] Room(string param)
        {
            string roomName = param;
            
            // 채팅방 없으면 만들기
            if (string.IsNullOrEmpty(Query.GetInstance().GetRoom(roomName)))
            {
                Query.GetInstance().SetRoom(roomName);

                return Process("<FIRST>");
            }

            // 존재하는 채팅방 메세지 불러오기
            else
            {
                List<string> data = Query.GetInstance().GetMessage(roomName);
                string sendData = string.Join("&", data);
                // 방 이름을 앞에 달아서 보내주기
                return Process(roomName + "<AND>" + sendData);
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

            return Process(roomName + "<AND>" + sendData);
        }

        public byte[] Message(string param)
        {
            string roomName = param.Split(',')[0];
            int pageNation = int.Parse(param.Split(',')[1]);

            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data) + "<PLUS>";

            return Process(roomName + "<AND>" + sendData);
        }


        public byte[] Image(string param)
        {
            string userId = param;

            string imagePath = Query.GetInstance().GetImagePath(userId);

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = System.Drawing.Image.FromFile(imagePath);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();

                return Process(userId, ms.ToArray());
            }
        }
     

        public byte[] MyImage(byte[] Image)
        {
            string path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\" + userId + ".jpg";

            using (MemoryStream ms = new MemoryStream(Image, 11, Image.Length - 11))
            {
                Image image = System.Drawing.Image.FromStream(ms);
                image.Save(path, ImageFormat.Jpeg);
                image.Dispose();
            }

            Query.GetInstance().SetImage(userId, path);

            return Process("<RFRH>");
        }

        public byte[] DefaultImage(string param)
        {
            Query.GetInstance().SetImage(userId, null);

            return Process("<RFRH>");
        }

        private byte[] Process(string data)
        {
            byte[] type = BitConverter.GetBytes((int)packetType.Text);
            byte[] text = Encoding.UTF8.GetBytes(data);

            byte[] send = new byte[type.Length + text.Length];

            type.CopyTo(send, 0);
            text.CopyTo(send, 4);

            return send;
        }

        private byte[] Process(string userId, byte[] Image)
        {
            if(userId.Length < 10)
            {
                for(int i = userId.Length; i < 10; i++)
                {
                    userId += "&";
                }                
            }


            byte[] type = BitConverter.GetBytes((int)packetType.Image);
            byte[] id = Encoding.UTF8.GetBytes(userId);
            byte[] send = new byte[type.Length + id.Length + Image.Length];

            type.CopyTo(send, 0);
            id.CopyTo(send, 4);
            Image.CopyTo(send, 14);

            return send;
        }
    }
}

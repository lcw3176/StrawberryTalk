using StrawberryServer.DataBase;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace StrawberryServer.routes
{
    class Index
    {
        private string userId { get; set; }
        private string userNickname { get; set; }
        private Socket socket;
        private Auth auth;
        enum packetType { Text,Image };
        enum destination { Login, Join, Auth, Home, ChatRoom };
        
        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Login(string param)
        {
            userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            Encryption enc = new Encryption();
            enc.SetValue(userPw);

            string encPw = enc.GetValue();

            userNickname = Query.GetInstance().GetNickname(userId, encPw);
            bool isAuth = Query.GetInstance().GetAuth(userId);

            if (!string.IsNullOrEmpty(userNickname) && isAuth)
            {
                if (RoomManager.GetInstance().CheckUser(userId))
                {
                    RoomManager.GetInstance().AddUser(userNickname, socket);

                    return Process(destination.Login, "true");
                }
            
                else
                {
                    return Process(destination.Login, "already");
                }
            
            }

            else if(!string.IsNullOrEmpty(userNickname) && !isAuth)
            {
                return Process(destination.Login, "auth");
            }

            else
            {
                return Process(destination.Login, "false");
            }
        }

        public byte[] Join(string param)
        {
            userId = param.Split(',')[0];
            userNickname = param.Split(',')[1];
            string userPw = param.Split(',')[2];

            Encryption enc = new Encryption();
            enc.SetValue(userPw);

            string encPw = enc.GetValue();

            string emailExist = Query.GetInstance().GetEmail(userId);
            string nicknameExist = Query.GetInstance().GetNickname(userNickname);


            if(string.IsNullOrEmpty(nicknameExist) && string.IsNullOrEmpty(emailExist))
            {
                Query.GetInstance().SetUser(userId, userNickname, encPw);
                return Process(destination.Join, "true");
            }

            else if(!string.IsNullOrEmpty(emailExist))
            {
                return Process(destination.Join, "email");
            }

            else if(!string.IsNullOrEmpty(nicknameExist))
            {
                return Process(destination.Join, "nickname");
            }

            else
            {
                return Process(destination.Join, "false");
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

                return Process(destination.Auth, "set");
            }


            if (auth.CompareAuthNumber(int.Parse(request)))
            {
                Query.GetInstance().SetAuth(userId);
                return Process(destination.Auth, "true");
            }

            else
            {
                return Process(destination.Auth, "false");
            }
        }

        public byte[] Success(string param)
        {
            string data = Query.GetInstance().GetUserInfo(userNickname);

            return Process(destination.Home, data + "<INIT>");
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

            return Process(destination.Home, "<FIND>" + result + "<FIND>");
        }

        public byte[] Room(string param)
        {
            string roomName = param;
            
            // 채팅방 없으면 만들기
            if (string.IsNullOrEmpty(Query.GetInstance().GetRoom(roomName)))
            {
                Query.GetInstance().SetRoom(roomName);

                return Process(destination.ChatRoom, "<FIRST>");
            }

            // 존재하는 채팅방 메세지 불러오기
            else
            {
                List<string> data = Query.GetInstance().GetMessage(roomName);
                string message = string.Join("&", data);

                return Process(destination.ChatRoom, "<INIT>" + roomName + "<AND>" + message);
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

            return Process(destination.ChatRoom, roomName + "<AND>" + sendData);
        }

        public byte[] Message(string param)
        {
            string roomName = param.Split(',')[0];
            int pageNation = int.Parse(param.Split(',')[1]);

            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data) + "<PLUS>";

            return Process(destination.ChatRoom, roomName + "<AND>" + sendData);
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

            return Process(destination.Home, "<REFRESH>");
        }

        public byte[] DefaultImage(string param)
        {
            Query.GetInstance().SetImage(userId, null);

            return Process(destination.Home, "<REFRESH>");
        }

        private byte[] Process(destination viewModel, string data)
        {
            byte[] type = BitConverter.GetBytes((int)packetType.Text);
            byte[] togo = BitConverter.GetBytes((int)viewModel);
            byte[] text = Encoding.UTF8.GetBytes(data);

            byte[] send = new byte[type.Length + togo.Length + text.Length];

            type.CopyTo(send, 0);
            togo.CopyTo(send, 4);
            text.CopyTo(send, 8);

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

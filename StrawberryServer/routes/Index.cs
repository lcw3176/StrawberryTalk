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
        enum packetType { Text,Image }
        enum destination { Login, Join, Auth, Home, ChatRoom, Both }

        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Login(string param)
        {
            LoginResponse response = new LoginResponse();

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
                    response.SetTrue();
                    return Process(destination.Login, response, null);
                }
            
                else
                {
                    response.SetAlready();
                    return Process(destination.Login, response, null);
                }
            
            }

            else if(!string.IsNullOrEmpty(userNickname) && !isAuth)
            {
                response.SetAuth();
                return Process(destination.Login, response, null);
            }

            else
            {
                response.SetFalse();
                return Process(destination.Login, response, null);
            }
        }

        public byte[] Join(string param)
        {
            JoinResponse response = new JoinResponse();

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
                response.SetTrue();
                return Process(destination.Join, response, null);
            }

            else if(!string.IsNullOrEmpty(emailExist))
            {
                response.SetEmail();
                return Process(destination.Join, response, null);
            }

            else if(!string.IsNullOrEmpty(nicknameExist))
            {
                response.SetNickname();
                return Process(destination.Join, response, null);
            }

            else
            {
                response.SetFalse();
                return Process(destination.Join, response, null);
            }
        }

        public byte[] Auth(string param)
        {
            AuthResponse response = new AuthResponse();
            string request = param;

            if(request == "set")
            {
                auth = new Auth();
                auth.SetAuthNumber();
                auth.SendMail(userId);
                response.SetReady();

                return Process(destination.Auth, response, null);
            }


            if (auth.CompareAuthNumber(int.Parse(request)))
            {
                Query.GetInstance().SetAuth(userId);
                response.SetTrue();

                return Process(destination.Auth, response, null);
            }

            else
            {
                response.SetFalse();

                return Process(destination.Auth, response, null);
            }
        }

        public byte[] Success(string param)
        {
            HomeReponse reponse = new HomeReponse();

            string data = Query.GetInstance().GetUserInfo(userNickname);
            reponse.SetInit();

            return Process(destination.Home, reponse, data);
        }

        public byte[] User(string param)
        {
            HomeReponse reponse = new HomeReponse();
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

            reponse.SetFind();

            return Process(destination.Home, reponse, result);
        }

        public byte[] Room(string param)
        {
            ChatRoomResponse response = new ChatRoomResponse();
            string roomName = param;
            
            // 채팅방 없으면 만들기
            if (string.IsNullOrEmpty(Query.GetInstance().GetRoom(roomName)))
            {
                Query.GetInstance().SetRoom(roomName);
                response.SetFirst();

                return Process(destination.ChatRoom, response, null);
            }

            // 존재하는 채팅방 메세지 불러오기
            else
            {
                List<string> data = Query.GetInstance().GetMessage(roomName);
                string message = string.Join("&", data);

                response.SetInit();

                return Process(destination.ChatRoom, response, roomName + "/" + message);
            }
        }

        public byte[] Chat(string param)
        {
            ChatRoomResponse response = new ChatRoomResponse();
            string roomName = param.Split(',')[0];

            string fromUserName = param.Split(',')[1];
            string msg = param.Split(',')[2];
            
            Query.GetInstance().SetMessage(roomName, fromUserName, msg);
            string sendData = string.Join("&", fromUserName, msg);

            RoomManager.GetInstance().EchoRoomUsers(roomName, fromUserName, sendData);
            response.SetChat();

            return Process(destination.ChatRoom, response, roomName + "/" + sendData);
        }

        public byte[] Message(string param)
        {
            ChatRoomResponse response = new ChatRoomResponse();
            string roomName = param.Split(',')[0];
            int pageNation = int.Parse(param.Split(',')[1]);

            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data);
            response.SetPlus();

            return Process(destination.ChatRoom, response, roomName + "/" + sendData);
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
            HomeReponse reponse = new HomeReponse();
            string path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\" + userId + ".jpg";

            using (MemoryStream ms = new MemoryStream(Image, 11, Image.Length - 11))
            {
                Image image = System.Drawing.Image.FromStream(ms);
                image.Save(path, ImageFormat.Jpeg);
                image.Dispose();
            }

            Query.GetInstance().SetImage(userId, path);
            reponse.SetRefresh();

            return Process(destination.Home, reponse, null);
        }

        public byte[] DefaultImage(string param)
        {
            HomeReponse reponse = new HomeReponse();
            Query.GetInstance().SetImage(userId, null);
            reponse.SetRefresh();

            return Process(destination.Home, reponse, null);
        }

        private byte[] Process(destination viewModel, IResponse response, string data)
        {
            if (string.IsNullOrEmpty(data)) { data = "null"; }

            byte[] type = BitConverter.GetBytes((int)packetType.Text);
            byte[] togo = BitConverter.GetBytes((int)viewModel);
            byte[] res = BitConverter.GetBytes(response.ToInt());
            byte[] send;

            byte[] text = Encoding.UTF8.GetBytes(data);

            send = new byte[type.Length + togo.Length + res.Length + text.Length];

            type.CopyTo(send, 0);
            togo.CopyTo(send, 4);
            res.CopyTo(send, 8);
            text.CopyTo(send, 12);
            // response 수정 완료

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

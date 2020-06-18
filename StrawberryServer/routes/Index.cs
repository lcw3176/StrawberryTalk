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
        enum packetType { Text,Image};
        
        public Index(Socket socket)
        {
            this.socket = socket;
        }

        public byte[] Login(string param)
        {
            userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            bool isAuth = Query.GetInstance().GetUser(userId, userPw);

            if (isAuth)
            {
                if (RoomManager.GetInstance().CheckUser(userId))
                {
                    string userInfo = Query.GetInstance().GetUserInfo(userId);
                    RoomManager.GetInstance().AddUser(userId, socket);
                    return Process(userInfo);
                }
            
                else
                {
                    return Process("already");
                }
            
            }

            return Process("false");
        }

        public byte[] Join(string param)
        {
            userId = param.Split(',')[0];
            string userPw = param.Split(',')[1];

            if (Query.GetInstance().SetUser(userId, userPw))
            {
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
            string result = Query.GetInstance().GetUser(findUser);

            if (!string.IsNullOrEmpty(result))
            {
                Query.GetInstance().SetFriend(userId, findUser);
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
            // 단톡일 경우 맨 앞사람 프사로 갖다줌
            string userId = param.Split(',')[0];

            string imagePath = Query.GetInstance().GetImagePath(userId);

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = System.Drawing.Image.FromFile(imagePath);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();

                return Process(ms.ToArray());
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

        private byte[] Process(byte[] Image)
        {
            byte[] type = BitConverter.GetBytes((int)packetType.Image);

            byte[] send = new byte[type.Length + Image.Length];

            type.CopyTo(send, 0);
            Image.CopyTo(send, 4);

            return send;
        }
    }
}

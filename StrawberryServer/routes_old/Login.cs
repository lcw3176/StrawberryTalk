using StrawberryServer.DataBase;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class Login : IRoutes
    {
        string userId;
        string userPw;
        Socket socket;


        public byte[] Process()
        {
            string[] userList = Query.GetInstance().GetUserLogin().Split(',');

            // [0]: 아이디, [1]: 비밀번호
            for(int i = 0;i < userList.Length; i+=2)
            {
                if(userList[i] == userId && userList[i + 1] == userPw)
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

        public void SetInfo(string param, Socket socket)
        {
            this.socket = socket;
            userId = param.Split(',')[0];
            userPw = param.Split(',')[1];
        }

    }
}

using StrawberryServer.DataBase;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class Join : IRoutes
    {
        private string userId;
        private string userPw;

        public byte[] Process()
        {
            // 유저 회원가입
            if(Query.GetInstance().SetUser(userId, userPw))
            {
                return Encoding.UTF8.GetBytes("true");
            }

            else
            {
                return Encoding.UTF8.GetBytes("false");
            }
        }

        public void SetInfo(string param, Socket socket)
        {
            userId = param.Split(',')[0];
            userPw = param.Split(',')[1];
        }
    }
}

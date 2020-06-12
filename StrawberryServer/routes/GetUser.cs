using StrawberryServer.DataBase;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class GetUser : IRoutes
    {
        string userId;
        string friendsName;

        public byte[] Process()
        {
            string result = Query.GetInstance().GetPersonalNameFromUser(friendsName);

            if(result != "None") 
            {
                Query.GetInstance().SetFriend(userId, friendsName);
            }

            return Encoding.UTF8.GetBytes("<FIND>" + result);
        }

        public void SetInfo(string param, Socket socket)
        {
            userId = param.Split(',')[0];
            friendsName = param.Split(',')[1];
        }
    }
}

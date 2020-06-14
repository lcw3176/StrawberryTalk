using StrawberryServer.DataBase;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class SetImageDefault : IRoutes
    {
        private string userId;

        public byte[] Process()
        {
            Query.GetInstance().SetImage(userId, null);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }

        public void SetInfo(string param, Socket socket)
        {
            userId = param.Split(',')[0];
        }
    }
}

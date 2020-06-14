using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class Close : IRoutes
    {
        public byte[] Process()
        {
            return Encoding.UTF8.GetBytes("raise Exception");
        }

        public void SetInfo(string param, Socket socket)
        {
            string userId = param;
            RoomManager.GetInstance().RemoveUser(userId);
        }
    }
}

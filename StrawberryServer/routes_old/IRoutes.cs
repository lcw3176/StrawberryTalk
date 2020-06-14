using System.Net.Sockets;

namespace StrawberryServer.routes
{
    interface IRoutes
    {
        void SetInfo(string param, Socket socket);
        byte[] Process();

    }
}

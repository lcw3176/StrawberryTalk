using StrawberryServer.DataBase;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;

namespace StrawberryServer.routes
{
    class GetImage : IRoutes
    {
        string userId;

        public byte[] Process()
        {
            string imagePath = Query.GetInstance().GetImageFromUser(userId);

            using (MemoryStream ms = new MemoryStream())
            {
                Image image = Image.FromFile(imagePath);
                image.Save(ms, ImageFormat.Jpeg);
                image.Dispose();
                return ms.ToArray();
            }

        }


        public void SetInfo(string param, Socket socket)
        {
            userId = param.Split(',')[0];
        }
    }
}

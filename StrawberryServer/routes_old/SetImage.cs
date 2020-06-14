using StrawberryServer.DataBase;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class SetImage : IRoutes
    {
        Socket socket;
        string userId;
        string path;
        int len;
        byte[] byteImage;

        public byte[] Process()
        {
            // len == 0일땐 상태 메세지만 변경

            path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\" + userId + ".jpg";

            socket.Receive(byteImage);

            using (MemoryStream ms = new MemoryStream(byteImage))
            {
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters parameters = new EncoderParameters(1);
                EncoderParameter param = new EncoderParameter(encoder, 20L);
                parameters.Param[0] = param;

                Image image = Image.FromStream(ms);
                image.Save(path, jpgEncoder, parameters);
                image.Dispose();
            }

            Query.GetInstance().SetImage(userId, path);

            return Encoding.UTF8.GetBytes("<RFRH>");
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        // [0] 아이디, [1] 이미지 길이
        public void SetInfo(string param, Socket socket)
        {
            userId = param.Split(',')[0];
            len = int.Parse(param.Split(',')[1]);
            this.socket = socket;

            byteImage = new byte[len];

        }
    }
}

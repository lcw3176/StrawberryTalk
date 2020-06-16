using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Media;

namespace StrawberryClient.Model
{
    class SetProfileModel
    {
        private ImageSource profileImage;
        private string userId;
        private string path;

        public string Path
        {
            get { return path; }
            set { path = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set { profileImage = value; }
        }

        // 사진 설정 완료 메소드
        public void complete()
        {
            if(!string.IsNullOrEmpty(path))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters parameters = new EncoderParameters(1);
                    EncoderParameter param = new EncoderParameter(encoder, 20L);
                    parameters.Param[0] = param;

                    Image image = Image.FromFile(path);

                    // 사진 크기가 크면 줄여줌
                    if(image.Height + image.Width >= 3000)
                    {
                        Size size = new Size(1920, 1080);
                        Image resizeImage = new Bitmap(image, size);
                        resizeImage.Save(ms, jpgEncoder, parameters);
                        resizeImage.Dispose();
                    }

                    else
                    {
                        image.Save(ms, jpgEncoder, parameters);
                    }
                    
                    SocketConnection.GetInstance().ImageSend(ms.ToArray());

                    image.Dispose();
                    param.Dispose();
                    parameters.Dispose();
                    
                }
            }

        }

        // 기본 프로필 사진으로 설정
        public void setDefault()
        {
            SocketConnection.GetInstance().Send("DefaultImage", "null");
        }


        // 엔코더 가져오기
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
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryServer
{
    class Auth
    {
        private int number { get; set; }

        public void SetAuthNumber()
        {
            Random random = new Random();
            this.number = random.Next(100000, 999999);
        }

        public bool CompareAuthNumber(int recvNum)
        {
            if (this.number == recvNum)
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void SendMail(string userId)
        {
            MailMessage message = new MailMessage();
            message.From = new MailAddress("lcw3176@naver.com");
            message.To.Add(userId);
            message.Subject = "StrawberryTalk 인증입니다.";
            message.SubjectEncoding = Encoding.UTF8;

            message.Body = string.Format("StrawberryTalk 인증번호 입니다. {0}", number.ToString());
            message.BodyEncoding = Encoding.UTF8;

            SmtpClient smtp = new SmtpClient("smtp.naver.com", 587);
            smtp.EnableSsl = true;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            string serverId = Environment.GetEnvironmentVariable("ServerId", EnvironmentVariableTarget.User);
            string serverPw = Environment.GetEnvironmentVariable("ServerPw", EnvironmentVariableTarget.User);

            smtp.Credentials = new NetworkCredential(serverId, serverPw);
            smtp.Send(message);

        }
    }
}

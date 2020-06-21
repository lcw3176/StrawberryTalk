using StrawberryClient.Command;
using StrawberryClient.ViewModel;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace StrawberryClient.Model
{
    class LoginModel
    {

        private string userId;
        private string userPw = string.Empty;
        private StringBuilder serverPw = new StringBuilder();

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string UserPw
        {
            get
            {
                if (string.IsNullOrEmpty(userPw))
                {
                    return userPw;
                }

                else
                {
                    return new string('*', userPw.Length - 1) + userPw[userPw.Length - 1];
                }
            }

            set
            {
                // 유저가 비밀번호를 지울때
                if (userPw.Length > value.Length)
                {
                    serverPw.Remove(value.Length, userPw.Length - value.Length);
                    userPw = value;
                }

                // 유저가 비밀번호를 작성할 때
                else if (userPw.Length < value.Length)
                {
                    userPw = value;
                    serverPw.Append(UserPw[userPw.Length - 1]);
                }

            }
        }

        public LoginModel()
        {
            SocketConnection.GetInstance().Connect();
            SocketConnection.GetInstance().StartRecv();
            SocketConnection.GetInstance().LoginRecv += LoginRecv;
        }

        private void Detach()
        {
            SocketConnection.GetInstance().LoginRecv -= LoginRecv;
        }

        public void GoJoin()
        {
            Detach();
            UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
            update.Execute("Join");
        }

        private void LoginRecv(string param)
        {
            if (param == "false")
            {
                MessageBox.Show("다시 확인 바랍니다.");
            }

            else if (param == "already")
            {
                MessageBox.Show("다른 곳에서 접속 중입니다. 연결을 끊고 시도해 주세요.");
            }

            else if (param == "auth")
            {
                MessageBox.Show("인증받지 않은 아이디입니다. 이메일 인증을 진행해 주세요.");
                Detach();
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute("Auth");
            }

            else
            {
                Detach();
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute("Home");
            }

        }
      

        public void TryLogin()
        {
            SocketConnection.GetInstance().Send("Login", userId, serverPw.ToString());
        }

    }
}

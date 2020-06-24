using StrawberryClient.Command;
using StrawberryClient.Model.Enumerate;
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
            if(!SocketConnection.GetInstance().Connect())
            {
                MessageBox.Show("서버가 응답하지 않습니다. 잠시후 다시 시도해 주시기 바랍니다.");
                App.Current.Shutdown();
            }
            SocketConnection.GetInstance().StartRecv();
            SocketConnection.GetInstance().LoginRecv += Receive;
        }

        private void Detach()
        {
            SocketConnection.GetInstance().LoginRecv -= Receive;
        }

        public void GoJoin()
        {
            Detach();
            UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
            update.Execute("Join");
        }

        private void Receive(int cmd, string data)
        {
            if (cmd == (int)ResponseInfo.False)
            {
                MessageBox.Show("다시 확인 바랍니다.");
            }

            else if (cmd == (int)ResponseInfo.Already)
            {
                MessageBox.Show("다른 곳에서 접속 중입니다. 연결을 끊고 시도해 주세요.");
            }

            else if (cmd == (int)ResponseInfo.Auth)
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

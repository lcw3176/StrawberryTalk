using StrawberryClient.Command;
using StrawberryClient.Model.Enumerate;
using StrawberryClient.ViewModel;
using System.Text;
using System.Windows;

namespace StrawberryClient.Model
{
    class JoinModel
    {
        private string userId;
        private string userPw = string.Empty;
        private string userNickname;

        private StringBuilder serverPw = new StringBuilder();

        public string UserNickname
        {
            get { return userNickname; }
            set { userNickname = value; }
        }

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

        public JoinModel()
        {
            SocketConnection.GetInstance().JoinRecv += Receive;
        }
        
        private void Detach()
        {
            SocketConnection.GetInstance().JoinRecv -= Receive;
        }

        private void Receive(int cmd, string data)
        {
            if (cmd == (int)ResponseInfo.Email)
            {
                MessageBox.Show("이미 존재하는 계정입니다.");
            }

            else if(cmd == (int)ResponseInfo.Nickname)
            {
                MessageBox.Show("이미 존재하는 닉네임입니다.");
            }

            else
            {
                MessageBox.Show("가입 완료! 인증을 완료하셔야 이용 가능합니다.");
                Detach();
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute("Auth");
            }

        }

        public void TryJoin()
        {
            SocketConnection.GetInstance().Send("Join", userId, userNickname, serverPw.ToString());
        }

        public void GoBack()
        {
            Detach();
            UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
            update.Execute("Login");
        }


    }
}

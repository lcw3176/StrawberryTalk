using StrawberryClient.Command;
using StrawberryClient.Model.Enumerate;
using StrawberryClient.ViewModel;
using System.Windows;

namespace StrawberryClient.Model
{
    class AuthModel
    {
        private int authNumber;

        public int AuthNumber
        {
            get { return authNumber; }
            set { authNumber = value; }
        }

        public AuthModel()
        {
            SocketConnection.GetInstance().AuthRecv += Receive;
        }

        private void Detach()
        {
            SocketConnection.GetInstance().AuthRecv -= Receive;
        }

        private void Receive(int cmd, string data)
        {
            if(cmd == (int)ResponseInfo.True)
            {
                MessageBox.Show("인증이 완료되었습니다. 정상적인 이용이 가능합니다.");
                Detach();
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute("Login");
            }

            else if(cmd == (int)ResponseInfo.Ready)
            {
                MessageBox.Show("인증번호가 전송되었습니다. 인증을 진행해 주세요.");
            }

            else
            {
                MessageBox.Show("잘못된 번호입니다. 다시 한번 확인해 주세요.");
            }

        }

        public bool SetAuth()
        {
            SocketConnection.GetInstance().Send("Auth", "set");
            return true;
        }

        public bool GetAuth()
        {
            SocketConnection.GetInstance().Send("Auth", authNumber.ToString());
            return true;
        }
    }
}

using StrawberryClient.Command;
using StrawberryClient.Model;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace StrawberryClient.ViewModel
{
    class LoginViewModel : BaseViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand loginCommand { get; set; }
        public ICommand joinCommand { get; set; }
        private LoginModel loginModel;

        public LoginViewModel()
        {
            loginModel = new LoginModel();
            loginCommand = new RelayCommand(LoginExecuteMethod);
            joinCommand = new RelayCommand(joinExecuteMethod);
        }

        public string userId
        {
            get { return loginModel.UserId; }
            set
            {
                loginModel.UserId = value;
                OnPropertyUpdate("userId");
            }
        }

        public string userPw
        {
            get { return loginModel.UserPw; }
            set
            {
                loginModel.UserPw = value;
                OnPropertyUpdate("userPw");
            }
        }

        private void joinExecuteMethod(object obj)
        {
            UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
            update.Execute(obj);
        }

        private void LoginExecuteMethod(object obj)
        {
            if(string.IsNullOrEmpty(userId.Trim()) || string.IsNullOrEmpty(userPw.Trim())) 
            {
                MessageBox.Show("공백 입력은 허용되지 않습니다.");
                return; 
            }

            loginModel.Send();

            string result = loginModel.Login();

            if(result == "false")
            {
                MessageBox.Show("다시 확인 바랍니다.");
            }

            else if(result == "already")
            {
                MessageBox.Show("다른 곳에서 접속 중입니다. 연결을 끊고 시도해 주세요.");
            }

            else
            {
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute(obj, userId, result);
            }
        }

    }
}

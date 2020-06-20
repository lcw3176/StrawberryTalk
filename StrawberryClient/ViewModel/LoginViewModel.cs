using StrawberryClient.Command;
using StrawberryClient.Model;
using System.ComponentModel;
using System.Text.RegularExpressions;
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

            bool isMail = Regex.IsMatch(userId, @"(\w+\.)*\w+@(\w+\.)+[A-Za-z]+");

            if(!isMail)
            {
                MessageBox.Show("아이디는 이메일 형식이어야 합니다.");
                return;
            }


            loginModel.TryLogin();           
        }

    }
}

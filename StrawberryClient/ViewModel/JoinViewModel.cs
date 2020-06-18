using StrawberryClient.Command;
using StrawberryClient.Model;
using System.Windows;
using System.Windows.Input;

namespace StrawberryClient.ViewModel
{
    class JoinViewModel : BaseViewModel
    {
        public JoinModel model;
        public ICommand returnCommand { get; set; }
        public ICommand joinCommand { get; set; }

        public string userId
        {
            get { return model.UserId; }
            set
            {
                model.UserId = value;
                OnPropertyUpdate("userId");
            }
        }

        public string userPw
        {
            get { return model.UserPw; }
            set
            {
                model.UserPw = value;
                OnPropertyUpdate("userPw");
            }
        }


        public JoinViewModel()
        {
            model = new JoinModel();
            joinCommand = new RelayCommand(joinExecuteMethod);
            returnCommand = new RelayCommand(returnExecuteMethod);
        }

        private void joinExecuteMethod(object obj)
        {
            string result = model.TryJoin();

            if(result == "false")
            {
                MessageBox.Show("이미 존재하는 아이디 입니다.");
            }

            else
            {
                MessageBox.Show("가입 완료! 인증을 완료하셔야 이용 가능합니다.");
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute(obj);
            }
        }

        private void returnExecuteMethod(object obj)
        {
            UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
            update.Execute(obj);
        }
    }
}

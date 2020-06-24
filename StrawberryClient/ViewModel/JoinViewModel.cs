using StrawberryClient.Command;
using StrawberryClient.Model;
using System;
using System.Linq;
using System.Text.RegularExpressions;
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

        public string userNickname
        {
            get { return model.UserNickname; }
            set
            {
                model.UserNickname = value;
                OnPropertyUpdate("userNickname");
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
            bool isMail = Regex.IsMatch(userId, @"(\w+\.)*\w+@(\w+\.)+[A-Za-z]+");

            if(!isMail)
            {
                MessageBox.Show("아이디는 이메일 형식이어야 합니다.");
                return;
            }

            if(string.IsNullOrEmpty(userNickname.Trim()) || string.IsNullOrEmpty(userId.Trim()) || string.IsNullOrEmpty(userPw.Trim()))
            {
                MessageBox.Show("공백입력은 허용되지 않습니다.");
                return;
            }

            if(userNickname.Trim().Length > 10 || Regex.IsMatch(userNickname.Trim(), @"[ㄱ-ㅎ가힣]") || userNickname.Any(ch => Char.IsLetter(ch)))
            {
                MessageBox.Show("닉네임은 10글자 이하 영어만 허용됩니다.");
                return;
            }

            model.TryJoin();          
        }

        private void returnExecuteMethod(object obj)
        {
            model.GoBack();
        }
    }
}

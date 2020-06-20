using StrawberryClient.Command;
using StrawberryClient.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StrawberryClient.ViewModel
{
    class AuthViewModel : BaseViewModel
    {

        public ICommand comfirmCommand { get; set; }
        public ICommand reAuthCommand { get; set; }
        AuthModel model;
        
        public int authNumber
        {
            get { return model.AuthNumber; }
            set
            {
                model.AuthNumber = value;
                OnPropertyUpdate("autuNumber");
            }
        }

        public AuthViewModel()
        {
            comfirmCommand = new RelayCommand(confirmExecuteMethod);
            reAuthCommand = new RelayCommand(reAuthExecuteMethod);
            model = new AuthModel();
            model.SetAuth();
        }
        
        // 인증 재요청
        private void reAuthExecuteMethod(object obj)
        {
            model.SetAuth();
        }

        // 인증 확인 요청
        private void confirmExecuteMethod(object obj)
        {
            model.GetAuth();
        }
    }
}

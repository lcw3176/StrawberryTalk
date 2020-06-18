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
            if(model.SetAuth())
            {
                MessageBox.Show("재전송 되었습니다. 메일을 확인해 주세요");
            }

            else
            {
                MessageBox.Show("요청이 거부되었습니다. 잠시 후에 다시 시도해 주세요.");
            }
        }

        // 인증 확인 요청
        private void confirmExecuteMethod(object obj)
        {
            if(model.GetAuth())
            {
                MessageBox.Show("인증이 완료되었습니다. 정상적인 이용이 가능합니다.");
                UpdateViewCommand update = MainViewModel.GetInstance().updateViewCommand as UpdateViewCommand;
                update.Execute(obj);
            }

            else
            {
                MessageBox.Show("잘못된 번호입니다. 다시 한번 확인해 주세요.");
            }
        }
    }
}

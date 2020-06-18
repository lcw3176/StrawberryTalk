using StrawberryClient.ViewModel;
using System;
using System.Windows.Input;

namespace StrawberryClient.Command
{
    class UpdateViewCommand : ICommand
    {
        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
            this.viewModel.SelectedViewModel = new LoginViewModel();
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }


        public void Execute(object parameter)
        {
            if(parameter.ToString() == "Join")
            {
                JoinViewModel join = new JoinViewModel();
                viewModel.SelectedViewModel = join;
            }

            if(parameter.ToString() == "Login")
            {
                LoginViewModel login = new LoginViewModel();
                viewModel.SelectedViewModel = login;
            }

            if(parameter.ToString() == "Auth")
            {
                AuthViewModel auth = new AuthViewModel();
                viewModel.SelectedViewModel = auth;
            }
        }

        public void Execute(object parameter, string userId, string result)
        {
            
            if (parameter.ToString() == "Home")
            {
                HomeViewModel home = new HomeViewModel();
                home.Init(userId, result);
                viewModel.SelectedViewModel = home;
            }
        }


    }
}

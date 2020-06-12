using StrawberryClient.Command;
using System.Windows.Input;

namespace StrawberryClient.ViewModel
{
    class MainViewModel : BaseViewModel
    {
        private BaseViewModel _selectedViewModel;
        public static MainViewModel Instance;

        public BaseViewModel SelectedViewModel
        {
            get { return _selectedViewModel; }
            set
            {
                _selectedViewModel = value;
                OnPropertyUpdate(nameof(SelectedViewModel));
            }
        }

        public ICommand updateViewCommand { get; set; }

        public MainViewModel()
        {
            updateViewCommand = new UpdateViewCommand(this);
        }

        public static MainViewModel GetInstance()
        {
            if(Instance == null)
            {
                Instance = new MainViewModel();
            }

            return Instance;
        }

    }
}

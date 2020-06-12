using StrawberryClient.ViewModel;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.Model.ObservableCollection
{
    class Friends : BaseViewModel
    {
        private ImageSource FriendImage;

        public ICommand chatCommand { get; set; }
        public ICommand checkCommand { get; set; }
        public string friendsName { get; set; }
        public string friendsStatus { get; set; }
        public ImageSource friendsImage
        {
            get { return FriendImage; }
            set
            {
                FriendImage = value;
                OnPropertyUpdate("friendsImage");
            }
        }

        public ICommand viewProfileCommand { get; set; }
    }
}

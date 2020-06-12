using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.Model.ObservableCollection
{
    class ChatRooms
    {
        public string roomName { get; set; }
        public ICommand roomCommand { get; set; }
        public ImageSource roomImage { get; set; }
    }
}

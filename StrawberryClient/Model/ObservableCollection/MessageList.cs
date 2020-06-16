using System.Windows.Media;

namespace StrawberryClient.Model.ObservableCollection
{
    class MessageList
    {
        public string userName { get; set; }
        public string message { get; set; }
        public bool isMe { get; set; }
        //public bool sameBefore { get; set; }
        public ImageSource profileImage { get; set; }
    }
}

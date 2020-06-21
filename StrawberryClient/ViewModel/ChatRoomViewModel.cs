using StrawberryClient.Command;
using StrawberryClient.Model;
using StrawberryClient.Model.ObservableCollection;
using StrawberryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.ViewModel
{
    class ChatRoomViewModel : INotifyPropertyChanged
    {
        public delegate void Close(string room);
        public event Close closeEvent;

        public event PropertyChangedEventHandler PropertyChanged;
        public ChatRoomModel chatRoomModel;
        ChatRoomView roomView;
        public ScrollViewer scroll { get; set; }
        public ICommand sendMessageCommand { get; set; }
        public ICommand closeCommand { get; set; }


        public ObservableCollection<MessageList> messageList
        {
            get { return chatRoomModel.MessageList; }
            set
            { 
                chatRoomModel.MessageList = value;
                OnPropertyUpdate("messageList");
            }
        }

        public string userId
        {
            get { return chatRoomModel.UserId; }
            set { chatRoomModel.UserId = value; }
        }

        public string inputMessage
        {
            get { return chatRoomModel.InputMessage; }
            set
            {
                chatRoomModel.InputMessage = value;
                OnPropertyUpdate("inputMessage");
            }
        }

        public string roomName
        {
            get { return chatRoomModel.RoomName; }
            set { chatRoomModel.RoomName = value; }
        }

        public string showedRoomName
        {
            get { return chatRoomModel.ShowedRoomName; }
            set
            {
                chatRoomModel.ShowedRoomName = value;
                OnPropertyUpdate("showedRoomName");
            }
        }

        public ImageSource profileImage
        {
            get { return chatRoomModel.ProfileImage; }
            set
            {
                chatRoomModel.ProfileImage = value;
                OnPropertyUpdate("profileImage");
            }
        }

        public Dictionary<string, ImageSource> friendsImage
        {
            get { return chatRoomModel.FriendsImage; }
            set
            {
                chatRoomModel.FriendsImage = value;
            }
        }


        public ChatRoomViewModel()
        {
            chatRoomModel = new ChatRoomModel();
            sendMessageCommand = new RelayCommand(sendExecuteMethod);
            closeCommand = new RelayCommand(closeExecuteMethod);

            chatRoomModel.update += NotifyUpdate;
            chatRoomModel.move += MoveScrollToMiddle;
        }

        public void AttachSocket()
        {
            chatRoomModel.AttachAlarm();
        }

        public void DetachSocket()
        {
            chatRoomModel.DetachAlarm();
        }
        double height;

        // 스크롤 끝까지 당겼을 때 메세지 추가 요청
        public void scrollEnd(double height)
        {
            this.height = height;
            chatRoomModel.MoreMessage();
        }

 

        // 메세지 추가 로딩 후 스크롤 위치 이동
        private void MoveScrollToMiddle()
        {
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                scroll.ScrollToVerticalOffset(this.height);
            });
        }

        private void NotifyUpdate(string propertyName)
        {
            OnPropertyUpdate(propertyName);
        }


        // 메세지 보내기
        private void sendExecuteMethod(object obj)
        {
            if (string.IsNullOrEmpty(inputMessage.Trim())) { return; }
            chatRoomModel.Send();
            scroll.ScrollToEnd();
            inputMessage = string.Empty;
        }


        // 초기화
        public void Init(string roomName, string userId, string showedRoomName, ImageSource image, Dictionary<string, ImageSource> friendsImage)
        {
            AttachSocket();
            this.roomName = roomName;
            this.userId = userId;
            this.showedRoomName = showedRoomName;
            profileImage = image;
            this.friendsImage = friendsImage;

            SocketConnection.GetInstance().Send("Room", roomName);

            roomView = new ChatRoomView();
            roomView.DataContext = this;
            this.scroll = roomView.scrollView;
            roomView.endOfScroll += scrollEnd;

            roomView.Show();           
        }


        // 종료
        private void closeExecuteMethod(object obj)
        {
            DetachSocket();
            roomView.endOfScroll -= scrollEnd;
            chatRoomModel.update -= NotifyUpdate;
            chatRoomModel.move -= MoveScrollToMiddle;
            closeEvent(roomName);
            (obj as Window).Close();
        }


        private void OnPropertyUpdate(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}

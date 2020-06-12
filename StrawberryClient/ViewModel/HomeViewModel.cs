using StrawberryClient.Command;
using StrawberryClient.Model;
using StrawberryClient.Model.ObservableCollection;
using StrawberryClient.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.ViewModel
{
    class HomeViewModel : BaseViewModel
    {

        public event PropertyChangedEventHandler PropertyChanged;
        public HomeModel homeModel;
        public ICommand findUserCommand { get; set; }
        public ICommand setProfileCommand { get; set; }
        public ICommand addChatCommand { get; set; }

        public string findUser
        {
            get { return homeModel.FindUser; }
            set
            {
                homeModel.FindUser = value;
                OnPropertyUpdate("findUser");
            }
        }

        public ICommand chatCommand
        {
            get { return homeModel.ChatCommand; }
            set
            {
                homeModel.ChatCommand = value;
            }
        }

        public ICommand viewProfileCommand
        {
            get { return homeModel.ViewProfileCommand; }
            set { homeModel.ViewProfileCommand = value; }
        }


        public string userId
        {
            get { return homeModel.UserId; }
            set
            {
                homeModel.UserId = value;
                OnPropertyUpdate("userId");
            }
        }

        public string userStatus
        {
            get { return homeModel.UserStatus; }
            set
            {
                homeModel.UserStatus = value;
                OnPropertyUpdate("userStatus");
            }
        }

        public ImageSource userImage
        {
            get { return homeModel.UserImage; }
            set
            {
                homeModel.UserImage = value;
                OnPropertyUpdate("userImage");
            }
        }

        public ObservableCollection<Friends> friendsList
        {
            get { return homeModel.FriendsList; }
            set
            {
                homeModel.FriendsList = value;
                OnPropertyUpdate("friendsList");
            }
        }

        public ObservableCollection<ChatRooms> chatRoomsList
        {
            get { return homeModel.ChatRoomsList; }
            set
            {
                homeModel.ChatRoomsList = value;
                OnPropertyUpdate("chatRoomsList");
            }
        }

        public HomeViewModel()
        {
            homeModel = new HomeModel();
            homeModel.changed += new HomeModel.Changed(OnPropertyUpdate);
            chatCommand = new RelayCommand(chatExecuteMethod);
            findUserCommand = new RelayCommand(findUserExecuteMethod);
            setProfileCommand = new RelayCommand(setProfileExecuteMethod);
            viewProfileCommand = new RelayCommand(viewProfileExecuteMethod);
            addChatCommand = new RelayCommand(addChatExecuteMethod);
        }


        // 채팅방 만들기(개인 톡, 단체 톡)
        private void addChatExecuteMethod(object obj)
        {
            AddChatViewModel addChatViewModel =  new AddChatViewModel();
            addChatViewModel.onClose += new AddChatViewModel.Close(chatExecuteMethod);
            addChatViewModel.addChatList = friendsList;

            addChatView addChat = new addChatView() { DataContext = addChatViewModel };
            addChat.Show();
        }

        // 친구 프로필 사진 보기
        private void viewProfileExecuteMethod(object obj)
        {
            string viewUser =  (obj as TextBlock).Text;

            SetProfileViewModel viewModel = new SetProfileViewModel();
            viewModel.userId = "not";
            viewModel.profileImage = friendsList.FirstOrDefault(e => e.friendsName == viewUser).friendsImage;
            SetProfileView setProfile = new SetProfileView()
            {
                DataContext = viewModel,
            };
            setProfile.Show();
        }

        // 내 프로필 사진 설정
        private void setProfileExecuteMethod(object obj)
        {
            SetProfileViewModel viewModel = new SetProfileViewModel();
            viewModel.userId = this.userId;

            viewModel.profileImage = userImage;
            SetProfileView setProfile = new SetProfileView()
            {
                DataContext = viewModel,
            };
            setProfile.Show();
        }

        // 유저 검색 커맨드
        private void findUserExecuteMethod(object obj)
        {
            homeModel.GetUser();
            findUser = string.Empty;
        }

        // 채팅 시작 커맨드
        private void chatExecuteMethod(object obj)
        {

            string friendName;
            string roomName;

            // 단톡 만들때(addChatView에서 선택시 실행)
            if ((obj as TextBlock) == null)
            {
                friendName = obj.ToString().Replace("@", ",");
            }

            // 갠톡 만들때(homeView에서 직접 클릭시 실행)
            else
            {
                friendName = (obj as TextBlock).Text;
            }

            if (string.IsNullOrEmpty(friendName)) { return; }

            if (homeModel.isExist(friendName)) { return; }

            roomName = homeModel.SetChat(friendName.Replace(",", "@"));

            homeModel.addRooms(friendName);

            ShowRoom(friendName, roomName);
        }

        // 채팅창 띄우기
        private void ShowRoom(string friendName, string roomName)
        {
            ChatRoomViewModel roomViewModel = new ChatRoomViewModel();
            roomViewModel.closeEvent += new ChatRoomViewModel.Close(homeModel.Detach);

            string name;

            ImageSource image = friendsList.FirstOrDefault(e => e.friendsName == friendName.Split(',')[0]).friendsImage;
            Dictionary<string, ImageSource> friendsImage = new Dictionary<string, ImageSource>();

            for (int i = 0; i < friendName.Split(',').Length; i++)
            {
                name = friendName.Split(',')[i];
                friendsImage.Add(name, friendsList.FirstOrDefault(e => e.friendsName == name).friendsImage);
            }
             
            roomViewModel.AttachSocket();
            roomViewModel.Init(roomName, userId, friendName, image, friendsImage);                     
        }



        // 초기화
        public void SetInfo(string friendsId)
        {
            homeModel.Init(friendsId);
        }
    }
}

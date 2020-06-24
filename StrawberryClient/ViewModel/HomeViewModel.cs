using StrawberryClient.Command;
using StrawberryClient.Model;
using StrawberryClient.Model.ObservableCollection;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.ViewModel
{
    class HomeViewModel : BaseViewModel
    {
        public HomeModel homeModel;
        public ICommand findUserCommand { get; set; }
        public ICommand setProfileCommand { get; set; }
        public ICommand addChatCommand { get; set; }
        public ICommand refreshCommand { get; set; }

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
            chatCommand = new RelayCommand(chatExecuteMethod);
            findUserCommand = new RelayCommand(findUserExecuteMethod);
            setProfileCommand = new RelayCommand(setProfileExecuteMethod);
            viewProfileCommand = new RelayCommand(viewProfileExecuteMethod);
            addChatCommand = new RelayCommand(addChatExecuteMethod);
            refreshCommand = new RelayCommand(refreshExecuteMethod);

            homeModel.changed += OnPropertyUpdate;

            SocketConnection.GetInstance().Send("Success", "null");
        }

        // 새로고침 버튼
        private void refreshExecuteMethod(object obj)
        {
            homeModel.Refresh();
        }

        // 채팅방 만들기(개인 톡, 단체 톡)
        private void addChatExecuteMethod(object obj)
        {
            AddChatViewModel addChatViewModel =  new AddChatViewModel();
            addChatViewModel.onClose += chatExecuteMethod;
            addChatViewModel.Init(friendsList);
        }

        // 친구 프로필 사진 보기
        private void viewProfileExecuteMethod(object obj)
        {
            string viewUser =  (obj as TextBlock).Text;

            SetProfileViewModel viewModel = new SetProfileViewModel();
            viewModel.Init("not", friendsList.FirstOrDefault(e => e.friendsName == viewUser).friendsImage);
        }

        // 내 프로필 사진 설정
        private void setProfileExecuteMethod(object obj)
        {
            SetProfileViewModel viewModel = new SetProfileViewModel();
            viewModel.Init(userId, userImage);
        }

        // 유저 검색 커맨드
        private void findUserExecuteMethod(object obj)
        {
            homeModel.GetUser(findUser);
            this.findUser = string.Empty;
        }

        // 채팅 시작 커맨드
        private void chatExecuteMethod(object obj)
        {
            string showedRoomName;

            // 단톡 만들때(addChatView에서 선택시 실행)
            if ((obj as TextBlock) == null)
            {
                showedRoomName = obj.ToString();
            }

            // 갠톡 만들때(homeView에서 직접 클릭시 실행)
            else
            {
                showedRoomName = (obj as TextBlock).Text;
            }

            if(homeModel.IsFriend(showedRoomName))
            {
                homeModel.ShowRoom(showedRoomName);
            }
            
        }

    }
}

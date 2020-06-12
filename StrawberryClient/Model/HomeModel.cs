using StrawberryClient.Model.ObservableCollection;
using StrawberryClient.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Windows.Input;
using System.Windows.Media;

namespace StrawberryClient.Model
{
    class HomeModel
    {
        public delegate void Changed(string name);
        public event Changed changed;

        private string userId;
        private string userStatus;
        private ImageSource userImage;
        private ICommand chatCommand;
        private ICommand viewProfileCommand;
        private string findUser;
        private List<string> activateRoom = new List<string>();
        ObservableCollection<Friends> friendsList = new ObservableCollection<Friends>();
        ObservableCollection<ChatRooms> chatRoomsList = new ObservableCollection<ChatRooms>();
        List<ChatRoomView> roomViews = new List<ChatRoomView>();
        

        public string FindUser
        {
            get { return findUser; }
            set { findUser = value; }
        }

        public ICommand ViewProfileCommand
        {
            get { return viewProfileCommand; }
            set { viewProfileCommand = value; }
        }

        public ICommand ChatCommand
        {
            get { return chatCommand; }
            set { chatCommand = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string UserStatus
        {
            get { return userStatus; }
            set { userStatus = value; }
        }

        public ImageSource UserImage
        {
            get { return userImage; }
            set { userImage = value; }
        }

        public ObservableCollection<Friends> FriendsList
        {
            get { return friendsList; }
            set { friendsList = value; }
        }

        public ObservableCollection<ChatRooms> ChatRoomsList
        {
            get { return chatRoomsList; }
            set { chatRoomsList = value; }
        }


        public HomeModel()
        {
            SocketConnection.GetInstance().StartRecv();
            SocketConnection.GetInstance().Recv += new SocketConnection.Receive(Receive);
        }

        // 채팅방 존재 여부 확인
        public bool isExist(string friendNames)
        {
            List<string> sortName = new List<string>();
            sortName.Add(userId);

            foreach(string i in friendNames.Split(','))
            {
                sortName.Add(i);
            }

            sortName.Sort();

            string roomName = string.Join("&", sortName);

            foreach (string i in activateRoom)
            {
                if(i == roomName)
                {
                    return true;
                }
            }

            activateRoom.Add(roomName);

            return false;
        }

        // 채팅방 만들기(서버로 정보 전송, 활성화 된 방 목록에 추가)
        public string SetChat(string friendsName)
        {
            List<string> sortName = new List<string>();
            sortName.Add(userId);

            foreach(string i in friendsName.Split('@'))
            {
                sortName.Add(i);
            }
            
            sortName.Sort();

            string roomName = string.Join("&", sortName);

            SocketConnection.GetInstance().Send("Chat", roomName);

            return roomName;
        }

        // 유저 검색으로 찾은 후 추가
        public void GetUser()
        {
            if (string.IsNullOrEmpty(findUser) || findUser == this.userId) { return; }

            try
            {
                if(friendsList.First(e => e.friendsName == findUser).friendsName == findUser)
                {
                    return;
                }
            }

            catch
            {
                SocketConnection.GetInstance().Send("GetUser", userId, findUser);
                return;
            }

            SocketConnection.GetInstance().Send("GetUser", userId, findUser);
        }

        // 활성 채팅방 해제
        public void Detach(string roomName)
        {
            activateRoom.Remove(roomName);
        }


        // 경로 Search(유저 검색), Chat(채팅 시작), Change(프로필 설정 교체), Alarm(메세지 도착 감시)
        private void Receive(string param)
        {
            // 유저 검색했을 때
            if(param.Substring(0, 6) == "<FIND>")
            {
                string name = param.Substring(6, param.Length - 6);
                addFriend(name);
            }

            // 프로필 사진 교체 시, 새로고침 눌렀을 때
            else if(param.Substring(0, 6) == "<RFRH>")
            {
                SocketConnection.GetInstance().isRun = false;
                UserImage = GetImage(userId);

                changed("userImage");

                DispatcherService.Invoke((System.Action)(() =>
                {
                    for(int i = 0;i < friendsList.Count; i++)
                    {
                        friendsList[i].friendsImage = GetImage(friendsList[i].friendsName);
                    }

                    changed("friendsList");
                }));


                SocketConnection.GetInstance().isRun = true;
            }

            // 다른 사람한테서 채팅 왔을 때
            else
            {
                string sendRoomName = param.Split(new string[] { "<AND>" }, StringSplitOptions.None)[0];

                if (activateRoom.Count == 0)
                {
                    alarm(param);
                    return;
                }

                foreach (string i in activateRoom)
                {
                    if (i != sendRoomName)
                    {
                        alarm(param);
                    }

                }
            }
        }

        // 친구 목록 추가
        private void addFriend(string name)
        {
            if (name == "None") { return; }
            SocketConnection.GetInstance().isRun = false;

            DispatcherService.Invoke((System.Action)(() =>
            {
                friendsList.Add(new Friends()
                {
                    chatCommand = chatCommand,
                    friendsName = name,
                    friendsStatus = name,
                    friendsImage = GetImage(name),
                    viewProfileCommand = viewProfileCommand,
                });
            }));

            SocketConnection.GetInstance().isRun = true;

        }

        // 채팅방 목록 추가
        public void addRooms(string name)
        {
            SocketConnection.GetInstance().isRun = false;
            var item = chatRoomsList.FirstOrDefault(e => e.roomName == name);

            // 중복 추가 방지
            if (item == null)
            {
                DispatcherService.Invoke((System.Action)(() =>
                {
                    chatRoomsList.Add(new ChatRooms()
                    {
                        roomCommand = chatCommand,
                        roomName = name,
                        roomImage = GetImage(name.Split(',')[0]), // 단톡 빵꾸 코드
                    });
                }));
            }


            SocketConnection.GetInstance().isRun = true;
        }

        // 알람
        private void alarm(string param)
        {
            // 채팅방 처음 만들어 졌을때(유저끼리 처음 연결됬을때) 리턴
            if(param == "<FIRST>") { return; }

            string data = param.Split(new string[] { "<AND>" }, StringSplitOptions.None)[1];

            if (string.IsNullOrEmpty(data)) { return; }

            string[] result;
            string[] recvData = data.Split('&'); //[0] 이름, [1] 메세지

            var isExist = activateRoom.FirstOrDefault(e => e.Contains(recvData[0]));

            if(isExist != null) { return; }

            // [0] 이름, [1] 메세지
            if (recvData[0] != UserId && recvData.Length <= 2 && data.Substring(data.Length - 6, 6) != "<MADE>")
            {
                result = data.Replace("<CHAT>", string.Empty).Split('&');

                for (int i = 0; i < result.Length; i += 2)
                {
                    DispatcherService.Invoke((System.Action)(() =>
                    {
                        AlarmView alarmView = new AlarmView(result[i], result[i + 1]);
                        alarmView.Topmost = true;
                        alarmView.Show();
                    }));
                }
            }
        }

        // 유저 초기 세팅
        public void Init(string data)
        {
            if (string.IsNullOrEmpty(data)) { return; }

            SocketConnection.GetInstance().isRun = false;

            // 친구 목록이 비었을 때
            if (data == "<NEXT>")
            {
                UserImage = GetImage(userId);
                SocketConnection.GetInstance().isRun = true;
                return;
            }

            string[] usersData = data.Split(new string[] { "<NEXT>" }, StringSplitOptions.None);
            string[] friendsName = usersData[0].Split(',');
            string[] roomName = usersData[1].Split(',');

            // 내 프사 추가
            UserImage = GetImage(userId);

            if(!string.IsNullOrEmpty(friendsName[0]))
            {
                // 친구 목록 추가
                foreach (string i in friendsName)
                {
                    friendsList.Add(new Friends()
                    {
                        chatCommand = chatCommand,
                        friendsName = i,
                        friendsImage = GetImage(i),
                        viewProfileCommand = viewProfileCommand,
                    });
                }

            }

            if(!string.IsNullOrEmpty(roomName[0]))
            {
                List<string> filteredRoomName = new List<string>();
                string tempRoomName;

                // 채팅방 목록 추가
                foreach (string room in roomName)
                { 
                    foreach(string name in room.Split('&'))
                    {
                        // 채팅방 이름에서 유저 닉네임은 제거
                        if(name != this.UserId)
                        {
                            filteredRoomName.Add(name);
                        }
                    }

                    tempRoomName = string.Join(",", filteredRoomName);
                    

                    chatRoomsList.Add(new ChatRooms()
                    {
                        roomName = tempRoomName,
                        roomImage = GetImage(tempRoomName),  // 단톡 이미지 빵꾸 코드
                        roomCommand = chatCommand,
                    });

                    filteredRoomName.Clear();
                }
            }
            
            SocketConnection.GetInstance().isRun = true;
        }


        // 프로필 이미지 가져오기
        public ImageSource GetImage(string userId)
        {
            SocketConnection.GetInstance().Send("GetImage", userId);

            byte[] byteImage = SocketConnection.GetInstance().ImageReceive();
            ImageSourceConverter c = new ImageSourceConverter();

            using (MemoryStream inStream = new MemoryStream(byteImage))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    Image tempImage = Image.FromStream(inStream);
                    tempImage.Save(outStream, ImageFormat.Jpeg);
                    tempImage.Dispose();

                    return (ImageSource)c.ConvertFrom(outStream.ToArray());
                }
            }

        }
    }
}

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
using System.Net.WebSockets;
using StrawberryClient.Model.Enumerate;

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
        List<string> imageList = new List<string>();
        

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
            SocketConnection.GetInstance().HomeRecv += HomeRecv;
            SocketConnection.GetInstance().imageRecv += imageRecv;
        }


        private void HomeRecv(int cmd, string data)
        {
            // 초기 세팅
            if(cmd == (int)ResponseInfo.Init)
            {
                Console.WriteLine(data);
                this.userId = data.Split('/')[0];
                GetImage(userId, CollectionName.UserImage);

                data = data.Remove(0, userId.Length + 1);

                string[] friends = data.Split('/')[0].Split(',');
                // 친구 목록이 비어있지 않았을 때
                if (!string.IsNullOrEmpty(friends[0]))
                {
                    App.Current.Dispatcher.Invoke((Action)delegate 
                    {
                        foreach (string name in friends)
                        {
                            friendsList.Add(new Friends()
                            {
                                chatCommand = chatCommand,
                                friendsName = name,
                                viewProfileCommand = viewProfileCommand,
                            });

                            GetImage(name, CollectionName.friendsList);
                            data = data.Remove(0, name.Length + 1);
                        }
                    });

                }

                // 친구 목록 비었을 때
                else
                {
                    data = data.Remove(0, 1);
                }

                string[] rooms = data.Split('/')[0].Split(',');

                if (!string.IsNullOrEmpty(rooms[0]))
                {
                    List<string> tempRoomName = new List<string>();
                    string filteredRoomName;

                    App.Current.Dispatcher.Invoke((Action)delegate 
                    { 
                        // 채팅방 목록 추가
                        foreach (string room in rooms)
                        {
                            foreach (string name in room.Split('&'))
                            {
                                // 채팅방 이름에서 유저 닉네임은 제거
                                if (name != this.UserId)
                                {
                                    tempRoomName.Add(name);
                                }
                            }

                            filteredRoomName = string.Join(",", tempRoomName);

                            chatRoomsList.Add(new ChatRooms()
                            {
                                roomName = filteredRoomName,
                                roomCommand = chatCommand,
                            });

                            GetImage(filteredRoomName, CollectionName.chatRoomList);
                            tempRoomName.Clear();
                        }
                    });
                }
            }

            // 유저 검색했을 때
            if (cmd == (int)ResponseInfo.Find)
            {
                addFriend(data);
            }

            // 프로필 사진 교체 시
            if (cmd == (int)ResponseInfo.Refresh)
            {
                Refresh();
            }

            // 다른 사람한테 채팅 왔을 때
            if(cmd == (int)ResponseInfo.Chat)
            {
                string room = data.Split('/')[0];
                string message = data.Remove(0, room.Length + 1);

                if (activateRoom.Count == 0)
                {
                    alarm(message);
                }

                else
                {

                    foreach (string i in activateRoom)
                    {
                        if (i != room)
                        {
                            alarm(message);
                        }

                    }
                }                
            }

            changed("userId");
            changed("friendsList");
            changed("chatRoomsList");
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

        // 채팅방 만들기(활성화 된 방 목록에 추가)
        public string SetChat(string friendsName)
        {
            List<string> sortName = new List<string>();
            sortName.Add(userId);

            foreach(string i in friendsName.Split(','))
            {
                sortName.Add(i);
            }            
            sortName.Sort();

            string roomName = string.Join("&", sortName);

            return roomName;
        }

        // 유저 검색으로 찾은 후 추가
        public void GetUser(string findUser)
        {
            if (string.IsNullOrEmpty(findUser) || findUser == this.userId) { return; }

            var isRegistered = friendsList.FirstOrDefault(e => e.friendsName == findUser);

            if (isRegistered != null)
            {
                return;
            }

            SocketConnection.GetInstance().Send("User", findUser);
        }

        // 활성 채팅방 해제
        public void Detach(string roomName)
        {
            activateRoom.Remove(roomName);
        }

        // 친구 목록 추가
        private void addFriend(string name)
        {
            if (name == "None") { return; }

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                friendsList.Add(new Friends()
                {
                    chatCommand = chatCommand,
                    friendsName = name,
                    viewProfileCommand = viewProfileCommand,
                });
            });

            GetImage(name, CollectionName.friendsList);
        }

        // 채팅방 목록 추가
        public void addRooms(string name)
        {
            var item = chatRoomsList.FirstOrDefault(e => e.roomName == name);

            // 중복 추가 방지
            if (item == null)
            {
                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    chatRoomsList.Add(new ChatRooms()
                    {
                        roomCommand = chatCommand,
                        roomName = name,
                    });
                });

                GetImage(name, CollectionName.chatRoomList);
            }

        }

        // 전체 이미지 새로고침
        public void Refresh()
        {
            GetImage(userId, CollectionName.UserImage);

            App.Current.Dispatcher.Invoke((Action)delegate
            {
                for (int i = 0; i < friendsList.Count; i++)
                {
                    GetImage(friendsList[i].friendsName, CollectionName.friendsList);
                }
            });
        }

        // 알람
        private void alarm(string data)
        {
            string name = data.Split('&')[0];
            string message = data.Split('&')[1];
            App.Current.Dispatcher.Invoke((Action)delegate
            {
                AlarmView alarmView = new AlarmView(name, message);
                alarmView.Topmost = true;
                alarmView.Show();
            });
            
        }


        // 프로필 이미지 가져오기
        public void GetImage(string userId, CollectionName collectionName)
        {
            imageList.Add(userId + "/" + collectionName.ToString());

            SocketConnection.GetInstance().Send("Image", userId);
        }

        // 이미지 받아서 처리
        private void imageRecv(string id, byte[] image)
        {
            using (MemoryStream inStream = new MemoryStream(image, 14, image.Length - 14))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    Image tempImage = Image.FromStream(inStream);
                    tempImage.Save(outStream, ImageFormat.Jpeg);
                    tempImage.Dispose();

                    string userId = id.Replace("&", string.Empty).Trim();
                    string collection = imageList.Find(e => e.Contains(userId)).Split('/')[1];

                    ImageSourceConverter c = new ImageSourceConverter();

                    if (collection == CollectionName.UserImage.ToString())
                    {
                        UserImage = (ImageSource)c.ConvertFrom(outStream.ToArray());
                        changed("userImage");
                    }

                    else if (collection == CollectionName.friendsList.ToString())
                    {
                        friendsList.FirstOrDefault(e => e.friendsName == userId).friendsImage = (ImageSource)c.ConvertFrom(outStream.ToArray());
                        changed("friendsList");
                    }

                    else if (collection == CollectionName.chatRoomList.ToString())
                    {
                        chatRoomsList.FirstOrDefault(e => e.roomName == userId).roomImage = (ImageSource)c.ConvertFrom(outStream.ToArray());
                        changed("chatRoomsList");
                    }

                    imageList.Remove(userId + "/" + collection);

                }
            }
        }
    }
}

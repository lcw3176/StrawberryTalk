using StrawberryClient.Model.ObservableCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows.Media;

namespace StrawberryClient.Model
{
    class ChatRoomModel
    {
        public delegate void Update(string propName);
        public event Update update;

        public delegate void MoveScroll();
        public event MoveScroll move;

        private string roomName;
        private string showedRoomName;
        private string userId;
        private string inputMessage = string.Empty;
        private int pageNation = 1;
        private ImageSource profileImage;
        private Dictionary<string, ImageSource> friendsImage = new Dictionary<string, ImageSource>();
        ObservableCollection<MessageList> messageList = new ObservableCollection<MessageList>();
        enum ChatInfo { Init, First, Plus, Chat }
        public Dictionary<string, ImageSource> FriendsImage
        {
            get { return friendsImage; }
            set { friendsImage = value; }
        }

        public ObservableCollection<MessageList> MessageList
        {
            get { return messageList; }
            set { messageList = value; }
        }

        public ImageSource ProfileImage
        {
            get { return profileImage; }
            set { profileImage = value; }
        }

        public string RoomName
        {
            get { return roomName; }
            set { roomName = value; }
        }

        public string ShowedRoomName
        {
            get { return showedRoomName; }
            set { showedRoomName = value; }
        }

        public string InputMessage
        {
            get { return inputMessage; }
            set { inputMessage = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }
        
        public ChatRoomModel()
        {

        }

        public void AttachAlarm()
        {
            SocketConnection.GetInstance().ChatRecv += ChatRecv;
        }

        public void DetachAlarm()
        {
            SocketConnection.GetInstance().ChatRecv -= ChatRecv;
            MessageList = null;
            FriendsImage = null;
        }

        string isSame = string.Empty;

        private void ChatRecv(int cmd, string data)
        {
            //중복 제거
            if(cmd == (int)ChatInfo.First)
            {
                return;
            }

            if (cmd == (int)ChatInfo.Chat)
            {
                string room = data.Split('/')[0];
                string messageChunk = data.Remove(0, room.Length + 1);

                if (room == this.roomName)
                {
                    string userName = messageChunk.Split('&')[0];
                    string message = messageChunk.Split('&')[1];

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        // is Me
                        if (userId == userName)
                        {
                            MessageList.Add(new MessageList()
                            {
                                userName = userName,
                                message = message,
                                isMe = true,
                                sameBefore = (isSame == userName),
                            });
                        }

                        else
                        {
                            MessageList.Add(new MessageList()
                            {
                                userName = userName,
                                message = message,
                                isMe = false,
                                sameBefore = (isSame == userName),
                                profileImage = friendsImage[userName],
                            });
                        }
                    });

                    isSame = userName;
                }

            }

            // 채팅방 초기화, 첫 메세지 세팅
            if (cmd == (int)ChatInfo.Init)
            {
                string room = data.Split('/')[0];
                string messageChunk = data.Remove(0, room.Length + 1);


                if (room == this.RoomName)
                {
                    string[] msg = messageChunk.Split('&');
                    string[] temp;

                    // [0] 이름, [1] 메세지
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        for (int i = 0; i < msg.Length; i++)
                        {
                            temp = msg[i].Split(',');

                            // is Me
                            if (userId == temp[0])
                            {
                                MessageList.Add(new MessageList()
                                {
                                    userName = temp[0],
                                    message = temp[1],
                                    isMe = true,
                                    sameBefore = (isSame == temp[0]),
                                });
                            }

                            else
                            {
                                MessageList.Add(new MessageList()
                                {
                                    userName = temp[0],
                                    message = temp[1],
                                    isMe = false,
                                    sameBefore = (isSame == temp[0]),
                                    profileImage = friendsImage[temp[0]],
                                });
                            }

                            isSame = temp[0];
                        }
                    });
                }
            }

            // 메세지 추가 로딩
            if (cmd == (int)ChatInfo.Plus)
            {
                string room = data.Split('/')[0];
                string messageChunk = data.Remove(0, room.Length + 1);


                if (data.Length > 2 && room == this.roomName)
                {
                    string[] msg = messageChunk.Split('&');

                    // [0] 이름, [1] 메세지
                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        for (int i = 0; i < data.Length; i++)
                        {
                            string[] temp = msg[i].Split(',');

                            // is Me
                            if (userId == temp[0])
                            {
                                MessageList.Add(new MessageList()
                                {
                                    userName = temp[0],
                                    message = temp[1],
                                    isMe = true,
                                    sameBefore = (isSame == temp[0]),
                                });

                            }

                            else
                            {
                                MessageList.Add(new MessageList()
                                {
                                    userName = temp[0],
                                    message = temp[1],
                                    isMe = false,
                                    sameBefore = (isSame == temp[0]),
                                    profileImage = friendsImage[temp[0]],
                                });

                            }
                            isSame = temp[0];


                            MessageList.Move(messageList.Count - 1, i);
                        }
                    });

                    pageNation++;
                    move();
                }                
            }

            update("messageList");
        }

        // 메세지 전송
        public void Send()
        {
            SocketConnection.GetInstance().Send("Chat", roomName, userId, inputMessage);
        }

        // 할일: chat 명령 보내는쪽에 메세지 카운터 넣어서 보내기
        public void MoreMessage()
        {
            SocketConnection.GetInstance().Send("Message", roomName, pageNation.ToString());
        }

    }
}

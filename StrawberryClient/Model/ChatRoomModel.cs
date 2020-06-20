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
            SocketConnection.GetInstance().Recv += new SocketConnection.Receive(Receive);
        }

        public void DetachAlarm()
        {
            SocketConnection.GetInstance().Recv -= new SocketConnection.Receive(Receive);
            MessageList = null;
            FriendsImage = null;
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

        string isSame = string.Empty;

        // [0] fromUserName [1] msg
        private void Receive(string param)
        {
            string[] recvData = param.Split(new string[] { "<AND>" }, StringSplitOptions.None);
            string recvRoomName = recvData[0];

            if (recvRoomName != roomName) { return; }

            string messageChunk = recvData[1];


            // 메세지 지속적 주고 받는 상황 data.Split('&').Length <= 2 && 
            if (messageChunk.Contains("<CHAT>"))
            {
                string[] data = messageChunk.Replace("<CHAT>", string.Empty).Split('&');
                string name = data[0];
                string message = data[1];

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    // is Me
                    if (userId == name)
                    {
                        MessageList.Add(new MessageList()
                        {
                            userName = name,
                            message = message,
                            isMe = true,
                            sameBefore = (isSame == name),
                        });
                    }

                    else
                    {
                        MessageList.Add(new MessageList()
                        {
                            userName = name,
                            message = message,
                            isMe = false,
                            sameBefore = (isSame == name),
                            profileImage = friendsImage[name],
                        });
                    }
                });


                isSame = name;


            }

            // 메세지 추가 로딩
            else if(messageChunk.Contains("<PLUS>"))
            {
                string[] data = messageChunk.Replace("<PLUS>", string.Empty).Split('&');
                string[] temp;

                if (data.Length <= 1)
                {
                    return;
                }

                App.Current.Dispatcher.Invoke((Action)delegate
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        temp = data[i].Split(',');

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

            // 메세지 초기 세팅
            else
            {
                string[] msg = messageChunk.Split('&');

                if (string.IsNullOrEmpty(msg[0])) { return; }

                string[] temp;

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

            update("messageList");
        }

    }
}

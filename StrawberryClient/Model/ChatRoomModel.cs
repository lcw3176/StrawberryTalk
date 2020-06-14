using StrawberryClient.Model.ObservableCollection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            get { return new ObservableCollection<MessageList>(messageList.Reverse());}
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
            messageList.Clear();
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

        //string isSame = string.Empty;

        // [0] fromUserName [1] msg
        private void Receive(string param)
        {
            string isMe = param.Split(new string[] { "<AND>" }, StringSplitOptions.None)[0];

            if (isMe != roomName)
            {
                return;
            }

            string data = param.Split(new string[] { "<AND>" }, StringSplitOptions.None)[1];

            if (string.IsNullOrEmpty(data)) { return; }

            string[] result;

            // 메세지 지속적 주고 받는 상황
            // 이미 reverse 된 상황이기 때문에 이곳은 다르게 처리
            if (data.Split('&').Length <= 2 && data.Substring(data.Length - 6, 6) == "<CHAT>")
            {
                result = data.Replace("<CHAT>", string.Empty).Split('&');

                DispatcherService.Invoke((System.Action)(() =>
                {
                    ObservableCollection<MessageList> clone = new ObservableCollection<MessageList>(messageList.Reverse());
                    
                    for (int i = 0; i < result.Length; i += 2)
                    {
                        if(userId == result[i])
                        {
                            clone.Add(new MessageList()
                            {
                                userName = result[i],
                                message = result[i + 1],
                                isMe = (result[i] == userId),
                                //sameBefore = (isSame == result[i]),
                                //profileImage = friendsImage[result[i]],
                            });
                        }

                        else
                        {
                            clone.Add(new MessageList()
                            {
                                userName = result[i],
                                message = result[i + 1],
                                isMe = (result[i] == userId),
                                //sameBefore = (isSame == result[i]),
                                profileImage = friendsImage[result[i]],
                            });
                        }

                            //isSame = result[i];
                        }

                    messageList = new ObservableCollection<MessageList>(clone.Reverse());
                }));

            }

            // 메세지 추가 로딩
            else if(data.Substring(data.Length - 6, 6) == "<PLUS>")
            {
                result = data.Replace("<PLUS>", string.Empty).Split('&');
                string[] temp;

                // 20 19 18 17 16 15   .... 10 9 8 7 6 5
                // 메세지 add로 추가하는 형식으로 정함.
                // set에서 reverse해서 뱉어내기

                DispatcherService.Invoke((System.Action)(() =>
                {
                    if(result.Length <= 1)
                    {
                        return;
                    }

                    //messageList.Clear();

                    for (int i = 0;i < result.Length; i++)
                    {
                        temp = result[i].Split(',');

                        if(userId == temp[0])
                        {
                            messageList.Add(new MessageList()
                            {
                                userName = temp[0],
                                message = temp[1],
                                isMe = (temp[0] == userId),
                                //sameBefore = (isSame == result[i].Split(',')[0]),
                                //profileImage = friendsImage[temp[0]],
                            });

                        }

                        else
                        {
                            messageList.Add(new MessageList()
                            {
                                userName = temp[0],
                                message = temp[1],
                                isMe = (temp[0] == userId),
                                //sameBefore = (isSame == result[i].Split(',')[0]),
                                profileImage = friendsImage[temp[0]],
                            });

                        }

                        //isSame = temp[0];
                    }

                }));

                move();
                pageNation++;
            }

            // 메세지 초기 세팅
            else
            {
                result = data.Split('&');

                if (result[0].Length == 0) { return; }

                string[] temp;

                for (int i = 0; i < result.Length; i++)
                {
                    temp = result[i].Split(',');

                    DispatcherService.Invoke((System.Action)(() =>
                    {
                        if (userId == temp[0])
                        {
                            messageList.Add(new MessageList()
                            {
                                userName = temp[0],
                                message = temp[1],
                                isMe = (temp[0] == userId),
                                //sameBefore = (isSame == result[i].Split(',')[0]),
                                //profileImage = friendsImage[temp[0]],
                            });
                        }

                        else
                        {
                            messageList.Add(new MessageList()
                            {
                                userName = temp[0],
                                message = temp[1],
                                isMe = (temp[0] == userId),
                                //sameBefore = (isSame == result[i].Split(',')[0]),
                                profileImage = friendsImage[temp[0]],
                            });
                        }

                    }));

                    //isSame = temp[0];
                }
            }

            update("messageList");
        }
    }
}

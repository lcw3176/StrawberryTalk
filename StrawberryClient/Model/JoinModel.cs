using System.Text;

namespace StrawberryClient.Model
{
    class JoinModel
    {
        private string userId;
        private string userPw = string.Empty;
        private string userNickname;

        private StringBuilder serverPw = new StringBuilder();

        public string UserNickname
        {
            get { return userNickname; }
            set { userNickname = value; }
        }

        public string UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string UserPw
        {
            get
            {
                if (string.IsNullOrEmpty(userPw))
                {
                    return userPw;
                }

                else
                {
                    return new string('*', userPw.Length - 1) + userPw[userPw.Length - 1];
                }
            }

            set
            {
                // 유저가 비밀번호를 지울때
                if (userPw.Length > value.Length)
                {
                    serverPw.Remove(value.Length, userPw.Length - value.Length);
                    userPw = value;
                }

                // 유저가 비밀번호를 작성할 때
                else if (userPw.Length < value.Length)
                {
                    userPw = value;
                    serverPw.Append(UserPw[userPw.Length - 1]);
                }

            }
        }

        public JoinModel()
        {

        }

        public string TryJoin()
        {
            SocketConnection.GetInstance().Send("Join", userId, userNickname, serverPw.ToString());

            return SocketConnection.GetInstance().LoginRecv();

        }


    }
}

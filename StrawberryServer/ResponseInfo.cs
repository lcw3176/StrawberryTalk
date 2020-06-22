using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryServer
{
    interface IResponse
    {
        int ToInt();
    }

    enum ResponseInfo
    {
        True, False, Already, Auth, Email, Nickname, Set, Init, Find, First, Chat, Plus, Refresh
    }

    public class LoginResponse : IResponse
    {
        enum LoginInfo { True, False, Already, Auth }

        private LoginInfo info { get; set; }

        public void SetTrue()
        {
            this.info = LoginInfo.True;
        }

        public void SetFalse()
        {
            this.info = LoginInfo.False;
        }

        public void SetAlready()
        {
            this.info = LoginInfo.Already;
        }

        public void SetAuth()
        {
            this.info = LoginInfo.Auth;
        }

        public int ToInt()
        {
            return (int)this.info;
        }
    }

    public class JoinResponse : IResponse
    {
        enum JoinInfo { True, False, Email, Nickname }
        private JoinInfo info;

        public void SetTrue()
        {
            this.info = JoinInfo.True;
        }

        public void SetFalse()
        {
            this.info = JoinInfo.False;
        }

        public void SetEmail()
        {
            this.info = JoinInfo.Email;
        }

        public void SetNickname()
        {
            this.info = JoinInfo.Nickname;
        }

        public int ToInt()
        {
            return (int)this.info;
        }
    }

    public class AuthResponse : IResponse
    {
        enum AuthInfo { True, False, Ready }
        private AuthInfo info;

        public void SetTrue()
        {
            this.info = AuthInfo.True;
        }

        public void SetFalse()
        {
            this.info = AuthInfo.False;
        }

        public void SetReady()
        {
            this.info = AuthInfo.Ready;
        }

        public int ToInt()
        {
            return (int)this.info;
        }
    }

    public class HomeReponse : IResponse
    {
        enum HomeInfo { Init, Find, Refresh, Chat }
        private HomeInfo info;

        public void SetInit()
        {
            this.info = HomeInfo.Init;
        }

        public void SetFind()
        {
            this.info = HomeInfo.Find;
        }

        public void SetRefresh()
        {
            this.info = HomeInfo.Refresh;
        }

        public void SetChat()
        {
            this.info = HomeInfo.Chat;
        }

        public int ToInt()
        {
            return (int)this.info;
        }
    }

    public class ChatRoomResponse : IResponse
    {
        enum ChatInfo { Init, First, Plus, Chat }
        private ChatInfo info;

        public void SetInit()
        {
            this.info = ChatInfo.Init;
        }

        public void SetFirst()
        {
            this.info = ChatInfo.First;
        }

        public void SetChat()
        {
            this.info = ChatInfo.Chat;
        }

        public void SetPlus()
        {
            this.info = ChatInfo.Plus;
        }

        public int ToInt()
        {
            return (int)this.info;
        }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StrawberryClient.Model
{
    class AuthModel
    {
        private int authNumber;

        public int AuthNumber
        {
            get { return authNumber; }
            set { authNumber = value; }
        }

        public bool SetAuth()
        {
            SocketConnection.GetInstance().Send("Auth", "set");

            string result = SocketConnection.GetInstance().LoginRecv();

            if(result == "true")
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public bool GetAuth()
        {
            SocketConnection.GetInstance().Send("Auth", authNumber.ToString());

            string result = SocketConnection.GetInstance().LoginRecv();

            if(result == "true")
            {
                return true;
            }

            else
            {
                return false;   
            }
        }
    }
}

using StrawberryServer.DataBase;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace StrawberryServer.routes
{
    class MoreChat : IRoutes
    {
        string roomName;
        string fromUserName;
        int pageNation;

        // 채팅방 내용 추가 로딩
        public byte[] Process()
        {
            
            List<string> data = Query.GetInstance().GetMessage(roomName, pageNation);
            string sendData = string.Join("&", data) + "<PLUS>";

            return Encoding.UTF8.GetBytes(roomName + "<AND>" + sendData);
        }

        public void SetInfo(string param, Socket socket)
        {
            roomName = param.Split(',')[0];
            fromUserName = param.Split(',')[1];
            int.TryParse(param.Split(',')[2], out pageNation);
        }
    }
}

using StrawberryServer.DataBase;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace StrawberryServer
{
    /// <summary>
    /// 1. DB에서 채팅 20개씩만 조회하도록 만들기 O
    /// 2. 친구 추가 기능 만들기 0
    /// 3. 단톡도 고려해보기 O
    /// 4. 유저 가입창 만들기 O
    /// 5. 신규 유저 가입 시 어떻게 알림 줄건지도 생각해보기 0
    /// 6. 채팅방 이미지 전송
    /// 7. 보이스톡, 페이스톡도 시도
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += Exit;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            Query.GetInstance().Open();
            Query.GetInstance().initTable();

            socket.Listen(10);

            while (true)
            {
                Socket user = socket.Accept();
                ClientInfo client = new ClientInfo();
                client.SetInfo(user);

                Thread recv = new Thread(client.Start);
                recv.Start();
            }
        }

        static void Exit(object sender, EventArgs e)
        {
            Query.GetInstance().Close();
        }
    }
}

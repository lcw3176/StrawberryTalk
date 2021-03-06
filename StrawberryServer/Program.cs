﻿using StrawberryServer.DataBase;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

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
    /// 8. 회원 가입시 메일 인증 O
    /// 
    /// 문제점
    /// 1. 이미지, 텍스트 구별해서 전송 문제 O
    /// ==> 전송 시 앞에 정수형 식별자 넣어줌
    /// 2. 데이터 전송 끝점 인식 문제 ex) 같은 유저에게 요청 처리를 한번에 2개 시도, 데이터 연속적으로 많이 오면 구별 안되서 서버 펑 O
    /// ==> 데이터 사이즈를 미리 보내줌, send 소켓 버퍼 사이즈 0 설정
    /// https://support.microsoft.com/eu-es/help/214397/design-issues-sending-small-data-segments-over-tcp-with-winsock
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.ProcessExit += Exit;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, 3000));
            int count = 0;

            Query.GetInstance().Open();
            //Query.GetInstance().initTable();
            socket.Listen(10);

            while (true)
            {
                Socket user = socket.Accept();
                ClientThread client = new ClientThread();
                client.SetInfo(user);
                Task.Run(() => client.Start());
                count++;
                Console.WriteLine("현재 접속자 수: " + count);
            }
        }

        static void Exit(object sender, EventArgs e)
        {
            Query.GetInstance().Close();
        }
    }
}

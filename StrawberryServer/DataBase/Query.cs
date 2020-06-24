using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;

namespace StrawberryServer.DataBase
{
    class Query
    {
        string dbPath = @"Data Source=D:\user.db;";
        SQLiteConnection conn;

        public static Query Instacne;

        public static Query GetInstance()
        {
            if(Instacne == null)
            {
                Instacne = new Query();
            }

            return Instacne;
        }

        public void Open()
        {
            conn = new SQLiteConnection(dbPath);
            conn.Open();
        }

        public void Close()
        {
            conn.Close();
        }


        public void initTable()
        {
            string s1 = "CREATE table user(" +
                "sid integer PRIMARY KEY AUTOINCREMENT, " +
                "name varchar(30) not null, " +
                "nickname varchar(10) not null, " +
                "password varchar(44) not null, " +
                "auth varchar(5) not null, " +
                "image varchar(40))";

            string s2 = "CREATE table room(" +
                "sid integer PRIMARY KEY AUTOINCREMENT, " +
                "name varchar(30) not null)";

            string s3 = "create table message(" +
                "sid integer PRIMARY KEY AUTOINCREMENT, " +
                "roomName varchar(30), " +
                "fromUserName varchar(20), " +
                "message varchar(200), " +
                "foreign key(roomName) " +
                "references room(name) " +
                "on update cascade " +
                "on delete cascade)";

            string s5 = "create table friendsList(" +
                "name varchar(30), " +
                "friends varchar(30), " +
                "foreign key(name) " +
                "references user(nickname) " +
                "on update cascade " +
                "on delete cascade)";


            SQLiteCommand cmd;
            string sql;
            //sql = "drop table user";
            //cmd = new SQLiteCommand(sql, conn);
            //cmd.ExecuteNonQuery();

            sql = "drop table message";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = "drop table room";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            sql = "drop table friendsList";
            cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();

            //cmd = new SQLiteCommand(s1, conn);
            //cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(s2, conn);
            cmd.ExecuteNonQuery();
            
            cmd = new SQLiteCommand(s3, conn);
            cmd.ExecuteNonQuery();

            cmd = new SQLiteCommand(s5, conn);
            cmd.ExecuteNonQuery();

            cmd.Dispose();

            Console.WriteLine("Init complete");

        }

        // 유저 로그인 시 사용
        public string GetNickname(string userEmail, string userPw)
        {
            string sql = string.Format("SELECT nickname FROM user WHERE name = '{0}' AND password = '{1}'", userEmail, userPw);
            string nickname = string.Empty;

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                nickname = reader["nickname"].ToString();
            }

            reader.Close();
            cmd.Dispose();

            return nickname;
        }

        // 이메일 중복검사
        public string GetEmail(string email)
        {
            string sql = string.Format("SELECT name FROM user WHERE name = '{0}'", email);
            string mail = string.Empty;

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                mail = reader["name"].ToString();
            }

            reader.Close();
            cmd.Dispose();

            return mail;
        }

        // 특정 유저 닉네임 가져오기, 유저 검색 기능, 닉네임 중복 검사
        public string GetNickname(string nickname)
        {
            string sql = string.Format("SELECT nickname FROM user WHERE nickname = '{0}'", nickname);
            string name = string.Empty;

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                name = reader["nickname"].ToString();
            }

            reader.Close();
            cmd.Dispose();

            return name;

        }

        public bool GetAuth(string userId)
        {
            string sql = string.Format("SELECT auth FROM user WHERE name = '{0}'", userId);

            bool auth = false;

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while(reader.Read())
            {
                auth = bool.Parse(reader["auth"].ToString());
            }

            reader.Close();
            cmd.Dispose();

            return auth;
        }


        // user 이미지 가져오기
        public string GetImagePath(string userId)
        {
            
            string imagePath = string.Empty;

            if(userId.Split(',').Length < 2)
            {
                string sql = string.Format("SELECT image FROM user WHERE nickname = '{0}'", userId);
                SQLiteCommand cmd = new SQLiteCommand(sql, conn);
                SQLiteDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    imagePath = reader["image"].ToString();
                }

                reader.Close();
                cmd.Dispose();
            }

            else
            {
                imagePath = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\groupChat.jpg";
            }

            return imagePath;


        }


        
        // 채팅방 이름 가져오기
        public string GetRoom(string roomName)
        {
            string sql = string.Format("SELECT name FROM room WHERE name = '{0}'", roomName);
            string room = string.Empty;
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            
            while (reader.Read())
            {
                room = reader["name"].ToString();
            }
            
            reader.Close();
            cmd.Dispose();
            
            return room;            
        }


        // 채팅 메세지 가져오기
        public List<string> GetMessage(string roomName)
        {
            string sql = string.Format("" +
                  "SELECT message, fromUserName " +
                  "FROM message " +
                  "WHERE roomName = '{0}'" +
                  "ORDER BY sid " +
                  "DESC LIMIT 45", roomName); 
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);            
            SQLiteDataReader reader = cmd.ExecuteReader();
            
            List<string> msg = new List<string>();
            
            while (reader.Read())
            {
                msg.Add(reader["fromUserName"].ToString() + "," + reader["message"].ToString());
            }

            msg.Reverse();
            reader.Close();
            cmd.Dispose();
            
            return msg;            
        }


        // 채팅 메세지 가져오기 (추가 로딩 요청 시)
        public List<string> GetMessage(string roomName, int pageNation)
        {
            string sql = string.Format("" +
                "SELECT message, fromUserName " +
                "FROM message " +
                "WHERE roomName = '{0}' " +
                "ORDER BY sid " +
                "DESC LIMIT '{1}', '{2}'", roomName, (pageNation) * 45, (pageNation + 1) * 45);


            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            List<string> msg = new List<string>();

            while (reader.Read())
            {
                msg.Add(reader["fromUserName"].ToString() + "," + reader["message"].ToString());
            }

            msg.Reverse();
            reader.Close();
            cmd.Dispose();

            return msg;

        }

        // 로그인 성공 시 유저 정보 넘기기(친구 목록, 친구 상태 메세지)
        public string GetUserInfo(string userNickname)
        {
            List<string> friends = new List<string>();
            List<string> room = new List<string>();

            string sql = string.Format("" +
                "SELECT friends " +
                "FROM friendsList " +
                "WHERE name = '{0}'", userNickname);

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                friends.Add(reader["friends"].ToString());
            }

            sql = string.Format("" +
                "SELECT name " +
                "FROM room " +
                "WHERE name LIKE '%{0}%'", userNickname);

            cmd = new SQLiteCommand(sql, conn);
            reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                room.Add(reader["name"].ToString());
            }


            StringBuilder data = new StringBuilder();

            data.Append(userNickname);
            data.Append("/");

            data.Append(string.Join(",", friends));
            data.Append("/");

            data.Append(string.Join(",", room));
            data.Append("/");

            reader.Close();
            cmd.Dispose();
            
            return data.ToString();
        }


        // 친구 추가
        public void SetFriend(string userId, string friendName)
        {
            string sql = string.Format("" +
                "INSERT INTO friendsList(name, friends) " +
                "VALUES('{0}', '{1}')", userId, friendName);

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        // 유저 회원가입
        public void SetUser(string userId, string userNickname, string userPw)
        {
            string userImage = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\default.jpg";

            string sql = string.Format("" +
                "INSERT INTO " +
                "user(name, password, image, nickname, auth) " +
                "VALUES('{0}', '{1}', '{2}', '{3}', 'false')", userId, userPw, userImage, userNickname);
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();


            cmd.Dispose();

        }

        // 채팅방 세팅
        public void SetRoom(string roomName)
        {
            if (!string.IsNullOrEmpty(GetRoom(roomName))) { return; }                  
            string sql = string.Format("INSERT INTO room(name) VALUES('{0}')", roomName);
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        // 유저 인증 세팅
        public void SetAuth(string userId)
        {
            string sql = string.Format("" +
                "UPDATE user " +
                "SET auth = 'true' " +
                "WHERE name = '{0}'", userId);

            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }


        // 프로필 사진 설정
        public void SetImage(string userId, string path)
        {
            // null 일 경우 default 이미지 경로로 설정
            if(path == null)
            {
                path = @"D:\project\Cs\StrawberryTalk\StrawberryServer\Resource\UserImage\default.jpg";
            }

            string sql = string.Format(
                "UPDATE user " +
                "SET image = '{0}' " +
                "WHERE name = '{1}'", path, userId);
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            cmd.Dispose();

        }

        // 채팅 메세지 기록
        public void SetMessage(string roomName, string fromUserName, string msg)
        {
            msg = msg.Replace("'", "\''");

            string sql = string.Format("" +
                "INSERT INTO message(roomName, fromUserName, message) " +
                "VALUES('{0}', '{1}', '{2}')", roomName, fromUserName, msg);
            
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            
            cmd.Dispose();

        }

    }
}

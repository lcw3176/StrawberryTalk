# StrawberryTalk
카카오톡 클론 메신저

## 데이터베이스: Sqlite
<details>
<summary>friendsList 테이블</summary>
<div markdown="1">
<pre>
<code>

CREATE TABLE friendsList(
        name varchar(30), 
        friends varchar(30), 
        foreign key(name) 
        references user(name) 
        on update cascade 
        on delete cascade)

</pre>
</code>
</div>
</details>


<details>
<summary>message 테이블</summary>
<div markdown="1">
<pre>
<code>

CREATE TABLE message(
        sid integer PRIMARY KEY AUTOINCREMENT, 
        roomName varchar(30), 
        fromUserName varchar(20), 
        message varchar(100), 
        foreign key(roomName) 
        references room(name) 
        on update cascade 
        on delete cascade)


</pre>
</code>
</div>
</details>

<details>
<summary>room 테이블</summary>
<div markdown="1">
<pre>
<code>

CREATE TABLE room(
        sid integer PRIMARY KEY AUTOINCREMENT,
        name varchar(30) not null)


</pre>
</code>
</div>
</details>


<details>
<summary>user 테이블</summary>
<div markdown="1">
<pre>
<code>

CREATE TABLE user(
        sid integer PRIMARY KEY AUTOINCREMENT,
        name varchar(30) not null,
        password varchar(20) not null,
        image varchar(40),
        status varchar(50))


</pre>
</code>
</div>
</details>

## 통신 규약
|CRUD|Route|
|-----------|--------|
|로그인|Login/{id,pw}|
|회원가입|Join/{id,pw}|
|채팅방 메세지 로드|Room/{id,pw}|
|채팅 Echo|Chat/{id,pw}|
|채팅방 메세지 추가 로드|Message/{roomName}|
|프로필 사진 불러오기|Image/{id}|
|내 프로필 사진 설정|MyImage/{ImageByteLength}|
|프로필 사진 기본으로 변경|DefaultImage/{null}|

## 작동 방식 및 구현 기능
### 클라이언트 
* WPF 사용, MVVM 패턴
* 소켓 통신(TCP), 소켓 객체는 Singleton
* 프로필 사진 설정
* 메세지 알람
* 단체 채팅방
* 채팅 부분 조회(스크롤 올라갈 때마다 추가로 요청)


### 서버
#### Program.cs
* 유저 접속 시 ClientThread 클래스 생성

#### ClientThread.cs
* 클라이언트 요청 대기
* 요청에 따라 Reflection을 통한 동적 method(routes/Index.cs) 호출
* 자료 반환

#### RoomManager.cs
* 전체 유저 관리 클래스
* 유저 목록 추가, 삭제 담당(Dictionary<아이디, 소켓>)
* 중복 로그인 체크
* 알맞은 유저에게 메세지 전송(접속 안되어 있을 시 DB에 기록, 차후 접속 시 확인가능)



## 작동 모습
### 로그인, 회원가입(로그인, 중복 로그인 방지, 회원가입)
<div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565365-8e7c3280-ada3-11ea-9c60-0cbe075c9c6c.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565364-8d4b0580-ada3-11ea-9830-ccfed2fbc936.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565366-8e7c3280-ada3-11ea-8f90-978e5ca6a779.png">
 </div>
  
### 메인 홈(메인 창, 내 프로필 사진 설정, 친구 프로필 사진 열람)
<div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565408-eb77e880-ada3-11ea-8e3e-867c0885a1db.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565407-eadf5200-ada3-11ea-8ad7-d223cad4ab3b.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565405-e9ae2500-ada3-11ea-9330-69360de62472.png">
 </div>
 
 ### 채팅 과정(단체 채팅방 생성, 채팅 알람, 채팅 상황)
 <div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565432-1f530e00-ada4-11ea-9223-97c0f4ab23c2.png">
  <img width="100" src="https://user-images.githubusercontent.com/59993347/84565433-20843b00-ada4-11ea-9dcb-40fbb0551b92.png">
  <img width="500" src="https://user-images.githubusercontent.com/59993347/84565434-20843b00-ada4-11ea-9f40-8a8f5fb855b9.png">
 </div>

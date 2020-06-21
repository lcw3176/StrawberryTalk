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
        references user(nickname) 
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
        message varchar(200), 
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
        nickname varchar(10) not null,
        password varchar(44) not null,
        auth varchar(5) not null,
        image varchar(40))

</pre>
</code>
</div>
</details>

## 통신 규약
|CRUD|Route|
|-----------|--------|
|로그인|Login/{id,pw}|
|회원가입|Join/{id,pw}|
|계정 인증번호 요청|Auth/"set"|
|계정 인증번호 비교(최종승인)|Auth/{authNumber}|
|유저 검색|User/{id}|
|채팅방 메세지 로드|Room/{roomName}|
|채팅 Echo|Chat/{roomName, id, message}|
|채팅방 메세지 추가 로드|Message/{roomName, pagination}|
|프로필 사진 불러오기|Image/{id}|
|내 프로필 사진 설정|MyImage/{ImageByteLength}|
|프로필 사진 기본으로 변경|DefaultImage/"null"|

## 작동 방식 및 구현 기능
### 클라이언트 
* WPF 사용, MVVM 패턴
* 소켓 통신(TCP), 소켓 객체는 Singleton
* 유저 닉네임 검색으로 등록 후, 채팅 시작 가능
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

#### Auth.cs
* 회원가입시 유저 이메일 인증 클래스
* 네이버 smtp 사용
* 랜덤으로 6자리 정수 생성, 해당 유저 이메일로 전송
* 서버id와 pw는 서버 컴퓨터 환경변수에 저장, 필요시 읽어옴

#### Encryption.cs
* 비밀번호 암호화 클래스
* HMAC-SHA256 사용
* 비밀키는 환경변수에 저장, 필요시 읽어옴


## 작동 모습
### 로그인, 회원가입(로그인, 회원가입, 인증)
<div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220636-36b77a00-b3e8-11ea-9d89-0481f0d66662.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220638-37e8a700-b3e8-11ea-8593-769e6f0eb37c.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220640-37e8a700-b3e8-11ea-9374-58894a0c6662.png">
 </div>
  
### 메인 홈(메인 홈, 내 프로필 사진 설정, 사진 변경 후)
<div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220641-38813d80-b3e8-11ea-92e7-7348850bc6ca.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220642-38813d80-b3e8-11ea-9e97-85edfb67baf3.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220644-3919d400-b3e8-11ea-8975-bf6485c2c6b6.png">
 </div>
 
 ### 채팅 과정(단체 채팅방 생성, 채팅 진행)
 <div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220645-39b26a80-b3e8-11ea-882a-62f0b2fbc818.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220647-39b26a80-b3e8-11ea-9e05-4c323645d8b2.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/85220648-3a4b0100-b3e8-11ea-8b61-7db7c8ad71a3.png">
 </div>

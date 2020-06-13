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

## 사용 기술 
* WPF 사용, MVVM 패턴
* 소켓 통신(TCP)

## 구현한 기능
* 동일 ID 중복 로그인 방지
* 프로필 사진 설정
* 메세지 알람
* 단체 채팅방
* 채팅 부분 조회(스크롤 올라갈 때마다 추가로 요청)


## 작동 모습
### 로그인
<div>
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565365-8e7c3280-ada3-11ea-9c60-0cbe075c9c6c.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565364-8d4b0580-ada3-11ea-9830-ccfed2fbc936.png">
  <img width="300" src="https://user-images.githubusercontent.com/59993347/84565366-8e7c3280-ada3-11ea-8f90-978e5ca6a779.png">
 </div>
  


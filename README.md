# StrawberryTalk
카카오톡 클론 메신저

## 데이터베이스: Sqlite
<details>
<summary>friendsList 테이블</summary>
<div markdown="1">
<pre>
<code>

CREATE TABLE friendsList(name varchar(30), friends varchar(30), foreign key(name) references user(name) on update cascade on delete cascade)

</pre>
</code>
</div>
</details>


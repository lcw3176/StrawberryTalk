   M S B A M L   `   `   `  ���� K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  (    "StrawberryClient.View.ChatRoomView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5local#clr-namespace:StrawberryClient.View     -     �Name$  
chatWindow��
���   �$��1q� � Title$ ChatRoomView��$	��600��$	��400�� � AllowsTransparency .  � WindowStyle$
 None=�.���  � RowDefinitions �� $��20 ��� $��40 ��� $	��450 ��� $��* �.+�� $��0��
����d� � ColumnDefinitions �� $��* ��� $��40 �.+��� $��1��$��X��
������ $��0q����� *closeCommand+����  �� ElementName$ 
chatWindow���� $��1��
����d�$��16(� �� Padding$ 10q�$
��Wrap=����� *showedRoomName+ �� UpdateSourceTrigger$ PropertyChanged=��� -   $��
scrollView��$��2��$
��Auto=�
����d�$��0q�.���� -   	 ��Name$	 items������ *messageList+$ PropertyChanged=����� .R��� 
 �� Orientation$
 
Horizontal=�����  �� 
TargetType" �� �� Triggers ��  �� Binding �� *isMe+ �� Value true.a���  �� Property � �� �� Value$ Right=�.+��  �� $��60 ��� $��* �.+���  �� CornerRadius$ 20��$��50��$��50��$��3q�$��0������  j� Stretch$ UniformToFill=� �� ImageSource �� *profileImage+���� " �� ��  �� *isMe+ true.a���  � y�$ 	Collapsed=���  �� *
sameBefore+ true.a���  � y�$ 	Collapsed=��� $
 Vertical=�
������ $
��auto��$
��auto��$��10, 0q�$��1��.+��� $��16(����� *
userName+���� " �� ��  �� *isMe+ true.a���  � y�$ 	Collapsed=���  �� *
sameBefore+ true.a���  � y�$ 	Collapsed=��� $ 5��$
��Left=�$
��0, 3q����� " �� ��  �� *isMe+	 false.a���  � ��
 �������  �� *isMe+ true.a���  � ��
 � ���._��� $��13(�$
��Wrap=�$ 8q�
������ ���� *	message+�� $��3��$
 
Horizontal=�.+��� -    ��Name$ InputMessage�� �� TextWrapping$
 Wrap=�$	��330��$��0q����� *inputMessage+$ PropertyChanged=�.�� J� InputBindings �� 
 �� Key$ Return�� �� Command �� *sendMessageCommand+.���� $��전송��
��� ���$��30��$��30��$��10, 10, 10, 50q����� *sendMessageCommand+
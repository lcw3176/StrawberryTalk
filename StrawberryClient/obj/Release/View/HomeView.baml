   M S B A M L   `   `   `  ���� K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  $    StrawberryClient.View.HomeView   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     5local#clr-namespace:StrawberryClient.View     $��0q�  � 	Resources     MyButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Triggers P� Property P� Value �� 
TargetName��%�   &	        �� " ��.���  �@�OverridesDefaultStyle . ��  � �� �� " ��.R��� $��border��#��z��._��� $��Center=�$��Center=� P�  �@J�IsMouseOver . . ��� $ border�� �@J�Opacity$	 0.7Q�.��� 	 � ColumnDefinitions	 �� $��60 ��� $��* �.+�� $��0��
�����p�
 � RowDefinitions
 �� $��90 ��� $��70 ��� $��70 ��� $��* �.+��� -    ��Name$ 
homeButton��$��1��$��50��$��50��$��0q�#��[  .���� $!��../Resource/Images/home.png��$��Uniform=�$��30���� -   $ chatListButton��$��2��$��50��$��50��$��0q�#��[  .���� $$��../Resource/Images/message.png��$��Uniform=�$��30��� $��1��.+��� -    ��Name$ MainTabControl��$��0q�$��2��.@��� $	��홈��$y�	Collapsed=�.��� 
 �� $��80 ��� $��50 ��� $��90 ��� $��* �.+�� $��0��	 �� $��* ��� $��50 ��� $��50 ��� $��50 �.+��� $��0��$��친구��$��20(�$
��Left=�$��Bottom=�$��10q��� $��1��$��Right=�$��Bottom=�$��30��$��30��$��10q�$��0q�#��[  ���� *addChatCommand+����  �� ImageSource$$ ../Resource/Images/addChat.png���� $��2��$��Right=�$��Bottom=�$��30��$��30��$��10q�$��0q�#��[  ���� *findUserCommand+���� $% ../Resource/Images/findUser.png���� $��3��$��Right=�$��Bottom=�$��30��$��30��$��10q�$��0q�#��[  ���� *refreshCommand+���� $$ ../Resource/Images/refresh.png���� $��1��
�������$��15(�$��10q����� *
findUser+ �� UpdateSourceTrigger$ PropertyChanged=�.��  ��%9   '��       �� " ��.���  �@��CornerRadius$ 5��.�� J� InputBindings �� 
 �� Key$ Return�� �� Command �� *findUserCommand+.��� $��2��.+��� $
��Left=�$��Center=�#��[  .����  �� Orientation$ 
Horizontal=�$	��330��$��70��   �� Triggers��%S   '��       �� " �� P�  �@J�IsMouseOver . . ���  � ��
 �����.+��� $��3q�$��Center=�$��60�� �� CornerRadius$ 20������  j� Stretch$ UniformToFill=� �� *	userImage+$ PropertyChanged=�._��� 
������ $��0q�#��[  ���� *setProfileCommand+�� $ Vertical=�$��Center=�.+��� $��18(�$��5q����� *userId+$ PropertyChanged=��� 
�������$��2��
�������$��4q�$��Stretch=�$��Bottom=��� $��3��$
��Auto=�.���� ���� *friendsList+$ PropertyChanged=����� .R��� $
��Left=�$��Center=�#��[  ���� *chatCommand+����  �� ElementName$ chatRoom��.���� $ 
Horizontal=�$	��330��$��70��  ��%S   '��       �� " �� P�  �@J�IsMouseOver . . ���  � ��
 �����.+��� $ 20��$��60��$��3q�$��Center=����� $ UniformToFill=� �� *friendsImage+$ PropertyChanged=�._��� 
������ $��0q�#��[  ���� *viewProfileCommand+���� $ chatRoom���� $ Vertical=�$��Center=�.+���  ��Name$ chatRoom��$��18(�$��5q����� *friendsName+�� $��채팅방 모음��$y�	Collapsed=�.��� .+��� $��3��$
��Auto=�.���� ���� *chatRoomsList+$ PropertyChanged=����� .R��� $
��Left=�$��Center=�#��[  ���� *roomCommand+���� $
 room��.���� $ 
Horizontal=�$	��330��$��70��  ��%S   '��       �� " �� P�  �@J�IsMouseOver . . ���  � ��
 �����.+��� $ 20��$��3q�$��60��$��Center=����� $ UniformToFill=� �� *	roomImage+$ PropertyChanged=�._��� 
������ $��0q�#��[  �� $ Vertical=�$��Center=�.+��� $
 room��$��18(����� *
roomName+
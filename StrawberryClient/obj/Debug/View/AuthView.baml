   M S B A M L   `   `   `  ����K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  5      $    StrawberryClient.View.AuthView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:StrawberryClient.View     5      .��5	      � 
�������  � ColumnDefinitions  5
   
   �� 5      $��70 �6   �� 5      $��* �6   �� 5      $��70 �6    � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��30 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��0��5      $
��Auth��5      $��60(�5      $��Center=�5      $��Center=�5      �� 5    
   $��1��6   $��1��5!      $��인증번호��5"      $��18(�5#      $
��Left=�5$      $��Center=�5%      �� 5'   
   $��2��6   $��1��5(      $��Center=�5)      $��Stretch=�5*      $��30(�5+      $��5q�5,      ��5-      �� *
authNumber+ �� UpdateSourceTrigger$ PropertyChanged=��� 5/   
   $��3��6   $��1��50      $��8q�51      $��확인��52      $��20(�53      $��Login��55      ��54      �� *comfirmCommand+55      �� 57   
   $��4��6   $��1��58      $��Right=�59      $��Bottom=�5:      $��인증번호 재요청��5;      
������ $��0q�5=      $��5q�5>      ��5?      �� *reAuthCommand+�� 5A   
   $��5��6   $��3��5B      $��Center=�5C      $��Center=�5D      $��WrapWithOverflow=�5E      $��18(�5F      $=��7입력한 메일로 인증번호를 확인해 주세요��5G      5J      5K      
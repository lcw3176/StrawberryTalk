   M S B A M L   `   `   `  ����K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  5      $    StrawberryClient.View.JoinView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:StrawberryClient.View     5      .��5
      � 
�������  � ColumnDefinitions  5   
   �� 5      $��50 �6   �� 5      $��* �6   �� 5      $��50 �6    � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��30 �6   �� 5      $��50 �6   �� 5      $��30 �6   �� 5      $��50 �6   �� 5      $��30 �6   �� 5      $��50 �6   �� 5      $��50 �6   �� 5      $��40 �6   �� 5      $��* �6   .+�5   
   �� $��0��6   $��1��5       $��Join us!��5!      $��Center=�5"      $��Center=�5#      $��60(�5$      �� 5&   
   $��1��6   $��1��5'      $�� Email��5(      $��Center=�5)      $
��Left=�5*      $��20(�5+      �� 5-   
   $��2��6   $��1��5.      $��20(�50      $��Stretch=�51      $��Stretch=�52      $��10q�53      ��5/      �� *userId+ �� UpdateSourceTrigger$ PropertyChanged=�53      �� 55   
   $��3��6   $��1��56      $��	 NickName��57      $��Center=�58      $
��Left=�59      $��20(�5:      �� 5<   
   $��4��6   $��1��5=      $��20(�5?      $��Stretch=�5@      $��Stretch=�5A      $��10q�5B      ��5>      �� *userNickname+$ PropertyChanged=�5B      �� 5D   
   $��5��6   $��1��5E      $	�� PW��5F      $��Center=�5G      $
��Left=�5H      $��20(�5I      �� 5K   
   $��6��6   $��1��5L      $��20(�5N      $��Stretch=�5O      $��Stretch=�5P      $��10q�5Q      ��5M      �� *userPw+$ PropertyChanged=�5Q      �� 5S   
   $��7��6   $��1��5T      $��Submit��5U      $��20(�5V      $
��Auth��5X      $��Stretch=�5Y      $��Stretch=�5Z      $��10q�5[      ��5W      �� *joinCommand+5[      �� 5]   
   $��8��6   $��1��5^      $��Go Back��5_      $��Login��5a      $��Bottom=�5b      $��Right=�5c      $��10q�5d      
������ $��0q�5f      ��5`      �� *returnCommand+5f      5g      5h      
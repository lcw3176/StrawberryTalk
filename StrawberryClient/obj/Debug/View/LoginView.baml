   M S B A M L   `   `   `  ����K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  5      %    StrawberryClient.View.LoginView   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      5local#clr-namespace:StrawberryClient.View     5      .��5	      � 
�������  � ColumnDefinitions  5
   
   �� 5      $��80 �6   �� 5      $��* �6   �� 5      $��80 �6    � RowDefinitions 5   
   �� 5      $	��100 �6   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $��40 �6   �� 5      $��30 �6   �� 5      $��40 �6   �� 5      $��50 �6   �� 5      $��50 �6   .+�5   
   �� $��0��6   $��3��5      $��20q�5      $"��../Resource/Images/title.png��5      �� 5    
   $��1��6   $��3��5!      $��10q�5"      $!��../Resource/Images/logo.png��5#      �� 5%   
   $��2��6   $��1��5&      $��Email��5'      $��18(�5(      $
��Left=�5)      $��Center=�5*      �� 5,   
   $��3��6   $��1��5-      $��20(�5.      $��5q�5/      ��50      �� *userId+ �� UpdateSourceTrigger$ PropertyChanged=��� 52   
   $��4��6   $��1��53      $��PW��54      $��18(�55      $
��Left=�56      �� 58   
   $��5��6   $��1��59      $��20(�5:      $��5q�5;      ��5<      �� *userPw+$ PropertyChanged=�.��6O    J� InputBindings 5=      �� 5>      
 �� Key$ Return��6    �� CommandParameter$
 Home��5@       �� Command 5?      �� *loginCommand+5@      .��5A   %   5B      �� 5D   
   $��6��6   $��1��5E      $��Log-In��5F      $��25(�5G      $��5q�5H      $
��Home��5J      ��5I      �� *loginCommand+5J      �� 5L   
   $��7��6   $��2��5M      $��Sign-up��5N      $	��Top=�5O      $
��Left=�5P      
������ $��0q�5R      $��20(�5S      $
��Join��5U      ��5T      �� *joinCommand+5U      5W      5X      
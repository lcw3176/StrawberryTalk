   M S B A M L   `   `   `  ����K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null=#clr-namespace:StrawberryClient.ViewStrawberryClient.View  5   	   '    !StrawberryClient.View.addChatView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   5local#clr-namespace:StrawberryClient.View     5   	   -   5          �Name$  addChatWindow��5   	    � Title$ addChatView��5	   	   $	��600��6   $	��400��6*   .��5
      �  � RowDefinitions 5   
   �� 5      $��70 �6   �� 5      $��* �6   �� 5      $��50 �6   .+�5   
   �� $��0��6   $��대화상대 선택��5      $��10q�5      $��20(�5      �� 5   
   $��1��6   $
��Auto=�5      .��5      �� $��3q�6   ��5      �� *addChatList+ �� UpdateSourceTrigger$ PropertyChanged=���5      �� 5      .R�5      �� .+�5       ��  �� Orientation$ 
Horizontal=�6)   $��Stretch=�5!   )   $��70��5"   )    � 	Resources 5$   "    �� 
TargetType �� Triggers P� Property	 P� Value
 �� Property �� Value��%�   '��       �� 5%   &   " ��6,    5&   *   P� 5'   .    �@J�IsMouseOver	 . . �5)   2   �� 
 � ��
 �����5*   9   5+   /   5-   '   5.   #   .+�50   "   ��  d� 	IsChecked$ FalseY�6+   $
��Left=�51   +   $��Center=�52   +   ��53   +   �� *checkCommand+��54   +   ��  �� ElementName$ chatRoom�� 56   &   ��%_   '��       �� 57   *   " ��60   .�58   .   �� 
 �@��CornerRadius$ 20��6M   59   +   5:   '    � LayoutTransform 5<   &   �� 5=   *    �� ScaleX$ 3Q�69    �� ScaleY$ 3Q�6D   5?   #   �� 5A   "   
���   �$��2q�5B   )    �� CornerRadius$ 3��5C   )   $��3q�5D   )   ._�5F   &   �� 
������ $��0q�5G   -   .��5I   *   �� $��0��60   $��Center=�5J   1   $��60��5K   1   $��UniformToFill=�5M   1   ��5L   1   �� *friendsImage+5N   +   5O   '   5P   #   �� 5S   "    ��Name$ chatRoom��6,   $��18(�5U   ,   $��5q�5V   ,   ��5T   ,   �� *friendsName+5V   ,   5X      5Z      5[      5]      5^      �� 5`   
   $��2��6   $��확인��5a      $��Right=�5b      $��7q�5c      $	��100��5d      $��20(�5e      ��5f      �� *completeCommand+��5g      �� $ addChatWindow��5i      5j      
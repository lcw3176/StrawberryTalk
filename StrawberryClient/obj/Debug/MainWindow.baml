   M S B A M L   `   `   `  ����K  GStrawberryClient, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:StrawberryClientStrawberryClient  5   	   G(clr-namespace:StrawberryClient.ViewModelStrawberryClient.ViewModel  5   	   =#clr-namespace:StrawberryClient.ViewStrawberryClient.View  5   	   !    StrawberryClient.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:StrawberryClient     5   	   ?
viewModels(clr-namespace:StrawberryClient.ViewModel     5   	   5views#clr-namespace:StrawberryClient.View     5   	   -   5          �Name$  
mainWindow��5
   	    � Title$ Strawberry Talk��5   	   $	��600��6!   $	��400��6.    � WindowStyle$
 None=�6:    � AllowsTransparency .  � WindowStartupLocation$ CenterScreen=�5   	   
���   �$��1q�5   	   .��5      �  � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $��30 �6   �� 5      $��30 �6    � RowDefinitions 5   
   �� 5      $��20 �6   �� 5      $��* �6   .+�5    
   �� $��0��6   $��3��5!      
�����p�5#      �� 5%   
   -   5         ��Name$ closeButton��5%      $��0��5&      $��2��5'      $��X��5(      
�������
������ $��0q�5+      5,      �� 5.   
   -   5        $ 	minButton��5.      $��1��5/      $��0��50      $	��ㅡ��51      
�������
������ $��0q�54      55      �� 58   
   $��1��6   $��3��59      
������ ��5:      �� *SelectedViewModel+ �� UpdateSourceTrigger$ PropertyChanged=�5;      5=      5>      
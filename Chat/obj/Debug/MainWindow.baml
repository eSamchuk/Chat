   M S B A M L   `   `   `  ����?  ;Chat, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:ChatChat  5   	       Chat.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   $localclr-namespace:Chat     5   	   -   5          � Title$  Client��5   	   $��449.308��6   $	��277��6)    � 	Resources 5
         sendButtonStyle �� 
TargetType �� Property �� Value �� Triggers �� Binding �� Value  privateRecipientStyle �� ElementName	 �� Path  refresh��%l  &	        &	 �     &	 )    �� 5   
   " ��6(   .�5      ��  � {� .  6*    5      �� 5       6   �� *client.Connected+ True6C   .a�5      ��  � {� . 62   5      5      �� 5   
   " ��6.   .�5      ��  � {� 6*   �� $ 
chbPrivate��$	 	IsChecked�5      �� 5   
   $��refresh.png��6    5      .��5      � 
 � RowDefinitions
 5   
   �� 5      $	��26* �6   �� 5      $
��284* �6   �� 5      $	��26* �6   �� 5      $	��26* �6   �� 5      $	��25* �6   �� 5       $	��25* �6   �� 5!      $	��25* �6   .+�5$   
   �� $��0��6   $��Center=�6!   ��5%      �� $	 Login� �� StringFormat$ Logged as: {0}���� 5(   
   -   5        $��
lbMessages��5(      $��1��5)      ��5*      �� *
MessageCol+��5,      �� 5-      .R�5.      �� $��5q�6!   .+�5/      �� $��16(�6$   ��62   �� $	 Login��� 50      ��6$   �� $
	 Text��� 51      ��6$   �� $	 LastTime�$ HH:mm��52      53      55      ]� 57   
   $��2��6   .+�58      �� $	��204��59      $��2q�5:      ��58      �� $	 MessageText� �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=�5:      �� 5;      -   5        $��btSend��5<      $
��Send��5;      $��1q�5=      #��[  5>      ��5?      �� *
sendComm+5@      5A      �� 5C   
   $��3��6    �� Orientation$ 
Horizontal=�6"   .+�5D      �� -   5        $��
chbPrivate��5D      $��10,0,0,0q�5E      $��Center=�5F      $/��)Приватне повідомлення��5G      67   �� 5H      -   5        $��tbRecipient��5I      $��20,3,3,0q�5H      #��[ 5J      $��70��5L      $��20��6!   $	��Top=�6-   ��5K      �� *	Recipient+5L   -   5M      �� 5O   
   -   5        $��
btSendFile��5P      $��4��5O      $��2,2,2,0q�5Q      $��	Send File��5R      {�.  $��20��5T   K   $	��Top=�6W   ��6   �� *sendFileComm+6W   �� 5V   
   $��5��6   $��2q�5W      $��DowndloadFile��5X      {�5Y      �� $	 client.isFileWaiting���5Z      �� *getFileComm+�� 5\   
   -   5        $��	btSignOut��5]      $��6��5\      $��2q�5^      $��
Disconnect��5_      ��5`      �� *	closeComm+5b      5c      
   M S B A M L   `   `   `  ����M  ISolarSystemWarfare, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:SolarSystemWarfareSolarSystemWarfare  5   	   #    SolarSystemWarfare.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:SolarSystemWarfare     5   	   -   5        $��	TopWindow��5   	     � Title$  Solar System Warfare��5	   	    � WindowStartupLocation$ CenterScreen=�5
   	    � WindowState$ 	Maximized=�6.    � 	Resources 5         
EarthImage �� ImageSource  
EnemyImage  	ProjImage  ProjEarthImage  	FullHeart  
EmptyHeart��%^  &	        &	 4     &	 b     &	 �     &	 �     &	 �     �� 5   
   $  Images/EarthBattleShip.png��6)   �� 5   
   $ Images/EnemyShip.png��6)   �� 5   
   $ Images/Projectile.png��6(   �� 5   
   $  Images/ProjectileEarth.png��6-   �� 5   
   $ Images/fullHeart.png��6(   �� 5   
   $ Images/emptyHeart.png��6)   5      .��5      ]� .+�5   
   �� -   5        $��Panel1��5      $
��Left=�6#   $	��400��69   
���   � K� ZIndex$	 100��6X   .+�5      �� -   5        $��
HighScores��5      $��Center=�6&   $
��10pxq�6C   $��High Scores��6R   $��30(�6h   
��� ���6v   �� 5      -   5        $��Scores��5      $��Center=�6"   $
��10pxq�6?   $��High Scores��6N   $��25(�6d   
��� ���6r   5      �� 5   
   -   5        $��Panel2��5      $��Right=�6#   $	��400��6:   
���   �$	 100��6Y   .+�5      �� -   5        $��ScoreLbl��5      
��� ���$��20(�68   $
��Left=�5      $��Score: 0��6/   5      �� 5   
   -   5        $��	GameBoard��5       �� Orientation$ Vertical=�6&   ��5      �� 5      $  Images/spaceBackground.png��6   .+�5!      �� -   5        $��	HeartLine��5!      $ 
Horizontal=�6*   .+�5"      �� -	   5        $��Heart3��5"      $��15��6*   $��20��66   $y�Hidden=�6A   $
��Left=�6U   �� 5#      -
   5        $��Heart2��5#      $��15��6*   $��20��66   $y�Hidden=�6A   $
��Left=�6U   �� 5$      -   5        $��Heart1��5$      $��15��6*   $��20��66   $y�Hidden=�6A   $
��Left=�6U   5%      �� 5&      -   5        $��Space��5&      .+�5'      �� -   5        $��	GameTitle��5'      
�������$��Solar System Warfare��6<   $��40(�6[   $y�Hidden=�6i   �� 5(      -   5        $��GameOverLabel��5(      $��	Game Over��6-   
�������$��50(�6T   $y�Hidden=�6b   �� 5)      -   5        $��MusicCreditsTitle��5)      
�������$��Music Credits��6D   $��50(�6\   $y�Hidden=�6j   �� 5*      -   5        $��MusicCredits1��5*      
�������$0��*"The Lift" Kevin MacLeod (incompetech.com)��6@   $y�Hidden=�6u   �� 5+      -   5        $��MusicCredits2��5+      
�������$A��;Licensed under Creative Commons: By Attribution 3.0 License��6@   $y�Hidden=�6�   �� 5,      -   5        $��MusicCredits3��5,      
�������$1��+http://creativecommons.org/licenses/by/3.0/��6@   $y�Hidden=�6v   �� 5-      -   5        $��EnterHighScore��5-      $��Player1��60    �� 	MaxLength$ 25��6?   $	��100��6N   $y�Hidden=�6Z    � ToolTip$ 
Enter Name��6n   �� 5.      -   5        $��CommitHighScore��5.      $��Enter Score��60   $y�Hidden=�6F   6Z   �� 5/      -   5        $��StartGameBt��5/      $��
Start Game��6,   $y�Hidden=�6A   6U   �� 50      -   5        $��MusicCreditsBt��50      $��Music Credits��6/   $y�Hidden=�6G   6[   �� 51      -   5        $��MusicCreditsBackBt��51      $
��Back��63   $��15(�6B   $y�Hidden=�6P   6d   52      53      54      55      
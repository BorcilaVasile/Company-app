   M S B A M L   `   `   `  ����M  IAdministrare_firma, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullS.clr-namespace:Administrare_firma.MWM.ViewModel Administrare_firma.MWM.ViewModel  5   	   7 clr-namespace:Administrare_firmaAdministrare_firma  5   	   '    !Administrare_firma.MainPageWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   D	viewModel.clr-namespace:Administrare_firma.MWM.ViewModel     5   	   2local clr-namespace:Administrare_firma     5   	     � Title$  MainPageWindow��5	   	   $	��600��5
   	   $	��920��5   	   $��Center=�5   	   $��Center=�5   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	   
������  � AllowsTransparency .  � DataContext 5      4  .Administrare_firma.MWM.ViewModel.MainViewModel  5   
   .��5      �� ���#272537 �� CornerRadius$ 20��5      ._�5   
   �  � ColumnDefinitions 5      �� 5      $	��200 �6#   �� 5       � RowDefinitions 5      �� 5      $��75 �6    �� 5      .+�5"   
   �� $��Elysium��6   $��Center=�5#      $
��Left=�5$      $��25q�5%      
�������$��22(�5'      $��Fonts/Popins)�5(      �� 5*      $��1��6   .+�5+      	� $
��Home��6   $��50��5,      
�������$��14(�5.         MenuButtonTheme#��[  5/       d� 	IsChecked$
 TrueY�50      ��51      �� *HomeViewCommand+	� 52      $��Departments��6   $��50��53      
�������$��14(�55      #��[  56      $
 TrueY�57      ��58      �� *DepartmentsViewCommand+	� 59      $��Managers��6   $��50��5:      
�������$��14(�5<      #��[  5=      $
 TrueY�5>      ��5?      �� *ManagersViewCommand+	� 5@      $��	Employees��6   $��50��5A       
�������$��14(�5C       #��[  5D       $
 TrueY�5E       ��5F       �� *EmployeesViewCommand+5G      �� 5H      $��1��6   
�������$
��Left=�5J      $��Center=�5K      $	��250��5L      $��40��5M      $ 15��5N      	 J� Opacity$		 0.5Q�5O      $��1q�5P      $��20,0,0,0q�5Q      ._�5R      � .+�5S      �� -   5        
 ��Name$
 textBlockUsername��5U      $��Search��5T      $��1��5V        TextBlockStyle#��[ 5W      5X      �� 5Y      -   5         ��Name$ textBoxUsername��5Y        TextBoxStyle#��[ 5Z      $	��230��5\      5]      5^      �� 5_      $��1��6%   $��1��5`   %   $��10q�5a   %   ��5b   %   �� *CurrentView+5c      5e      5f      
   M S B A M L   `   `   `  ����
  WINTRE    WpfApp1.Page3   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Page3��5      .��5
      � ���	#FF1F1C1C.+�5   
   �� $��Technique Name:��6   $
��Left=�6*   $��	10,10,0,0q�6E   $	��Top=�6X   $��40��6p   $	��240��6|   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5   
   $��MITRE ATT&CK ID:��6   $
��Left=�6/   $��	10,50,0,0q�6J   $	��Top=�6]   $��40��6u   $	��240��6�   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5   
   $��Elevated privileges:��6   $
��Left=�6/   $��	10,90,0,0q�6J   $	��Top=�6]   $��40��6u   $	��240��6�   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5   
   $��Category/Tactic:��6   $
��Left=�6+   $��
10,130,0,0q�6F   $	��Top=�6Z   $��40��6r   $	��240��6~   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5   
   $��Selected Template:��6   $
��Left=�6-   $��
10,170,0,0q�6H   $	��Top=�6\   $��40��6t   $	��240��6�   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5   
   -   5         ��Name$ TechniqueName��5      $
��Left=�6)   $��40��6D   $��
255,10,0,0q�6P    �� TextWrapping$
 Wrap=�6d   $"��e.g. Local Account Discovery��6x   $	��Top=�6�   $	��475��6�   $��22(�6�   ���	#FFA3A3A3���	#FF4B4B4B���	#FF4B4B4B6�   �� 5   
   -   5        $ ID��5      $
��Left=�6   $��40��69   $��
255,50,0,0q�6E   $
 Wrap=�6Y   $��
e.g. T1087��6m   $	��Top=�6   $	��185��6�   $��22(�6�   ���	#FFA3A3A3���	#FF4B4B4B���	#FF4B4B4B6�   �� 5   
   -   5         ��Name$ Elevated��5      $
��Left=�6%   $��
255,90,0,0q�6@   $	��Top=�6T   $	��185��6l   $��40��6x   $��16(�6�   ���	#FFACACAC.@�5      �� .��6   No6   �� 5      .��6   Yes6    5      �� 5   
   -   5        $ SelectedTactic��5      $
��Left=�6+   $��255,130,0,0q�6F   $	��Top=�6[   $	��185��6s   $��40��6   $��16(�6�   .@�5      �� .��6   Code Execution6+   �� 5      .��6   Persistence6(   �� 5      .��6   Defence Evasion6,   �� 5      .��6   Credential Theft6-   �� 5      .��6   	Discovery6&   �� 5      .��6   
Collection6'   �� 5      .��6   Data Exfiltration6.   �� 5      .��6   Command and Control60   �� 5      .��6   Impact6#   5       �� 5!   
   -   5        $ SelectedTemplate��5!      $
��Left=�6-   $��255,170,0,0q�6H   $	��Top=�6]   $	��185��6u   $��40��6�   $��16(�6�   .@�5"      �� .��6   CMD6    �� 5#      .��6   
PowerShell6'   5$      �� 5%   
   $��Description:��6   $
��Left=�6'   $��
10,376,0,0q�6B   $	��Top=�6V   $��40��6n   $	��315��6z   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5&   
   -   5        $ CommandInput��5&       �� AcceptsReturn . $
��Left=�6=   $��
10,251,0,0q�6X   $
 Wrap=�6l   $��e.g. net user��6�   $	��Top=�6�   $	��120��6�   $	��720��6�   $��14(�6�   ���	#FF4B4B4B���	#FFA3A3A3���	#FF4B4B4B6  �� 5'   
   -   5         ��Name$ ButtonAddNewTechnique��5'      $��Add New Technique��60   $
��Left=�6L   $��
595,65,0,0q�6g   $	��Top=�6{   $	��135��6�   $��40��6�   ���	#FFACACAC���	#FF4B4B4B6�   �� 5(   
   $��Command(s):��6   $
��Left=�6&   $��
10,211,0,0q�6A   $	��Top=�6U   $��40��6m   $	��240��6y   $��22(�6�   ���	#FF2887B8$��Verdana)�6�   �� 5)   
   -   5        $ DescBox��5)       . $
��Left=�68   $	��259��6S   $��
10,416,0,0q�6`   $
 Wrap=�6t   $���e.g. Monitor for the usage of "net user", this can be achieved using Sysmon and filtering EventData based on CommandLine values��6�   $	��Top=�6  $
��1019��61  $��14(�6>  ���	#FFA3A3A3���	#FF4B4B4B���	#FF4B4B4B6�  �� 5*   
   $P��JMake sure your syntax is correct! Refer to manual for further information.��6   $
��Left=�6e   $��335,220,0,0q�6�   $	��Top=�6�   $��31��6�   $	��395��6�   ���	#FFFF35006�   5+      5,      
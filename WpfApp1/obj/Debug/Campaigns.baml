   M S B A M L   `   `   `  ����
  WINTRE    WpfApp1.Page5   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5        N� Title$  Page5��5      .��5
      � ���	#FF1F1C1C.+�5   
   �� -   5         ��Name$ CampaignTabs��5      $
��Left=�6+   $	��688��6F   $	��Top=�6S   $
��1054��6k   ���	#FF4B4B4B���	#FF4B4B4B.@�5      �� $��Create Campaign��6   ���	#FF4B4B4B���	#FFACACAC.��5      � ���	#FF1F1C1C � ColumnDefinitions 5      �� 5      $
��352* �6+   �� 5      $
��663* �6+   .+�5      �� -   5         ��Name$ clearCampaignFields��5      $��Clear Fields��6:   $
��Left=�6Q   $��158,106,0,0q�6l   $	��Top=�6�   $	��112��6�   $��1��6�   ���	#FF4B4B4B���	#FFACACAC$��24��6  �� 5      -   5        $! SelectTechniquesForCampaign��5      $��Add Technique��6B   $
��Left=�6Z   $��158.5,335,0,0q�6u   $	��Top=�6�   $	��112��6�   $��1��6�   ���	#FFACACAC���	#FF4B4B4B$��24��6  �� 5      $��Campaign Title:��6   $
��Left=�66   $��	31,46,0,0q�6Q   $	��Top=�6d   ���	#FF2887B8$��16(�6�   �� 5      $��Campaign Description:��6   $
��Left=�6<   $��
31,164,0,0q�6W   $	��Top=�6k   ���	#FF2887B8$��16(�6�   �� 5      $��Techniques Summary:��6   $
��Left=�6:   $��158,379,0,0q�6U   $	��Top=�6j   ���	#FF2887B8$��1��6�   $��16(�6�   �� 5      $��Tactics Summary:��6   $
��Left=�67   $��
31,379,0,0q�6R   $	��Top=�6f   ���	#FF2887B8$��16(�6�   �� 5      -   5        $ SaveCampaign��5      $��Save Campaign��63   $
��Left=�6K   $��
158,77,0,0q�6f   $	��Top=�6z   $	��111��6�   $��1��6�   ���	#FF4B4B4B���	#FFACACAC$��24��6�   6�   �� 5      -   5         ��Name$ CampaignTitle��5      $
��Left=�65   $��67��6P   $��	31,77,0,0q�6\    �� TextWrapping$
 Wrap=�6o   $	��Top=�6�   $	��429��6�   $��2��6�   ���	#FF4B4B4B���	#FFA3A3A3���	#FF4B4B4B6�   �� 5      -   5        $ CampaignDesc��5      $
��Left=�64   $	��164��6O   $��
31,195,0,0q�6\   $
 Wrap=�6p   $	��Top=�6�   $	��429��6�   $��2��6�   ���	#FF4B4B4B���	#FFA3A3A3���	#FF4B4B4B6�   �� 5      -   5         ��Name$ SelectedTactic��5      $
��Left=�67   $��158.5,195,0,0q�6R   $	��Top=�6i   $	��428��6�   $��28��6�   $��1��6�   �� 5      -   5        $ SelectedTechnique��5      $
��Left=�6:   $��158.5,259,0,0q�6U   $	��Top=�6l   $	��428��6�   $��28��6�   $��1��6�   �� 5      $��1. Select tactic:��6   $
��Left=�68   $��158.5,164,0,0q�6S   $	��Top=�6j   ���	#FF2887B8$��1��6�   $��16(�6�   �� 5      $��2. Select technique:��6   $
��Left=�6;   $��158.5,228,0,0q�6V   $	��Top=�6m   ���	#FF2887B8$��1��6�   $��16(�6�   �� 5      -	   5         ��Name$ TacticsSummary��5       $
��Left=�68   $��
31,410,0,0q�6S   $
��Wrap=�6g   $	��Top=�6{   $	��240��6�   $	��429��6�   ���	#FF4B4B4B$��2��6�   ���	#FFA3A3A36�   �� 5       -
   5        $ TechniquesSummary��5        $
��Left=�6;   $��158,410,0,0q�6V   $
��Wrap=�6k   $	��Top=�6   $	��240��6�   $	��429��6�   ���	#FF4B4B4B$��1��6�   ���	#FFA3A3A36�   5!      5"      �� 5#      -   5        $��View Campaigns��5#      ���	#FF4B4B4B���	#FFACACAC.��5$      � ���	#FF1F1C1C.+�5%      �� $X��RDouble click the campaign you'd like to run to load its techniques into the queue:��6   $
��Left=�6y   $��	10,12,0,0q�6�   $	��Top=�6�   ���	#FF2887B8$��16(�6�    � Cursor$  ��6�   &   System.Windows.Controls.DataGrid  5&      -   5        	  Name$	 	Campaigns��5&      
   SelectionMode$
 Single=�62   $
��Left=�6I   $	��601��6d   $��	10,49,0,0q�6q   $	��Top=�6�   $
��1012��6�   ���	#FF4B4B4B   AlternatingRowBackground �	#FF4B4B4B   RowBackground �	#FF606060 �� AlternationCount$ 2��6�   6  5'      5(      5)      5*      5+      
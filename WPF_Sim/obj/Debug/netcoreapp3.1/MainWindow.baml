   M S B A M L   `   `   `  ����B  >WPF_Sim, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WPF_SimWPF_Sim  5   	   @ <Model, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null,"clr-namespace:Model;assembly=ModelModel 5   	       WPF_Sim.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WPF_Sim     5   	   4Model"clr-namespace:Model;assembly=Model     5   	     � Title$  
MainWindow��5
   	   $	��450��6   $	��800��6)   $
{�True��65    � DataContext 5         WPF_Sim.Data_Context  5   
   .��5      � ���	#FF525252.+�5   
   �� -   5         ��Name$ 	MainTrack��5      $��24,27,24,22q�6#   ]� 5   
   $��20��6   $	��Top=�6    $	��Top=�68   .+�5      o� .@�5      3  -System.Windows.Controls.Ribbon.RibbonMenuItem  $
��Menu��6!   .@�5      �� -   5        $��
Race Stats��5      62   �� 5      -   5        $��Competition Stats��5      69   �� 5      -   5        ���	#FFA01212
�������$��Competitie afsluiten��5   H   
�������6z   5      5      5      �� 5   
   -   5         ��Name$ 
_trackName��5      $
��Left=�6#  $��0,20,0,0q�6>  $	��Top=�6P  
���������6$   �� *	TrackName+ �� BindsDirectlyToSource$ False�� �� IsAsync$ False�� �� NotifyOnTargetUpdated$ False�� �� NotifyOnValidationError$ False�� �� NotifyOnSourceUpdated$ False��"	 �� ValidatesOnNotifyDataErrors$
	 True��
 �� ValidatesOnExceptions$
 False�� �� ValidatesOnDataErrors$ False��6h  5      5      
   M S B A M L   `   `   `  ����K  GDigitRecognition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DigitRecognitionDigitRecognition  5   	   ,    &DigitRecognition.CreateNeuralNetWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DigitRecognition     5   	     � Title$  Tworzenie sieci��5   	   $	��300��5	   	   $	��300��5
   	    � WindowStartupLocation$ CenterOwner=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      �� 5      �� 5      $��4* �6   �� 5      �� 5      �� 5       � ColumnDefinitions 5   
   �� 5      �� 5      .+�5   
   �� $��Nazwa wartstwy��5      $��Center=�5      $��Center=�5      $
��Bold"�5      �� 5"   
   $��1��5#      $��Liczba neuronów��5$      $��Center=�5%      $��Center=�5&      $
��Bold"�5'      �� 5*   
   $��1��5+      $��
Wejściowa��5,      $��Center=�5-      $��Center=�5.      �� 51   
   -   5        $��inputNeuronTextBox��52      $��1��53      $��1��54      $��5q�55      {�.  $��Center=�57      $��Center=�58      �� 5;   
   $��3��5<      $��
Wyjściowa��5=      $��Center=�5>      $��Center=�5?      �� 5B   
   -   5        $��outputNeuronTextBox��5C      $��3��5D      $��1��5E      $��5q�5F      {�.  $��Center=�5H      $��Center=�5I      �� 5L   
   -   5        $��addLayerButton��5M      $��4��5N      $��Dodaj warstwę��5O      $��3q�5P      5Q      �� 5T   
   -   5        $��removeLayerButton��5U      $��4��5V      $��1��5W      $��Usuń warstwę��5X      $��3q�5Y      {�.  5[      �� 5^   
   -   5        $��okButton��5_      $��5��5`      $��2��5a      $��Gotowe��5b      $��3q�5c      5d      �� 5g   
   $��2��5h      $��2��5i      $��5q�5j      .��5l      � -   5        $��HiddenLayersGrid��5l       5m      �� 5n      �� 5o      5q      5r      5t      5u      
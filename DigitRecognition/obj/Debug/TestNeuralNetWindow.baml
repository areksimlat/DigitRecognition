   M S B A M L   `   `   `  ����K  GDigitRecognition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DigitRecognitionDigitRecognition  5   	   *    $DigitRecognition.TestNeuralNetWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:DigitRecognition     5   	   -   5          � Title$  Testowanie sieci��5   	   $	��400��5	   	   $	��560��5
   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��10q�6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      �� 5      $��40 �6   �� 5      $��30 �6   .+�5   
   ��  �� Orientation$ 
Horizontal=�6   $��Center=�6.   .+�5      �� $��Próbka:��6   �� 5      -   5        $��currentSampleLabel��5      $��0��6.   �� 5      $��/��6   �� 5      -   5        $��allSamplesLabel��5      $��0,0,5,0q�6+   �� 5      $��Oczekiwano:��6   �� 5      -   5        $��expectedValueLabel��5      $��0,0,5,0q�6.   �� 5      $��
Otrzymano:��6   �� 5      -   5        $��receivedValueLabel��5      $��0,0,5,0q�6.   �� 5       $��Prawdopodobieństwo:��6   �� 5!      -   5        $��probabilityLabel��5!      5"      �� 5$   
   $��1��6   .��5%      � -   5        $��
SampleGrid��5%       � ColumnDefinitions 5&      �� 5'      �� 5(      �� 5)      �� 5*      �� 5+       5.      �� 5/      $��30 �6$   .+�52      �� $��Próbka��6   $
��Bold"�6)   $��Center=�6;   $��Center=�6V   �� 53      $��
Poprawnych��6   $
��Bold"�6-   $��1��6?   $��Center=�6O   $��Center=�6j   �� 54      $��	Błędych��6   $
��Bold"�6*   $��2��6=   $��Center=�6M   $��Center=�6h   �� 55      $��
Wszystkich��6   $
��Bold"�6-   $��3��6?   $��Center=�6O   $��Center=�6j   �� 56      $��%��6   $
��Bold"�6$   $��4��66   $��Center=�6F   $��Center=�6a   57      58      � 5:   
   -   5        $��testProgressBar��5:      $��2��6-   $��0,5,0,5q�6:   �� 5<   
   -	   5        $��abortButton��5<      $��3��6$   $��0,5,0,5q�61   $��Przerwij��6B   6U   5=      5>      
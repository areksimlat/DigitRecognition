   M S B A M L   `   `   `  ���� K  GDigitRecognition, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:DigitRecognitionDigitRecognition  *    $DigitRecognition.TestNeuralNetWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:DigitRecognition     -     � Title$  Testowanie sieci��$	��400��$	��560�� � WindowStartupLocation$ CenterOwner=� � 
ResizeMode$ NoResize=�.��� $��10q� � RowDefinitions �� $��40 ��� �� $��40 ��� $��30 �.+���  �� Orientation$ 
Horizontal=�$��Center=�.+��� $��Próbka:���� -   $��currentSampleLabel��$��0���� $��/���� -   $��allSamplesLabel��$��0,0,5,0q��� $��Oczekiwano:���� -   $��expectedValueLabel��$��0,0,5,0q��� $��
Otrzymano:���� -   $��receivedValueLabel��$��0,0,5,0q��� $��Prawdopodobieństwo:���� -   $��probabilityLabel���� $��1��.��� -   $��
SampleGrid�� � ColumnDefinitions �� �� �� �� ��  �� $��30 �.+��� $��Próbka��$
��Bold"�$��Center=�$��Center=��� $��
Poprawnych��$
��Bold"�$��1��$��Center=�$��Center=��� $��	Błędych��$
��Bold"�$��2��$��Center=�$��Center=��� $��
Wszystkich��$
��Bold"�$��3��$��Center=�$��Center=��� $��%��$
��Bold"�$��4��$��Center=�$��Center=�� -   $��testProgressBar��$��2��$��0,5,0,5q��� -	   $��abortButton��$��3��$��0,5,0,5q�$��Przerwij��
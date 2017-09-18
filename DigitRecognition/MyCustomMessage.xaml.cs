using System.Windows;

namespace DigitRecognition
{
    /// <summary>
    /// Logika interakcji dla klasy MyCustomMessage.xaml
    /// </summary>
    public partial class MyCustomMessage : Window
    {
        public string Message
        {
            get { return Message; }
            set { Text.Content = value; }
        }

        public MyCustomMessage()
        {
            InitializeComponent();
        }
    }
}

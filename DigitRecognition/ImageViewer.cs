using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DigitRecognition
{
    class ImageViewer
    {
        public int Width { get; }
        public int Height { get; }
        private Label[][] labels;        


        public ImageViewer(Grid imageGrid, int height, int width)
        {
            Height = height;
            Width = width;

            initLabels();
            initGrid(imageGrid);
        }

        private void initLabels()
        {
            labels = new Label[Height][];

            for (int i = 0; i < Height; i++)
                labels[i] = new Label[Width];
        }

        private void initGrid(Grid imageGrid)
        {
            for (int i = 0; i < Height; i++)
                imageGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < Width; i++)
                imageGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Label label = new Label()
                    {
                        Background = Brushes.White
                    };

                    Grid.SetRow(label, i);
                    Grid.SetColumn(label, j);
                    imageGrid.Children.Add(label);
                    labels[i][j] = label;
                }
            }
        }

        public void Draw(BitArray pixels)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    labels[i][j].Background = pixels[i * Height + j] ? Brushes.Black : Brushes.White;
        }

        public void ShowGrid()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    labels[i][j].BorderThickness = new Thickness(0.3);
                    labels[i][j].BorderBrush = Brushes.Red;
                }
        }

        public void HideGrid()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    labels[i][j].BorderThickness = new Thickness(0);
        }

        public void EnableEdit()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    labels[i][j].MouseDown += Label_MouseDown;
                    labels[i][j].MouseMove += Label_MouseMove;
                }
        }

        public void DisableEdit()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                {
                    labels[i][j].MouseDown -= Label_MouseDown;
                    labels[i][j].MouseMove -= Label_MouseMove;
                }                    
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Label label = (Label)e.Source;

            if (e.LeftButton == MouseButtonState.Pressed)
                label.Background = Brushes.Black;
            else if (e.RightButton == MouseButtonState.Pressed)
                label.Background = Brushes.White;
        }

        private void Label_MouseMove(object sender, MouseEventArgs e)
        {
            Label label = (Label)e.Source;

            if (e.LeftButton == MouseButtonState.Pressed)
                label.Background = Brushes.Black;
            else if (e.RightButton == MouseButtonState.Pressed)
                label.Background = Brushes.White;
        }

        public void Clear()
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    labels[i][j].Background = Brushes.White;
        }

        public BitArray GetCurrentImage()
        {
            BitArray pixels = new BitArray(Height * Width);

            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    pixels[i * Height + j] = (labels[i][j].Background == Brushes.Black);

            return pixels;
        }
    }
}

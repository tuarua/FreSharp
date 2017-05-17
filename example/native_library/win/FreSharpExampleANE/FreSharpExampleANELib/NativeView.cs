using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;

namespace FreExampleSharpLib {
    public partial class NativeView {
        private Image _button;
        public void Init() {
            InitializeComponent();
        }

        public BitmapSource Convert(Bitmap bitmap) {
            var bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);

            var bitmapSource = BitmapSource.Create(
                bitmapData.Width, bitmapData.Height, 96, 96, PixelFormats.Bgra32, null,
                bitmapData.Scan0, bitmapData.Stride * bitmapData.Height, bitmapData.Stride);

            bitmap.UnlockBits(bitmapData);
            return bitmapSource;
        }

        public void AddImage(Bitmap image) {
            _button = new Image {
                Width = image.Width,
                Height = image.Height,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Source = Convert(image),
                Margin = new Thickness(256,0,0,0)
            };
            MainGrid.Children.Add(_button);
        }

    }
}
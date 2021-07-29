using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;

namespace LilliCad_Sample {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        private void FileOpen_Click(object sender, RoutedEventArgs e) {
            var d = new OpenFileDialog {
                Filter = "lcd files (*.lcd)|*.lcd"
            };
            if (d.ShowDialog() == true) {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                using var r = new StreamReader(d.FileName, Encoding.GetEncoding("shift_jis"));
                var reader = new LilliCadReader();
                var s = reader.Read(r);
                Part_Output.Text = s;
            }
        }
    }
}

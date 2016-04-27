using System.Windows;
using System.Windows.Data;
using iFactr.Core;
using iFactr.Core.Layers;
using iFactr.Wpf;
using System.ComponentModel;

namespace Customers.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            WpfFactory.Initialize();
            iApp.OnLayerLoadComplete += (iLayer layer) => { WpfFactory.Instance.OutputLayer(layer); };

            //Instantiate your iFactr application and set the Factory App property
            WpfFactory.TheApp = new Customers.MyApp();

            Content = WpfFactory.Instance.MainWindow;
            iApp.Navigate(WpfFactory.TheApp.NavigateOnLoad);
        }

        // Prevent accidental closing by offering a pop-up confirmation message when user closes window
        protected override void OnClosing(CancelEventArgs e)
        {
            if (iApp.CurrentNavContext.ActiveLayer != null
                && !iApp.CurrentNavContext.ActiveLayer.Name.Contains("Login"))
                e.Cancel = MessageBox.Show("Do you want to exit?", null, MessageBoxButton.YesNo) == MessageBoxResult.No;
            base.OnClosing(e);
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LockedCounter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TimeCounter Counter { get; private set; }
        public MainWindow()
        {
            InitializeComponent();
            Counter = DI.Get<TimeCounter>();
            DataContext = Counter;
            Counter.Start();
        }

        protected async override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            await Counter.Stop();
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            Counter.Reset();
        }

        private void Statistics_Click(object sender, RoutedEventArgs e)
        {
            new StatisticsWindow().Show();
        }
    }
}

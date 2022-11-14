using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace WPF_Sim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += OnDriversChanged;

            WPFVisual.DrawTrack(Data.CurrentRace.track);

            DriversChangedEventArgs changedArgs = new DriversChangedEventArgs();
            changedArgs.track = Data.CurrentRace.track;
        }

        public void OnDriversChanged(DriversChangedEventArgs dc)
        {
            this.MainTrack.Dispatcher.BeginInvoke(
                DispatcherPriority.Render,
                new Action(() =>
                {
                    this.MainTrack.Source = null;
                    this.MainTrack.Source = WPFVisual.DrawTrack(Data.CurrentRace.track);
                }));

        }
    }
}

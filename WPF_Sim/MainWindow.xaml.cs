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
        private RaceStats _raceStats;
        private CompetitionStats _competitionStats;
        private Data_Context Data_Context;
        public MainWindow()
        {
            InitializeComponent();
            Data_Context = new Data_Context();
            this.DataContext = Data_Context;
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
                    if(dc.track == null)
                    {
                        this.MainTrack.Source = null;
                        Data.CurrentRace.DriversChanged -= OnDriversChanged;
                        Data.CurrentRace.DriversChanged -= Data_Context.OnDriversChanged;
                        if (_competitionStats != null)
                        {
                            //Data.CurrentRace.DriversChanged -= _competitionStats.OnDriversChanged;
                            Data.CurrentRace.DriversChanged -= _competitionStats.Data_Context.OnDriversChanged;
                        }
                        Data.NextRace();
                        if(_competitionStats != null)
                        {
                            //Data.CurrentRace.DriversChanged += _competitionStats.OnDriversChanged;
                            Data.CurrentRace.DriversChanged += _competitionStats.Data_Context.OnDriversChanged; 
                        }
                        Data.CurrentRace.DriversChanged += this.OnDriversChanged;
                        Data.CurrentRace.DriversChanged += Data_Context.OnDriversChanged;
                        ImageProcessor.ClearCache();
                        ImageProcessor.DrawBackground(1280, 750);
                        //Thread.Sleep(4000);
                    }
                    this.MainTrack.Source = null;
                    this.MainTrack.Source = WPFVisual.DrawTrack(Data.CurrentRace.track);
                }));
            Data_Context.OnLapTimes(dc);
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_RaceStats_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Racestats is geopend hopelijk");
            _raceStats = new RaceStats();
            _raceStats.Show();
            
        }

        private void MenuItem_CompetitionStats_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Competition is geopend hopelijk");
            _competitionStats = new CompetitionStats();
            _competitionStats.Show();
            
        }
    }
}

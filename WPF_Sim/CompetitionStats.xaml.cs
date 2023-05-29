using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WPF_Sim
{
    /// <summary>
    /// Interaction logic for CompetitionStats.xaml
    /// </summary>
    public partial class CompetitionStats : Window
    {
        public Data_Context Data_Context;
        public CompetitionStats()
        {
            InitializeComponent();
            Data_Context = new Data_Context();
            Data.CurrentRace.DriversChanged += Data_Context.OnDriversChanged;
            this.DataContext = Data_Context;

        }

        //public void OnDriversChanged(DriversChangedEventArgs dc)
        //{
        //    if (dc.track == null)
        //    {
        //        data.currentrace.driverschanged -= ondriverschanged;
        //        data.currentrace.driverschanged -= data_context.ondriverschanged;
        //        Data.CurrentRace.DriversChanged += this.OnDriversChanged;
        //        Data.CurrentRace.DriversChanged += Data_Context.OnDriversChanged;
        //        Thread.Sleep(4000);
        //    }
        //}
    }
}

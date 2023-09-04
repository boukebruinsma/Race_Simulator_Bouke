using Controller;
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

namespace WPF_Sim
{
    /// <summary>
    /// Interaction logic for RaceStats.xaml
    /// </summary>
    public partial class RaceStats : Window
    {
        public Data_Context Data_Context;
        public RaceStats()
        {
            InitializeComponent();
            Data_Context = new Data_Context();
            Data.CurrentRace.DriversChanged += Data_Context.OnDriversChanged;
            this.DataContext = Data_Context;
        }
    }
}

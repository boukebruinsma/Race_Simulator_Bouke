using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WPF_Sim
{
    public class Data_Context : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TrackName { get; set; }
        public string LastLaptime { get; set; }

        public Data_Context()
        {
            TrackName = "Baan 1";
            LastLaptime = "-";
        }

        public void OnDriversChanged(DriversChangedEventArgs dc)
        {
            TrackName = Data.CurrentRace.track.Name;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }

        public void OnLapTimes(DriversChangedEventArgs dc)
        {
            if(Data.Competition.TimeController._list.Count > 0)
            {
                LastLaptime = Data.Competition.TimeController._list[0].ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
            }
        }
    }
}

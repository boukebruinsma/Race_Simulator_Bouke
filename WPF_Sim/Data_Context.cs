using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace WPF_Sim
{
    public class Data_Context : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string TrackName { get; set; }
        public double LastLaptime { get; set; }
        public double LastLaptime2 { get; set; }

        public string LapTimeAantal { get; set; }

        public ObservableCollection<double> ListOfLaptimes1 { get; set; }
        public ObservableCollection<double> ListOfLaptimes2 { get; set; }

        public ObservableCollection<String> TotalPoints1 { get; set; }
        public ObservableCollection<String> TotalPoints2 { get; set; }

        public String FastestLap { get; set; }

        public Data_Context()
        {
            if(Data.CurrentRace != null)
            {
                if(Data.CurrentRace.track != null)
                {
                    TrackName = Data.CurrentRace.track.Name;
                } 
            }
            LastLaptime = 0;
            LastLaptime2 = 0;
            LapTimeAantal = "0";
            FastestLap = "-";
            ListOfLaptimes1 = new ObservableCollection<double>();
            ListOfLaptimes2 = new ObservableCollection<double>();
            TotalPoints1 = new ObservableCollection<String>();
            TotalPoints2 = new ObservableCollection<String>();
            
        }

        public void OnDriversChanged(DriversChangedEventArgs dc)
        {
            TrackName = Data.CurrentRace.track.Name;
            if (Data.Competition.TimeController._list.Count > 0)
            {
                //ListOfLaptimes.Clear();
                List<GetLapTime> timeControllerList = Data.Competition.TimeController._list.Cast<GetLapTime>().ToList();

                if (dc.track == null)
                {
                    List<GetPoints> pointsControllerList = Data.Competition.PointsController._list.Cast<GetPoints>().ToList();                    

                    App.Current.Dispatcher.Invoke((Action)delegate
                    {
                        TotalPoints1.Add("After " + TrackName + ": " + pointsControllerList.Where(i => i.Name.Equals("Max")).First().ScoredPoints + " points scored");
                        TotalPoints2.Add("After " + TrackName + ": " + pointsControllerList.Where(i => i.Name.Equals("Charles")).First().ScoredPoints + " points scored");
                        
                        ListOfLaptimes1.Clear();
                        ListOfLaptimes2.Clear();
                    });
                }

                //om laatste laptime op te slaan
                if (timeControllerList[0].LapTimes != null)
                {                    
                    LastLaptime = timeControllerList[0].LapTimes.Last();
                }
                if (timeControllerList.Count > 1)
                {                    
                    LastLaptime2 = timeControllerList[1].LapTimes.Last();
                }

                //om laptimes te weergeven
                if (timeControllerList.Count > 1)
                {
                    //Links
                    if (ListOfLaptimes1.Count > 0)
                    {
                        if (!timeControllerList[0].LapTimes.Last().Equals(ListOfLaptimes1.Last()))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                ListOfLaptimes1.Add(timeControllerList[0].LapTimes.Last());
                                FastestLap = Data.Competition.TimeController.FindBestParticipant();
                            });
                        }
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ListOfLaptimes1.Add(timeControllerList[0].LapTimes.Last());
                        });
                    }


                    //Rechts:
                    if (ListOfLaptimes2.Count > 0)
                    {
                        if (!timeControllerList[1].LapTimes.Last().Equals(ListOfLaptimes2.Last()))
                        {
                            App.Current.Dispatcher.Invoke((Action)delegate
                            {
                                ListOfLaptimes2.Add(timeControllerList[1].LapTimes.Last());
                                FastestLap = Data.Competition.TimeController.FindBestParticipant();
                            });
                        }
                    }
                    else
                    {
                        App.Current.Dispatcher.Invoke((Action)delegate
                        {
                            ListOfLaptimes2.Add(timeControllerList[1].LapTimes.Last());
                        });
                    }
                }
            }
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(""));
        }
    }
}

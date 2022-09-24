using System;
using System.Threading;
using Controller;
using Model;

namespace Race_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Visual.Initialize();

            
            Visual.DrawTrack(Data.CurrentRace.track);
            Data.CurrentRace.DriversChanged += Visual.OnDriversChanged;
            DriversChangedEventArgs changedArgs = new DriversChangedEventArgs();
            changedArgs.track = Data.CurrentRace.track;
            Thread.Sleep(3000);
            for (; ; )
            {

                Thread.Sleep(1000);
            }
        }
    }
}

using Controller;
using NUnit.Framework;
using System.Collections.Generic;

namespace ControllerTest
{
    [TestFixture]
    class Controller_Race_UpdateTimeShould
    {
        private Race _race;

        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            Data.NextRace();
            _race = Data.CurrentRace;
        }

        [Test]
        public void UpdateTime_ShouldAddNewLapTimeWhenNull()
        {
            _race.LapTimesLeftRight[0].LapTimes = null;

            _race.UpdateTimes(0); 
            //checken of er nu een laptime in zit
            Assert.IsNotNull(_race.LapTimesLeftRight[0].LapTimes);
            //tellen of er precies 1 lap in de laptimes zit
            Assert.AreEqual(_race.LapTimesLeftRight[0].LapTimes.Count, 1);
            //checken of de sectiontimes geleegd zijn
            Assert.IsEmpty(_race.LapTimesLeftRight[0].SectionTimes);
        }
        [Test]
        public void UpdateTime_ShouldAddSecondLapTime()
        {
            _race.LapTimesLeftRight[0].LapTimes = new List<double>();
            _race.LapTimesLeftRight[0].LapTimes.Add(4.0);

            _race.UpdateTimes(0);
            //checken of er een laptime in zit
            Assert.IsNotNull(_race.LapTimesLeftRight[0].LapTimes);
            //tellen of er precies 2 laps in de laptimes zit
            Assert.AreEqual(_race.LapTimesLeftRight[0].LapTimes.Count, 2);
            //checken of de sectiontimes geleegd zijn
            Assert.IsEmpty(_race.LapTimesLeftRight[0].SectionTimes);
        }

    }
}

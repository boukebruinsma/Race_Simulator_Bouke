using Controller;
using NUnit.Framework;

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
            _race._lapTimesLeftRight[0].LapTimes = null;

            _race.UpdateTimes(0);
            //checken of er nu een laptime in zit
            Assert.IsNotNull(_race._lapTimesLeftRight[0].LapTimes);
            //tellen of er precies 1 lap in de laptimes zit
            Assert.AreEqual(_race._lapTimesLeftRight[0].LapTimes.Count, 1);
            //checken of de sectiontimes geleegd zijn
            Assert.IsEmpty(_race._lapTimesLeftRight[0].SectionTimes);

        }

    }
}

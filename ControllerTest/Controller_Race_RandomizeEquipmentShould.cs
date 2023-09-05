using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Controller_Race_RandomizeEquipmentShould
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
        public void RandomizeEquipment_SpeedAndPerformanceShouldBeBetween70And100()
        {
            //Als de speed en performance tussen de 70 en 100 zijn dan klopt het, want de standaard waardes zijn in Data.cs op 0 gezet. 
            //De randomizeequipment functie geeft het dan een waarde tussen de 70 en 100.
            _race.RandomizeEquipment();

            //Speed van Links
            Assert.GreaterOrEqual(_race.participants[0].Equipment.Speed, 70);
            Assert.LessOrEqual(_race.participants[0].Equipment.Speed, 100);
            //Performance van Links
            Assert.GreaterOrEqual(_race.participants[0].Equipment.Performance, 70);
            Assert.LessOrEqual(_race.participants[0].Equipment.Performance, 100);
            //Speed van Rechts
            Assert.GreaterOrEqual(_race.participants[1].Equipment.Speed, 70);
            Assert.LessOrEqual(_race.participants[1].Equipment.Speed, 100);
            //Performance van Rechts
            Assert.GreaterOrEqual(_race.participants[1].Equipment.Performance, 70);
            Assert.LessOrEqual(_race.participants[1].Equipment.Performance, 100);
        }

        [Test]
        public void RandomizeEquipment_RandomEquipmentBreaking()
        {
            IParticipant testParticipant = _race.participants[0];
            int oldSpeed = testParticipant.Equipment.Speed;
            int oldQuality = testParticipant.Equipment.Quality;
            for (int i = 0; i < 1000; i++)
            {
                _race.RandomizeEquipmentIsBroken(testParticipant);
            }

            Assert.Less(testParticipant.Equipment.Speed, oldSpeed);
            Assert.Less(testParticipant.Equipment.Quality, oldQuality);
        }
    }

}

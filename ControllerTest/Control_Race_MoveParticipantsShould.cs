using Controller;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Control_Race_MoveParticipantsShould
    {
        Race _race;

        [SetUp]
        public void SetUp()
        {
            Data.Initialize();
            Data.NextRace();
            _race = Data.CurrentRace;
        }

        [Test]
        public void MoveParticipants_Participants_Have_Moved()
        {
            //Test links en rechts of ze aan het begin van de race op 0 null staan
            Assert.AreEqual(_race.positiesOpBaan[Data.Competition.Participants[0]], 0);
            Assert.AreEqual(_race.positiesOpBaan[Data.Competition.Participants[1]], 0);
            _race.MoveParticipants();
            //Test links en rechts of ze zijn bewogen
            Assert.Greater(_race.positiesOpBaan[Data.Competition.Participants[0]], 0); 
            Assert.Greater(_race.positiesOpBaan[Data.Competition.Participants[1]], 0); 
        }

        [Test]
        public void MoveParticipants_Broken_Dont_Move()
        {
            int position = _race.positiesOpBaan[Data.Competition.Participants[0]];
            Data.Competition.Participants[0].Equipment.IsBroken = true;
            _race.MoveParticipants();
            //testen dat de participant niet beweegt wanneer equipment.isbroken is true
            Assert.AreEqual(position, _race.positiesOpBaan[Data.Competition.Participants[0]]);
        }
    }
}

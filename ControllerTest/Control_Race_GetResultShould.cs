using Controller;
using Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace ControllerTest
{
    [TestFixture]
    class Control_Race_GetResultShould
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
        public void GetResult_WinnerPoints_Are_Added()
        {
            _race.GetResult(0);
            //pointscontroller is niet leeg
            Assert.IsNotEmpty(Data.Competition.PointsController._list);
            //winnaar heeft 25 punten gekregen
            GetPoints winnerPoints = Data.Competition.PointsController._list[0] as GetPoints;
            Assert.AreEqual(25, winnerPoints.ScoredPoints);
            //tweede plek heeft 18 punten gekregen
            GetPoints loserPoints = Data.Competition.PointsController._list[1] as GetPoints;
            Assert.AreEqual(18, loserPoints.ScoredPoints);
        }
    }
}

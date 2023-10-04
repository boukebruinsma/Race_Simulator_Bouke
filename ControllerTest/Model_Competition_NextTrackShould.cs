using System;
using Controller;
using Model;
using NUnit.Framework;

namespace ControllerTest
{
    [TestFixture]
    public class Model_Competition_NextTrackShould
    {
        private Competition _competition;

        [SetUp]
        public void SetUp()
        {
            _competition = new Competition();
        }

        [Test]
        public void NextTrack_EmptyQueue_ReturnNull()
        {
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_OneInQueue_ReturnTrack()
        {
            SectionTypes[] sectionTypes = { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            Track testTrack = new Track("TestBaan", sectionTypes);
            _competition.Tracks.Enqueue(testTrack);
            var result = _competition.NextTrack();                        
            Assert.AreEqual(testTrack, result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] sectionTypes = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            _competition.Tracks.Enqueue(new Track("TestBaan", sectionTypes));
            _competition.NextTrack();
            var result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] sectionTypes = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            Track testTrack = new Track("TestBaan", sectionTypes);
            Track testTrack2 = new Track("TestBaan2", sectionTypes);
            _competition.Tracks.Enqueue(testTrack);
            _competition.Tracks.Enqueue(testTrack2);
            var result1 = _competition.NextTrack();
            var result2 = _competition.NextTrack();
            Assert.AreEqual(testTrack, result1);
            Assert.AreEqual(testTrack2, result2);
        }
    }
}

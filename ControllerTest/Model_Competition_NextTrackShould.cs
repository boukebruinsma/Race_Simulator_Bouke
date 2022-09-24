using System;
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
            SectionTypes[] sectionTypes = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            _competition.Tracks.Enqueue(new Track("TestBaan", sectionTypes));
            var result = _competition.NextTrack();                        
            Assert.AreEqual(_competition.Tracks.Dequeue(), result);
        }

        [Test]
        public void NextTrack_OneInQueue_RemoveTrackFromQueue()
        {
            SectionTypes[] sectionTypes = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            _competition.Tracks.Enqueue(new Track("TestBaan", sectionTypes));
            var result = _competition.NextTrack();
            result = _competition.NextTrack();
            Assert.IsNull(result);
        }

        [Test]
        public void NextTrack_TwoInQueue_ReturnNextTrack()
        {
            SectionTypes[] sectionTypes = { SectionTypes.Straight, SectionTypes.LeftCorner, SectionTypes.Finish };
            _competition.Tracks.Enqueue(new Track("TestBaan", sectionTypes));
            var result1 = _competition.NextTrack();
            _competition.Tracks.Enqueue(new Track("TestBaan2", sectionTypes));
            var result2 = _competition.NextTrack();
            Assert.AreEqual(_competition.Tracks.Dequeue(), result1);
            Assert.AreEqual(_competition.Tracks.Dequeue(), result2);
        }
    }
}

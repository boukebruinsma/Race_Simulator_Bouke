using System;
using Model;
namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();

        }
        public static void AddParticipants()
        {
            Competition.Participants.Add(new Driver { Name = "Max", Equipment = new Car { Speed = 100, Performance = 100, Quality = 25 } });
            Competition.Participants.Add(new Driver { Name = "Charles", Equipment = new Car { Speed = 100, Performance = 100, Quality = 24} });

        }

        public static void AddTracks()
        {
            SectionTypes[] sectionTypes = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner};           

            //SectionTypes[] sectionTypes = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight};

            Competition.Tracks.Enqueue(new Track("Baan 1", sectionTypes));            
            Competition.Tracks.Enqueue(new Track("Baan 2", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 3", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 4", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 5", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 6", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 7", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 8", sectionTypes));
        }

        public static void NextRace()
        {
            if (!(Competition.Tracks.Equals(null)))
            {

                CurrentRace = new Race(Competition.NextTrack(), Competition.Participants);
            }
        }
    }
}




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
            Competition.Participants.Add(new Driver { Name = "Max", Equipment = new Car { Speed = 100, Performance = 100 } });
            Competition.Participants.Add(new Driver { Name = "Charles", Equipment = new Car { Speed = 25, Performance = 25 } });

        }

        public static void AddTracks()
        {
            SectionTypes[] sectionTypes = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner};           

            //SectionTypes[] sectionTypes = { SectionTypes.RightCorner, SectionTypes.Finish, SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.Straight};

            Competition.Tracks.Enqueue(new Track("Baan 1", sectionTypes));            
            Competition.Tracks.Enqueue(new Track("Baan 2", sectionTypes));
            Competition.Tracks.Enqueue(new Track("Baan 3", sectionTypes));
        }

        public static void NextRace()
        {
            if (!(Competition.NextTrack().Equals(null)))
            {
                CurrentRace = new Race(Competition.NextTrack(), Competition.Participants);
            }
        }
    }
}




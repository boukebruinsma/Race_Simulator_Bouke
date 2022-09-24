using System;
using System.Collections.Generic;
namespace Model
{
    public class Competition
    {
        public List<IParticipant> Participants { get; set; }
        public Queue<Track> Tracks { get; set; }

        public Competition()
        {
            Tracks = new Queue<Track>();
            Participants = new List<IParticipant>();
        }

        public Track NextTrack()
        {
            if (Tracks.Count == 0)
            {
                return null;
            }
            return Tracks.Dequeue();
        }
    }
}

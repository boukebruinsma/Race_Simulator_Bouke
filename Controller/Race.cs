using System;
using Model;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;

namespace Controller
{
    public class Race
    {
        public Track track { get; set; }
        public List<IParticipant> participants { get; set; }
        
        public DateTime StartTime { get; set; }
        

        private Random _random;
        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();

        //de value is hoever de participant heeft gereden
        private Dictionary<IParticipant, int> _positiesOpBaan = new Dictionary<IParticipant, int>();
        private Dictionary<IParticipant, int> _rondjesGeredenPerDeelnemer = new Dictionary<IParticipant, int>();


        private Timer timer;

        public delegate void Changed(DriversChangedEventArgs dc);

        public event Changed DriversChanged;

        public Race(Track track, List<IParticipant> participants)
        {
            this.track = track;
            this.participants = participants;
            //RandomizeEquipment();
            PositionParticipants(track, participants);
            foreach (IParticipant participant in participants)
            {
                _positiesOpBaan.Add(participant, 0);
                _rondjesGeredenPerDeelnemer.Add(participant, 0);
            }
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEvent;
            timer.AutoReset = true;
            timer.Start();
            
        }

        public void PublishDriversChanged(DriversChangedEventArgs e)
        {
            //PositionParticipants(track, participants);
            foreach (KeyValuePair<IParticipant, int> item in _positiesOpBaan)
            {
                Debug.WriteLine(item.Key.Name + ": " + item.Value); 
            }
            MoveParticipants();
            DriversChanged(e);
            //Debug.WriteLine("huidige posities: ");
            //Debug.WriteLine("------------------------");
            //foreach (var data in _positions)
            //{

            //    if(data.Value.Left != null)
            //    {
            //        Debug.WriteLine("Linker coureur: " + data.Value.Left.Name);
            //    }

            //    if (data.Value.Right != null)
            //    {
            //        Debug.WriteLine("Rechter coureur: " + data.Value.Right.Name);
            //    }

                
            //    Debug.WriteLine("-");
            //}
        }

        public void OnTimedEvent(object sender, EventArgs e)
        {

            PublishDriversChanged(new DriversChangedEventArgs { track = this.track});
        }

        public void Start()
        {
            timer.Enabled = true;
        }

        public SectionData GetSectionData(Section section)
        {            
            if (_positions.TryGetValue(section, out var data))
            {
                return data;
            }

            return new SectionData();
        }

        public void RandomizeEquipment()
        {
            int count = participants.Count;
            for(int i = 0; i < count; i++)
            {
                _random = new Random(DateTime.Now.Millisecond);
                participants[i].Equipment.Quality = _random.Next();
                participants[i].Equipment.Performance = _random.Next();
            }
        }

        private void PositionParticipants(Track track, List<IParticipant> participants)
        {
            int nextParticipant = 0;
            
            foreach (Section section in track.Sections)
            {
                var sectionData = GetSectionData(section);
 
                _positions[section] = sectionData;
                if (section.SectionType == SectionTypes.StartGrid && participants.Count > nextParticipant)
                {
                    sectionData.Left = participants[nextParticipant++];
                    //sectionData.LeftStartTime = DateTime.Now;
                    if (participants.Count > nextParticipant)
                    {
                        sectionData.Right = participants[nextParticipant++];
                        //sectionData.RightStartTime = DateTime.Now;
                    }

                    _positions[section] = sectionData;
                }
            }
            
        }

        public void MoveParticipants()
        {
            Section previousSection = track.Sections.Last.Value;
            Dictionary<Section, SectionData> copiedPositions = _positions;
            int counter = 0;
            bool leftIsDone = false;
            bool rightIsDone = false;
            foreach (Section section in track.Sections)
            {
                
                if(copiedPositions[previousSection].Left != null && !leftIsDone)
                {
                    int distancePerCount = (copiedPositions[previousSection].Left.Equipment.Performance + copiedPositions[previousSection].Left.Equipment.Speed) / 2;
                    _positiesOpBaan[copiedPositions[previousSection].Left] += distancePerCount;
                    
                    int afstandAanEindSection = counter - 1;
                    if (counter < 2)
                    {
                        afstandAanEindSection = counter + 7;
                    }

                    if (_positiesOpBaan[copiedPositions[previousSection].Left] >= afstandAanEindSection * 100)
                    {

                        if (_positiesOpBaan[_positions[previousSection].Left] >= 800)
                        {
                            _rondjesGeredenPerDeelnemer[_positions[previousSection].Left]++;
                            _positiesOpBaan[_positions[previousSection].Left] -= 800;
                            if (_rondjesGeredenPerDeelnemer[_positions[previousSection].Left] == 2)
                            {
                                _positions[previousSection].Left = null;
                            }
                        }
                    

                        if (counter == 8)
                        {
                            _positions[track.Sections.First.Value].Left = copiedPositions[track.Sections.Last.Value].Left;
                            _positions[track.Sections.Last.Value].Left = copiedPositions[track.Sections.First.Value].Left;

                        }
                        else
                        {
                            IParticipant backup = _positions[section].Left;
                            _positions[section].Left = _positions[previousSection].Left;
                            _positions[previousSection].Left = backup;
                        }

                        leftIsDone = true;
                    }

                }

                if (copiedPositions[previousSection].Right != null && !rightIsDone)
                {
                    int distancePerCount = (copiedPositions[previousSection].Right.Equipment.Performance + copiedPositions[previousSection].Right.Equipment.Speed) / 2;
                    _positiesOpBaan[copiedPositions[previousSection].Right] += distancePerCount;

                    int afstandAanEindSection = counter - 1;
                    if (counter < 2)
                    {
                        afstandAanEindSection = counter + 7;
                    }

                    if (_positiesOpBaan[copiedPositions[previousSection].Right] >= afstandAanEindSection * 100)
                    {

                        if (_positiesOpBaan[_positions[previousSection].Right] >= 800)
                        {
                            _rondjesGeredenPerDeelnemer[_positions[previousSection].Right]++;
                            _positiesOpBaan[_positions[previousSection].Right] -= 800;
                            if(_rondjesGeredenPerDeelnemer[_positions[previousSection].Right] == 2)
                            {
                                _positions[previousSection].Right = null;
                            }
                        }

                        if (counter == 8)
                        {
                            _positions[track.Sections.First.Value].Right = copiedPositions[track.Sections.Last.Value].Right;
                            _positions[track.Sections.Last.Value].Right = copiedPositions[track.Sections.First.Value].Right;

                        }
                        else
                        {
                            IParticipant backup = _positions[section].Right;
                            _positions[section].Right = _positions[previousSection].Right;
                            _positions[previousSection].Right = backup;
                        }

                        rightIsDone = true;
                    }

                }

                if(rightIsDone && leftIsDone)
                {
                    break;
                }

                previousSection = section;
                counter++;
            }


            
            
            
        }

    }
}

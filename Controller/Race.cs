using System;
using Model;
using System.Collections.Generic;
using System.Timers;
using System.Diagnostics;
using System.Linq;

namespace Controller
{
    public class Race
    {
        public Track track { get; set; }
        public List<IParticipant> participants { get; set; }
        
        public DateTime StartTime { get; set; }
        

        private Random _random;

        private int _rondjesTeRijden = 2;
        private bool leftIsDone = false;
        private bool rightIsDone = false;
        private bool leftFinishedFirst;
        private Stopwatch watchLeft = new Stopwatch();
        private Stopwatch watchRight = new Stopwatch();

        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();

        //de value is hoever de participant heeft gereden
        private Dictionary<IParticipant, int> _positiesOpBaan = new Dictionary<IParticipant, int>();
        private Dictionary<IParticipant, int> _rondjesGeredenPerDeelnemer = new Dictionary<IParticipant, int>();
        private GetLapTime _lapTimeLeft = new GetLapTime();
        private GetLapTime _lapTimeRight = new GetLapTime();


        private Timer timer;

        public delegate void Changed(DriversChangedEventArgs dc);

        public event Changed DriversChanged;

        public Race(Track track, List<IParticipant> participants)
        {
            if (track != null)
            {
                StartTime = DateTime.Now;
                _random = new Random();
                this.track = track;
                this.participants = participants;
                Debug.WriteLine("dit is het begin van de race. De baan is: " + track.Name);
                RandomizeEquipment();
                PositionParticipants(track, participants);
                foreach (IParticipant participant in participants)
                {
                    _positiesOpBaan.Add(participant, 0);
                    _rondjesGeredenPerDeelnemer.Add(participant, 0);
                }
                _lapTimeLeft.Name = participants[0].Name;
                _lapTimeLeft.SectionTimes = new Dictionary<Section, TimeSpan>();

                _lapTimeRight.Name = participants[1].Name;
                _lapTimeRight.SectionTimes = new Dictionary<Section, TimeSpan>();

                watchLeft.Start();
                watchRight.Start();
                timer = new Timer(500);
                timer.Elapsed += OnTimedEvent;
                timer.AutoReset = true;
                timer.Start();
            }
            
        }

        public void PublishDriversChanged(DriversChangedEventArgs e)
        {
            
            MoveParticipants();
            if (rightIsDone && leftIsDone)
            {
                if (leftFinishedFirst)
                {
                    GetResult(0);
                }
                else
                {
                    GetResult(1);
                }
                DisposeRace();
            }
            else
            {
                DriversChanged(e);
            }
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
                participants[i].Equipment.Speed = _random.Next(70, 100);
                participants[i].Equipment.Performance = _random.Next(70, 100);
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

        public void RemoveBrokenSign(int leftOrRight)
        {
            Data.Competition.Participants[leftOrRight].Name = Data.Competition.Participants[leftOrRight].Name.Remove(0, 1);
        }



        public string GetResult(int winnaar)
        {           
            Data.Competition.PointsController.PutList(new GetPoints(Data.Competition.Participants[winnaar].Name, 25 ));
            if (winnaar == 0)
            {
                winnaar = 2;
            }

            //als de auto van de verliezer kapot is op het moment dat de winnaar finished
            if (Data.Competition.Participants[winnaar - 1].Equipment.IsBroken)
            {
                RemoveBrokenSign(1);
            }
            Data.Competition.PointsController.PutList(new GetPoints(Data.Competition.Participants[winnaar - 1].Name, 18));
            return "";
        }

        public void RandomizeEquipmentBroken(SectionData data)
        {
            
        }

        public void MoveParticipants()
        {
            Section previousSection = track.Sections.Last.Value;
            Dictionary<Section, SectionData> copiedPositions = _positions;
            int counter = 0;
            //om te bepalen of de foreach nog moet doorgaan
            bool leftHasMoved = false;
            bool rightHasMoved = false;

            foreach (Section section in track.Sections)
            {   
                //Linker Deelnemer
                if(copiedPositions[previousSection].Left != null && !leftHasMoved)
                {
                    if (!copiedPositions[previousSection].Left.Equipment.IsBroken)
                    {
                        if (_random.Next(copiedPositions[previousSection].Left.Equipment.Quality) == 1)
                        {
                            copiedPositions[previousSection].Left.Name = "†" + copiedPositions[previousSection].Left.Name;
                            copiedPositions[previousSection].Left.Equipment.IsBroken = true;
                        }
                        else
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
                                watchLeft.Stop();
                                if (_positiesOpBaan[_positions[previousSection].Left] >= 800)
                                {
                                    
                                    if (_lapTimeLeft.LapTimes == null)
                                    {
                                        _lapTimeLeft.LapTimes = new List<double>();
                                    }

                                    double newLapTime = 0.0;

                                    foreach (var item in _lapTimeLeft.SectionTimes)
                                    {
                                        newLapTime += item.Value.TotalSeconds;
                                    }
                                    _lapTimeLeft.LapTimes.Add(newLapTime);
                                    Data.Competition.TimeController.PutList(_lapTimeLeft);
                                    _lapTimeLeft.SectionTimes.Clear();

                                    _rondjesGeredenPerDeelnemer[_positions[previousSection].Left]++;
                                    _positiesOpBaan[_positions[previousSection].Left] -= 800;
                                    if (_rondjesGeredenPerDeelnemer[_positions[previousSection].Left] == _rondjesTeRijden)
                                    {
                                        _positions[previousSection].Left = null;
                                        leftIsDone = true;
                                        if (!rightIsDone)
                                        {
                                            leftFinishedFirst = true;
                                        }
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

                                _lapTimeLeft.SectionTimes.Add(previousSection, watchLeft.Elapsed);
                                watchLeft.Reset();
                                watchLeft.Start();

                                leftHasMoved = true;
                            }
                        }

                    
                    }
                    else if (_random.Next(3) == 2)
                    {
                        RemoveBrokenSign(0);
                        copiedPositions[previousSection].Left.Equipment.IsBroken = false;
                        copiedPositions[previousSection].Left.Equipment.Quality -= 3;
                        copiedPositions[previousSection].Left.Equipment.Speed -= 15;
                    }


                }
                //Rechter Deelnemer
                if (copiedPositions[previousSection].Right != null && !rightHasMoved)
                {
                    if (!copiedPositions[previousSection].Right.Equipment.IsBroken)
                    {
                        if (_random.Next(copiedPositions[previousSection].Right.Equipment.Quality) == 1)
                        {
                            copiedPositions[previousSection].Right.Name = "†" + copiedPositions[previousSection].Right.Name;
                            copiedPositions[previousSection].Right.Equipment.IsBroken = true;

                        }
                        else
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
                                    

                                    if (_lapTimeRight.LapTimes == null)
                                    {
                                        _lapTimeRight.LapTimes = new List<double>();
                                    }

                                    double newLapTime = 0.0;

                                    foreach (var item in _lapTimeRight.SectionTimes)
                                    {
                                        newLapTime += item.Value.TotalSeconds;
                                    }
                                    _lapTimeRight.LapTimes.Add(newLapTime);
                                    Data.Competition.TimeController.PutList(_lapTimeRight);

                                    _lapTimeRight.SectionTimes.Clear();
                                    _rondjesGeredenPerDeelnemer[_positions[previousSection].Right]++;
                                    _positiesOpBaan[_positions[previousSection].Right] -= 800;
                                    if (_rondjesGeredenPerDeelnemer[_positions[previousSection].Right] == _rondjesTeRijden)
                                    {
                                        _positions[previousSection].Right = null;
                                        rightIsDone = true;
                                        if (!leftIsDone)
                                        {
                                            leftFinishedFirst = false;
                                        }
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

                                _lapTimeRight.SectionTimes.Add(previousSection, watchRight.Elapsed);
                                watchRight.Reset();
                                watchRight.Start();

                                rightHasMoved = true;
                            }
                        }

                    }
                    else if (_random.Next(3) == 2)
                    {
                        RemoveBrokenSign(1);
                        copiedPositions[previousSection].Right.Equipment.IsBroken = false;
                        copiedPositions[previousSection].Right.Equipment.Quality -= 3;
                        copiedPositions[previousSection].Right.Equipment.Speed -= 15;
                    }




                }
                

                if(rightHasMoved && leftHasMoved)
                {
                    break;
                }

                previousSection = section;
                counter++;
            } 
        }

        public void DisposeRace()
        {
            Debug.WriteLine("race is over");
            timer.Elapsed -= OnTimedEvent;
            DriversChanged(new DriversChangedEventArgs { });
        }
    }
}

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
        private List<Stopwatch> stopwatches = new List<Stopwatch>();

        private Dictionary<Section, SectionData> _positions = new Dictionary<Section, SectionData>();

        //de value is hoever de participant heeft gereden
        public Dictionary<IParticipant, int> positiesOpBaan = new Dictionary<IParticipant, int>();
        private Dictionary<IParticipant, int> _rondjesGeredenPerDeelnemer = new Dictionary<IParticipant, int>();
        public List<GetLapTime> LapTimesLeftRight = new List<GetLapTime>();


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
                    positiesOpBaan.Add(participant, 0);
                    _rondjesGeredenPerDeelnemer.Add(participant, 0);
                }

                LapTimesLeftRight.Add(new GetLapTime());
                LapTimesLeftRight.Add(new GetLapTime());

                //linker
                LapTimesLeftRight[0].Name = participants[0].Name;
                LapTimesLeftRight[0].SectionTimes = new Dictionary<Section, TimeSpan>();

                //rechter
                LapTimesLeftRight[1].Name = participants[1].Name;
                LapTimesLeftRight[1].SectionTimes = new Dictionary<Section, TimeSpan>();

                stopwatches.Add(new Stopwatch());
                stopwatches.Add(new Stopwatch());

                stopwatches[0].Start();
                stopwatches[1].Start();
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

            PublishDriversChanged(new DriversChangedEventArgs { track = this.track });
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
            for (int i = 0; i < count; i++)
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



        public void GetResult(int winnaar)
        {
            Data.Competition.PointsController.PutList(new GetPoints(Data.Competition.Participants[winnaar].Name, 25));

            //voor de verliezende participant. 
            //als de winnaar de 0 index heeft, dan wordt het omgezet naar 2. zo komt op regel 183 het goed uit met [winnaar -1]. dan is het dus 1. 
            //als de winnaar index 1 heeft, is het dus 0 op regel 183. (1 - 1 = 0)
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
        }

        public bool RandomizeEquipmentIsBroken(IParticipant participant)
        {
            if (_random.Next(participant.Equipment.Quality) == 1)
            {
                participant.Name = "†" + participant.Name;
                participant.Equipment.IsBroken = true;
                participant.Equipment.Quality -= 3;
                participant.Equipment.Speed -= 15;
                return true;
            }
            return false;
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
                if (copiedPositions[previousSection].Left != null && !leftHasMoved && !leftIsDone)
                {
                    if (!copiedPositions[previousSection].Left.Equipment.IsBroken)
                    {
                        if (!RandomizeEquipmentIsBroken(copiedPositions[previousSection].Left))
                        {
                            leftHasMoved = HasMoved(0, counter, section, copiedPositions, previousSection);
                        }
                    }
                    else if (_random.Next(3) == 2)
                    {
                        RemoveBrokenSign(0);
                        copiedPositions[previousSection].Left.Equipment.IsBroken = false;
                    }

                }
                //----

                //Rechter Deelnemer
                if (copiedPositions[previousSection].Right != null && !rightHasMoved && !rightIsDone)
                {
                    if (!copiedPositions[previousSection].Right.Equipment.IsBroken)
                    {

                        if (!RandomizeEquipmentIsBroken(copiedPositions[previousSection].Right))
                        {
                            rightHasMoved = HasMoved(1, counter, section, copiedPositions, previousSection);
                        }

                    }
                    else if (_random.Next(3) == 2)
                    {
                        RemoveBrokenSign(1);
                        copiedPositions[previousSection].Right.Equipment.IsBroken = false;
                    }
                }
                //----

                if (rightHasMoved && leftHasMoved)
                {
                    break;
                }

                previousSection = section;
                counter++;
            }
        }

        public bool HasMoved(int sideIndex, int counter, Section section, Dictionary<Section, SectionData> copiedPositions, Section previousSection)
        {
            //om te bepalen of de acties moeten worden uitgevoerd voor de rechter of linker participant
            IParticipant copiedParticipant;
            if (sideIndex == 0)
            {
                copiedParticipant = copiedPositions[previousSection].Left;
            }
            else
            {
                copiedParticipant = copiedPositions[previousSection].Right;
            }

            int distancePerCount = (copiedParticipant.Equipment.Performance + copiedParticipant.Equipment.Speed) / 2;
            positiesOpBaan[copiedParticipant] += distancePerCount;

            //counter om te bepalen op welke sector de participant hoort
            int afstandAanEindSection = counter - 1;
            if (counter < 2)
            {
                afstandAanEindSection = counter + 7;
            }

            //wanneer de participant moet worden verplaatst naar de volgende section
            if (positiesOpBaan[copiedParticipant] >= afstandAanEindSection * 100)
            {
                //zodat ik de kant van de participants met de 0 of 1 index kan aanroepen om het verschill tussen links en rechts te bepalen
                //daardoor minder dubbele code
                List<IParticipant> participants = new List<IParticipant>();
                participants.Add(_positions[previousSection].Left);
                participants.Add(_positions[previousSection].Right); 

                stopwatches[sideIndex].Stop();

                //als de participant een heel rondje heeft gereden
                if (positiesOpBaan[participants[sideIndex]] >= 800)
                {
                    UpdateTimes(sideIndex);

                    _rondjesGeredenPerDeelnemer[participants[sideIndex]]++;
                    positiesOpBaan[participants[sideIndex]] -= 800;

                    //als het laatste rondje is gereden
                    if (_rondjesGeredenPerDeelnemer[participants[sideIndex]] == _rondjesTeRijden)
                    {
                        participants[sideIndex] = null;
                        if (sideIndex == 0)
                        {
                            leftIsDone = true;
                            if (!rightIsDone)
                            {
                                leftFinishedFirst = true;
                            }
                        }
                        else
                        {
                            rightIsDone = true;
                            if (!leftIsDone)
                            {
                                leftFinishedFirst = false;
                            }
                        }

                    }
                }

                //---het daadwerkelijke verplaatsen van de participant:
                if (counter == 8)
                {
                    if (sideIndex == 0)
                    {
                        _positions[track.Sections.First.Value].Left = copiedPositions[track.Sections.Last.Value].Left;
                        _positions[track.Sections.Last.Value].Left = copiedPositions[track.Sections.First.Value].Left;
                    }
                    else
                    {
                        _positions[track.Sections.First.Value].Right = copiedPositions[track.Sections.Last.Value].Right;
                        _positions[track.Sections.Last.Value].Right = copiedPositions[track.Sections.First.Value].Right;
                    }
                }
                else
                {
                    IParticipant backup;
                    if (sideIndex == 0)
                    {
                        backup = _positions[section].Left;
                        _positions[section].Left = _positions[previousSection].Left;
                        _positions[previousSection].Left = backup;
                    }
                    else
                    {
                        backup = _positions[section].Right;
                        _positions[section].Right = _positions[previousSection].Right;
                        _positions[previousSection].Right = backup;
                    }
                }
                //---

                //last laptime erin zetten
                LapTimesLeftRight[sideIndex].SectionTimes.Add(previousSection, stopwatches[sideIndex].Elapsed);
                stopwatches[sideIndex].Reset();
                stopwatches[sideIndex].Start();
                //---

                return true;
            }
            return false;
        }

        //voor toevoegen laptime op basis van sectiontimes
        public void UpdateTimes(int sideIndex)
        {
            if (LapTimesLeftRight[sideIndex].LapTimes == null)
            {
                LapTimesLeftRight[sideIndex].LapTimes = new List<double>();
            }

            double newLapTime = 0.0;

            foreach (var item in LapTimesLeftRight[sideIndex].SectionTimes)
            {
                newLapTime += item.Value.TotalSeconds;
            }
            LapTimesLeftRight[sideIndex].LapTimes.Add(newLapTime);
            Data.Competition.TimeController.PutList(LapTimesLeftRight[sideIndex]);
            LapTimesLeftRight[sideIndex].SectionTimes.Clear();
        }

        public void DisposeRace()
        {
            Debug.WriteLine("race is voorbij");
            timer.Elapsed -= OnTimedEvent;
            DriversChanged(new DriversChangedEventArgs { });
        }
    }
}

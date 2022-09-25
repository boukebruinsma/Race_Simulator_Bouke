using System;
using System.Diagnostics;
using System.Threading;
using Controller;
using Model;
namespace Race_Simulator
{
    public static class Visual
    {

        public static void Initialize()
        {
            Data.Initialize();
            Data.NextRace();
            Data.CurrentRace.DriversChanged += OnDriversChanged;
        }

        #region graphics

        private static string[] _finishHorizontalEast = { "-------", "   #   ", "   0   ", "Finish ", "   o   ", "   #   ", "-------" };
        private static string[] _finishHorizontalWest = { "-------", "   #   ", "   0   ", "Finish ", "   o   ", "   #   ", "-------" };
        private static string[] _finishVerticalNorth = { "|     |", "|     |", "|     |", "Finish|", "| 0 o |", "|     |", "|     |" };
        private static string[] _finishVerticalSouth = { "|     |", "|     |", "| o 0 |", "Finish|", "|     |", "|     |", "|     |" };

        private static string[] _straightHorizontalEast = { "-------", "       ", "   0   ", "       ", "   o   ", "       ", "-------" };
        private static string[] _straightHorizontalWest = { "-------", "       ", "   o   ", "       ", "   0   ", "       ", "-------" };
        private static string[] _straightVerticalNorth = { "|     |", "|     |", "|     |", "| 0 o |", "|     |", "|     |", "|     |" };
        private static string[] _straightVerticalSouth = { "|     |", "|     |", "|     |", "| o 0 |", "|     |", "|     |", "|     |" };

        private static string[] _rightCornerEast = { "---    ", "  \\    ", "    \\  ", "  0  \\ ", " o    |", "      |", "\\     |" };
        private static string[] _RightCornerSouth = { "/     |", "      |", "  o   |", "   0 / ", "    /  ", "  /    ", "--     " };
        private static string[] _rightCornerWest = { "|     \\", "|      ", "|   o  ", " \\ 0   ", "  \\    ", "    \\  ", "      –" };
        private static string[] _rightCornerNorth = { "    --–", "   /   ", "  /    ", "   0   ", " /  o  ", "       ", "|     /" };

        private static string[] _leftCornerNorth = { "---    ", "  \\    ", "    \\  ", "  o  \\ ", " 0    |", "      |", "\\     |" };
        private static string[] _leftCornerEast = { "/     |", "      |", "  0   |", "   o / ", "    /  ", "  /    ", "--     " };
        private static string[] _leftCornerSouth = { "|     \\", "|      ", "|   0  ", " \\ o   ", "  \\    ", "    \\  ", "      –" };
        private static string[] _leftCornerWest = { "    --–", "   /   ", "  /    ", "   o   ", " /  0  ", "       ", "|     /" };

        private static string[] _startGridHorizontalE = { "-------", "       ", "   0   ", " Start ", "   o   ", "       ", "-------" };    
        private static string[] _startGridHorizontalW = { "-------", "       ", "   o   ", " Start ", "   0   ", "       ", "-------" };
        private static string[] _startGridVerticalN = { "|Start|", "|     |", "|     |", "| 0 o |", "|     | ", "|     |", "|     |" };
        private static string[] _startGridVerticalS = { "|     |", "|     |", "|     |", "| o 0 |", "|     |", "|     |", "|Start|" };

        #endregion

        static int left = 0;
        static int top = 0;
        static int sectCount = 0;

        public static void DrawSector(String[] type, SectionData sectionData)
        {
            
            Console.SetCursorPosition(left, top);
            foreach (String layer in type)
            {                               
                Console.Write(placeParticipant(layer, sectionData.Left, sectionData.Right));
                top++;
                Console.SetCursorPosition(left, top);
            }

            sectCount++;
            
            

            if (sectCount <= 3)
            {
                top = 0;
                left = left + 7;
                Console.SetCursorPosition(left, top);
            }

            else if (sectCount == 4)
            {
                top = 7;
                left = 21;
                Console.SetCursorPosition(left, top);
            }

            else if (sectCount >= 5)
            {
                top = 7;
                if(left > 6)
                {
                    left = left - 7;
                }
                Console.SetCursorPosition(left, top);
            }
            
        }


        public static void DrawTrack(Track track)
        {
            
            int direction = 0;

            track = Data.CurrentRace.track;

            foreach (Section sect in track.Sections)
            {
                SectionData sectdata = Data.CurrentRace.GetSectionData(sect);
                if (sect.SectionType == SectionTypes.Straight)
                {
                    if (direction == 0)
                    { 
                        DrawSector(_straightVerticalNorth, sectdata);
                    }
                    else if (direction == 1)
                    {
                        DrawSector(_straightHorizontalEast, sectdata);
                    }
                    else if (direction == 2)
                    {
                        DrawSector(_straightVerticalSouth, sectdata);
                    }
                    else if (direction == 3)
                    {
                        DrawSector(_straightHorizontalWest, sectdata);
                    }
                }

                if (sect.SectionType == SectionTypes.LeftCorner)
                {
                    if (direction == 0)
                    {
                        DrawSector(_leftCornerNorth, sectdata);
                        direction = 3;
                    }
                    else if (direction == 1)
                    {
                        DrawSector(_leftCornerEast, sectdata);
                        direction = 0;
                    }
                    else if (direction == 2)
                    {
                        DrawSector(_leftCornerSouth, sectdata);
                        direction = 1;
                    }
                    else if (direction == 3)
                    {
                        DrawSector(_leftCornerWest, sectdata);
                        direction = 2;
                    }
                   
                }

                if (sect.SectionType == SectionTypes.RightCorner)
                {
                    if (direction == 1)
                    {
                        DrawSector(_rightCornerEast, sectdata);
                        direction = 2;
                    }
                    else if (direction == 2)
                    {
                        DrawSector(_RightCornerSouth, sectdata);
                        direction = 3;
                    }
                    else if (direction == 3)
                    {
                        DrawSector(_rightCornerWest, sectdata);
                        direction = 0;
                    }
                    else if (direction == 0)
                    {
                        DrawSector(_rightCornerNorth, sectdata);
                        direction = 1;
                    }
       
                }
                if (sect.SectionType == SectionTypes.Finish)
                {
                    if (direction == 0)
                    {
                        DrawSector(_finishVerticalNorth, sectdata);
                    }
                    else if (direction == 1)
                    {
                        DrawSector(_finishHorizontalEast, sectdata);
                    }
                    else if (direction == 2)
                    {
                        DrawSector(_finishVerticalSouth, sectdata);
                    }
                    else if (direction == 3)
                    {
                        DrawSector(_finishHorizontalWest, sectdata);
                    }
                }

                if (sect.SectionType == SectionTypes.StartGrid)
                {
                    if (direction == 0)
                    {
                        DrawSector(_startGridVerticalN, sectdata);
                    }
                    else if (direction == 1)
                    {
                        DrawSector(_startGridHorizontalE, sectdata);
                    }
                    else if (direction == 2)
                    {
                        DrawSector(_startGridVerticalS, sectdata);
                    }
                    else if (direction == 3)
                    {
                        DrawSector(_startGridHorizontalW, sectdata);
                    }
                }

            }
            left = 0;
            top = 0;
            sectCount = 0;

        }

        public static void OnDriversChanged(DriversChangedEventArgs dc)
        {
            
            Console.Clear();

            if(dc.track == null)
            {
                Console.WriteLine("de race op " + Data.CurrentRace.track.Name + " is afgelopen.");
                Thread.Sleep(4000);
                Console.Clear();
                Data.CurrentRace.DriversChanged -= OnDriversChanged;
                Data.NextRace();
                Data.CurrentRace.DriversChanged += Visual.OnDriversChanged;
                DrawTrack(Data.CurrentRace.track);
            }
            else
            {
                DrawTrack(dc.track);
            }
                      
        }

        
        

        public static string placeParticipant(string input, IParticipant participant, IParticipant participant2)
        {
            string firstLetter = participant?.Name.Substring(0,1)??" ";
            string firstLetter2 = participant2?.Name.Substring(0, 1)??" ";
            
            string output = input.Replace("0", firstLetter);
            output = output.Replace("o", firstLetter2);
            return output;
        }
    }
}

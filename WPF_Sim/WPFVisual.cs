using Controller;
using Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Media.Imaging;

namespace WPF_Sim
{
    static class WPFVisual
    {
        #region graphics
        public const string Rb = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\rb.png";
        public const string RbBroken = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\rb_broken.png";
        public const string Fr = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\fr.png";
        public const string FrBroken = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\fr_broken.png";

        public const string Straight = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\straight.png";
        public const string StartGrid = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\startgrid.png";
        public const string Finish = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\finish.png";
        public const string Right = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\right.png";
        public const string Left = "C:\\Users\\Bouke\\Source\\Repos\\Race_Simulator_Bouke\\WPF_Sim\\images\\left.png";
        #endregion

        static int left = 0;
        static int top = 0;
        static int sectCount = 0;

        public static BitmapSource DrawTrack(Track track)
        {
            ImageProcessor.DrawBackground(1280, 750);

            int direction = 0;

            track = Data.CurrentRace.track;

            foreach (Section sect in track.Sections)
            {
                SectionData sectdata = Data.CurrentRace.GetSectionData(sect);
                if (sect.SectionType == SectionTypes.Straight)
                {
                    DrawSector(direction, Straight, sectdata);
                }

                if (sect.SectionType == SectionTypes.LeftCorner)
                {
                    if (direction == 0)
                    {
                        direction = 3;
                    }
                    else
                    {
                        direction--;
                    }
                    DrawSector(direction, Left, sectdata);
                }

                if (sect.SectionType == SectionTypes.RightCorner)
                {
                    if (direction == 3)
                    {
                        direction = 0;
                    }
                    else
                    {
                        direction++;
                    }
                    DrawSector(direction, Right, sectdata);
                    
                }
                if (sect.SectionType == SectionTypes.Finish)
                {
                    DrawSector(direction, Finish, sectdata);
                }

                if (sect.SectionType == SectionTypes.StartGrid)
                {
                    DrawSector(direction, StartGrid, sectdata);                   
                }

            }
            left = 0;
            top = 0;
            sectCount = 0;


            //ImageProcessor.DrawSectorImage(Right);
            
            
            return ImageProcessor.CreateBitmapSourceFromGdiBitmap(ImageProcessor.InsertImage("empty"));
        }

        public static void DrawSector(int direction, String type, SectionData sectionData)
        {
            ImageProcessor.DrawSectorImage(type, left, top, direction, 0);
            PlaceParticipant(direction, sectionData);
            

            sectCount++;

            if (sectCount <= 3)
            {
                top = 0;
                left = left + 320;
            }

            else if (sectCount == 4)
            {
                top = 320;
                left = 960;
            }

            else if (sectCount >= 5)
            {
                top = 320;
                if (left > 319)
                {
                    left = left - 320;
                }
            }
        }

        public static void PlaceParticipant(int direction, SectionData sectionData)
        {
            if (sectionData.Left != null)
            {
                if (sectionData.Left.Equipment.IsBroken)
                {
                    ImageProcessor.DrawSectorImage(RbBroken, left, top, direction, -70);
                }
                else
                {
                    ImageProcessor.DrawSectorImage(Rb, left, top, direction, -70);
                }
            }

            if (sectionData.Right != null)
            {
                if (sectionData.Right.Equipment.IsBroken)
                {
                    ImageProcessor.DrawSectorImage(FrBroken, left, top, direction, 70);
                }
                else
                {
                    ImageProcessor.DrawSectorImage(Fr, left, top, direction, 70);
                }
            }
        }

        

    }
}

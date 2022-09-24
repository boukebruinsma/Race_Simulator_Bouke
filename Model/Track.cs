using System;
using System.Collections.Generic;

namespace Model
{
    public class Track
    {
        public String Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public Track(String Name, SectionTypes[] sections)
        {
            this.Name = Name;
            InitSections(sections);

        }

        public LinkedList<Section> InitSections(SectionTypes[] sections)
        {
            Sections = new LinkedList<Section>();
            foreach (SectionTypes sectionType in sections)
            {
                Section section = new Section();
                section.SectionType = sectionType;
                Sections.AddLast(section);
            }
            return Sections;
        }


    }
}

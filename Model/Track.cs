using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }
        public bool HasGeneratedSections { get; set; }

        public Track(string name, SectionTypes[] sections)
        {
            this.Name = name;
            this.Sections = ConvertSectionTypes(sections);
        }

        private LinkedList<Section> ConvertSectionTypes(SectionTypes[] sections)
        {
            LinkedList<Section> sectionList = new LinkedList<Section>();

            foreach (SectionTypes sectionType in sections)
            {
                Section section = new Section(sectionType);
                sectionList.AddLast(section);
            }

            return sectionList;
        }

        public LinkedList<Section> GetStartSections()
        {
            LinkedList<Section> sectionList = new LinkedList<Section>();

            foreach (Section section in Sections)
            {
                if (section.SectionType == SectionTypes.StartGrid)
                    sectionList.AddFirst(section);
                else
                    return sectionList;
            }
            return sectionList;
        }

        public Section GetNextSection(Section section)
        {
            bool foundCurrent = false;
            for (var s = Sections.First; s.Next != null; s = s.Next)
            {
                if (foundCurrent)
                    return s.Value;
                foundCurrent = s.Value.Equals(section);
            }
            return Sections.First.Value;
        }

        public Section GetPreviousSection(Section section)
        {
            Section prevSection = Sections.Last.Value;
            for (var s = Sections.First; s.Next != null; s = s.Next)
            {
                if (s.Value.Equals(section))
                    return prevSection;
            
                prevSection = s.Value;
            }
            return Sections.Last.Value;
        }
    }
}

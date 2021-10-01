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
            foreach (Section s in Sections)
            {
                if (foundCurrent)
                    return section;

                foundCurrent = s == section;
            }
            return section;
        }

        public Section GetPreviousSection(Section section)
        {
            Section previousSection = section;
            foreach (Section s in Sections)
            {
                if (s == section)
                    return previousSection;

                previousSection = s;
            }
            return previousSection;
        }
    }
}

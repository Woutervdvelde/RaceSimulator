using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

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
    }
}

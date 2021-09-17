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

            this.Sections = new LinkedList<Section>();
            foreach (SectionTypes sectionType in sections)
            {
                Section section = new Section(sectionType);
                this.Sections.AddLast(section);
            }
        }
    }
}

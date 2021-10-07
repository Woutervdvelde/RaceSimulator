using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Section
    {
        public SectionTypes SectionType { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public string[] Graphic { get; set; }

        public static int Length = 1;

        public Section(SectionTypes sectionType)
        {
            this.SectionType = sectionType;
        }
    }
}

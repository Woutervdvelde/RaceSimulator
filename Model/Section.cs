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
        public Directions Direction { get; set; }
        public SectionDirections SectionDirection { get; set; }
        public string[] Graphic { get; set; }
        public string Image { get; set; }

        public static int Length = 100;

        public Section(SectionTypes sectionType)
        {
            this.SectionType = sectionType;
        }
    }
}

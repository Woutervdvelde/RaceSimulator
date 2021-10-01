using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class SectionData
    {
        public Section Section { get; set; }
        public IParticipant Left { get; set; }
        public int DistanceLeft { get; set; }
        public IParticipant Right { get; set; }
        public int DistanceRight { get; set; }
        public Queue<IParticipant> Waiting { get; set; }

        public SectionData()
        {
            Waiting = new Queue<IParticipant>();
        }
    }
}

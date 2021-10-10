using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Car : IEquipment
    {
        public int Quality { get; set; }
        public int Performance { get; set; }
        public int Speed { get; set; }
        public bool IsBroken { get; set; }

        public static int MIN_SPEED = 5;
        public static int MIN_PERFORMANCE = 5;
        public static int MIN_QUALITY = 5;        
        public static int MAX_SPEED = 20;
        public static int MAX_PERFORMANCE = 20;
        public static int MAX_QUALITY = 20;
    }
}

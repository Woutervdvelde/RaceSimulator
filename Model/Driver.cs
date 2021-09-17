﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }

        public Driver(string name, IEquipment car, TeamColors teamColor)
        {
            this.Name = name;
            this.Equipment = car;
            this.TeamColor = teamColor;
        }
    }
}

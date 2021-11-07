using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace Model
{
    public class Driver : IParticipant
    {
        public string Name { get; set; }
        public int Points { get; set; }
        public IEquipment Equipment { get; set; }
        public TeamColors TeamColor { get; set; }
        public bool IsLeft { get; set; }
        public string Image { get => $"Cars\\Car_{TeamColor}.png"; }

        public Driver(string name, IEquipment car, TeamColors teamColor)
        {
            this.Name = name;
            this.Equipment = car;
            this.TeamColor = teamColor;
        }
    }
}

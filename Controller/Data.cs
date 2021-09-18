﻿using System;
using System.Collections.Generic;
using System.Text;
using Model;

namespace Controller
{
    public static class Data
    {
        public static Competition Competition { get; set; }
        public static Race CurrentRace { get; set; }

        public static void Initialize()
        {
            Competition = new Competition();
            AddParticipants();
            AddTracks();
        }

        public static void AddParticipants()
        {
            Competition.Participants.Add(new Driver("meow", new Car(), TeamColors.Blue));
            Competition.Participants.Add(new Driver("bork", new Car(), TeamColors.Green));
        }

        public static void AddTracks()
        {
            Track beginner = new Track("Beginner", new[] { SectionTypes.StartGrid, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });
            Track advanced = new Track("Beginner", new[] { SectionTypes.StartGrid, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.Straight, SectionTypes.RightCorner, SectionTypes.RightCorner, SectionTypes.Finish });

            Competition.Tracks.Enqueue(beginner);
            Competition.Tracks.Enqueue(advanced);
        }

        public static void NextRace() 
        {
            Track NextTrack = Competition.NextTrack();
            if (NextTrack != null)
                CurrentRace = new Race(NextTrack, Competition.Participants);
        }
    }
}
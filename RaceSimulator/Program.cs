using System;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
            Data.Initialize();
            Data.NextRace();

            Visual.Initialize(Data.CurrentRace);
            Visual.DrawTrack(Data.CurrentRace.Track);

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}

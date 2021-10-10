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

            Data.CurrentRace.RaceFinished += NextRace;
            Data.CurrentRace.Start();

            for (; ; )
            {
                Thread.Sleep(100);
                //if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                //    Data.CurrentRace.MoveParticipants();
            }
        }

        static void NextRace(object source, EventArgs args)
        {
            Visual.ShowLeaderboard();
            if (Console.ReadKey(true).Key == ConsoleKey.Enter)
            {
                Data.NextRace();
                Visual.Initialize(Data.CurrentRace);
                Visual.DrawTrack(Data.CurrentRace.Track);

                Data.CurrentRace.RaceFinished += NextRace;
                Data.CurrentRace.Start();
            }
        }
    }
}

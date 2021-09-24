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
            Console.WriteLine(Data.CurrentRace.Track.Name);

            Console.WriteLine("Participants:");
            Data.CurrentRace.Participants.ForEach(participant => {
                Console.WriteLine($"\t - {participant.Name}");
            });

            for (; ; )
            {
                Thread.Sleep(100);
            }
        }
    }
}

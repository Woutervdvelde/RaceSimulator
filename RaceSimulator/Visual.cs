using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using Controller;
using Model;

namespace RaceSimulator
{
    public static class Visual
    {
        private static Race _currentRace;
        private static Directions _direction;
        private static int _lastX;
        private static int _lastY;
        private static int _offsetX;
        private static int _offsetY;

        public static void Initialize(Race currentRace)
        {
            Console.Clear();
            _currentRace = currentRace;
            _direction = Directions.East;
            _lastX = 0;
            _lastY = 0;
            _offsetX = 0;
            _offsetY = 0;
            currentRace.DriversChanged += OnDriversChanged;
        }

        #region graphics


        private static int _width = 9;
        private static int _height = 4;
        private static char _driverBroken = '¤';
        private static char _graphSide = '█';
        private static char _graphTop = '▀';
        private static char _graphBottom = '▄';
        private static string[] _startHorizontal = { "═════════", "      1] ", " 2]      ", "═════════" };
        private static string[] _finishHorizontal = { "════░════", "  1 ░    ", "  2 ░    ", "════░════" };
        private static string[] _straightHorizontal = { "═════════", "    1    ", "    2    ", "═════════" };
        private static string[] _straightVertical = { "║       ║", "║       ║", "║ 1   2 ║", "║       ║" };

        private static string[] _cornerNorthWest = { "╝       ║", "  1  2  ║", "        ║", "════════╝" };
        private static string[] _cornerNorthEast = { "║       ╚", "║  1  2  ", "║        ", "╚════════" };
        private static string[] _cornerSouthWest = { "════════╗", "        ║", " 1   2  ║", "╗       ║" };
        private static string[] _cornerSouthEast = { "╔════════", "║        ", "║  1  2  ", "║       ╔" };

        #endregion

        public static void DrawTrack(Track track)
        {
            if (!track.HasGeneratedSections)
                GenerateCoordinatesAndGraphics(track);

            foreach (Section section in track.Sections)
            {
                DrawSection(section);
            }
        }

        public static void DrawSection(Section section)
        {
            string[] graphic = PlaceParticipantsOnGraphic(section);
            for (int i = 0; i < section.Graphic.Length; i++)
            {
                Console.SetCursorPosition(section.X * _width + _offsetX, section.Y * _height + _offsetY + i);
                Console.Write(graphic[i]);
            }
        }

        private static string[] PlaceParticipantsOnGraphic(Section section)
        {

            string[] graphic = (string[])section.Graphic.Clone();
            SectionData data = _currentRace.GetSectionData(section);

            for (int i = 0; i < section.Graphic.Length; i++)
            {
                if (!section.Graphic[i].Contains("1") && !section.Graphic[i].Contains("2"))
                {
                    graphic[i] = section.Graphic[i];
                    continue;
                }

                if (data.Left != null)
                    if (data.Left.Equipment.IsBroken)
                        graphic[i] = graphic[i].Replace('1', _driverBroken);
                    else
                        graphic[i] = graphic[i].Replace('1', data.Left.Name[0]);
                else
                    graphic[i] = graphic[i].Replace('1', ' ');

                if (data.Right != null)
                    if (data.Right.Equipment.IsBroken)
                        graphic[i] = graphic[i].Replace('2', _driverBroken);
                    else
                        graphic[i] = graphic[i].Replace('2', data.Right.Name[0]);
                else
                    graphic[i] = graphic[i].Replace('2', ' ');
            }

            return graphic;
        }

        public static void OnDriversChanged(object source, EventArgs args)
        {
            DriversChangedEventArgs driverArgs = args as DriversChangedEventArgs;
            if (driverArgs != null)
                DrawTrack(driverArgs.Track);
        }

        public static void GenerateCoordinatesAndGraphics(Track track)
        {
            foreach (Section section in track.Sections)
            {
                GenerateGraphics(section);
                GenerateCoordinates(section);
            }
            _offsetX = Math.Abs(_offsetX);
            _offsetY = Math.Abs(_offsetY);
            track.HasGeneratedSections = true;
        }

        private static void GenerateGraphics(Section section)
        {
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    section.Graphic = _startHorizontal;
                    break;
                case SectionTypes.Finish:
                    section.Graphic = _finishHorizontal;
                    break;
                case SectionTypes.Straight:
                    if (_direction == Directions.North || _direction == Directions.South)
                        section.Graphic = _straightVertical;
                    else
                        section.Graphic = _straightHorizontal;
                    break;
                case SectionTypes.LeftCorner:
                    if (_direction == Directions.North)
                        section.Graphic = _cornerSouthWest;
                    if (_direction == Directions.East)
                        section.Graphic = _cornerNorthWest;
                    if (_direction == Directions.South)
                        section.Graphic = _cornerNorthEast;
                    if (_direction == Directions.West)
                        section.Graphic = _cornerSouthEast;

                    if (_direction - 1 < 0)
                        _direction = Directions.West;
                    else
                        _direction--;
                    break;
                case SectionTypes.RightCorner:
                    if (_direction == Directions.North)
                        section.Graphic = _cornerSouthEast;
                    if (_direction == Directions.East)
                        section.Graphic = _cornerSouthWest;
                    if (_direction == Directions.South)
                        section.Graphic = _cornerNorthWest;
                    if (_direction == Directions.West)
                        section.Graphic = _cornerNorthEast;

                    if ((int)_direction + 1 > 3)
                        _direction = Directions.North;
                    else
                        _direction++;
                    break;
            }
        }

        private static void GenerateCoordinates(Section section)
        {
            section.X = _lastX;
            section.Y = _lastY;
            switch (_direction)
            {
                case Directions.North:
                    _lastY--;
                    break;
                case Directions.East:
                    _lastX++;
                    break;
                case Directions.South:
                    _lastY++;
                    break;
                case Directions.West:
                    _lastX--;
                    break;
            }

            if (_lastX * _width < _offsetX)
                _offsetX = _lastX * _width;
            if (_lastY * _height < _offsetY)
                _offsetY = _lastY * _height;
        }

        private static int CalculateHighestTextWidth()
        {
            int highest = 0;
            foreach (IParticipant p in _currentRace.Participants)
                if (p.Name.Length > highest)
                    highest = p.Name.Length;
            return highest;
        }

        public static void ShowLeaderboard()
        {
            DrawTrack(_currentRace.Track);
            int textWidth = CalculateHighestTextWidth() + 2;
            int totalWidth = textWidth + 6;
            int left = Console.WindowWidth / 2 - (textWidth + 6) / 2;
            int top = Console.WindowHeight / 2 - _currentRace.Participants.Count;

            Console.SetCursorPosition(left, top - 1);
            for (int s = 0; s < totalWidth; s++) Console.Write(_graphBottom);

            for (int i = 0; i < _currentRace.Leaderboard.Count; i++)
            {
                string name = _currentRace.Leaderboard.ElementAt(i).Name;
                int widthOffset = (textWidth - name.Length) / 2;
                bool widthOffsetExtra = (textWidth - name.Length) % 2 != 0;
                Console.SetCursorPosition(left, top + i);

                Console.Write($"{_graphSide} {i + 1} {_graphSide}");
                for (int s = 0; s < widthOffset; s++) Console.Write(' ');

                string nameOutput = widthOffsetExtra ? $" {name}" : name;
                Console.Write(nameOutput);

                for (int s = 0; s < widthOffset; s++) Console.Write(' ');
                Console.Write(_graphSide);
            }

            Console.SetCursorPosition(left, top + _currentRace.Leaderboard.Count);
            for (int s = 0; s < totalWidth; s++) Console.Write(_graphTop);

            string nextInfo = "Press Enter to proceed...";
            Console.SetCursorPosition(left + totalWidth - nextInfo.Length, top + _currentRace.Leaderboard.Count + 1);
            Console.Write(nextInfo);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Model;

namespace RaceSimulator
{
    public enum Directions
    {
        North, East, South, West
    }

    public static class Visual
    {
        private static Directions _direction;
        private static int _lastX;
        private static int _lastY;
        private static int _offsetX;
        private static int _offsetY;

        public static void Initialize()
        {
            _direction = Directions.East;
            _lastX = 0;
            _lastY = 0;
            _offsetX = 0;
            _offsetY = 0;
        }

        #region graphics

        public static string CarDefault = "█";

        private static int _width = 9;
        private static int _height = 4;
        private static string[] _startHorizontal = { "═════════", "      L] ", " R]      ", "═════════" };
        private static string[] _finishHorizontal = { "════░════", "  L ░    ", "  R ░    ", "════░════" };
        private static string[] _straightHorizontal = { "═════════", "    L    ", "    R    ", "═════════" };
        private static string[] _straightVertical = { "║       ║", "║       ║", "║ L   R ║", "║       ║" };

        private static string[] _cornerNorthWest = { "╝       ║", "  L  R  ║", "        ║", "════════╝" };
        private static string[] _cornerNorthEast = { "║       ╚", "║  L  R  ", "║        ", "╚════════" };
        private static string[] _cornerSouthWest = { "════════╗", "        ║", " L   R  ║", "╗       ║" };
        private static string[] _cornerSouthEast = { "╔════════", "║        ", "║  L  R  ", "║       ╔" };

        #endregion

        public static void DrawTrack(Track track)
        {
            GenerateCoordinatesAndGraphics(track.Sections);
            foreach (Section section in track.Sections)
            {
                for (int i = 0; i < section.Graphic.Length; i++)
                {
                    Console.SetCursorPosition(section.X * _width + _offsetX, section.Y * _height + _offsetY + i);
                    Console.Write(section.Graphic[i]);
                    Thread.Sleep(25);
                }
            }
            Initialize();
        }

        private static void GenerateCoordinatesAndGraphics(LinkedList<Section> sections)
        {
            foreach (Section section in sections)
            {
                GenerateGraphics(section);
                GenerateCoordinates(section);
            }
            _offsetX = Math.Abs(_offsetX);
            _offsetY = Math.Abs(_offsetY);
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
    }
}

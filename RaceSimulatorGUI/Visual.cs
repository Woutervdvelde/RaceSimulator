using System;
using System.Text;
using Model;
using Controller;
using System.Collections.Generic;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace RaceSimulatorGUI
{
    public static class Visual
    {
        private static Race _currentRace;
        private static Directions _direction;
        private static int _lastX;
        private static int _lastY;
        private static int _minX;
        private static int _maxX;
        private static int _minY;
        private static int _maxY;
        private static Bitmap _baseTrack;

        #region Graphics

        private const int _width = 250;
        private const int _height = 200;
        private const string _straight_horizontal = ".\\Resources\\Straight_Horizontal.png";
        private const string _straight_vertical = ".\\Resources\\Straight_Vertical.png";
        private const string _corner_NE = ".\\Resources\\Corner_NE.png";
        private const string _corner_NW = ".\\Resources\\Corner_NW.png";
        private const string _corner_SE = ".\\Resources\\Corner_SE.png";
        private const string _corner_SW = ".\\Resources\\Corner_SW.png";

        #endregion

        public static void Initialize(Race race)
        {
            _currentRace = race;
            _direction = Directions.East;
            _lastX = 0;
            _lastY = 0;
            _minX = 0;
            _maxX = 0;
            _minY = 0;
            _maxY = 0;
        }

        public static BitmapSource DrawTrack(Track track)
        {
            GenerateCoordinatesAndGraphics(track);
            Bitmap baseMap = DrawBaseTrack(track);
            Bitmap map = DrawParticipants(baseMap);

            BitmapSource b = ResourceManager.CreateBitmapSourceFromGdiBitmap(map);
            return b;
        }

        public static Bitmap DrawBaseTrack(Track track)
        {
            if (_baseTrack != null) return _baseTrack;

            Bitmap baseTrack = ResourceManager.GetEmptyImage((_maxX - _minX), (_maxY - _minY));
            Graphics graphics = Graphics.FromImage(baseTrack);

            foreach (Section section in track.Sections)
            {
                Bitmap b = ResourceManager.GetImage(section.Image);
                graphics.DrawImage(b, section.X * _width, section.Y * _height);
            }
            _baseTrack = baseTrack;

            return baseTrack;
        }

        public static Bitmap DrawParticipants(Bitmap baseTrack)
        {
            return baseTrack;
        }

        private static void GenerateCoordinatesAndGraphics(Track track)
        {
            if (track.HasGeneratedSections) return;

            foreach (Section section in track.Sections)
            {
                GenerateGraphics(section);
                GenerateCoordinates(section);
            }
            track.HasGeneratedSections = true;
        }

        private static void GenerateGraphics(Section section)
        {
            section.Direction = _direction;
            switch (section.SectionType)
            {
                case SectionTypes.StartGrid:
                    section.Image = _straight_horizontal;
                    break;
                case SectionTypes.Finish:
                    section.Image = _straight_horizontal;
                    break;
                case SectionTypes.Straight:
                    if (_direction == Directions.North || _direction == Directions.South)
                        section.Image = _straight_vertical;
                    else
                        section.Image = _straight_horizontal;
                    break;
                case SectionTypes.LeftCorner:
                    if (_direction == Directions.North)
                        section.Image = _corner_SW;
                    if (_direction == Directions.East)
                        section.Image = _corner_NW;
                    if (_direction == Directions.South)
                        section.Image = _corner_NE;
                    if (_direction == Directions.West)
                        section.Image = _corner_SE;

                    if (_direction - 1 < 0)
                        _direction = Directions.West;
                    else
                        _direction--;
                    break;
                case SectionTypes.RightCorner:
                    if (_direction == Directions.North)
                        section.Image = _corner_SE;
                    if (_direction == Directions.East)
                        section.Image = _corner_SW;
                    if (_direction == Directions.South)
                        section.Image = _corner_NW;
                    if (_direction == Directions.West)
                        section.Image = _corner_NE;

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

            if (_lastX * _width < _minX)
                _minX = _lastX * _width;
            if (_lastY * _height < _minY)
                _minY = _lastY * _height;

            if (_lastX * _width > _maxX)
                _maxX = _lastX * _width;
            if (_lastY * _height > _maxY)
                _maxY = _lastY * _height;
        }
    }
}

using System.Diagnostics;
using Font = Microsoft.Maui.Graphics.Font;

namespace MauiAcceleromonitorApp.CustomComponents
{
    public class TemporalGraph : GraphicsView
    {
        static int s_maxDataLength = 50;
        List<float> _data = new List<float>(s_maxDataLength);
        List<DateTime> _timestamp = new List<DateTime>(s_maxDataLength);


        //
        private string _myLabel;
        public string MyLabel
        {
            get => _myLabel;
            init
            {
                Debug.Print("setter is called."); // In the InitializeComponent @ MainPage
                _myLabel = value;
            }
        }

        public Color MyColor { get; init; }

        public Color MyAxesColor { get; set; }

        public double MyFontSize { get; init; }

        public double MyStrokeSize { get; init; }


        //
        public void FeedData(float acc)
        {
            DateTime timestamp = DateTime.Now;

            if (_timestamp.Count == s_maxDataLength)
            {
                _timestamp.Remove(_timestamp[0]);
                _data.Remove(_data[0]);
            }

            _timestamp.Add(timestamp);
            _data.Add(acc);
        }

        public TemporalGraph()
        {

            // Set drawable function
            this.Drawable = new GraphDrawable(this);

        }

        // For draw function
        private class GraphDrawable : IDrawable
        {
            private TemporalGraph _graph;

            float _width;
            float _height;

            float _fs; // font size

            // axes
            PointF _origin;
            PointF _xmax, _ymax;

            // grid
            int _ngridsx = 10;
            int _ngridsy = 5;

            float _gridHeight;
            float _gridWidth;

            float _graphHeight;
            float _graphWidth;

            float _dx;

            public GraphDrawable(TemporalGraph graph)
            {
                _graph = graph;
            }

            public void Draw(ICanvas canvas, RectF dirtyRect)
            {
                //canvas.FillColor = Colors.Orange;
                //canvas.FillRectangle(dirtyRect);


                CalcCoordinates(dirtyRect);

                //canvas.Font = new Font("Arial");
                canvas.Font = new Font("monospace");
                canvas.FontSize = _fs;
                canvas.FontColor = _graph.MyAxesColor;

                // grid
                canvas.StrokeSize = 1;
                canvas.StrokeColor = Colors.LightGray;

                for (int i = 0; i < _ngridsy / 2; i++)
                {
                    float dgy = (i + 1) * _gridHeight;

                    canvas.DrawLine(_origin.Offset(0, dgy), _origin.Offset(_graphWidth, dgy));
                    canvas.DrawLine(_origin.Offset(0, -dgy), _origin.Offset(_graphWidth, -dgy));
                }

                for (int i = 0; i < _ngridsx; i++)
                {
                    float gx = (i + 1) * _gridWidth;

                    canvas.DrawLine(_origin.Offset(gx, +_graphHeight / 2), _origin.Offset(gx, -_graphHeight / 2));
                }

                // axis
                canvas.StrokeSize = 2;
                //canvas.StrokeColor = Colors.Black;
                canvas.StrokeColor = _graph.MyAxesColor;

                canvas.DrawLine(_origin, _origin.Offset(_graphWidth, 0)); // x-axis
                canvas.DrawLine(_origin.Offset(0, +_graphHeight / 2), _origin.Offset(0, -_graphHeight / 2));


                // labels (vertical)
                canvas.DrawString("0", _origin.X - _fs, _origin.Y - _fs / 2, _fs, _fs, HorizontalAlignment.Center, VerticalAlignment.Center);
                canvas.DrawString("+", _origin.X - _fs, _origin.Y - _fs / 2 - _graphHeight / 4, _fs, _fs, HorizontalAlignment.Center, VerticalAlignment.Center);
                canvas.DrawString("-", _origin.X - _fs, _origin.Y - _fs / 2 + _graphHeight / 4, _fs, _fs, HorizontalAlignment.Center, VerticalAlignment.Center);


                // data
                canvas.StrokeSize = (float)_graph.MyStrokeSize;
                canvas.StrokeColor = _graph.MyColor;

                PathF path = new PathF();

                float r = _graphHeight / 2;

                if (_graph._data.Count > 0)
                    path.MoveTo(_origin.Offset(0, _graph._data[0] * r));

                for (int k = 0; k < _graph._data.Count; k++)
                {
                    path.LineTo(_origin.Offset(k * _dx, _graph._data[k] * r));
                }

                canvas.DrawPath(path);

                //canvas.DrawString(_graph.MyLabel, 100, 100, HorizontalAlignment.Center);



            }

            private void CalcCoordinates(RectF dirtyRect)
            {
                _width = dirtyRect.Width;
                _height = dirtyRect.Height;

                _fs = (float)_graph.MyFontSize;

                // graph size
                float w = _width * 0.8f;
                float h = _height * 0.8f;

                PointF bottomLeft = new PointF(30, _height - 30);

                _xmax = bottomLeft.Offset(w, 0);
                _ymax = bottomLeft.Offset(0, -h);

                _origin = bottomLeft.Offset(0, -h / 2);

                _dx = w / s_maxDataLength;

                _gridHeight = h / _ngridsy;
                _gridWidth = w / _ngridsx;

                _graphHeight = h;
                _graphWidth = w;
            }
        }
    }
}
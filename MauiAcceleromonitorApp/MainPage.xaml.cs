using MauiAcceleromonitorApp.CustomComponents;
using System.Numerics;

namespace MauiAcceleromonitorApp
{
    public partial class MainPage : ContentPage
    {

        int _samplingInterval = 100; // [ms]

        public MainPage()
        {
            InitializeComponent();

            Color color;

            if (Application.Current.RequestedTheme == AppTheme.Dark)
            {
                color = Resources["DarkAxesColor"] as Color;
                //color = Colors.White;
            }
            else
            {
                color = Resources["LightAxesColor"] as Color;
                //color = Colors.Black;
            }

            graphx.MyAxesColor = color;
            graphy.MyAxesColor = color;
            graphz.MyAxesColor = color;

            var recorder = new AccelarationRecorder();

            _ = UpdateXYZGraphs(graphx, graphy, graphz, recorder);
        }

        // Update function
        async Task UpdateXYZGraphs(TemporalGraph graphx, TemporalGraph graphy, TemporalGraph graphz, AccelarationRecorder recorder)
        {
            while (true)
            {
                Vector3 acc = recorder.GetCurrentData();

                graphx.FeedData(acc.X);
                graphy.FeedData(acc.Y);
                graphz.FeedData(acc.Z);

                graphx.Invalidate();
                graphy.Invalidate();
                graphz.Invalidate();

                await Task.Delay(_samplingInterval);
            }
        }
    }
}
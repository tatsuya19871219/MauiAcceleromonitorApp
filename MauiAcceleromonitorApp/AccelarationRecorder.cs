using System.Diagnostics;
using System.Numerics;

namespace MauiAcceleromonitorApp
{
    public class AccelarationRecorder
    {

        public bool IsSupported { get; private set; }

        Vector3 _acc;

        public AccelarationRecorder()
        {
            IsSupported = Accelerometer.Default.IsSupported;

            if (IsSupported)
            {
                Debug.Print("Accelerometer is supported!");

                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.ShakeDetected += Accelerometer_ShakeDetected;
                Accelerometer.Start(SensorSpeed.UI);

                Debug.Print($"IsMonitoring = {Accelerometer.IsMonitoring}");
            }
            else Debug.Print("Accelerometer is not supported!");
        }

        public Vector3 GetCurrentData()
        {
            if (!IsSupported)
            {
                //Debug.Print("Not supported");
                return new Vector3();
            }

            return _acc;
        }


        // event listeners
        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            //Debug.Print($"Accel: {e.Reading}");

            _acc = e.Reading.Acceleration;

        }

        private void Accelerometer_ShakeDetected(object sender, EventArgs e)
        {
            Debug.Print("Shake Detected!");
        }

    }
}
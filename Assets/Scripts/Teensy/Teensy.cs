using System;
using System.IO.Ports;
using UnityEngine;

public class Teensy : MonoBehaviour
{
    public event EventHandler<OnFlexChangedEventArgs> OnFlexChanged;
    public class OnFlexChangedEventArgs : EventArgs
    {
        public float Value;
    }

    [SerializeField]
    private string portName;

    private SerialPort port;

    void Start()
    {
        // Teensy uses the maximum baud rate available by the USB connection, so any value will be ignored.
        port = new SerialPort(portName, 9600)
        {
            // The Teensy’s Serial only counts as connected when the PC asserts the DTR line.
            // Since Unity does not that by default, with "while (!Serial)" in the sketch setup,
            // Teensy would wait forever.
            DtrEnable = true,
            ReadTimeout = 100
        };
        port.Open();
    }

    void Update()
    {
        if (port == null || !port.IsOpen) return;

        try
        {
            // Reads bytes from the port's receive buffer.
            while (port.BytesToRead > 0)
            {
                if (float.TryParse(port.ReadLine(), out float value))
                {
                    FlexSensor flexSensor = new(value);
                    OnFlexChanged?.Invoke(this, new OnFlexChangedEventArgs
                    {
                        Value = flexSensor.Deserialize()
                    });
                }
            }
        }
        catch (Exception)
        {
            // Either timeout expired (port.ReadLine() has waited port.ReadTimeout ms)
            // or half line has been received (e.g. "12" instead of "1234").
        }
    }

    public void Send(ISerializer _serializer)
    {
        port.WriteLine(_serializer.Serialize());
    }

    void OnDestroy()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }
}

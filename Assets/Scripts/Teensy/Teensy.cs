using System;
using System.Globalization;
using System.IO.Ports;
using System.Text.RegularExpressions;
using Unity.Scripting.LifecycleManagement;
using UnityEngine;

public partial class Teensy : MonoBehaviour
{
    [AutoStaticsCleanup]
    public static int ANALOG_WRITE_RES { get; private set; }

    public event EventHandler<OnFlexChangedEventArgs> OnFlexChanged;
    public class OnFlexChangedEventArgs : EventArgs
    {
        public float Value;
    }

    [SerializeField]
    private string portName;

    private SerialPort port;
    private readonly Regex protocolRegex = new(@"^(?<dest>[TU])(?<topic>[A-Z])(?:,(?<sub>[A-Z0-9]+))?:(?<value>-?\d+(?:\.\d+)?)$");

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
                string line = port.ReadLine();
                ParseLine(line);
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

    private void ParseLine(string _line)
    {
        Match match = protocolRegex.Match(_line.Trim());
        if (!match.Success) return;

        string dest = match.Groups["dest"].Value;
        if (dest != "U") return;

        string topic = match.Groups["topic"].Value;
        string sub = match.Groups["sub"].Success ? match.Groups["sub"].Value : null;
        // CultureInfo.InvariantCulture forces to expect a dot as decimal separator, as Teensy does,
        // regardless of the computer's language settings (e.g. in Italy the comma is used instead).
        float value = float.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture);
        // if (!float.TryParse(match.Groups["value"].Value, NumberStyles.None, CultureInfo.InvariantCulture, out float value)) return;

        switch (topic)
        {
            // Configuration
            case "C":
                switch (sub)
                {
                    // Analog Write Resolution
                    case "AWR":
                        ANALOG_WRITE_RES = (int)value;
                        break;
                    default:
                        return;
                }
                break;
            // Sensor
            case "S":
                switch (sub)
                {
                    // Flex sensor
                    case "FLEX":
                        FlexSensor flexSensor = new(value);
                        OnFlexChanged?.Invoke(this, new OnFlexChangedEventArgs
                        {
                            Value = flexSensor.Deserialize()
                        });
                        break;
                    default:
                        return;
                }
                break;
            default:
                return;
        }
    }

    void OnDestroy()
    {
        if (port != null && port.IsOpen)
        {
            port.Close();
        }
    }
}

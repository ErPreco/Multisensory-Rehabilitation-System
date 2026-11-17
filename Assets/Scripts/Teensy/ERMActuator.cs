using UnityEngine;

public class ERMActuator : ISerializer
{
    private readonly float value;
    private readonly float maxValue;
    private readonly int actuatorMaxValue;

    public ERMActuator(float _value, float _maxValue)
    {
        value = _value;
        maxValue = _maxValue;
        actuatorMaxValue = (int)Mathf.Pow(2, Teensy.ANALOG_WRITE_RES) - 1;
    }

    public string Serialize()
    {
        int actuatorValue = Mathf.RoundToInt(Mathf.Clamp01(value / maxValue) * actuatorMaxValue);
        Debug.Log(actuatorValue);
        return $"TH,ERM:{actuatorValue}";
    }
}

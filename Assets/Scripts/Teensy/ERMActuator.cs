using UnityEngine;

public class ERMActuator : ISerializer
{
    private readonly float value;

    public ERMActuator(float _value)
    {
        value = _value;
    }

    public string Serialize()
    {
        int clampedValue = Mathf.RoundToInt(Mathf.PingPong(value, 1023));
        return $"E,{clampedValue}";
    }
}

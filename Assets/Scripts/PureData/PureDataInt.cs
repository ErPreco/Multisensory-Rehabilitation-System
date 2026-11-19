using UnityEngine;

public abstract class PureDataInt : MonoBehaviour
{
    public int Value { get; private set; }

    public void Send(int _value)
    {
        Value = _value;
    }
}

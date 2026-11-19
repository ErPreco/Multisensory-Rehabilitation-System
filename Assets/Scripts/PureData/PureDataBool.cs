using UnityEngine;

public abstract class PureDataBool : MonoBehaviour
{
    public bool Value { get; private set; }

    public void Send(bool _value)
    {
        Value = _value;
    }
}

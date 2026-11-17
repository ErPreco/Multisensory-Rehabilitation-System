public class FlexSensor : IDeserializer<float>
{
    private readonly float value;

    public FlexSensor(float _value)
    {
        value = _value;
    }

    public float Deserialize()
    {
        return 1 - value;
    }
}

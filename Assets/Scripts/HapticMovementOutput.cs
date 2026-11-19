using UnityEngine;

[System.Serializable]
public class HapticMovementOutput : MovementOutput
{
    protected override float StartDelay => playTime;

    [SerializeField]
    private Teensy teensy;

    private float playTime;

    protected override void OnStart()
    {
        playTime = (OffScreenX - PatientMarkerX) / Movement.Speed;
    }

    protected override void OnTick(float _, float _value)
    {
        ERMActuator ERMActuator = new(_value, Movement.WindowHeight);
        teensy.Send(ERMActuator);
    }
}

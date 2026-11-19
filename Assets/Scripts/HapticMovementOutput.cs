using UnityEngine;

[System.Serializable]
public class HapticMovementOutput : MovementOutput
{
    protected override float StartDelay => playTime;

    [SerializeField]
    private Teensy teensy;
    [SerializeField]
    private Patient patient;

    private float playTime;

    protected override void OnStart()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float offScreenOffset = 2;
        float offScreenX = Camera.main.orthographicSize * screenRatio + offScreenOffset;
        float patientMarkerX = patient.MarkerPosition.x;
        playTime = (offScreenX - patientMarkerX) / Movement.Speed;
    }

    protected override void OnTick(float _, float _value)
    {
        ERMActuator ERMActuator = new(_value, Movement.WindowHeight);
        teensy.Send(ERMActuator);
    }
}

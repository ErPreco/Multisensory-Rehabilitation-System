using UnityEngine;

[System.Serializable]
public class AuditoryMovementOutput : MovementOutput
{
    protected override float StartDelay => playTime;

    [SerializeField]
    private ToggleAudio toggleAudio;
    [SerializeField]
    private Frequency frequency;
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

    protected override void OnBegin()
    {
        toggleAudio.Send(true);
    }

    protected override void OnTick(float _, float _value)
    {
        frequency.Send((int)(_value / Movement.WindowHeight * 432));
    }

    protected override void OnFinish()
    {
        toggleAudio.Send(false);
    }
}

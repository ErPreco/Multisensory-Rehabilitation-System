using UnityEngine;

[System.Serializable]
public class AuditoryMovementOutput : MovementOutput
{
    protected override float StartDelay => playTime;

    [SerializeField]
    private PDToggleMetroData toggleMetroData;
    [SerializeField]
    private PDToggleAudioData toggleAudioData;
    [SerializeField]
    private PDFrequencyData frequencyData;
    [SerializeField, Range(0, 300)]
    private uint minFrequency = 150;
    [SerializeField, Range(200, 800)]
    private uint maxFrequency = 500;

    private float playTime;

    protected override void OnStart()
    {
        playTime = (OffScreenX - PatientMarkerX) / Movement.Speed;
        maxFrequency = (uint)Mathf.Max(minFrequency, maxFrequency);

        toggleMetroData.Send(true);
    }

    protected override void OnBegin()
    {
        toggleMetroData.Send(false);
        toggleAudioData.Send(true);
    }

    protected override void OnTick(float _, float _value)
    {
        uint frequencyDelta = maxFrequency - minFrequency;
        int frequency = (int)(minFrequency + _value / Movement.WindowHeight * frequencyDelta);
        frequencyData.Send(frequency);
    }

    protected override void OnFinish()
    {
        toggleAudioData.Send(false);
    }
}

using UnityEngine;

[System.Serializable]
public abstract class MovementOutput
{
    public bool IsFinished { get; private set; }
    protected VerticalMovementSO Movement { get; private set; }
    protected float OffScreenX { get; private set; }
    protected float PatientMarkerX { get; private set; }
    protected virtual float StartDelay => 0;
    protected virtual bool ZeroCentered => false;
    /// <summary>True while the output still has something to do after the movement ended.</summary>
    protected virtual bool HasPendingWork => false;

    [SerializeField, Tooltip("Ticks per second, 0 means every frame.")]
    private uint tickRate;

    private float nextTick;
    private bool hasBegun;

    public void Start(VerticalMovementSO _movement, Patient _patient)
    {
        Movement = _movement;

        // TODO: make it constant wrt bips (e.g. 4), independent from the screen size (ultrawide => 15 bips)
        PatientMarkerX = _patient.MarkerPosition.x;
        float screenRatio = (float)Screen.width / Screen.height;
        float screenHalfWidth = Camera.main.orthographicSize * screenRatio;
        OffScreenX = (Mathf.Floor((screenHalfWidth - PatientMarkerX) / Movement.Speed) + 2) * Movement.Speed + PatientMarkerX;

        OnStart();
    }

    public void Update(float _time)
    {
        OnUpdate();

        float localTime = _time - StartDelay;

        if (!hasBegun && localTime >= 0f)
        {
            hasBegun = true;
            OnBegin();
        }

        bool isPlaying = localTime >= 0f && localTime <= Movement.TotalDuration;

        if (isPlaying && (tickRate == 0 || localTime >= nextTick))
        {
            if (tickRate != 0)
            {
                nextTick = localTime + 1f / tickRate;
            }

            OnTick(localTime, Movement.Evaluate(localTime, ZeroCentered));
        }

        if (!IsFinished && localTime > Movement.TotalDuration && !HasPendingWork)
        {
            IsFinished = true;
            OnFinish();
        }
    }

    protected virtual void OnStart() { }
    protected virtual void OnBegin() { }
    protected virtual void OnUpdate() { }
    protected virtual void OnTick(float _localTime, float _value) { }
    protected virtual void OnFinish() { }
}

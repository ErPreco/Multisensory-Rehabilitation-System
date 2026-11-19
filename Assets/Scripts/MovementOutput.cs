using UnityEngine;

[System.Serializable]
public abstract class MovementOutput
{
    public bool IsFinished { get; private set; }
    protected VerticalMovement Movement { get; private set; }
    protected virtual float StartDelay => 0;
    protected virtual bool ZeroCentered => false;
    /// <summary>True while the output still has something to do after the movement ended.</summary>
    protected virtual bool HasPendingWork => false;

    [SerializeField, Tooltip("Ticks per second, 0 means every frame.")]
    private uint tickRate;

    private float nextTick;

    public void Start(VerticalMovement _movement)
    {
        Movement = _movement;

        OnStart();
    }

    public void Update(float _time)
    {
        OnUpdate();

        float localTime = _time - StartDelay;
        bool isPlaying = localTime >= 0f && localTime <= Movement.TotalDuration;

        if (isPlaying && (tickRate == 0 || localTime >= nextTick))
        {
            if (tickRate != 0)
            {
                nextTick = localTime + 1f / tickRate;
            }

            OnTick(localTime, Movement.Evaluate(localTime, ZeroCentered));
        }

        IsFinished = localTime > Movement.TotalDuration && !HasPendingWork;
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate() { }
    protected abstract void OnTick(float _localTime, float _value);
}

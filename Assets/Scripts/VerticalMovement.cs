using UnityEngine;

[CreateAssetMenu(fileName = "VerticalMovementSO", menuName = "ScriptableObjects/VerticalMovement")]
public class VerticalMovementSO : ScriptableObject
{
    public uint UpDownDuration => upDownDuration;
    public uint HoldingDuration => holdingDuration;
    public uint Reps => reps;
    public float Speed => speed;
    public uint TotalDuration => (upDownDuration + holdingDuration) * 2 * reps;
    public float ScreenMultiplier => Camera.main.orthographicSize * 2 * screenHeightAmplitudeRatio / upDownDuration;
    public float WindowHeight => ScreenMultiplier * UpDownDuration;

    [SerializeField, Tooltip("Duration in seconds of the individual up and down movement (not the total duration).")]
    private uint upDownDuration;
    [SerializeField, Tooltip("Duration in seconds of the individual up and down holding position (not the total duration).")]
    private uint holdingDuration;
    [SerializeField, Range(0.1f, 1), Tooltip("Percentage of the screen used to display the squared-off wave of the balloons.")]
    private float screenHeightAmplitudeRatio;
    [SerializeField]
    private uint reps;
    [SerializeField, Range(1, 2), Tooltip("Horizontal speed of the wave")]
    private float speed = 1.5f;

    public float Evaluate(float _time, bool _zeroCentered = false)
    {
        // Calculate y of a squared-off wave at time, where
        //       ___         ___
        //     /    \      /    \
        //   /       \___/       \___
        //  ^           ^           ^
        //  0           k           2k
        //
        // t1 : upDownDuration
        // t2 : holdingDuration
        // k  : period (2*t1 + 2*t2)
        //
        // Mathf.PingPong travels along k/2, and the plateaus are clamped at -t1/2 and t1/2,
        // then the amplitude is scaled according to the screen height (Camera.orthographicSize)

        float pingPongY = Mathf.PingPong(_time + holdingDuration * 0.5f, upDownDuration + holdingDuration);
        float halfHolding = holdingDuration * 0.5f;
        float zeroCenterOffset = _zeroCentered ? upDownDuration * 0.5f : 0;
        float y = ScreenMultiplier * (Mathf.Clamp(pingPongY, halfHolding, halfHolding + upDownDuration) - halfHolding - zeroCenterOffset);

        return y;
    }
}

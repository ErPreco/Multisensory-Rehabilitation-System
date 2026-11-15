using UnityEngine;

public class Patient : MonoBehaviour
{
    [SerializeField]
    private Teensy teensy;
    [SerializeField]
    private Transform marker;
    [SerializeField]
    private Balloons balloons;

    private Vector3 nextPosition;

    void OnEnable()
    {
        teensy.OnFlexChanged += Teensy_OnFlexChanged;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, nextPosition, 0.6f);
    }

    private void Teensy_OnFlexChanged(object _sender, Teensy.OnFlexChangedEventArgs _event)
    {
        float y = _event.Value * balloons.BalloonsWindow - balloons.BalloonsWindow * 0.5f;
        nextPosition = new(marker.position.x, y);
    }

    void OnDisable()
    {
        teensy.OnFlexChanged -= Teensy_OnFlexChanged;
    }
}

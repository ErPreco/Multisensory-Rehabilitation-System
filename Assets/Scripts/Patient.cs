using UnityEngine;

public class Patient : MonoBehaviour
{
    public Vector3 MarkerPosition => marker.position;

    [SerializeField]
    private Teensy teensy;
    [SerializeField]
    private Transform marker;
    [SerializeField]
    private Balloons balloons;
    [SerializeField, Range(0.05f, 0.3f)]
    private float colliderRadius = 0.15f;

    private Vector3 nextPosition;

    void OnEnable()
    {
        teensy.OnFlexChanged += Teensy_OnFlexChanged;
    }

    void Update()
    {
        marker.position = Vector3.Lerp(marker.position, nextPosition, 0.6f);
    }

    void FixedUpdate()
    {
        Collider2D collider = Physics2D.OverlapCircle(marker.position, colliderRadius);
        if (collider != null && collider.TryGetComponent(out Balloon balloon))
        {
            balloon.Pop();
        }
    }

    private void Teensy_OnFlexChanged(object _sender, Teensy.OnFlexChangedEventArgs _event)
    {
        float y = _event.Value * balloons.BalloonsWindow - balloons.BalloonsWindow * 0.5f;
        nextPosition = new(marker.position.x, y, 0);
    }

    void OnDisable()
    {
        teensy.OnFlexChanged -= Teensy_OnFlexChanged;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(marker.position, colliderRadius);
    }
}

using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Balloon : MonoBehaviour
{
    [SerializeField]
    private GameObject visual;

    private CircleCollider2D collider;

    void Start()
    {
        collider = GetComponent<CircleCollider2D>();
    }

    public void Pop()
    {
        visual.SetActive(false);
        collider.enabled = false;
    }
}

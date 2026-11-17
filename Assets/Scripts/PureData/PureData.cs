using UnityEngine;
using UnityEngine.UI;

public class PureData : MonoBehaviour
{
    public bool Toggle { get; private set; }

    [SerializeField]
    private Button button;

    void Start()
    {
        button.onClick.AddListener(SendToPD);
    }

    private void SendToPD()
    {
        Toggle = !Toggle;
    }
}

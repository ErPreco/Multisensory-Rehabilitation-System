using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KneeMovement : MonoBehaviour
{
    [SerializeField]
    private VerticalMovement movement;

    [Header("Visual Movement")]
    [SerializeField]
    private bool playVisual;
    [SerializeField]
    private VisualMovementOutput visualMovementOutput;

    [Header("Haptic Movement")]
    [SerializeField]
    private bool playHaptic;
    [SerializeField]
    private HapticMovementOutput hapticMovementOutput;

    void Start()
    {
        StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        List<MovementOutput> outputs = new();
        if (playVisual) outputs.Add(visualMovementOutput);
        if (playHaptic) outputs.Add(hapticMovementOutput);

        foreach (MovementOutput output in outputs)
        {
            output.Start(movement);
        }

        float t = 0;
        while (outputs.Any(output => !output.IsFinished))
        {
            foreach (MovementOutput output in outputs)
            {
                output.Update(t);
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
}

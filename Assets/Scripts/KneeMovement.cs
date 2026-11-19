using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KneeMovement : MonoBehaviour
{
    [SerializeField]
    private VerticalMovementSO movement;
    [SerializeField]
    private Patient patient;

    [Header("Visual Output")]
    [SerializeField]
    private bool playVisual;
    [SerializeField]
    private VisualMovementOutput visualMovementOutput;

    [Header("Haptic Output")]
    [SerializeField]
    private bool playHaptic;
    [SerializeField]
    private HapticMovementOutput hapticMovementOutput;

    [Header("Auditory Output")]
    [SerializeField]
    private bool playAudio;
    [SerializeField]
    private AuditoryMovementOutput auditoryMovementOutput;

    void Start()
    {
        StartCoroutine(StartMovement());
    }

    private IEnumerator StartMovement()
    {
        List<MovementOutput> outputs = new();
        if (playVisual) outputs.Add(visualMovementOutput);
        if (playHaptic) outputs.Add(hapticMovementOutput);
        if (playAudio) outputs.Add(auditoryMovementOutput);

        foreach (MovementOutput output in outputs)
        {
            output.Start(movement, patient);
        }

        float t = 0;
        while (outputs.Any(output => !output.IsFinished))
        {
            foreach (MovementOutput output in outputs)
            {
                if (output.IsFinished) continue;
                output.Update(t);
            }

            t += Time.deltaTime;
            yield return null;
        }
    }
}

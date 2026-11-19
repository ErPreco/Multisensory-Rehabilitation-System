using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VisualMovementOutput : MovementOutput
{
    protected override bool ZeroCentered => true;
    protected override bool HasPendingWork => ballonsTransformQueue.Count > 0;

    [SerializeField]
    private Balloon balloonPrefab;

    private readonly Queue<Transform> ballonsTransformQueue = new();

    protected override void OnUpdate()
    {
        foreach (Transform balloonTransform in ballonsTransformQueue)
        {
            Vector3 ballonPosition = balloonTransform.position;
            ballonPosition.x -= Movement.Speed * Time.deltaTime;
            balloonTransform.position = ballonPosition;
        }

        float despawnLimit = -OffScreenX;
        while (ballonsTransformQueue.Count > 0 && ballonsTransformQueue.Peek().position.x <= despawnLimit)
        {
            Object.Destroy(ballonsTransformQueue.Dequeue().gameObject);
        }
    }

    protected override void OnTick(float _, float _value)
    {
        Vector3 spawnPosition = new(OffScreenX, _value, 0);
        Balloon balloon = Object.Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

        ballonsTransformQueue.Enqueue(balloon.transform);
    }
}

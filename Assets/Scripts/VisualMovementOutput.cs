using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class VisualMovementOutput : MovementOutput
{
    protected override bool ZeroCentered => true;
    protected override bool HasPendingWork => ballonsTransformQueue.Count > 0;

    [SerializeField]
    private Balloon balloonPrefab;

    private readonly Queue<Transform> ballonsTransformQueue = new();
    float offScreenX;

    protected override void OnStart()
    {
        float screenRatio = (float)Screen.width / Screen.height;
        float offScreenOffset = 2;
        offScreenX = Camera.main.orthographicSize * screenRatio + offScreenOffset;
    }

    protected override void OnUpdate()
    {
        foreach (Transform balloonTransform in ballonsTransformQueue)
        {
            Vector3 ballonPosition = balloonTransform.position;
            ballonPosition.x -= Movement.Speed * Time.deltaTime;
            balloonTransform.position = ballonPosition;
        }

        float despawnLimit = -offScreenX;
        while (ballonsTransformQueue.Count > 0 && ballonsTransformQueue.Peek().position.x <= despawnLimit)
        {
            Object.Destroy(ballonsTransformQueue.Dequeue().gameObject);
        }
    }

    protected override void OnTick(float _, float _value)
    {
        Vector3 spawnPosition = new(offScreenX, _value, 0);
        Balloon balloon = Object.Instantiate(balloonPrefab, spawnPosition, Quaternion.identity);

        ballonsTransformQueue.Enqueue(balloon.transform);
    }
}

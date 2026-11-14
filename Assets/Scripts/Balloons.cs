using System;
using System.Collections.Generic;
using UnityEngine;

public class Balloons : MonoBehaviour
{
    [SerializeField]
    private GameObject balloonPrefab;
    [SerializeField, Range(2, 8)]
    private float upDownDuration = 4;
    [SerializeField, Range(0, 5)]
    private float holdingDuration = 1;
    [SerializeField, Range(1, 1.5f)]
    private float balloonSpeed = 1;

    private readonly List<Transform> ballonsTransformList = new();

    async void Start()
    {
        await SpawnBalloons();
    }

    void Update()
    {
        foreach (Transform balloonTransform in ballonsTransformList)
        {
            Vector3 ballonPosition = balloonTransform.position;
            ballonPosition.x -= balloonSpeed * Time.deltaTime;
            balloonTransform.position = ballonPosition;
        }
    }

    private async Awaitable SpawnBalloons()
    {
        int time = 0;

        while (true)
        {
            Transform balloonTransform = Instantiate(balloonPrefab, transform).transform;
            Vector3 balloonPosition = balloonTransform.position;
            balloonPosition.y = GetBallonPositionByFunction(time);
            balloonTransform.position = balloonPosition;

            ballonsTransformList.Add(balloonTransform);

            time++;

            await Awaitable.WaitForSecondsAsync(1);
        }
    }

    private float GetBallonPositionByFunction(float _time)
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
        // Mathf.PingPong travels along k/2, and the plateaus are clamped at -t1/2 and t1/2

        float multiplier = 2;

        float y = Mathf.PingPong(_time + holdingDuration * 0.5f, upDownDuration + holdingDuration);
        float halfHolding = holdingDuration * 0.5f;
        return multiplier * (Mathf.Clamp(y, halfHolding, halfHolding + upDownDuration) - halfHolding - upDownDuration * 0.5f);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloons : MonoBehaviour
{
    [SerializeField]
    private GameObject balloonPrefab;
    [SerializeField, Range(2, 8), Tooltip("Duration in seconds of the individual up and down movement (not the total duration)")]
    private int upDownDuration = 4;
    [SerializeField, Range(0, 5), Tooltip("Duration in seconds of the individual up and down holding position (not the total duration)")]
    private int holdingDuration = 1;
    [SerializeField, Range(1, 1.5f), Tooltip("Horizontal speed of the balloons")]
    private float balloonSpeed = 1;
    [SerializeField, Range(0.4f, 0.9f), Tooltip("Percentage of the screen used to display the squared-off wave of the balloons")]
    private float screenHeightAmplitudeRatio = 0.7f;

    private readonly List<Transform> ballonsTransformList = new();

    void Start()
    {
        StartCoroutine(SpawnBalloons());
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

    private IEnumerator SpawnBalloons()
    {
        WaitForSeconds waitForSeconds1 = new(1);
        int time = 0;

        while (true)
        {
            Transform balloonTransform = Instantiate(balloonPrefab, transform).transform;
            Vector3 balloonPosition = balloonTransform.position;
            balloonPosition.y = GetBallonSpawnPosition(time);
            balloonTransform.position = balloonPosition;

            ballonsTransformList.Add(balloonTransform);

            time++;

            yield return waitForSeconds1;
        }
    }

    private float GetBallonSpawnPosition(float _time)
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

        float y = Mathf.PingPong(_time + holdingDuration * 0.5f, upDownDuration + holdingDuration);
        float halfHolding = holdingDuration * 0.5f;
        float multiplier = Camera.main.orthographicSize * 2 * screenHeightAmplitudeRatio / upDownDuration;
        return multiplier * (Mathf.Clamp(y, halfHolding, halfHolding + upDownDuration) - halfHolding - upDownDuration * 0.5f);
    }
}

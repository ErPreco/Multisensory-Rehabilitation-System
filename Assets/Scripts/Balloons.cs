using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloons : MonoBehaviour
{
    public float BalloonsWindow { get; private set; }

    [SerializeField]
    private GameObject balloonPrefab;
    [SerializeField, Range(2, 8), Tooltip("Duration in seconds of the individual up and down movement (not the total duration)")]
    private uint upDownDuration = 4;
    [SerializeField, Range(0, 5), Tooltip("Duration in seconds of the individual up and down holding position (not the total duration)")]
    private uint holdingDuration = 1;
    [SerializeField, Range(1, 1.5f), Tooltip("Horizontal speed of the balloons")]
    private float balloonSpeed = 1;
    [SerializeField, Range(0.4f, 0.9f), Tooltip("Percentage of the screen used to display the squared-off wave of the balloons")]
    private float screenHeightAmplitudeRatio = 0.5f;
    [SerializeField]
    private uint reps = 10;

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
        uint balloonsCounter = 0;
        uint balloonsPerRep = (upDownDuration + holdingDuration) * 2;
        uint totalBalloonsCount = 1 + balloonsPerRep * reps;

        while (totalBalloonsCount > 0)
        {
            Transform balloonTransform = Instantiate(balloonPrefab, transform).transform;
            balloonTransform.position = GetBallonSpawnPosition(balloonsCounter);

            ballonsTransformList.Add(balloonTransform);

            balloonsCounter++;
            totalBalloonsCount--;

            yield return waitForSeconds1;
        }
    }

    private Vector2 GetBallonSpawnPosition(float _time)
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
        // Mathf.PingPong travels along k/2, and the plateaus are clamped at -t1/2 and t1/2,
        // then the amplitude is scaled according to the screen height (Camera.orthographicSize)
        // and offset off screen on x

        float pingPongY = Mathf.PingPong(_time + holdingDuration * 0.5f, upDownDuration + holdingDuration);
        float halfHolding = holdingDuration * 0.5f;
        float multiplier = Camera.main.orthographicSize * 2 * screenHeightAmplitudeRatio / upDownDuration;
        float y = multiplier * (Mathf.Clamp(pingPongY, halfHolding, halfHolding + upDownDuration) - halfHolding - upDownDuration * 0.5f);
        BalloonsWindow = multiplier * upDownDuration;

        float ratio = (float)Screen.width / Screen.height;
        float offScreenOffset = 2;
        float x = Camera.main.orthographicSize * ratio + offScreenOffset;

        return new(x, y);
    }
}

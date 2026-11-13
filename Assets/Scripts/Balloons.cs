using System;
using System.Collections.Generic;
using UnityEngine;

public class Balloons : MonoBehaviour
{
    [SerializeField]
    private GameObject balloonPrefab;
    [SerializeField, Range(2, 8)]
    private float halfMovementDuration = 4;
    [SerializeField, Range(0, 5)]
    private float holdingDuration = 1;
    [SerializeField, Range(0.5f, 2)]
    private float balloonPeriod = 1;

    private readonly float fPI = Mathf.PI;
    private float sinHalfPeriod;
    private float time;
    private float timeSinceLastBallonSpawned;
    private List<Transform> ballonsTransformList = new();

    void Start()
    {
        sinHalfPeriod = halfMovementDuration + holdingDuration;
    }

    void Update()
    {
        time += Time.deltaTime;
        timeSinceLastBallonSpawned += Time.deltaTime;

        if (timeSinceLastBallonSpawned >= balloonPeriod)
        {
            timeSinceLastBallonSpawned = 0;

            Transform balloonTransform = Instantiate(balloonPrefab, transform).transform;
            Vector3 balloonPosition = balloonTransform.position;
            balloonPosition.y = SinFunction(time);
            balloonTransform.position = balloonPosition;

            ballonsTransformList.Add(balloonTransform);
        }

        foreach (Transform balloonTransform in ballonsTransformList)
        {
            Vector3 ballonPosition = balloonTransform.position;
            ballonPosition.x -= balloonPeriod * Time.deltaTime;
            balloonTransform.position = ballonPosition;
        }
    }

    private float SinFunction(float _x)
    {
        return 3 * Mathf.Sin(fPI / sinHalfPeriod * _x - fPI / 2);
    }
}

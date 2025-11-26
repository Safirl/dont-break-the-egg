using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections;
using Levels;
using Scenes;


enum FingerState {
    Grounded,
    Lifting,
    MovingToTarget,
    Planting
}   

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float timeOffset = 1f;
    private float _t = 0f;

    private Dictionary<string, FingerState> _fingerStates = new Dictionary<string, FingerState>();
    
    [SerializeField] private FingerStepper thumbStepper;
    [SerializeField] private FingerStepper indexStepper;
    [SerializeField] private FingerStepper middleStepper;
    [SerializeField] private FingerStepper ringStepper;
    [SerializeField] private FingerStepper pinkyStepper;

    private void Awake()
    {
        if (!playerTransform)
        {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        }

        if (!playerTransform)
        {
            Debug.LogError("No player found");
            return;
        }
        StartCoroutine(FingerUpdateCoroutine());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveHandOntoSpline();
    }

    void MoveHandOntoSpline()
    {
        if (!spline) return;
        
        var timeLeft = Level.Instance.TimeLeft +  timeOffset;
        var totalTime = Level.Instance.TotalTime +  timeOffset;
        _t = (totalTime - timeLeft) / totalTime;

        spline.Spline.Evaluate(_t, out var pos, out var tan, out var up);

        // Conversion en world space
        pos = spline.transform.TransformPoint(pos);
        tan = spline.transform.TransformDirection(tan);
        up  = spline.transform.TransformDirection(up);

        transform.SetPositionAndRotation(pos, Quaternion.LookRotation(tan, up));
    }

    IEnumerator FingerUpdateCoroutine()
    {
        while (true)
        {
            do
            {
                thumbStepper.TryMove();
                middleStepper.TryMove();
                pinkyStepper.TryMove();
                yield return null;
            } while (thumbStepper.IsMoving || middleStepper.IsMoving || pinkyStepper.IsMoving);
            do
            {
                indexStepper.TryMove();
                ringStepper.TryMove();
                yield return null;
            } while (ringStepper.IsMoving || indexStepper.IsMoving);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //An event would be better but that means we would have to look for smth to listen to it.
            Level.Instance.KillPlayer();
        }
    }
}

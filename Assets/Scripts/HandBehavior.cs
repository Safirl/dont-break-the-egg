using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System.Collections;


enum FingerState {
    Grounded,
    Lifting,
    MovingToTarget,
    Planting
}   

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float timeOffset = 1f;
    private float t = 0f;
    
    private Dictionary<string, FingerState> _fingerStates = new Dictionary<string, FingerState>();
    
    [SerializeField] private FingerStepper thumbStepper;
    [SerializeField] private FingerStepper indexStepper;
    [SerializeField] private FingerStepper middleStepper;
    [SerializeField] private FingerStepper ringStepper;
    [SerializeField] private FingerStepper pinkyStepper;

    private void Awake()
    {
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
        
        float timeLeft = GameManager.Instance.timeLeft +  timeOffset;
        float totalTime = GameManager.Instance._totalTime +  timeOffset;
        t = (totalTime - timeLeft) / totalTime;

        spline.Spline.Evaluate(t, out var pos, out var tan, out var up);

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
}

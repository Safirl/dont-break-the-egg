using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float timeOffset = 1f;
    private float t = 0f;

    [SerializeField] private GameObject armPrefab;
    private List<GameObject> arms = new List<GameObject>();
    private List<Vector3> positionHistory = new List<Vector3>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float timeLeft = GameManager.Instance.timeLeft +  timeOffset;
        float totalTime = GameManager.Instance.totalTime +  timeOffset;
        t = (totalTime - timeLeft) / totalTime;

        spline.Spline.Evaluate(t, out var pos, out var tan, out var up);

        // Conversion en world space
        pos = spline.transform.TransformPoint(pos);
        tan = spline.transform.TransformDirection(tan);
        up  = spline.transform.TransformDirection(up);

        transform.SetPositionAndRotation(pos, Quaternion.LookRotation(tan, up));

        var index = 0;
        foreach (var arm in arms)
        {
                        
            // var point = positionHistory[Mathf.Clamp(index, 0, positionHistory.Count - 1)];
            // arm
        }
    }
    
    void GrowArm()
    {
        arms.Add(Instantiate(armPrefab));
    }
}

using UnityEngine;
using UnityEngine.Splines;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    [SerializeField] private float timeOffset = 1f;
    // [SerializeField] private float speed;
    private float t = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        float timeLeft = GameManager.Instance.timeLeft +  timeOffset;
        float totalTime = GameManager.Instance.totalTime +  timeOffset;
        t = (totalTime - timeLeft) / totalTime;

        spline.Evaluate(t, out var position, out var tangent, out var up);
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(tangent, up);
    }
}

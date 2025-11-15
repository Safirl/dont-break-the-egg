using UnityEngine;
using UnityEngine.Splines;

public class HandBehavior : MonoBehaviour
{
    [SerializeField] private SplineContainer spline;
    // [SerializeField] private float speed;
    private float t = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
               
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime;
        t %= 1f;

        spline.Evaluate(t, out var position, out var tangent, out var up);
        transform.position = position;
        transform.rotation = Quaternion.LookRotation(tangent, up);
    }
}

using System.Collections;
using UnityEngine;

public class FingerStepper : MonoBehaviour
{
    [SerializeField] private Transform homeTransform;
    //How far the finger tip can get away from homeTransform
    [SerializeField] private float maxRadiusDistance;
    
    [SerializeField] private float stepDuration;
    [SerializeField] float stepOvershootFraction;
    
    public bool IsMoving { get; private set; }

    IEnumerator MoveToHome()
    {
        IsMoving = true;
        
        Vector3 startPoint = transform.position;
        Quaternion startRot = transform.rotation;
        
        Quaternion endRot = homeTransform.rotation;
        
        Vector3 towardHome = (homeTransform.position - transform.position);
        float overshootDistance = maxRadiusDistance * stepOvershootFraction;
        Vector3 overshootVector = towardHome * overshootDistance;
        overshootVector = Vector3.ProjectOnPlane(overshootVector, Vector3.up);
        
        Vector3 endPoint = homeTransform.position + overshootVector;
        
        Vector3 centerPoint = (startPoint + endPoint) / 2;
        centerPoint += homeTransform.up * Vector3.Distance(startPoint, endPoint) / 2f;

        float elapsedTime = 0f;

        do
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / stepDuration;
            normalizedTime = Easing.EaseInOutCubic(normalizedTime);
            
            // Quadratic bezier curve
            transform.position =
                Vector3.Lerp(
                    Vector3.Lerp(startPoint, centerPoint, normalizedTime),
                    Vector3.Lerp(centerPoint, endPoint, normalizedTime),
                    normalizedTime
                );
            
            // transform.position = Vector3.Lerp(startPoint, centerPoint, normalizedTime);
            transform.rotation = Quaternion.Slerp(startRot, endRot, normalizedTime);
            
            yield return null;
        }
        while (elapsedTime < stepDuration);
        IsMoving = false;
    }

    public void TryMove()
    {
        if (IsMoving) return;
        
        var distFromHome = Vector3.Distance(homeTransform.position, transform.position);

        if (distFromHome > maxRadiusDistance)
        {
            StartCoroutine(MoveToHome());
        }
    }
}

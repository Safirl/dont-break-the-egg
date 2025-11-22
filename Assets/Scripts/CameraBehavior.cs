using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Levels;
using UnityEngine.Events;

public class CameraBehavior : MonoBehaviour
{
    // [SerializeField] private Transform playerPosition;
    [SerializeField] private Transform targetPosition;
    [SerializeField] private float damping = .8f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startRotation;

    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    private void Start()
    {
        if (!Level.Instance) Debug.LogError("CAMERABEHAVIOR : GameLevel doesn't exist"); 
        Level.Instance.OnLevelStarted += OnLevelStarted;
    }

    public void OnLevelStarted()
    {
        Level.Instance.OnLevelStarted -= OnLevelStarted;
        if (!GameManager.Instance.isDevMode)
        {
            // transform.position = targetPosition.position + offset;
            transform.DOMove(targetPosition.position + offset, 10f).SetEase(Ease.InOutExpo)
                .OnComplete(() => {
                    OnIntroAnimationCompleted.Invoke();
                });
        } else {
            transform.position = targetPosition.position + offset;
            OnIntroAnimationCompleted.Invoke();
        }
    }

    private void LateUpdate()
    {
        if (!Level.Instance.IsLevelInitialized) return;
        
        Vector3 newTargetPosition = targetPosition.position + offset;

        float compareDistSqr = .001f;
        
        if (newTargetPosition == transform.position) return;
        
        if ((newTargetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = newTargetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, newTargetPosition, 1 - Mathf.Exp(-damping * Time.deltaTime));
        }
    }
    
    
    private void OnDisable()
    {
        if (!Level.Instance)
        {
            Level.Instance.OnLevelStarted -= OnLevelStarted;
        }
    }

}

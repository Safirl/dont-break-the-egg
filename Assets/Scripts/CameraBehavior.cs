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
    public bool isDevMode;
    public bool isIntroAnimationComplete = false;

    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (!pan || !player)
        // {
        //     pan = GameObject.FindWithTag("Target").transform;
        //     player = GameObject.FindWithTag("Player").transform;
        // }
    }

    public void SetTargetPosition(Transform tp)
    {
        targetPosition = tp;
    }

    private void Awake()
    {
        if (!(GameLevel.Instance is GameLevel)) throw new Exception("CAMERABEHAVIOR : GameLevel doesn't exist"); 
        
        GameLevel.Instance.OnLevelStarted += OnLevelStarted;
        
    }
    
    
    // Update is called once per frame
    void Update()
    {
    }

    public void OnLevelStarted()
    {

        if (!isDevMode)
        {
            
            transform.position = targetPosition.position + offset;
            
            transform.DOMove(targetPosition.position + offset, 10f).SetEase(Ease.InOutExpo)
                .OnComplete(() => {
                    OnIntroAnimationCompleted.Invoke();
                    isIntroAnimationComplete = true;
                });
        } else {
            transform.position = targetPosition.position + offset;
            OnIntroAnimationCompleted.Invoke();
                    isIntroAnimationComplete = true;
        }
            
            
            
    }

    private void LateUpdate()
    {


        if (!GameManager.Instance.IsGameRunning) return;


        if (!targetPosition)
        {
            targetPosition = new GameObject("FakeTarget").transform;
        }
        
        
        Vector3 newTargetPosition = targetPosition.position + offset;

        float compareDistSqr = .001f;

        
        if (newTargetPosition == transform.position) return;
        
        
        
        if ((newTargetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = newTargetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, newTargetPosition, Time.deltaTime * damping);
        }
    }
    
    
    private void OnDisable()
    {
        if (GameLevel.Instance != null)
        {
            GameLevel.Instance.OnLevelStarted -= OnLevelStarted;
        }
    }

}

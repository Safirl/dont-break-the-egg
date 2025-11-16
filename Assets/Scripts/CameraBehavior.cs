using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using Levels;
using UnityEngine.Events;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform pan;
    [SerializeField] private float damping = .8f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startRotation;
    public bool isDevMode;
    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (!pan || !player)
        {
            pan = GameObject.FindWithTag("Target").transform;
            player = GameObject.FindWithTag("Player").transform;
        }
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
            
            transform.position = pan.position + offset;
            
            transform.DOMove(player.position + offset, 10f).SetEase(Ease.InOutExpo)
                .OnComplete(() => {
                    OnIntroAnimationCompleted.Invoke();
                });
        } else {
            transform.position = player.position + offset;
            OnIntroAnimationCompleted.Invoke();
        }
            
            
            
    }

    private void LateUpdate()
    {
        print(GameManager.Instance.IsGameRunning);
                
        if (!GameManager.Instance.IsGameRunning) return;
        
        
        if (!player)
            throw new Exception("Player or gameObject is null");
        
        
        var targetPosition = player.position + offset;

        var compareDistSqr = .001f;

        
        if (targetPosition == transform.position) return;
        
        
        
        if ((targetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);
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

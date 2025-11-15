using System;
using UnityEngine;
using UnityEngine.UIElements;
using DG.Tweening;
using UnityEngine.Events;

public class CameraBehavior : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform pan;
    [SerializeField] private float damping = .8f;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 startRotation;

    public delegate void OnIntroAnimationCompletedDelegate();

    public OnIntroAnimationCompletedDelegate OnIntroAnimationCompleted;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameManager.Instance.OnLevelStarted += OnLevelStarted ;
        if (!pan || !player)
        {
            pan = GameObject.FindWithTag("Target").transform;
            player = GameObject.FindWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnLevelStarted()
    {
        transform.position = pan.position + offset;
        
        transform.DOMove(player.position + offset, 10f).SetEase(Ease.InOutExpo).OnComplete(() =>
        {
            OnIntroAnimationCompleted.Invoke();
        });
    }

    private void LateUpdate()
    {
        if (!player)
        {
            print("player or gameObject is null!");
            return;
        }

        if (!GameManager.Instance.HasLevelStarted)
        {
            return;
        }
        
        var targetPosition = player.position + offset;
        //easing
        var compareDistSqr = .001f;

        if (targetPosition == transform.position)
        {
            return;
        }
        
        if ((targetPosition - transform.position).sqrMagnitude <= compareDistSqr)
        {
            transform.position = targetPosition;
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * damping);
        }
    }
}

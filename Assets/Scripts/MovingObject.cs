using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingObject : MonoBehaviour
{
    [SerializeField] private Vector3 impulseForce;
    [SerializeField] private float cooldown;
    [SerializeField] private Rigidbody rigidBody;
    private float delta = 0f;
    private float elapsedTime;

    private void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime < cooldown + delta) return;
        
        delta = (Random.value -.5f)*2;
        print(cooldown);
        rigidBody.AddForce(impulseForce,  ForceMode.Impulse);
        elapsedTime = 0f;
    }
}

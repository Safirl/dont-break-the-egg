using System;
using Levels;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class EggBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private float strength = 5;
    [SerializeField] private float jumpStrength = 10;
    [SerializeField] private float maxVelocity = 10;
    [SerializeField] private float collisionDotProduct = .85f;
    [SerializeField] private float destructionSpeed = 8;
    [FormerlySerializedAs("collisionMask")] [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float moveDistance = 3f;
    
    private float jumpDelay = .2f;
    private float jumpCooldown;

    private void Start()
    {
        //GameLevel.Instance.OnLevelStarted += OnLevelStarted;
        //GameLevel.Instance.OnLevelEnded += OnLevelEnded;
    }

    
    private void OnEnable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelStarted += OnLevelStarted;
            LevelManager.Instance.OnLevelEnded += OnLevelEnded;
        }
    }

    private void OnDisable()
    {
        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.OnLevelStarted -= OnLevelStarted;
            LevelManager.Instance.OnLevelEnded -= OnLevelEnded;
        }
    }
    
        
    private void OnLevelStarted()
    {
        // rigidBody.useGravity = true;
    }
    
    private void OnLevelEnded()
    {
        // rigidBody.useGravity = true;
    }

    private void Update()
    { 
        jumpCooldown += Time.deltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (LevelManager.Instance && !LevelManager.Instance.IsLevelRunning) return;
        
        var force = Vector3.zero;
        //If the player is to high, we don't want to allow movements
        Physics.Raycast(gameObject.transform.position, new Vector3(0,-1,0), out RaycastHit hit, moveDistance,collisionMask);
        if (!hit.collider)
        {
            return;
        }

        //jump
        if (Keyboard.current.spaceKey.wasPressedThisFrame && jumpCooldown > jumpDelay)
        {
            Physics.Raycast(gameObject.transform.position, new Vector3(0,-1,0), out RaycastHit jumpHit, jumpDistance,collisionMask);
            if (jumpHit.collider != null)
            {
                force.y += jumpStrength;
                jumpCooldown = 0f;
            }
        }

        //sides
        if (rigidBody.linearVelocity.x > -maxVelocity && Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            force.x -= strength;
        }
        if (rigidBody.linearVelocity.x < maxVelocity && Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            force.x += strength;
        }

        //forward/backward
        if (rigidBody.linearVelocity.z > -maxVelocity && Keyboard.current.upArrowKey.wasPressedThisFrame)
        {
            force.z -= strength;
        }
        if (rigidBody.linearVelocity.z < maxVelocity && Keyboard.current.downArrowKey.wasPressedThisFrame)
        {
            force.z += strength;
        }

        PushEgg(force);
    }

    void PushEgg(Vector3 direction)
    {
        rigidBody.AddForce(direction, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        var contactNormal = other.contacts[0].normal;
        if (Vector3.Dot(contactNormal, new Vector3(0, 1, 0)) > collisionDotProduct &&
            rigidBody.linearVelocity.y > destructionSpeed)
        {
            print("Y destroyed" + rigidBody.linearVelocity.y);    
        }
        // else if (Math.Abs(Vector3.Dot(contactNormal, new Vector3(1, 0, 0))) > collisionDotProduct &&
        //          Math.Abs(rigidBody.linearVelocity.x) > destructionSpeed)
        // {
        //     print("X destroyed" + rigidBody.linearVelocity.x);
        // }
        // else if (Math.Abs(Vector3.Dot(contactNormal, new Vector3(0, 0, 1))) > collisionDotProduct &&
        //          Math.Abs(rigidBody.linearVelocity.z) > destructionSpeed)
        // {
        //     print("Z destroyed" + rigidBody.linearVelocity.z);
        // }
        
    }
}

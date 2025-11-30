using System;
using System.Collections.Generic;
using Levels;
using Scenes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EggBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private List<GameObject> brokenEggParts;
    
    [SerializeField] private float strength = 5;
    [SerializeField] private float jumpStrength = 10;
    [SerializeField] private float maxVelocity = 10;
    [SerializeField] private float collisionDotProduct = .85f;
    [SerializeField] private float destructionSpeed = 8;
    [FormerlySerializedAs("collisionMask")] [SerializeField] private LayerMask collisionMask;
    [SerializeField] private float jumpDistance = 1f;
    [SerializeField] private float moveDistance = 3f;
    private Vector3 strengthInput;
    private bool jumpPressed;
    private bool jumped = true;
    private bool leftPressed;
    private bool movedLeft = true;
    private bool rightPressed;
    private bool movedRight = true;
    private bool upPressed;
    private bool movedUp = true;
    private bool downPressed;
    private bool movedDown = true;
    
    private float jumpDelay = .5f;
    private float jumpCooldown;

    private void Start()
    {
        jumpCooldown = jumpDelay;
        Level.Instance.OnPlayerKilled += BreakEgg;
    }

    private void Update()
    { 
        jumpCooldown += Time.deltaTime;
        //If we didn't do the movement, we don't want to change the keyPressed state
        if (jumped)
        {
            jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            if (jumpPressed) jumped = false;
        }

        if (movedLeft)
        {
            leftPressed = Keyboard.current.leftArrowKey.wasPressedThisFrame;
            if (leftPressed) movedLeft = false;
        }

        if (movedRight)
        {
            rightPressed = Keyboard.current.rightArrowKey.wasPressedThisFrame;
            if (rightPressed) movedRight = false;
        }

        if (movedUp)
        {
            upPressed = Keyboard.current.upArrowKey.wasPressedThisFrame;
            if (upPressed) movedUp = false;
        }

        if (movedDown)
        {
            downPressed = Keyboard.current.downArrowKey.wasPressedThisFrame;
            if (downPressed) movedDown = false;
        }
    }

    private void FixedUpdate()
    {
        if (Level.Instance && !Level.Instance.IsLevelInitialized) return;
        
        strengthInput = Vector3.zero;
        //If the player is too high, we don't want to allow movements
        Physics.Raycast(gameObject.transform.position, new Vector3(0,-1,0), out RaycastHit hit, moveDistance,collisionMask);
        if (!hit.collider)
        {
            return;
        }

        //jump
        if (jumpPressed && jumpCooldown > jumpDelay)
        {
            Physics.Raycast(gameObject.transform.position, new Vector3(0,-1,0), out RaycastHit jumpHit, jumpDistance,collisionMask);
            if (jumpHit.collider)
            {
                strengthInput.y += jumpStrength;
                jumpCooldown = 0f;
            }
            jumped = true;
        }

        //sides
        if (rightPressed && rigidBody.linearVelocity.x > -maxVelocity)
        {
            strengthInput.x -= strength;
            movedRight = true;
        }
        if (leftPressed && rigidBody.linearVelocity.x < maxVelocity)
        {
            strengthInput.x += strength;
            movedLeft = true;
        }

        //forward/backward
        if (upPressed && rigidBody.linearVelocity.z > -maxVelocity)
        {
            strengthInput.z -= strength;
            movedUp = true;
        }
        if (downPressed && rigidBody.linearVelocity.z < maxVelocity)
        {
            strengthInput.z += strength;
            movedDown = true;
        }

        PushEgg(strengthInput);
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
    
    private void BreakEgg()
    {
        var eggRenderer = GetComponent<MeshRenderer>();
        if (eggRenderer)
        {
            eggRenderer.enabled = false;
        }
        foreach (var brokenEggPart in brokenEggParts)
        {
            if (!brokenEggPart)
            {
                Debug.LogWarning("Invalid egg part found");
                continue;
            }
            brokenEggPart.SetActive(true);
            var eggRigidBody = brokenEggPart.GetComponent<Rigidbody>();
            if (!eggRigidBody) return;

            var force = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(0f, 3.0f), Random.Range(-3.0f, 3.0f));
            eggRigidBody.AddForce(force, ForceMode.Impulse);
        }
    }
}

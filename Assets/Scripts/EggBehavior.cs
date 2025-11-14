using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class EggBehavior : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private float m_strength = 5;
    [SerializeField] private float m_maxVelocity = 10;

    // Update is called once per frame
    void Update()
    {
        var force = Vector3.zero;
        
        //up
        if (Math.Abs(m_rigidBody.linearVelocity.y) < m_maxVelocity)
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                // force.y += m_strength;
            }
        }

        //sides
        if (Math.Abs(m_rigidBody.linearVelocity.x) < m_maxVelocity)
        {
            if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
            {
                force.x += m_strength;
            }
            if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
            {
                force.x -= m_strength;
            }
        }

        //forward/backward
        if (Math.Abs(m_rigidBody.linearVelocity.z) < m_maxVelocity)
        {
            if (Keyboard.current.upArrowKey.wasPressedThisFrame)
            {
                force.z -= m_strength;
            }
            if (Keyboard.current.downArrowKey.wasPressedThisFrame)
            {
                force.z += m_strength;
            }
        }

        PushEgg(force);
        print(m_rigidBody.linearVelocity);
    }

    void PushEgg(Vector3 direction)
    {
        m_rigidBody.AddForce(direction, ForceMode.Impulse);
    }
}

using System;
using Levels;
using UnityEngine;

namespace Zones
{
    public class DangerZone : Zone
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                //An event would be better but that means we would have to look for smth to listen to it.
                Level.Instance.KillPlayer();
            }
        }
    }
}
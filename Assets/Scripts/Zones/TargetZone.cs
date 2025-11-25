using UnityEngine;

namespace Zones
{
    public class TargetZone : Zone
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnZoneEntered?.Invoke();
        }
    }
}
using UnityEngine;

namespace Zones
{
    public class TargetZone : Zone
    {
        public bool breakEgg;
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnZoneEntered?.Invoke();
        }
    }
}
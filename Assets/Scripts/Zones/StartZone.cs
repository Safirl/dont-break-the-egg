using UnityEngine;

namespace Zones
{
    public class StartZone : Zone
    {
        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            OnZoneExited?.Invoke();
        }
    }
}
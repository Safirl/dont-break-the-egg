using UnityEngine;

namespace Zones
{
    public class PanZone : Zone
    {
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            GameManager.WinLevel();
        }
    }
}
using UnityEngine;

namespace Zones
{
    public class PanZone : Zone
    {
        private void OnTriggerEnter(Collider other)
        {
            GameManager.WinLevel();
        }
    }
}
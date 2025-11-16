using UnityEngine;

namespace Zones
{
    public class StartZone : Zone
    {
        private void OnTriggerExit(Collider other)
        {
            GameManager.StartLevel();
        }
    }
}
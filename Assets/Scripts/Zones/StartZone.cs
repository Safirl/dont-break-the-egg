using UnityEngine;

namespace Zones
{
    public class StartZone : Zone
    {
        private void OnTriggerExit(Collider other)
        {
            print("START THE LVL");
        }
    }
}
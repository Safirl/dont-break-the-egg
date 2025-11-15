using UnityEngine;

namespace Zones
{
    public class PanZone : Zone
    {
        private void OnTriggerEnter(Collider other)
        {
            print("END THE LVL");
        }
    }
}
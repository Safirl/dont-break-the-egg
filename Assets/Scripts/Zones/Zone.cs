using System;
using UnityEngine;

namespace Zones
{
    public abstract class Zone : MonoBehaviour
    {
        public delegate void OnZoneTriggeredDelegate();
    
        public OnZoneTriggeredDelegate OnZoneEntered;
        public OnZoneTriggeredDelegate OnZoneExited;
    }
}

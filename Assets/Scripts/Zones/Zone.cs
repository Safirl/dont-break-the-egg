using System;
using UnityEngine;

namespace Zones
{
    public abstract class Zone : MonoBehaviour
    {
    
        //Inutile, GameManager est un singleton tu peux le récupérer via GameManager.Instance
        public GameManager GameManager;
    }
}

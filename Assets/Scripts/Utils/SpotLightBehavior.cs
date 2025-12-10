using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SpotLightBehavior : MonoBehaviour
{
    [SerializeField] private Light spotLight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spotLight.intensity = 0f;
        spotLight.DOIntensity(10f, 2f).SetEase(Ease.OutExpo).SetDelay(.8f);
    }
}

using UnityEngine;

namespace Levels
{
    public class TransitionAnimation : MonoBehaviour
    {
        public delegate void OnFadeOutTransitionOverDelegate();
        public OnFadeOutTransitionOverDelegate FadeOutTransitionOver;
        public void OnFadeOutTransitionOver()
        {
            FadeOutTransitionOver?.Invoke();
            print("transition over");
        }
    }
}

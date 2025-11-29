using UnityEngine;
using UnityEngine.UI;
using Zones;

namespace Levels
{
    public class LevelWin : Level
    {
        public Button nextButton;

        public new void Start()
        {
            if (!targetZone)
            {
                targetZone = FindAnyObjectByType<TargetZone>();
                if (!targetZone)
                {
                    Debug.LogError($"Target zone not found on {gameObject.name}");
                    return;
                }
            }
            targetZone.OnZoneEntered += OnPlayerReachedEnd;
            // nextButton.onClick.AddListener(OnButtonClicked);
            IsLevelInitialized = true;
        }

        void OnButtonClicked()
        {
            nextButton.onClick.RemoveListener(OnButtonClicked);
            OnLevelEnded?.Invoke(Scenes.Scenes.NEXT_LEVEL);
        }

        public override void OnPlayerReachedEnd()
        {
            targetZone.OnZoneEntered -= OnPlayerReachedEnd;
            // IsPlayerRunning = false;
            // // IsLevelInitialized = false;
            OnLevelEnded?.Invoke(Scenes.Scenes.NEXT_LEVEL);
        }
    }
}

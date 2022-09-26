using UnityEngine;
using DG.Tweening;

namespace Emir
{
    public static class CommonTypes
    {
        //GENERICS
        public static int DEFAULT_FPS = 60;
        public static int DEFAULT_THREAD_SLEEP_MS = 100;

        //INTERFACES
        public static float UI_DEFAULT_FLY_CURRENCY_DURATION = 0.5F;

        //SOUNDS
        public static string SFX_CLICK = "CLICK";
        public static string SFX_CURRENCY_FLY = "CURRENCY_FLY";
        public static string SFX_WIN = "WIN";
        public static string SFX_LOSE = "LOSE";

        //DATA KEYS
        public static string PLAYER_DATA_KEY = "player_data";
        public static string LEVEL_ID_DATA_KEY = "level_data";
        public static string SOUND_STATE_KEY = "sound_state_data";
        public static string VIBRATION_STATE_KEY = "vibration_state_data";
        public static string CURRENCY_DATA_KEY = "Currency";

        //ANIMATION KEYS

        public static string IDLE_KEY = "Idle";
        public static string RUN_KEY = "Run";
        public static string FALLING_KEY = "Falling";
        public static string DANCE_KEY = "Dance";
        public static string DEFEAT_KEY = "Defeat";

        //TAGS
        public static string COLLECTABLE_TAG = "Collectable";
        public static string COLLECT_TAG = "Collect";
        public static string JUMP_TAG = "Jump";
        public static string SPEED_UP_TAG = "SpeedUp";
        public static string RANDOM_GATE = "Gate/RandomGate";
        public static string ORDER_GATE = "Gate/OrderGate";
        public static string OBSTACLE_TAG = "Obstacle";
        public static string LAVA_TAG = "Lava";

#if UNITY_EDITOR

        public static string EDITOR_LEVELS_PATH = "Levels/";
        public static string EDITOR_GAME_SETTINGS_PATH = "GameSettings";

#endif
    }

    public static class GameUtils
    {
        public static void SwitchCanvasGroup(CanvasGroup a, CanvasGroup b, float duration = 0.25F)
        {
            Sequence sequence = DOTween.Sequence();

            if (a != null)
                sequence.Join(a.DOFade(0, duration));
            if (b != null)
                sequence.Join(b.DOFade(1, duration));

            sequence.OnComplete(() =>
            {
                if (a != null)
                    a.blocksRaycasts = false;
                if (b != null)
                    b.blocksRaycasts = true;
            });

            sequence.Play();
        }

        public static Vector2 WorldToCanvasPosition(RectTransform canvas, Camera camera, Vector3 worldPosition)
        {
            Vector2 tempPosition = camera.WorldToViewportPoint(worldPosition);

            tempPosition.x *= canvas.sizeDelta.x;
            tempPosition.y *= canvas.sizeDelta.y;

            tempPosition.x -= canvas.sizeDelta.x * canvas.pivot.x;
            tempPosition.y -= canvas.sizeDelta.y * canvas.pivot.y;

            return tempPosition;
        }

        public static void OpenTrail(this TrailRenderer Trail)
        {
            Trail.emitting = true;
        }

        public static void CloseTrail(this TrailRenderer Trail)
        {
            Trail.emitting = false;
        }
    }
}
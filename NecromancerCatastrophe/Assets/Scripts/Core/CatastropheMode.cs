using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Визуальное и игровое управление режимом "Катастрофа".
    /// Подписывается на CatastropheActivatedMsg.
    /// </summary>
    public class CatastropheMode : MonoBehaviour
    {
        [Header("Визуал")]
        public CanvasGroup screenDarken; // Полупрозрачная панель поверх экрана
        public float darkenTargetAlpha = 0.5f;
        public float transitionSpeed = 2f;

        private bool active = false;
        private float targetAlpha = 0f;

        void OnEnable()
        {
            EventBus.Subscribe<Core.CatastropheActivatedMsg>(OnCatastropheChanged);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe<Core.CatastropheActivatedMsg>(OnCatastropheChanged);
        }

        private void OnCatastropheChanged(Core.CatastropheActivatedMsg msg)
        {
            active = msg.Active;
            targetAlpha = active ? darkenTargetAlpha : 0f;
            if (screenDarken != null)
                screenDarken.gameObject.SetActive(true);
        }

        void Update()
        {
            if (screenDarken != null)
            {
                screenDarken.alpha = Mathf.MoveTowards(screenDarken.alpha, targetAlpha, transitionSpeed * Time.deltaTime);
                if (!active && screenDarken.alpha <= 0f)
                    screenDarken.gameObject.SetActive(false);
            }

            // Тряска экрана во время Катастрофы (упрощение)
            if (active)
            {
                var shake = Mathf.Sin(Time.time * 20f) * 0.02f;
                // Камера можно подёргать здесь
            }
        }
    }
}

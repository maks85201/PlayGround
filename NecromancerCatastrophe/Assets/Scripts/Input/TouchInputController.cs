using UnityEngine;

namespace Necromancer.Input
{
    /// <summary>
    /// Обработка касаний для управления одной рукой (вертикальная ориентация).
    /// В прототипе кнопки навыков обрабатываются через Unity UI (OnClick).
    /// Этот скрипт может использоваться для жестов: тап по экрану = автофокус цели.
    /// </summary>
    public class TouchInputController : MonoBehaviour
    {
        void Update()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    // Если не нажали по UI, можно применить логику касания
                    if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    {
                        // Например, фокус атаки героя по точке касания
                        HandleGameplayTouch(touch.position);
                    }
                }
            }

            // ПК-эмуляция
            if (Input.GetMouseButtonDown(0))
            {
                if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    HandleGameplayTouch(Input.mousePosition);
            }
        }

        private void HandleGameplayTouch(Vector2 screenPos)
        {
            var ray = Camera.main.ScreenPointToRay(screenPos);
            if (Physics2D.Raycast(ray.origin, ray.direction))
            {
                // Можно бросить луч к врагу для фокуса
                Debug.Log("[Touch] Касание по миру");
            }
        }
    }
}

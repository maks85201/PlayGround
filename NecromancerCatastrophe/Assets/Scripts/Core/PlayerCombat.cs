using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Боевой компонент героя. Автоатака ближайшего врага + навыки через NecromancerController.
    /// </summary>
    [RequireComponent(typeof(PlayerStats))]
    public class PlayerCombat : MonoBehaviour
    {
        private PlayerStats stats;
        private Transform currentTarget;
        private float attackTimer;

        [Header("Визуал прототипа")]
        public SpriteRenderer spriteRenderer;
        public Color heroColor = new Color(0.6f, 0.8f, 1f, 1f); // Голубой неон

        void Awake()
        {
            stats = GetComponent<PlayerStats>();
            if (spriteRenderer != null)
                spriteRenderer.color = heroColor;
        }

        void Update()
        {
            if (stats.catastropheActive)
            {
                // В режиме Катастрофы герой наносит массовый урон continually
                return;
            }

            attackTimer += Time.deltaTime;
            float interval = 1f / stats.attackSpeed;

            if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
            {
                currentTarget = FindNearestEnemy();
            }

            if (currentTarget != null && attackTimer >= interval)
            {
                attackTimer = 0f;
                Attack();
            }
        }

        private Transform FindNearestEnemy()
        {
            var enemies = FindObjectsOfType<EnemyInstance>();
            Transform nearest = null;
            float minDist = float.MaxValue;

            foreach (var e in enemies)
            {
                if (!e.isAlive) continue;
                float d = Vector2.Distance(transform.position, e.transform.position);
                if (d < minDist)
                {
                    minDist = d;
                    nearest = e.transform;
                }
            }
            return nearest;
        }

        private void Attack()
        {
            if (currentTarget == null) return;

            var enemy = currentTarget.GetComponent<EnemyInstance>();
            if (enemy != null)
            {
                int damage = Mathf.RoundToInt(10f + stats.strength * 2f);
                enemy.TakeDamage(damage);
            }
        }

        public void TakeDamage(int amount) => stats.TakeDamage(amount);

        public Transform GetCurrentTarget() => currentTarget;
    }
}

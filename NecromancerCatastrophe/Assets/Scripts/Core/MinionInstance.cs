using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Runtime-экземпляр миньона на сцене. Содержит текущие статы и состояние.
    /// </summary>
    public class MinionInstance : MonoBehaviour
    {
        public MinionData data;
        public int level = 1;

        public int currentHealth;
        public int currentDamage;
        public float attackSpeed;
        public float moveSpeed;

        public Transform target; // Текущая цель (враг)
        public float attackTimer;
        public bool isAlive = true;

        [Header("Опциональные ссылки")]
        public SpriteRenderer spriteRenderer;
        public Rigidbody2D rb;

        public void Initialize(MinionData data, int level)
        {
            this.data = data;
            this.level = level;
            currentHealth = Mathf.RoundToInt(data.baseHealth * (1f + 0.2f * (level - 1)));
            currentDamage = Mathf.RoundToInt(data.baseDamage * (1f + 0.2f * (level - 1)));
            attackSpeed = data.baseAttackSpeed;
            moveSpeed = data.baseMoveSpeed;
            isAlive = true;

            if (spriteRenderer != null)
                spriteRenderer.color = data.spriteColor;
        }

        public void TakeDamage(int amount)
        {
            if (!isAlive) return;
            currentHealth -= amount;
            if (currentHealth <= 0)
                Die();
        }

        private void Die()
        {
            isAlive = false;
            EventBus.Publish(new MinionDiedMsg { Minion = this, Position = transform.position });
            gameObject.SetActive(false); // Вернуть в пул через PoolManager позже
        }

        private void Update()
        {
            if (!isAlive || target == null) return;

            attackTimer += Time.deltaTime;
            float attackInterval = 1f / attackSpeed;

            // Простое движение к цели (в прототипе без навигации — линейное сближение)
            if (Vector2.Distance(transform.position, target.position) > data.attackRange)
            {
                transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            }

            if (attackTimer >= attackInterval && Vector2.Distance(transform.position, target.position) <= data.attackRange)
            {
                attackTimer = 0f;
                Attack();
            }
        }

        private void Attack()
        {
            var enemy = target.GetComponent<EnemyInstance>();
            if (enemy != null)
                enemy.TakeDamage(currentDamage);
        }
    }
}

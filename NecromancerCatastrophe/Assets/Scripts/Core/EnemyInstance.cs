using UnityEngine;

namespace Necromancer.Core
{
    public class EnemyInstance : MonoBehaviour
    {
        public EnemyData data;
        public int currentHealth;
        public float moveSpeed;
        public float attackTimer;
        public bool isAlive = true;

        public Transform target; // Обычно герой
        public SpriteRenderer spriteRenderer;

        public void Initialize(EnemyData data)
        {
            this.data = data;
            currentHealth = data.baseHealth;
            moveSpeed = data.moveSpeed;
            attackTimer = 0f;
            isAlive = true;

            if (spriteRenderer != null)
                spriteRenderer.color = Color.white;
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
            EventBus.Publish(new EnemyDiedMsg
            {
                GoldReward = data.goldReward,
                SoulReward = data.soulReward,
                Position = transform.position,
                Data = data
            });
            gameObject.SetActive(false);
        }

        private void Update()
        {
            if (!isAlive || target == null) return;

            transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

            attackTimer += Time.deltaTime;
            if (attackTimer >= data.attackCooldown && Vector2.Distance(transform.position, target.position) <= data.attackRange)
            {
                attackTimer = 0f;
                var player = target.GetComponent<PlayerCombat>();
                if (player != null)
                    player.TakeDamage(data.baseDamage);
            }
        }
    }
}

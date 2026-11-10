using System.Collections;
using UnityEngine;

namespace Necromancer.Necromancy
{
    /// <summary>
    /// Управляет душами: появление на сцене, полёт к герою, сбор.
    /// Подписывается на EnemyDiedMsg.
    /// </summary>
    public class SoulSystem : MonoBehaviour
    {
        public static SoulSystem Instance { get; private set; }

        [Header("Настройки")]
        public float soulMoveSpeed = 12f;
        public float autoCollectRadius = 2.5f;
        public GameObject soulPrefab; // Программ-арт: можно оставить пустым, будет круг

        void Awake()
        {
            Instance = this;
        }

        void OnEnable()
        {
            EventBus.Subscribe<Core.EnemyDiedMsg>(OnEnemyDied);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe<Core.EnemyDiedMsg>(OnEnemyDied);
        }

        private void OnEnemyDied(Core.EnemyDiedMsg msg)
        {
            SpawnSoul(msg.Position, msg.SoulReward);
        }

        private void SpawnSoul(Vector3 position, int amount)
        {
            // В прототипе создаём объект "душа" и запускаем полёт к герою
            var go = new GameObject("Soul");
            go.transform.position = position;
            var soul = go.AddComponent<SoulEntity>();
            soul.Initialize(amount, soulMoveSpeed, autoCollectRadius);
        }
    }

    public class SoulEntity : MonoBehaviour
    {
        private int amount;
        private float speed;
        private float collectRadius;
        private Transform heroTarget;

        public void Initialize(int amount, float speed, float collectRadius)
        {
            this.amount = amount;
            this.speed = speed;
            this.collectRadius = collectRadius;
            var gm = GameManager.Instance;
            heroTarget = gm != null ? gm.playerTransform : null;

            // Программ-арт: зелёный круг
            var sr = gameObject.AddComponent<SpriteRenderer>();
            sr.color = new Color(0.2f, 1f, 0.4f, 1f);
            sr.size = Vector2.one * 0.3f;
            var coll = gameObject.AddComponent<CircleCollider2D>();
            coll.isTrigger = true;
            var rb = gameObject.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = true;
        }

        void Update()
        {
            if (heroTarget == null)
            {
                Destroy(gameObject, 5f);
                return;
            }

            transform.position = Vector2.MoveTowards(transform.position, heroTarget.position, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, heroTarget.position) <= collectRadius)
                Collect();
        }

        private void Collect()
        {
            var stats = heroTarget.GetComponent<PlayerStats>();
            if (stats != null)
                stats.AddSouls(amount);
            Destroy(gameObject);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
                Collect();
        }
    }
}

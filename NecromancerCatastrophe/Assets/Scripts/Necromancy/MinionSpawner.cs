using UnityEngine;
using System.Collections.Generic;

namespace Necromancer.Necromancy
{
    /// <summary>
    /// Спавн миньонов (нежити). В прототипе создаёт Program-art объекты с MinionInstance.
    /// </summary>
    public class MinionSpawner : MonoBehaviour
    {
        public static MinionSpawner Instance { get; private set; }

        [Header("Программ-арт прототипа")]
        public Color defaultMinionColor = new Color(0.8f, 0.8f, 0.9f, 1f);

        void Awake()
        {
            Instance = this;
        }

        public MinionInstance Spawn(Data.MinionData data, Vector3 position)
        {
            var go = new GameObject($"Minion_{data.displayName}");
            go.transform.position = position;

            var minion = go.AddComponent<MinionInstance>();
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = data.spriteColor;
            sr.size = Vector2.one * 0.5f;
            var coll = go.AddComponent<CircleCollider2D>();
            coll.isTrigger = true;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = false;

            minion.spriteRenderer = sr;
            minion.rb = rb;
            minion.Initialize(data, 1);
            minion.tag = "Minion";

            return minion;
        }
    }
}

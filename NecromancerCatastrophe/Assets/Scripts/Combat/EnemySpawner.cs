using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Necromancer.Combat
{
    /// <summary>
    /// Спавн врагов волнами. Читает WaveData из LevelData.
    /// </summary>
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance { get; private set; }

        [Header("Точка спавна (верх экрана)")]
        public Transform spawnPoint;

        [Header("Программ-арт")]
        public Color defaultEnemyColor = Color.red;

        private Coroutine waveRoutine;
        public bool isSpawning { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void StartWaves(Data.LevelData levelData)
        {
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);
            waveRoutine = StartCoroutine(SpawnWaves(levelData));
        }

        public void StopSpawning()
        {
            if (waveRoutine != null)
                StopCoroutine(waveRoutine);
            isSpawning = false;
        }

        private IEnumerator SpawnWaves(Data.LevelData levelData)
        {
            isSpawning = true;

            foreach (var wave in levelData.waves)
            {
                Debug.Log($"[EnemySpawner] Волна {wave.waveIndex}");
                for (int i = 0; i < wave.enemyTypes.Length; i++)
                {
                    var enemyData = wave.enemyTypes[i];
                    int count = wave.counts.Length > i ? wave.counts[i] : 1;

                    for (int j = 0; j < count; j++)
                    {
                        SpawnEnemy(enemyData);
                        yield return new WaitForSeconds(wave.spawnIntervalSeconds / Mathf.Max(1, count));
                    }
                }

                // Ждём пока враги волны не кончатся (упрощение — фиксированная пауза)
                yield return new WaitForSeconds(wave.delayBeforeNextWaveSeconds);
            }

            isSpawning = false;
            Debug.Log("[EnemySpawner] Все волны завершены");
            EventBus.Publish(new Core.WaveCompletedMsg { WaveIndex = levelData.waveCount });
        }

        private void SpawnEnemy(Data.EnemyData data)
        {
            Vector3 pos = spawnPoint != null ? spawnPoint.position : new Vector3(0, 6, 0);
            pos.x += Random.Range(-2f, 2f);

            var go = new GameObject($"Enemy_{data.displayName}");
            go.transform.position = pos;

            var enemy = go.AddComponent<EnemyInstance>();
            var sr = go.AddComponent<SpriteRenderer>();
            sr.color = defaultEnemyColor;
            sr.size = Vector2.one * 0.6f;
            var coll = go.AddComponent<CircleCollider2D>();
            coll.isTrigger = true;
            var rb = go.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.isKinematic = false;

            enemy.spriteRenderer = sr;
            enemy.Initialize(data);

            // Цель — герой
            if (GameManager.Instance != null && GameManager.Instance.playerTransform != null)
                enemy.target = GameManager.Instance.playerTransform;

            enemy.tag = "Enemy";
        }
    }
}

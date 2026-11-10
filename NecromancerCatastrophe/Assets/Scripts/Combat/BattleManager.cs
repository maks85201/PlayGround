using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Necromancer.Combat
{
    /// <summary>
    /// Управляет ходом боя: волны, победа/поражение, переход к награде.
    /// Вешается на сцену Battle.
    /// </summary>
    public class BattleManager : MonoBehaviour
    {
        public static BattleManager Instance { get; private set; }

        [Header("Конфиг")]
        public Data.LevelData currentLevelData;

        [Header("Состояние")]
        public bool battleActive = false;
        public bool battleWon = false;
        public bool battleLost = false;

        void Awake()
        {
            Instance = this;
        }

        void OnEnable()
        {
            EventBus.Subscribe<Core.PlayerDiedMsg>(OnPlayerDied);
            EventBus.Subscribe<Core.WaveCompletedMsg>(OnWaveCompleted);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe<Core.PlayerDiedMsg>(OnPlayerDied);
            EventBus.Unsubscribe<Core.WaveCompletedMsg>(OnWaveCompleted);
        }

        void Start()
        {
            if (currentLevelData == null)
            {
                Debug.LogWarning("[BattleManager] LevelData не задан — загружен тестовый этап");
                StartTestBattle();
            }
            else
            {
                StartBattle(currentLevelData);
            }
        }

        public void StartBattle(Data.LevelData levelData)
        {
            currentLevelData = levelData;
            battleActive = true;
            battleWon = false;
            battleLost = false;

            EnemySpawner.Instance.StartWaves(levelData);
            Debug.Log($"[BattleManager] Бой начался: {levelData.levelName}");
        }

        private void StartTestBattle()
        {
            // Создаём упрощённый тестовый уровень
            currentLevelData = ScriptableObject.CreateInstance<Data.LevelData>();
            currentLevelData.levelId = 999;
            currentLevelData.levelName = "Тестовый этап";
            currentLevelData.waveCount = 3;

            // Нужны бы данные врага — в прототипе можно оставить пусто, спавн вручную через кнопку
            Debug.Log("[BattleManager] Тестовый бой");
        }

        private void OnPlayerDied(Core.PlayerDiedMsg msg)
        {
            battleLost = true;
            battleActive = false;
            EnemySpawner.Instance.StopSpawning();
            Debug.Log("[BattleManager] ПОРАЖЕНИЕ");
            EventBus.Publish(new Core.StageCompletedMsg { StageId = -1 });
            // Экран поражения через UI
        }

        private void OnWaveCompleted(Core.WaveCompletedMsg msg)
        {
            battleWon = true;
            battleActive = false;
            Debug.Log("[BattleManager] ПОБЕДА");
            EventBus.Publish(new Core.StageCompletedMsg { StageId = currentLevelData.levelId });
        }
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Necromancer.Core
{
    /// <summary>
    /// Центральный менеджер игры. Хранит ссылку на игрока, текущий этап, состояние.
    /// Паттерн Singleton (DontDestroyOnLoad для Bootstrap).
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Игрок")]
        public Transform playerTransform;
        public PlayerStats playerStats;
        public PlayerCombat playerCombat;
        public Necromancy.NecromancerController necromancer;

        [Header("Состояние")]
        public bool isInBattle = false;
        public int currentStageId = 1;

        [Header("Настройки")]
        public bool destroyOnSceneChange = false;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            if (!destroyOnSceneChange)
                DontDestroyOnLoad(gameObject);
        }

        public void StartBattle(int stageId)
        {
            currentStageId = stageId;
            isInBattle = true;
            SceneManager.LoadScene("Battle");
        }

        public void ReturnToHub()
        {
            isInBattle = false;
            SceneManager.LoadScene("MainMenu");
        }

        public void GameOver()
        {
            EventBus.Publish(new Core.PlayerDiedMsg());
            // В прототипе просто перезагрузка сцены боя
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}

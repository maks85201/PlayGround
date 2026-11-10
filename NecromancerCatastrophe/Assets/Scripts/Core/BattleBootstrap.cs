using UnityEngine;
using UnityEngine.SceneManagement;

namespace Necromancer.Core
{
    /// <summary>
    /// Вешается на объект в сцене Battle. Привязывает ссылки GameManager к игроку,
    /// находит UIManager и запускает первый этап.
    /// </summary>
    public class BattleBootstrap : MonoBehaviour
    {
        [Header("Уровень")]
        public Data.LevelData levelToLoad;

        void Awake()
        {
            // Связь GameManager (DontDestroyOnLoad) с игрой на этой сцене
            var gm = GameManager.Instance;

            // Находим игрока на сцене
            var player = FindObjectOfType<PlayerCombat>();
            if (player != null)
            {
                gm.playerTransform = player.transform;
                gm.playerStats = player.GetComponent<PlayerStats>();
                gm.playerCombat = player;
                gm.necromancer = player.GetComponent<Necromancy.NecromancerController>();
            }

            // Инициализируем UI
            var ui = FindObjectOfType<UIManager>();
            if (ui != null)
                ui.StartBattleUI();

            // Подключаем навыки к кнопкам (пример: первые 4 навыка из контроллера)
            if (gm.necromancer != null && ui != null && gm.necromancer.allSkills.Length > 0)
            {
                for (int i = 0; i < Mathf.Min(ui.skillButtons.Length, gm.necromancer.allSkills.Length); i++)
                {
                    ui.SetSkillButton(i, gm.necromancer.allSkills[i], gm.necromancer.skillLevels[i]);
                }
            }
        }

        void Start()
        {
            // Автостарт боя, если передан уровень
            if (levelToLoad != null && BattleManager.Instance != null)
                BattleManager.Instance.StartBattle(levelToLoad);
        }
    }
}

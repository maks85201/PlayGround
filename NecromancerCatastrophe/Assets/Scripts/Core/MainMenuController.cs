using UnityEngine;
using UnityEngine.SceneManagement;

namespace Necromancer.Core
{
    public class MainMenuController : MonoBehaviour
    {
        public Data.LevelData firstStage;

        public void StartGame()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.StartBattle(firstStage.levelId);
            SceneManager.LoadScene("Battle");
        }

        public void OpenSettings()
        {
            Debug.Log("[Menu] Настройки (заглушка)");
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Necromancer.UI
{
    /// <summary>
    /// Централизованный UI-менеджер для боевого экрана и меню.
    /// Подписывается на события и обновляет текст/полоски.
    /// </summary>
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Боевой HUD")]
        public TextMeshProUGUI soulText;
        public TextMeshProUGUI goldText;
        public TextMeshProUGUI manaText;
        public TextMeshProUGUI healthText;
        public Slider healthBar;
        public Slider manaBar;
        public Slider catastropheBar;
        public TextMeshProUGUI minionCountText;
        public TextMeshProUGUI waveText;

        [Header("Кнопки навыков")]
        public SkillButton[] skillButtons; // Разместить в инспекторе по порядку

        [Header("Экраны")]
        public GameObject victoryPanel;
        public GameObject defeatPanel;
        public GameObject mainMenuPanel;
        public GameObject hudPanel;

        void Awake()
        {
            Instance = this;

            if (victoryPanel != null) victoryPanel.SetActive(false);
            if (defeatPanel != null) defeatPanel.SetActive(false);
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (hudPanel != null) hudPanel.SetActive(false);
        }

        void OnEnable()
        {
            EventBus.Subscribe<Core.CurrencyChangedMsg>(OnCurrencyChanged);
            EventBus.Subscribe<Core.CatastropheFillChangedMsg>(OnCatastropheFillChanged);
            EventBus.Subscribe<Core.SoulCollectedMsg>(OnSoulCollected);
            EventBus.Subscribe<Core.StageCompletedMsg>(OnStageCompleted);
            EventBus.Subscribe<Core.PlayerDiedMsg>(OnPlayerDied);
        }

        void OnDisable()
        {
            EventBus.Unsubscribe<Core.CurrencyChangedMsg>(OnCurrencyChanged);
            EventBus.Unsubscribe<Core.CatastropheFillChangedMsg>(OnCatastropheFillChanged);
            EventBus.Unsubscribe<Core.SoulCollectedMsg>(OnSoulCollected);
            EventBus.Unsubscribe<Core.StageCompletedMsg>(OnStageCompleted);
            EventBus.Unsubscribe<Core.PlayerDiedMsg>(OnPlayerDied);
        }

        void Update()
        {
            // Обновление полос hero (если ссылки есть)
            var stats = GameManager.Instance?.playerStats;
            if (stats != null)
            {
                if (healthBar != null) healthBar.value = Mathf.MoveTowards(healthBar.value, (float)stats.currentHealth / stats.maxHealth, 5f * Time.deltaTime);
                if (manaBar != null) manaBar.value = Mathf.MoveTowards(manaBar.value, (float)stats.currentMana / stats.maxMana, 5f * Time.deltaTime);
                if (catastropheBar != null) catastropheBar.value = stats.catastropheFill;
                if (healthText != null) healthText.text = $"{stats.currentHealth}/{stats.maxHealth}";
                if (manaText != null) manaText.text = $"{stats.currentMana}/{stats.maxMana}";
                if (minionCountText != null)
                    minionCountText.text = $"Армия: {Necromancy.NecromancerController.Instance?.activeMinions.Count ?? 0}/{stats.minionControlLimit}";
            }
        }

        private void OnCurrencyChanged(Core.CurrencyChangedMsg msg)
        {
            if (msg.CurrencyId == "Souls" && soulText != null)
                soulText.text = $"Души: {msg.NewTotal}";
            if (msg.CurrencyId == "Gold" && goldText != null)
                goldText.text = $"Золото: {msg.NewTotal}";
        }

        private void OnCatastropheFillChanged(Core.CatastropheFillChangedMsg msg)
        {
            if (catastropheBar != null)
                catastropheBar.value = msg.FillNormalized;
        }

        private void OnSoulCollected(Core.SoulCollectedMsg msg)
        {
            // Можно добавить плавающий текст "+N"
        }

        private void OnStageCompleted(Core.StageCompletedMsg msg)
        {
            if (msg.StageId < 0)
                ShowDefeat();
            else
                ShowVictory();
        }

        private void OnPlayerDied(Core.PlayerDiedMsg msg)
        {
            ShowDefeat();
        }

        private void ShowVictory()
        {
            if (hudPanel != null) hudPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        private void ShowDefeat()
        {
            if (hudPanel != null) hudPanel.SetActive(false);
            if (defeatPanel != null) defeatPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void OpenMainMenu()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
            if (hudPanel != null) hudPanel.SetActive(false);
            if (victoryPanel != null) victoryPanel.SetActive(false);
            if (defeatPanel != null) defeatPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        public void StartBattleUI()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (hudPanel != null) hudPanel.SetActive(true);
            Time.timeScale = 1f;
        }

        public void SetSkillButton(int index, Data.SkillData data, int level)
        {
            if (index < 0 || index >= skillButtons.Length) return;
            skillButtons[index].Setup(data, level);
        }
    }
}

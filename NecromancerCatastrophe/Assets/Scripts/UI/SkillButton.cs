using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Necromancer.UI
{
    /// <summary>
    /// Кнопка навыка на боевом экране. Тап = использование навыка.
    /// </summary>
    public class SkillButton : MonoBehaviour
    {
        public int skillIndex;
        private Data.SkillData data;
        private int level;

        [Header("UI")]
        public Button button;
        public Image iconImage;
        public TextMeshProUGUI cooldownText;
        public TextMeshProUGUI manaCostText;
        public Image cooldownFill;

        private float cooldownTimer;

        public void Setup(Data.SkillData skillData, int lvl)
        {
            data = skillData;
            level = lvl;

            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(OnClick);
            }

            if (manaCostText != null)
                manaCostText.text = data.manaCost.ToString();

            UpdateVisuals();
        }

        private void OnClick()
        {
            if (cooldownTimer > 0f) return;

            var necro = Necromancy.NecromancerController.Instance;
            if (necro != null && necro.TryUseSkill(skillIndex))
            {
                cooldownTimer = data.GetCooldown(level);
                UpdateVisuals();
            }
        }

        void Update()
        {
            if (cooldownTimer > 0f)
            {
                cooldownTimer -= Time.deltaTime;
                if (cooldownTimer < 0f) cooldownTimer = 0f;
                UpdateVisuals();
            }
        }

        private void UpdateVisuals()
        {
            if (cooldownFill != null)
                cooldownFill.fillAmount = cooldownTimer > 0 ? cooldownTimer / data.GetCooldown(level) : 0f;

            if (cooldownText != null)
                cooldownText.text = cooldownTimer > 0 ? Mathf.CeilToInt(cooldownTimer).ToString() : "";

            var stats = GameManager.Instance.playerStats;
            bool canCast = stats != null && stats.currentMana >= data.manaCost && cooldownTimer <= 0;
            if (button != null)
                button.interactable = canCast && level > 0;

            var img = button.GetComponent<Image>();
            if (img != null)
                img.color = canCast ? Color.white : new Color(0.4f, 0.4f, 0.4f, 0.7f);
        }
    }
}

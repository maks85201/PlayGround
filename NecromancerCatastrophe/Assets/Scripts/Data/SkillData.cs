using UnityEngine;

namespace Necromancer.Data
{
    /// <summary>
    /// Данные навыка героя. Один навык = один активный слот в бою.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSkillData", menuName = "Necromancer/Data/Skill Data", order = 3)]
    public class SkillData : ScriptableObject
    {
        [Header("Основное")]
        public string skillId;
        public string displayName;
        [TextArea(2, 4)] public string description;
        public SkillType type;
        public int unlockLevel;

        [Header("Стоимость и КД")]
        public int manaCost;
        public float baseCooldownSeconds;
        public float cooldownReductionPerLevel; // -= за уровень навыка

        [Header("Эффект")]
        public int baseDamage;
        public float damagePerLevel;
        public float radius;                // Радиус действия (0 = цель)
        public float durationSeconds;       // Длительность эффекта/бафа

        [Header("Визуал/Звук")]
        public Color effectColor = new Color(0.2f, 1f, 0.4f, 1f); // Неоново-зелёный по умолчанию
        public AudioClip castSound;
        public GameObject vfxPrefab;

        [Header("Прокачка")]
        public int maxLevel = 5;
        public int[] soulCostPerLevel;      // Цена улучшения навыка за уровни

        public float GetCooldown(int level)
        {
            return Mathf.Max(0.5f, baseCooldownSeconds - cooldownReductionPerLevel * (level - 1));
        }

        public int GetManaCost(int level) => manaCost; // Упрощено
    }

    public enum SkillType { Summon, AoE, Debuff, Buff, Curse, Ultimate, Control }
}

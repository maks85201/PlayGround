using UnityEngine;

namespace Necromancer.Data
{
    /// <summary>
    /// Данные миньона (нежити). Создаются через Create -> Necromancer Data -> Minion Data.
    /// Являются шаблонами; runtime-экземпляр хранится в MinionInstance.
    /// </summary>
    [CreateAssetMenu(fileName = "NewMinionData", menuName = "Necromancer/Data/Minion Data", order = 1)]
    public class MinionData : ScriptableObject
    {
        [Header("Основное")]
        public string minionId;             // Уникальный ID (например, "minion_skeleton_warrior")
        public string displayName;          // Отображаемое имя
        public MinionRarity rarity;         // Редкость
        public GameObject prefab;           // Префаб миньона (в прототипе может быть не задан — используется цветной квадрат)

        [Header("Статы базы")]
        public int baseHealth;
        public int baseDamage;
        public float baseAttackSpeed;       // Атаки в секунду
        public float baseMoveSpeed;
        public float attackRange;

        [Header("Призыв")]
        public int soulCost;                // Стоимость призыва в душах
        public int requiredHeroLevel;       // Требование к уровню героя
        public int maxControlSlots;         // Сколько слотов контроля нежити занимает (обычно 1)

        [Header("Синергии")]
        public string[] synergizesWith;     // ID миньонов, с которыми даёт бонус
        public float synergyBonus;          // % бонуса урона при синергии

        [Header("Визуал")]
        public Color spriteColor = Color.white; // Для program-art прототипа
    }

    public enum MinionRarity
    {
        Common,
        Rare,
        Epic,
        Legendary,
        Mythic,
        Catastrophic
    }
}

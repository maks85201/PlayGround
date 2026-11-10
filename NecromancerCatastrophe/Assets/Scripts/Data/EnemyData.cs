using UnityEngine;

namespace Necromancer.Data
{
    /// <summary>
    /// Данные врага. Шаблон для EnemySpawner.
    /// </summary>
    [CreateAssetMenu(fileName = "NewEnemyData", menuName = "Necromancer/Data/Enemy Data", order = 2)]
    public class EnemyData : ScriptableObject
    {
        [Header("Основное")]
        public string enemyId;
        public string displayName;
        public EnemyRank rank;
        public EnemyType type;
        public GameObject prefab;

        [Header("Статы")]
        public int baseHealth;
        public int baseDamage;
        public float moveSpeed;
        public float attackRange;
        public float attackCooldown;        // Секунды между атаками

        [Header("Награды")]
        public int goldReward;
        public int soulReward;
        public int experienceReward;

        [Header("Сопротивления и слабости")]
        public ElementType weakness;
        public ElementType resistance;

        [Header("Поведение")]
        public bool isRanged;
        public GameObject projectilePrefab; // Для дистанционных врагов
    }

    public enum EnemyRank { Normal, Elite, Boss, HunterA, HunterS }
    public enum EnemyType { Melee, Ranged, Tank, Caster, Summoner }
    public enum ElementType { Physical, Necrotic, Holy, Fire, Poison, Shadow }
}

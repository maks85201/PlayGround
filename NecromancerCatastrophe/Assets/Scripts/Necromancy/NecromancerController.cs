using System.Collections.Generic;
using UnityEngine;

namespace Necromancer.Necromancy
{
    /// <summary>
    /// Управляет призывом нежити, навыками героя и армией 미ньонов.
    /// Вешается на героя (Player).
    /// </summary>
    public class NecromancerController : MonoBehaviour
    {
        public static NecromancerController Instance { get; private set; }

        [Header("Ссылки")]
        public PlayerStats stats;
        public PlayerCombat combat;

        [Header("Армия")]
        public List<MinionInstance> activeMinions = new List<MinionInstance>();
        public int maxMinions => stats.minionControlLimit;

        [Header("Навыки (Data)")]
        public SkillData[] allSkills;
        public int[] skillLevels; // Parallel to allSkills

        void Awake()
        {
            Instance = this;
            if (stats == null) stats = GetComponent<PlayerStats>();
            if (combat == null) combat = GetComponent<PlayerCombat>();

            // Инициализация уровней навыков (4 доступны со старта)
            skillLevels = new int[allSkills.Length];
            for (int i = 0; i < allSkills.Length; i++)
                skillLevels[i] = (i < 4) ? 1 : 0;
        }

        void Update()
        {
            // Очистка мёртвых из списка
            activeMinions.RemoveAll(m => m == null || !m.isAlive);
        }

        #region Призыв

        public bool TrySummonMinion(MinionData minionData)
        {
            if (activeMinions.Count >= maxMinions)
            {
                Debug.Log("[Necromancer] Лимит армии reached");
                return false;
            }
            if (!stats.CanAffordSouls(minionData.soulCost))
            {
                Debug.Log("[Necromancer] Недостаточно душ");
                return false;
            }
            if (level < minionData.requiredHeroLevel) return false;

            stats.SpendSouls(minionData.soulCost);
            var minion = MinionSpawner.Instance.Spawn(minionData, transform.position + Random.insideUnitCircle * 1.5f);
            if (minion != null)
            {
                activeMinions.Add(minion);
                EventBus.Publish(new Core.MinionSpawnedMsg { Minion = minion });
                return true;
            }
            return false;
        }

        public int level => stats.level;

        #endregion

        #region Навыки

        public bool TryUseSkill(int index)
        {
            if (index < 0 || index >= allSkills.Length) return false;
            var data = allSkills[index];
            int lvl = skillLevels[index];
            if (lvl == 0) return false; // Не разблокирован

            if (!stats.CanAffordMana(data.manaCost)) return false;

            float cd = data.GetCooldown(lvl);
            // Проверка КД упрощена — можно добавить массив timers
            stats.SpendMana(data.manaCost);
            ExecuteSkill(data, lvl);
            EventBus.Publish(new Core.SkillUsedMsg { SkillId = data.skillId });
            return true;
        }

        private void ExecuteSkill(SkillData data, int level)
        {
            int damage = Mathf.RoundToInt(data.baseDamage + data.damagePerLevel * (level - 1));
            switch (data.type)
            {
                case SkillType.Summon:
                    // Поднять скелета — призывает базового миньона (первый в списке данных можно захардкодить через кнопку UI)
                    Debug.Log($"[Skill] {data.displayName}: призыв скелета, урон {damage}");
                    break;
                case SkillType.AoE:
                    ApplyAoE(damage, data.radius);
                    break;
                case SkillType.Debuff:
                    ApplyFear(data.radius, data.durationSeconds);
                    break;
                case SkillType.Ultimate:
                    ActivateCatastrophe();
                    break;
                default:
                    Debug.Log($"[Skill] {data.displayName} использован, урон {damage}");
                    break;
            }
        }

        private void ApplyAoE(int damage, float radius)
        {
            var enemies = FindObjectsOfType<EnemyInstance>();
            foreach (var e in enemies)
            {
                if (!e.isAlive) continue;
                if (Vector2.Distance(transform.position, e.transform.position) <= radius)
                    e.TakeDamage(damage);
            }
            // Визуал можно добавить через VFX
        }

        private void ApplyFear(float radius, float duration)
        {
            var enemies = FindObjectsOfType<EnemyInstance>();
            foreach (var e in enemies)
            {
                if (!e.isAlive) continue;
                if (Vector2.Distance(transform.position, e.transform.position) <= radius)
                {
                    // Просто замедляем (упрощение)
                    e.moveSpeed *= 0.5f;
                }
            }
        }

        private void ActivateCatastrophe()
        {
            stats.SetCatastropheActive(true);
            // Экран темнеет, урон армии ×2, массовый урон врагам
            ApplyAoE(Mathf.RoundToInt(200f + stats.intellect * 5f), 12f);
            foreach (var m in activeMinions)
            {
                if (m != null && m.isAlive)
                    m.attackSpeed *= 2f;
            }
            // Автодеактивация через 8 секунд (упрощение — корутину лучше)
            StartCoroutine(DeactivateCatastropheAfter(8f));
        }

        private System.Collections.IEnumerator DeactivateCatastropheAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            stats.SetCatastropheActive(false);
            foreach (var m in activeMinions)
            {
                if (m != null && m.isAlive)
                    m.attackSpeed /= 2f;
            }
        }

        public void LevelUpSkill(int index)
        {
            if (index < 0 || index >= allSkills.Length) return;
            if (skillLevels[index] >= allSkills[index].maxLevel) return;
            skillLevels[index]++;
            Debug.Log($"[Necromancer] Навык {allSkills[index].displayName} теперь уровень {skillLevels[index]}");
        }

        #endregion
    }
}

using UnityEngine;

namespace Necromancer.Economy
{
    /// <summary>
    /// Начисление наград после этапа.
    /// </summary>
    public class RewardSystem : MonoBehaviour
    {
        public static RewardSystem Instance { get; private set; }

        void Awake()
        {
            Instance = this;
        }

        public void GrantStageRewards(int souls, int gold, int crystals = 0)
        {
            var stats = GameManager.Instance.playerStats;
            stats.AddSouls(souls);
            stats.gold += gold;
            stats.darkCrystals += crystals;

            EventBus.Publish(new Core.CurrencyChangedMsg { CurrencyId = "Souls", Amount = souls, NewTotal = stats.souls });
            EventBus.Publish(new Core.CurrencyChangedMsg { CurrencyId = "Gold", Amount = gold, NewTotal = stats.gold });

            Debug.Log($"[Reward] +{souls} душ, +{gold} золота, +{crystals} кристаллов");
        }
    }
}

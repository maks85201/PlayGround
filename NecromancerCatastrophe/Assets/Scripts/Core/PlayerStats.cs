using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Статы игрока (героя). Единственный экземпляр на сцене через GameManager.
    /// </summary>
    public class PlayerStats : MonoBehaviour
    {
        [Header("Базовые")]
        public int level = 1;
        public int experience;
        public int experienceToNext = 100;

        [Header("Здоровье и мана")]
        public int maxHealth = 100;
        public int currentHealth;
        public int maxMana = 50;
        public int currentMana;
        public float manaRegenPerSecond = 5f;

        [Header("Боевые")]
        public int strength;
        public int intellect;
        public int stamina;
        public int minionControlLimit = 5; // Контроль нежити
        public float attackSpeed = 1f;
        public float criticalChance = 0.05f;
        public float criticalDamage = 1.5f;
        public int defense;
        public int resistance;

        [Header("Ресурсы")]
        public long souls;
        public long gold;
        public long darkCrystals;
        public long systemShards;
        public long catastropheEssence;

        [Header("Шкала Катастрофы")]
        public float catastropheFill; // 0..1
        public bool catastropheActive;

        public event System.Action<int, int> OnHealthChanged;
        public event System.Action<int, int> OnManaChanged;
        public event System.Action<long> OnSoulsChanged;
        public event System.Action<float> OnCatastropheFillChanged;

        void Awake()
        {
            currentHealth = maxHealth;
            currentMana = maxMana;
        }

        void Update()
        {
            if (!catastropheActive)
            {
                currentMana = Mathf.Min(maxMana, currentMana + Mathf.RoundToInt(manaRegenPerSecond * Time.deltaTime));
                OnManaChanged?.Invoke(currentMana, maxMana);
            }
        }

        public void Heal(int amount)
        {
            currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);
        }

        public void TakeDamage(int amount)
        {
            int final = Mathf.Max(1, amount - defense);
            currentHealth = Mathf.Max(0, currentHealth - final);
            OnHealthChanged?.Invoke(currentHealth, maxHealth);

            // Катастрофа заполняется от урона
            AddCatastropheFill(0.02f);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                EventBus.Publish(new PlayerDiedMsg());
            }
        }

        public void AddSouls(int amount)
        {
            souls += amount;
            OnSoulsChanged?.Invoke(souls);
            AddCatastropheFill(0.01f * amount);
        }

        public void SpendSouls(int amount)
        {
            if (souls >= amount)
            {
                souls -= amount;
                OnSoulsChanged?.Invoke(souls);
            }
        }

        public bool CanAffordSouls(int amount) => souls >= amount;
        public bool CanAffordMana(int amount) => currentMana >= amount;
        public void SpendMana(int amount) => currentMana = Mathf.Max(0, currentMana - amount);

        public void AddCatastropheFill(float amount)
        {
            if (catastropheActive) return;
            catastropheFill = Mathf.Clamp01(catastropheFill + amount);
            OnCatastropheFillChanged?.Invoke(catastropheFill);

            if (catastropheFill >= 1f)
            {
                catastropheFill = 1f;
                EventBus.Publish(new CatastropheFillChangedMsg { FillNormalized = 1f, ReachedMax = true });
            }
            else
            {
                EventBus.Publish(new CatastropheFillChangedMsg { FillNormalized = catastropheFill, ReachedMax = false });
            }
        }

        public void SetCatastropheActive(bool active)
        {
            catastropheActive = active;
            if (!active)
                catastropheFill = 0f;
            EventBus.Publish(new CatastropheActivatedMsg { Active = active });
            EventBus.Publish(new CatastropheFillChangedMsg { FillNormalized = catastropheFill, ReachedMax = active });
        }
    }
}

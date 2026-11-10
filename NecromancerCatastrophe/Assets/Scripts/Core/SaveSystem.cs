using System;
using System.IO;
using UnityEngine;

namespace Necromancer.Core
{
    /// <summary>
    /// Сохранение прогресса. В прототипе PlayerPrefs; легко заменить на JSON/шифрование.
    /// </summary>
    public static class SaveSystem
    {
        private const string Prefix = "Necromancer_";

        public static void SavePlayer(PlayerStats stats)
        {
            PlayerPrefs.SetInt(Prefix + "Level", stats.level);
            PlayerPrefs.SetInt(Prefix + "Experience", stats.experience);
            PlayerPrefs.SetInt(Prefix + "MaxHealth", stats.maxHealth);
            PlayerPrefs.SetInt(Prefix + "CurrentHealth", stats.currentHealth);
            PlayerPrefs.SetInt(Prefix + "MaxMana", stats.maxMana);
            PlayerPrefs.SetInt(Prefix + "CurrentMana", stats.currentMana);
            PlayerPrefs.SetInt(Prefix + "Strength", stats.strength);
            PlayerPrefs.SetInt(Prefix + "Intellect", stats.intellect);
            PlayerPrefs.SetInt(Prefix + "Stamina", stats.stamina);
            PlayerPrefs.SetInt(Prefix + "MinionControlLimit", stats.minionControlLimit);
            PlayerPrefs.SetFloat(Prefix + "AttackSpeed", stats.attackSpeed);
            PlayerPrefs.SetFloat(Prefix + "CriticalChance", stats.criticalChance);
            PlayerPrefs.SetFloat(Prefix + "CriticalDamage", stats.criticalDamage);
            PlayerPrefs.SetInt(Prefix + "Defense", stats.defense);
            PlayerPrefs.SetInt(Prefix + "Resistance", stats.resistance);

            PlayerPrefs.SetString(Prefix + "Souls", stats.souls.ToString());
            PlayerPrefs.SetString(Prefix + "Gold", stats.gold.ToString());
            PlayerPrefs.SetString(Prefix + "DarkCrystals", stats.darkCrystals.ToString());
            PlayerPrefs.SetString(Prefix + "SystemShards", stats.systemShards.ToString());
            PlayerPrefs.SetString(Prefix + "CatastropheEssence", stats.catastropheEssence.ToString());

            PlayerPrefs.Save();
        }

        public static void LoadPlayer(PlayerStats stats)
        {
            stats.level = PlayerPrefs.GetInt(Prefix + "Level", 1);
            stats.experience = PlayerPrefs.GetInt(Prefix + "Experience", 0);
            stats.maxHealth = PlayerPrefs.GetInt(Prefix + "MaxHealth", 100);
            stats.currentHealth = PlayerPrefs.GetInt(Prefix + "CurrentHealth", 100);
            stats.maxMana = PlayerPrefs.GetInt(Prefix + "MaxMana", 50);
            stats.currentMana = PlayerPrefs.GetInt(Prefix + "CurrentMana", 50);
            stats.strength = PlayerPrefs.GetInt(Prefix + "Strength", 0);
            stats.intellect = PlayerPrefs.GetInt(Prefix + "Intellect", 0);
            stats.stamina = PlayerPrefs.GetInt(Prefix + "Stamina", 0);
            stats.minionControlLimit = PlayerPrefs.GetInt(Prefix + "MinionControlLimit", 5);
            stats.attackSpeed = PlayerPrefs.GetFloat(Prefix + "AttackSpeed", 1f);
            stats.criticalChance = PlayerPrefs.GetFloat(Prefix + "CriticalChance", 0.05f);
            stats.criticalDamage = PlayerPrefs.GetFloat(Prefix + "CriticalDamage", 1.5f);
            stats.defense = PlayerPrefs.GetInt(Prefix + "Defense", 0);
            stats.resistance = PlayerPrefs.GetInt(Prefix + "Resistance", 0);

            long.TryParse(PlayerPrefs.GetString(Prefix + "Souls", "0"), out stats.souls);
            long.TryParse(PlayerPrefs.GetString(Prefix + "Gold", "0"), out stats.gold);
            long.TryParse(PlayerPrefs.GetString(Prefix + "DarkCrystals", "0"), out stats.darkCrystals);
            long.TryParse(PlayerPrefs.GetString(Prefix + "SystemShards", "0"), out stats.systemShards);
            long.TryParse(PlayerPrefs.GetString(Prefix + "CatastropheEssence", "0"), out stats.catastropheEssence);
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}

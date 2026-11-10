using UnityEngine;

namespace Necromancer.Data
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "Necromancer/Data/Item Data", order = 5)]
    public class ItemData : ScriptableObject
    {
        public string itemId;
        public string displayName;
        public ItemType type;
        public Sprite icon; // В прототипе может быть null
        public int value;
        public string description;
    }

    public enum ItemType { Currency, Consumable, Equipment, Rune, SkillFragment }
}

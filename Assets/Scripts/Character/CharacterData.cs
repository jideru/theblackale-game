using System.Collections.Generic;
using UnityEngine;

namespace BlackAle.Character
{
    public enum CharacterClass
    {
        Thief,
        Warrior,
        Druid,
        Priest
    }

    [System.Serializable]
    public class CharacterStats
    {
        public int strength;
        public int dexterity;
        public int wisdom;
        public int charisma;

        public CharacterStats(int str, int dex, int wis, int cha)
        {
            strength = str;
            dexterity = dex;
            wisdom = wis;
            charisma = cha;
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string name;
        public string description;
        public Color color;

        public InventoryItem(string itemId, string name, string description, Color color)
        {
            this.itemId = itemId;
            this.name = name;
            this.description = description;
            this.color = color;
        }
    }

    [System.Serializable]
    public class CharacterData
    {
        public string characterName;
        public CharacterClass characterClass;
        public string abilityName;
        public string abilityDescription;
        public CharacterStats stats;
        public List<InventoryItem> inventory;
        public int maxInventorySize;

        public CharacterData(string name, CharacterClass charClass, string ability,
            string abilityDesc, CharacterStats stats, int maxInv = 9)
        {
            characterName = name;
            characterClass = charClass;
            abilityName = ability;
            abilityDescription = abilityDesc;
            this.stats = stats;
            inventory = new List<InventoryItem>();
            maxInventorySize = maxInv;
        }

        public bool CanAddItem()
        {
            return inventory.Count < maxInventorySize;
        }

        public bool AddItem(InventoryItem item)
        {
            if (!CanAddItem()) return false;
            inventory.Add(item);
            return true;
        }

        public bool RemoveItem(string itemId)
        {
            return inventory.RemoveAll(i => i.itemId == itemId) > 0;
        }

        public static CharacterData CreateThief()
        {
            return new CharacterData(
                "Grimjaw Lockfinger",
                CharacterClass.Thief,
                "Pick Lock",
                "Can pick locks on chests, doors, and other locked containers.",
                new CharacterStats(8, 16, 10, 12)
            );
        }

        public static CharacterData CreateWarrior()
        {
            return new CharacterData(
                "Brokk Ironhand",
                CharacterClass.Warrior,
                "Force Open",
                "Can use brute strength to force open stuck or barred objects.",
                new CharacterStats(16, 10, 8, 10)
            );
        }

        public static CharacterData CreateDruid()
        {
            return new CharacterData(
                "Mossvein Oakroot",
                CharacterClass.Druid,
                "Nature Magic",
                "Can use the old magic of stone and root to affect nature-aligned objects.",
                new CharacterStats(10, 12, 16, 8)
            );
        }

        public static CharacterData CreatePriest()
        {
            return new CharacterData(
                "Brother Aldar",
                CharacterClass.Priest,
                "Minor Miracle",
                "Can invoke small divine miracles to heal, bless, or purify.",
                new CharacterStats(10, 8, 14, 14)
            );
        }
    }
}

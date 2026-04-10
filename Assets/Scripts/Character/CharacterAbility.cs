using UnityEngine;

namespace BlackAle.Character
{
    public abstract class CharacterAbility : MonoBehaviour
    {
        public abstract string AbilityName { get; }
        public abstract CharacterClass RequiredClass { get; }

        public abstract bool CanUseOn(string targetItemId);
        public abstract void UseAbility(string targetItemId);
    }

    public class PickLockAbility : CharacterAbility
    {
        public override string AbilityName => "Pick Lock";
        public override CharacterClass RequiredClass => CharacterClass.Thief;

        public override bool CanUseOn(string targetItemId)
        {
            // Future: check if item has "pick_lock" requirement
            return true;
        }

        public override void UseAbility(string targetItemId)
        {
            Debug.Log($"[PickLock] Attempting to pick lock on {targetItemId}");
            // Future: implement lock picking logic
        }
    }
}

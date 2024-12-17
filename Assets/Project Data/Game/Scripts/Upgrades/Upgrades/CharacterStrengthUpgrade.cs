using UnityEngine;
using Watermelon.Upgrades;

namespace Watermelon
{
    [CreateAssetMenu(fileName = "Character Strength Upgrade", menuName = "Content/Upgrades/Character Strength Upgrade")]
    public class CharacterStrengthUpgrade : Upgrade<CharacterStrengthUpgrade.CharacterStrengthUpgradeStage>
    {
        public override void Initialise()
        {

        }

        [System.Serializable]
        public class CharacterStrengthUpgradeStage : BaseUpgradeStage
        {
            [SerializeField] int itemsCapacity;
            public int ItemsCapacity => itemsCapacity;

            [SerializeField] int animalsCapacity;
            public int AnimalsCapacity => animalsCapacity;
        }
    }
}
using TagTool.Tags;

namespace TagTool.Ai
{
    [TagStructure(Size = 0x8)]
    public class CharacterCombatformProperties : TagStructure
	{
        /// <summary>
        /// Distance at which the combatform will be forced into berserking.
        /// </summary>
        public float BerserkDistance;

        /// <summary>
        /// Chance of which the combatform will be forced into berserking this second.
        /// </summary>
        public float BerserkChance;
    }
}

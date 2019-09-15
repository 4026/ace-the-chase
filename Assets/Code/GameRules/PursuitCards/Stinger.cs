namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Add damage to the top of the player's deck.
    /// </summary>
    public class Stinger : PursuitCard
    {
        public int Damage;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddDamageToTopOfDeck(Damage)
                .Done();
        }
    }
}
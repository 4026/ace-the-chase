namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// Win the game if the player has amassed the required amount of Lead.
    /// </summary>
    public class Hideout : RouteCard
    {
        public int LeadRequired;

        public override Chase Play(Chase currentState)
        {
            ChaseMutator mutator = new ChaseMutator(currentState);

            if (currentState.Lead >= LeadRequired) 
            {
                mutator.SetPlayerHasWon();
            }

            return mutator
                .DiscardFromRoute(this)
                .Done();
        }
    }
}
namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Reduces the player's lead
    /// </summary>
    public class SuspectSighted : PursuitCard
    {
        public int LeadDecrease;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddLead(-LeadDecrease)
                .Done();
        }
    }
}
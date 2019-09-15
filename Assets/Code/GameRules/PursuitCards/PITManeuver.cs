namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Reduces the player's control next turn
    /// </summary>
    public class PITManeuver : PursuitCard
    {
        public int ControlDecrease;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddLead(-ControlDecrease)
                .Done();
        }
    }
}
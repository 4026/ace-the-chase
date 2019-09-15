namespace AceTheChase.GameRules.PursuitCards
{
    /// <summary>
    /// Increases the pursuit speed.
    /// </summary>
    public class PoliceHelicopter : PursuitCard
    {
        public int PursuitSpeedIncrease;

        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState)
                .AddPursuitSpeed(PursuitSpeedIncrease)
                .Done();
        }
    }
}
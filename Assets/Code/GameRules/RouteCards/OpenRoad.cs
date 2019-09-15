namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// No effect.
    /// </summary>
    public class OpenRoad : RouteCard
    {
        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState).DiscardFromRoute(this).Done();
        }
    }
}
namespace AceTheChase.GameRules
{
    /// <summary>
    /// Enum of the possible phases that the game can be in while in a chase.
    /// </summary>
    public enum ChasePhase
    { 
        Setup,
        SelectingCard,
        SelectingParameters,
        PlayingAnimation,
        ResolvingPursuitAndRoute,
        Victory,
        Defeat
    }
}
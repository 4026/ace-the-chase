using AceTheChase.UI;

namespace AceTheChase.GameRules
{
    /// <summary>
    /// A card from the pursuit deck that the player must face in a chase.
    /// </summary>
    public interface IPursuitCard : ICard
    {
        /// <summary>
        /// Given the current chase state, mutate and return the state to represent the effect of
        /// playing this card.
        /// </summary>
        Chase Play(Chase currentState, UIManager uiManager);
    }
}
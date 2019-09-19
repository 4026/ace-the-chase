using UnityEngine;

namespace AceTheChase.GameRules.RouteCards
{
    /// <summary>
    /// No effect.
    /// </summary>
    [CreateAssetMenu(menuName = "Cards/Routes/Open Road", fileName = "Routes_OpenRoad")]
    public class OpenRoad : RouteCard
    {
        public override Chase Play(Chase currentState)
        {
            return new ChaseMutator(currentState).DiscardFromRoute(this).Done();
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using AceTheChase.GameRules.RouteCards;
using AceTheChase.Utils;
using UnityEngine;

namespace AceTheChase.GameRules
{
    [CreateAssetMenu(menuName = "ChaseStartInfo", fileName = "ChaseStartInfo")]
    public class ChaseStartInfo : ScriptableObject
    {
        public int StartingLead;
        public int StartingPlayerSpeed;
        public int StartingPursuitSpeed;
        public int StartingControl;

        public int StartingMaxLead;
        public int StartingMaxPlayerSpeed;
        public int StartingMaxPursuitSpeed;
        public int StartingMaxControl;       

        public int RouteDeckSize;

        public PlayerCardDriver SelectedPlayerDriver = PlayerCardDriver.None;
        public IPlayerCard[] SelectedPlayerCards;
        public PlayerCard[] AllDamageCards;
        public RouteCard[] AllRouteCards;
        public PursuitCard[] AllPursuitCards;

        public Deck<IPlayerCard> StartingPlayerDeck => new Deck<IPlayerCard>(SelectedPlayerCards);
        public Deck<IRouteCard> StartingRouteDeck
        {
            get
            {
                // Generate a list containing two copies of every (non-hideout) route card, and 
                // randomly select a subset of them to form the route deck.
                IList<IRouteCard> routeCards = AllRouteCards
                    .Where(card => !(card is Hideout))
                    .SelectMany(card => new [] { card as IRouteCard, card as IRouteCard })
                    .ToList()
                    .RandomSubset(RouteDeckSize - 2);

                // Always add two hideout cards to the route deck.
                RouteCard hideout = AllRouteCards.First(card => card is Hideout);
                routeCards.Add(hideout);
                routeCards.Add(hideout);

                return new Deck<IRouteCard>(routeCards);
            }
        }
        public Deck<IPursuitCard> StartingPursuitDeck => new Deck<IPursuitCard>(AllPursuitCards);
    }
}
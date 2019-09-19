using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AceTheChase.GameRules;

namespace AceTheChase.UI
{
    [CreateAssetMenu(menuName = "Palettes/UI Palette", fileName = "UIPalette")]
    public class UIPalette : ScriptableObject
    {
        public class CardTypeColourScheme
        {
            public Color TitleColourTop;
            public Color TitleColourBottom;
            public Color TitleHighlightTop;
            public Color TitleHighlightBottom;
            public Color BorderGradientTop;
            public Color BorderGradientBottom;
            public Sprite Icon;
        }

        public class PlayerCardTypeColourScheme : CardTypeColourScheme
        {
            public PlayerCardType CardType;
        }
        
        public class RouteCardTypeColourScheme : CardTypeColourScheme
        {
            public RouteCardType CardType;
        }

        [SerializeField] public static PlayerCardTypeColourScheme[] PlayerCardTypeColours;
        [SerializeField] public static RouteCardTypeColourScheme[] RouteCardTypeColours;
        [SerializeField] public static CardTypeColourScheme PursuitCardColours;

        public static CardTypeColourScheme GetCardTypeColorScheme(PlayerCardType type)
        {
            foreach (PlayerCardTypeColourScheme scheme in PlayerCardTypeColours)
            {
                if (scheme.CardType == type)
                {
                    return scheme;
                }
            }
            Debug.LogError("CardTypeColorScheme not found for player card type: " + type.ToString());
            return new CardTypeColourScheme();
        }

        public static CardTypeColourScheme GetCardTypeColorScheme(RouteCardType type)
        {
            foreach (RouteCardTypeColourScheme scheme in RouteCardTypeColours)
            {
                if (scheme.CardType == type)
                {
                    return scheme;
                }
            }
            Debug.LogError("CardTypeColorScheme not found for route card type: " + type.ToString());
            return new CardTypeColourScheme();
        }


    }
}

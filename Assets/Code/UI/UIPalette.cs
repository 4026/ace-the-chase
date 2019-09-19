using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AceTheChase.GameRules;
using System;

namespace AceTheChase.UI
{
    [CreateAssetMenu(menuName = "Palettes/UI Palette", fileName = "UIPalette")]
    public class UIPalette : ScriptableObject
    {
        static UIPalette _instance = null;
        public static UIPalette Instance
        {
            get
            {
                if (!_instance)
                    _instance = Resources.Load<UIPalette>("Data/Resources/UIPalette");
                return _instance;
            }
        }

        [Serializable]
        public class CardTypeColourScheme
        {
            [SerializeField]
            public Color TitleColourTop;
            [SerializeField]
            public Color TitleColourBottom;
            [SerializeField]
            public Color TitleHighlightTop;
            [SerializeField]
            public Color TitleHighlightBottom;
            [SerializeField]
            public Color BorderGradientTop;
            [SerializeField]
            public Color BorderGradientBottom;
            [SerializeField]
            public Sprite Icon;
        }

        [Serializable]
        public class PlayerCardTypeColourScheme : CardTypeColourScheme
        {
            [SerializeField]
            public PlayerCardType CardType;
        }

        [Serializable]
        public class RouteCardTypeColourScheme : CardTypeColourScheme
        {
            [SerializeField]
            public RouteCardType CardType;
        }

        [SerializeField]
        public PlayerCardTypeColourScheme[] PlayerCardTypeColours;
        [SerializeField]
        public RouteCardTypeColourScheme[] RouteCardTypeColours;
        [SerializeField]
        public CardTypeColourScheme PursuitCardColours;

        [SerializeField]
        public CardTypeColourScheme DefaultCardColours;

        public CardTypeColourScheme GetCardTypeColorScheme(PlayerCardType type)
        {
            foreach (PlayerCardTypeColourScheme scheme in PlayerCardTypeColours)
            {
                if (scheme.CardType == type)
                {
                    return scheme;
                }
            }
            Debug.LogError("CardTypeColorScheme not found for player card type: " + type.ToString());
            return DefaultCardColours;
        }

        public CardTypeColourScheme GetCardTypeColorScheme(RouteCardType type)
        {
            foreach (RouteCardTypeColourScheme scheme in RouteCardTypeColours)
            {
                if (scheme.CardType == type)
                {
                    return scheme;
                }
            }
            Debug.LogError("CardTypeColorScheme not found for route card type: " + type.ToString());
            return DefaultCardColours;
        }


    }
}

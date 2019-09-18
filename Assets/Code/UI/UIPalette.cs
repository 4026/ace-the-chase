using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AceTheChase.GameRules;

namespace AceTheChase.UI
{
    public class UIPalette : ScriptableObject
    {
        public struct CardTypeColourScheme
        {
            public PlayerCardType CardType;
            public Color TitleColour;
            public Color TitleHighlightColour;
            public Sprite Icon;
        }
        [SerializeField] public static Color CardTypeColours;
    }
}

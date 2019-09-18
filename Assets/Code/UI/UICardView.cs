using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AceTheChase.GameRules;

namespace AceTheChase.UI
{
    public class UICardView : MonoBehaviour
    {
        [SerializeField] private Image m_titleBG;
        [SerializeField] private Image m_titleHighlight;
        [SerializeField] private Image m_baseBorder;

        [SerializeField] private Image m_cardTypeIcon;
        [SerializeField] private Image m_cardImage;

        [SerializeField] private Text m_cardName;
        [SerializeField] private Text m_cardControlCost;
        [SerializeField] private Text m_cardEffects;
        [SerializeField] private Text m_cardFlavourText;

        public void SetupCard(PlayerCardType playerCardType, int ID)
        {
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AceTheChase.GameRules;

namespace AceTheChase.UI
{
    public class UICardView : MonoBehaviour
    {


        [SerializeField]
        private Image m_cardTypeIcon;
        [SerializeField]
        private Image m_cardImage;

        [SerializeField]
        private Image m_baseBorder;
        [SerializeField]
        private UIGradient m_borderGradient;
        [SerializeField]
        private UIGradient m_titleGradient;
        [SerializeField]
        private UIGradient m_titleHighlight;

        [SerializeField]
        private Text m_cardName;
        [SerializeField]
        private Text m_cardControlCost;
        [SerializeField]
        private Text m_cardEffects;
        [SerializeField]
        private Text m_cardFlavourText;

        [SerializeField]
        private GameObject m_cardControlCostLayer;
        [SerializeField]
        private GameObject m_flavourTextLayer;

        public void Setup(PursuitCard card)
        {
            m_cardFlavourText.text = card.Name;
            m_cardControlCostLayer.SetActive(false);

            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.PursuitCardColours;
            SetColourScheme(scheme);

            m_cardName.text = card.Name;
            m_cardEffects.text = card.Description;
            m_cardImage.sprite = card.CardImage;
            SetFlavourText(card.FlavourText);
        }

        public void Setup(PlayerCard card)
        {
            m_cardControlCostLayer.SetActive(true);
            
            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.GetCardTypeColorScheme(card.CardType);
            SetColourScheme(scheme);

            m_cardName.text = card.Name;
            m_cardEffects.text = card.Description;
            m_cardControlCost.text = card.ControlCost.ToString();
            m_cardImage.sprite = card.CardImage;
            SetFlavourText(card.FlavourText);
        }

        public void Setup(RouteCard card)
        {
            m_cardControlCostLayer.SetActive(false);
            m_flavourTextLayer.SetActive(string.IsNullOrEmpty(card.FlavourText) == false);
            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.GetCardTypeColorScheme(card.CardType);
            SetColourScheme(scheme);

            m_cardName.text = card.Name;
            m_cardEffects.text = card.Description;
            m_cardImage.sprite = card.CardImage;
            SetFlavourText(card.FlavourText);
        }

        private void SetFlavourText(string flavourText)
        {
            if (string.IsNullOrEmpty(flavourText) == false)
            {
                m_flavourTextLayer.SetActive(true);
                m_cardFlavourText.text = flavourText;
            }
            else
            {
                m_flavourTextLayer.SetActive(false);
            }
        }


        private void SetColourScheme(UIPalette.CardTypeColourScheme scheme)
        {
            m_cardTypeIcon.sprite = scheme.Icon;
            m_baseBorder.color = scheme.BorderGradientBottom;

            m_borderGradient.m_color1 = scheme.BorderGradientTop;
            m_borderGradient.m_color2 = scheme.BorderGradientBottom;

            m_titleGradient.m_color1 = scheme.TitleColourTop;
            m_titleGradient.m_color2 = scheme.TitleColourBottom;

            m_titleHighlight.m_color1 = scheme.TitleHighlightTop;
            m_titleHighlight.m_color2 = scheme.TitleHighlightBottom;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using AceTheChase.GameRules;
namespace AceTheChase.UI
{
    public class UICardView : MonoBehaviour
    {
        public event EventHandler OnClick;

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

        //cardtype (for special effects)
        [SerializeField]
        private GameObject m_cardTypeLayer;
        [SerializeField]
        private Text m_cardTypeText;

        //deck building thingy
        [SerializeField]
        private GameObject m_numberOwnedLayer;
        [SerializeField]
        private Text m_numberOwnedText;
        [SerializeField]
        private Image m_numberOwnedBorder;

        private ICard m_card;

        public void Setup(IPursuitCard card)
        {
            m_cardFlavourText.text = card.Name;
            m_cardControlCostLayer.SetActive(false);

            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.PursuitCardColours;
            SetColourScheme(scheme);
            m_card = card;

            m_cardName.text = card.Name;
            m_cardEffects.text = card.Description;
            m_cardImage.sprite = card.CardImage;
            SetFlavourText(card.FlavourText);
        }

        public void Setup(IPlayerCard card)
        {
            m_cardControlCostLayer.SetActive(true);

            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.GetCardTypeColorScheme(card.Driver);
            SetColourScheme(scheme);
            m_card = card;

            m_cardName.text = card.Name;
            m_cardEffects.text = card.Description;
            m_cardControlCost.text = card.ControlCost.ToString();
            m_cardImage.sprite = card.CardImage;
            SetFlavourText(card.FlavourText);
        }

        public void Setup(IRouteCard card)
        {
            m_cardControlCostLayer.SetActive(false);
            m_flavourTextLayer.SetActive(string.IsNullOrEmpty(card.FlavourText) == false);
            UIPalette.CardTypeColourScheme scheme = UIPalette.Instance.GetCardTypeColorScheme(card.CardType);
            SetColourScheme(scheme);
            m_card = card;

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
            m_baseBorder.color = scheme.BorderGradientBottom;

            m_borderGradient.SetColours(scheme.BorderGradientTop, scheme.BorderGradientBottom);
            m_titleGradient.SetColours(scheme.TitleColourTop, scheme.TitleColourBottom);
            m_titleHighlight.SetColours(scheme.TitleHighlightTop, scheme.TitleHighlightBottom);
            m_numberOwnedBorder.color = scheme.BorderGradientTop;
        }

        public ICard GetCard()
        {
            return m_card;
        }

        public void Clicked()
        {
            Debug.Log($"Clicked on {this.m_card.Name}.");
            OnClick?.Invoke(this.gameObject, null);
        }

        public void SetNumberOwned(int numberOwned)
        {
            if (numberOwned > 0)
            {
                m_numberOwnedLayer.SetActive(true);
                m_numberOwnedText.text = "x" + numberOwned.ToString();
            }
            else
            {
                m_numberOwnedLayer.SetActive(false);
            }
        }
    }
}

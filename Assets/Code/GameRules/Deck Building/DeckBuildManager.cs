using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AceTheChase.Utils;
using AceTheChase.GameRules;
using AceTheChase.UI;

public class DeckBuildManager : MonoBehaviour
{
    List<IPlayerCard> builtDeck;
    public List<PlayerCard> cardPool;

    public Transform uiPlayerDeck;
    public Transform uiCardPool;

    Dictionary<string, UICardView> uiPlayerDeckCards;
    Dictionary<string, UICardView> uiCardPoolCards;

    public GameObject cardUIPrefab;

    int maxDuplicates = 2;
    int maxDeckSize = 21;
    int maxOutOfFactionCards = 4;
    PlayerCardDriver deckFaction = PlayerCardDriver.StuntDriver;



    public void Start()
    {
        this.builtDeck = new List<IPlayerCard>();
        if(uiPlayerDeck == null)
        {
            Debug.LogError("UIPLAYERDECK NOT SETUP IN DECK BUILD MANAGER");
        }

        if (uiCardPool == null)
        {
            Debug.LogError("UICARDPOOL NOT SETUP IN DECK BUILD MANAGER");
        }

        if (cardUIPrefab == null)
        {
            Debug.LogError("CARD UI PREFAB NOT SETUP IN DECK BUILD MANAGER");
        }

        uiPlayerDeckCards = new Dictionary<string, UICardView>();
        uiCardPoolCards = new Dictionary<string, UICardView>();

        for (var i = 0; i < cardPool.Count; ++i)
        {
            IPlayerCard card = cardPool[i];
            GameObject uiCardObject = Instantiate(cardUIPrefab);
            UICardView uICardView = uiCardObject.GetComponent<UICardView>();
            uICardView.Setup(cardPool[i]);
            uICardView.transform.parent = uiCardPool;
            uICardView.OnClick += OnUICardViewClicked;
            uiCardPoolCards.Add(uICardView.GetCard().Name, uICardView);
        }
    }

    public void OnUICardViewClicked(object sender, EventArgs e)
    {
        Debug.Log("We know");
        GameObject uiCardView = sender as GameObject;
        if (uiCardView != null)
        {
            UICardView cardView = uiCardView.GetComponent<UICardView>();
            if (cardView != null)
            {
                if (uiCardView.transform.parent == uiPlayerDeck)
                {
                    this.RemoveCardFromDeck(cardView.GetCard() as IPlayerCard);
                }
                else if (uiCardView.transform.parent == uiCardPool)
                {
                    this.AddCardToDeck(cardView.GetCard() as IPlayerCard);
                }
            }
        }
    }

    public void AddCardToDeck(IPlayerCard card)
    {
        if(card == null)
        {
            return;
        }

        string failureReason;
        if (!ValidateAddCard(card, out failureReason))
        {
            Debug.Log($"DeckBuilding validation failure : {failureReason}");
            return;
        }

        builtDeck.Add(card);
        UICardView uiCardView;
        if(uiPlayerDeckCards.TryGetValue(card.Name, out uiCardView))
        {
            //Increment number of on card
        }
        else if(uiCardPoolCards.TryGetValue(card.Name, out uiCardView))
        {
            GameObject g = Instantiate(uiCardView.gameObject);
            UICardView view = g.GetComponent<UICardView>();
            view.Setup(card);
            view.OnClick += OnUICardViewClicked;
            uiPlayerDeckCards.Add(card.Name, view);
            g.transform.parent = uiPlayerDeck;
        }
    }

    public void RemoveCardFromDeck(IPlayerCard card)
    {
        if(card == null)
        {
            return;
        }

        if(builtDeck.Remove(card))
        {
            int numLeft = builtDeck.FindAll((obj) => { return obj.Name == card.Name; }).Count;
            UICardView uiCardView;
            if(uiPlayerDeckCards.TryGetValue(card.Name, out uiCardView))
            {
                if(numLeft > 0)
                {
                    //Update number of on card
                }
                else
                {
                    Destroy(uiCardView.gameObject);
                    uiPlayerDeckCards.Remove(card.Name);
                }
            }
        }
    }

    public void ClearDeck()
    {
        builtDeck.Clear();
        foreach(var keyValue in uiPlayerDeckCards)
        {
            Destroy(keyValue.Value.gameObject);
        }
        uiPlayerDeckCards.Clear();
    }


    public bool ValidateAddCard(IPlayerCard card, out string reason)
    {

        if (builtDeck.Count >= maxDeckSize)
        {
            reason = "Deck is max size";
            return false;
        }

        if (card.Driver != deckFaction)
        {
            IList<IPlayerCard> outOfFactionCards = builtDeck.FindAll((obj) => { return obj.Driver != deckFaction; });
            if (outOfFactionCards.Count >= maxOutOfFactionCards)
            {
                reason = "Deck has maximum number of out of faction cards";
                return false;
            }
        }

        IList<IPlayerCard> cards = builtDeck.FindAll((obj) => { return obj.Name == card.Name; });
        if(cards.Count >= maxDuplicates)
        {
            reason = "Deck has maximum number of duplicates of this card";
            return false;
        }
        reason = "";
        return true;
    }
}

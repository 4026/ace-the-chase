﻿using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using AceTheChase.Utils;
using AceTheChase.GameRules;
using AceTheChase.UI;
using UnityEngine.SceneManagement;

public class DeckBuildManager : MonoBehaviour
{
    List<IPlayerCard> builtDeck;
    [SerializeField]
    public List<PlayerCard> cardPool;

    public Transform uiPlayerDeck;
    public Transform uiCardPool;
    public UIGradient deckBuilderBackground;

    public Color stuntColorTop;
    public Color stuntColorBottom;
    public Color mechanicColorTop;
    public Color mechanicColorBottom;
    public Color navigatorColorTop;
    public Color navigatorColorBottom;

    public ChaseStartInfo initInfo;

    public GameObject errorMessage;
    public Text errorMessageText;

    Dictionary<string, UICardView> uiPlayerDeckCards;
    Dictionary<string, UICardView> uiCardPoolCards;

    public GameObject cardUIPrefab;

    public int maxDuplicates = 2;
    public int maxDeckSize = 21;
    public int maxOutOfFactionCards = 4;
    PlayerCardDriver deckFaction = PlayerCardDriver.None;



    public void Start()
    {
        //Error checks
        if (uiPlayerDeck == null)
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

        if (deckBuilderBackground == null)
        {
            Debug.LogError("DECK BUILDER BACKGROUND NOT SETUP IN DECK BUILD MANAGER");
        }

        if (initInfo == null)
        {
            Debug.LogError("CHASE START INFO NOT SETUP IN DECK BUILD MANAGER");
        }
        // ---

        if (errorMessage != null)
        {
            errorMessage.SetActive(false);
        }

        this.builtDeck = new List<IPlayerCard>();
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

        switch (initInfo.SelectedPlayerDriver)
        {
            case PlayerCardDriver.StuntDriver:
                ChangeDriverToStunt();
                break;
            case PlayerCardDriver.Navigator:
                ChangeDriverToNavigator();
                break;
            default: //Intended to catch the none case
                ChangeDriverToMechanic();
                break;
        }

        if (initInfo.SelectedPlayerCards != null)
        {
            for (var i = 0; i < initInfo.SelectedPlayerCards.Length; ++i)
            {
                AddCardToDeck(initInfo.SelectedPlayerCards[i]);
            }
        }
    }

    public void OnUICardViewClicked(object sender, EventArgs e)
    {
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
                    PlayerCard prototype = cardView.GetCard() as PlayerCard;
                    PlayerCard clone = Instantiate(prototype);
                    this.AddCardToDeck(clone);
                }
            }
        }
    }

    public void AddCardToDeck(IPlayerCard card)
    {
        if (card == null)
        {
            return;
        }

        string failureReason;
        if (!ValidateAddCard(card, out failureReason))
        {
            if (errorMessage)
            {
                errorMessage.SetActive(true);
                StartCoroutine("HideErrorMessage");
            }
            if (errorMessageText)
            {
                errorMessageText.text = failureReason;
            }

            Debug.Log($"DeckBuilding validation failure : {failureReason}");
            return;
        }

        builtDeck.Add(card);
        UICardView uiCardView;
        if (uiPlayerDeckCards.TryGetValue(card.Name, out uiCardView))
        {
            IList<IPlayerCard> cards = builtDeck.FindAll((obj) => { return obj.Name == card.Name; });
            uiCardView.SetNumberOwned(cards.Count);
        }
        else if (uiCardPoolCards.TryGetValue(card.Name, out uiCardView))
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
        if (card == null)
        {
            return;
        }

        if (builtDeck.Remove(card))
        {
            int numLeft = builtDeck.FindAll((obj) => { return obj.Name == card.Name; }).Count;
            UICardView uiCardView;
            if (uiPlayerDeckCards.TryGetValue(card.Name, out uiCardView))
            {
                if (numLeft > 0)
                {
                    uiCardView.SetNumberOwned(numLeft);
                }
                else
                {
                    Destroy(uiCardView.gameObject);
                    uiPlayerDeckCards.Remove(card.Name);
                }
            }
        }
    }

    public void Save()
    {
        initInfo.SelectedPlayerCards = builtDeck.ToArray();
        initInfo.SelectedPlayerDriver = deckFaction;
    }


    public void Submit()
    {
        Save();
        SceneManager.LoadScene("Main");
    }

    public void GoBack()
    {
        Save();
        SceneManager.LoadScene("TitleScreen");
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


    public void ChangeDriverToStunt()
    {
        if(deckFaction != PlayerCardDriver.StuntDriver)
        {
            ClearDeck();
        }
        deckBuilderBackground.SetColours(stuntColorTop, stuntColorBottom);
        deckFaction = PlayerCardDriver.StuntDriver;
    }

    public void ChangeDriverToNavigator()
    {
        if (deckFaction != PlayerCardDriver.Navigator)
        {
            ClearDeck();
        }
        deckBuilderBackground.SetColours(navigatorColorTop, navigatorColorBottom);
        deckFaction = PlayerCardDriver.Navigator;
    }

    public void ChangeDriverToMechanic()
    {
        if (deckFaction != PlayerCardDriver.Mechanic)
        {
            ClearDeck();
        }
        deckBuilderBackground.SetColours(mechanicColorTop, mechanicColorBottom);
        deckFaction = PlayerCardDriver.Mechanic;
    }


    public bool ValidateAddCard(IPlayerCard card, out string reason)
    {

        if (builtDeck.Count >= maxDeckSize)
        {
            reason = "Your deck cannot have more than 21 cards in it.";
            return false;
        }

        if (card.Driver != deckFaction && card.Driver != PlayerCardDriver.None)
        {
            IList<IPlayerCard> outOfFactionCards = builtDeck.FindAll((obj) => { 
            return obj.Driver != deckFaction 
                && obj.Driver != PlayerCardDriver.None; 
            });
            if (outOfFactionCards.Count >= maxOutOfFactionCards)
            {
                reason = "You cannot have more than 4 cards from different specialities.";
                return false;
            }
        }

        IList<IPlayerCard> cards = builtDeck.FindAll((obj) => { return obj.Name == card.Name; });
        if(cards.Count >= maxDuplicates)
        {
            reason = "You cannot have more than 2 copies of any card in your deck";
            return false;
        }
        reason = "";
        return true;
    }

    private IEnumerator HideErrorMessage ()
    {
        yield return new WaitForSeconds(2);
        errorMessage.SetActive(false);
    }
}

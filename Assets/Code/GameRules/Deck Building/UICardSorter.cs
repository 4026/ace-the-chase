using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using AceTheChase.GameRules;
using AceTheChase.UI;

public class UICardSorter : MonoBehaviour
{
    public int numChildren = 0;

    private class PlayerCardSortable : IComparable<PlayerCardSortable>
    {
        public GameObject g;
        public PlayerCard card;

        public PlayerCardSortable(GameObject g, PlayerCard card)
        {
            this.g = g;
            this.card = card;
        }

        public int CompareTo(PlayerCardSortable other)
        {
            return card.CompareTo(other.card);
        }
    }

    public void Start()
    {
        numChildren = transform.childCount;
    }

    public void Update()
    {
        if(transform.childCount != numChildren)
        {
            List<PlayerCardSortable> cardsToSort = new List<PlayerCardSortable>();
            for(var i = 0; i < transform.childCount; ++i)
            {
                UICardView cardView = transform.GetChild(i).GetComponent<UICardView>();
                if (cardView)
                {
                    PlayerCard card = cardView.GetCard() as PlayerCard;
                    if (card)
                    {
                        cardsToSort.Add(new PlayerCardSortable(transform.GetChild(i).gameObject, card));
                    }
                }
            }
            cardsToSort.Sort();
            for(var i =0; i < cardsToSort.Count; ++i)
            {
                cardsToSort[i].g.transform.SetSiblingIndex(i);
            }
            numChildren = transform.childCount;
        }
    }
}

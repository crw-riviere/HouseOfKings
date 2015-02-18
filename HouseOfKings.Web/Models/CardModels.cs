using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace HouseOfKings.Web.Models
{
    public enum Suit
    {
        Clubs = 0,
        Diamonds = 1,
        Hearts = 2,
        Spades = 3
    }

    public class Deck
    {
        private static Random rnd = new Random();

        private List<Card> cards;

        public List<Card> Cards
        {
            get
            {
                if (this.cards == null)
                {
                    this.cards = BuildDeck().ToList();
                }

                return this.cards;
            }
            set { this.cards = value; }
        }

        public static IEnumerable<Card> BuildDeck()
        {
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (int i in Enumerable.Range(1, 13))
                {
                    yield return new Card() { Suit = suit, Number = i };
                }
            }
        }

        public int CardCount
        {
            get
            {
                return this.Cards.Count;
            }
        }

        public int KingCount
        {
            get
            {
                return this.Cards.Count(x => x.Number == 13);
            }
        }

        public Card PickRandomCard()
        {
            int cardCount = this.Cards.Count;

            if (cardCount > 0)
            {
                var card = this.Cards.ElementAt(rnd.Next(cardCount));
                this.Cards.Remove(card);
                return card;
            }

            return null;
        }
    }

    public class Card
    {
        [Range(1, 13)]
        public int Number { get; set; }

        [Required]
        public Suit Suit { get; set; }
    }
}
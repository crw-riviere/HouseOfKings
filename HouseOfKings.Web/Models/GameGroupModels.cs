using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HouseOfKings.Web.Models
{
    public class GameGroup
    {
        private Deck deck;
        private List<Player> players;

        [Key]
        public string Name { get; set; }

        [NotMapped]
        public Deck Deck
        {
            get
            {
                if (this.deck == null)
                {
                    this.deck = new Deck();
                }
                return this.deck;
            }
            set
            {
                this.deck = value;
            }
        }

        [NotMapped]
        public Player CurrentTurn { get; set; }

        [NotMapped]
        public List<Player> Players
        {
            get
            {
                if (this.players == null)
                {
                    this.players = new List<Player>();
                }
                return this.players;
            }
            set
            {
                this.players = value;
            }
        }
    }

    public class Player
    {
        public string ConnectionId { get; set; }

        public string Username { get; set; }

        public string Id { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CardGameApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BlackJackDeck blackJack = new BlackJackDeck();

            var hand = blackJack.DealCards();
            Console.WriteLine("Your hand: ");
            int points = blackJack.RevealHand(hand);
            blackJack.CheckForWinner(points);


            Console.WriteLine();


            var dealerHand = blackJack.DealCards();
            Console.WriteLine("The dealers hand: ");
            int dealerPoints= blackJack.RevealHand(dealerHand);



            Console.WriteLine();


            blackJack.ChooseHitOrStand(hand, points);

            Console.ReadLine();
        }
    }

    public abstract class Hand
    {
        public void ShowHand(List<PlayingCardModel> hand)
        {
            Console.WriteLine("Your hand: ");

            foreach (var card in hand)
            {
                Console.WriteLine($"{card.Value.ToString()} {card.Suit.ToString()} ");
            }
        }
    }

    public abstract class Deck
    {
        protected List<PlayingCardModel> fullDeck = new List<PlayingCardModel>();
        protected List<PlayingCardModel> drawPile = new List<PlayingCardModel>();
        protected List<PlayingCardModel> discardPile = new List<PlayingCardModel>();

        protected void CreateDeck()
        {
            fullDeck.Clear();

            for (int suit = 0; suit < 4; suit++)
            {
                for (int val = 0; val < 13; val++)
                {
                    fullDeck.Add(new PlayingCardModel { Suit = (CardSuit)suit, Value = (CardValue)val });
                }
            }
        }

        public void ShuffleDeck()
        {
            var rnd = new Random();
            drawPile = fullDeck.OrderBy(x => rnd.Next()).ToList();
        } 

        public abstract List<PlayingCardModel> DealCards();

        public virtual PlayingCardModel DrawOneCard()
        {
            PlayingCardModel output =  drawPile.Take(1).First();
            drawPile.Remove(output); 
            return output;
        }

        public void DiscardCard(PlayingCardModel cardToDiscard)
        {
            discardPile.Add(cardToDiscard);
        }

        
    }

    public class BlackJackDeck : Deck
    {
        public BlackJackDeck()
        {
            CreateDeck();
            ShuffleDeck();
        }

        public override List<PlayingCardModel> DealCards()
        {
            List<PlayingCardModel> output = new List<PlayingCardModel>();

            for (int i = 0; i < 2; i++)
            {
                output.Add(DrawOneCard());
            }

            return output;
        }

        public int RevealHand(List<PlayingCardModel> hand)
        {

            Dictionary<string, int> parseTxtToInt = new Dictionary<string, int>
        {
            { "Ace", 1 },
            { "Two", 2 },
            { "Three", 3 },
            { "Four", 4 },
            { "Five", 5 },
            { "Six", 6 },
            { "Seven", 7 },
            { "Eight", 8 },
            { "Nine", 9 },
            { "Ten", 10 },
            { "Jack", 10 },
            { "Queen", 10 },
            { "King", 10 },
        };

            int totalPoints = 0;

            foreach (var card in hand)
            {
                Console.WriteLine($"{card.Value.ToString()} {card.Suit.ToString()} ");

                string numberTxt = card.Value.ToString();


                if (parseTxtToInt.ContainsKey(numberTxt))
                {
                    int numericValue = parseTxtToInt[numberTxt];
                    totalPoints += numericValue;

                }
            }

            Console.WriteLine($"total points: {totalPoints}");

            return totalPoints;
        }

        public void ChooseHitOrStand(List<PlayingCardModel> hand, int points)
        {
            bool continueToHit = false;

            do
            {
                Console.Write("Do you choose hit or stand: ");
                string hitOrStand = Console.ReadLine();

                if (hitOrStand == "hit") 
                {
                    continueToHit = true;
                }
                else
                {
                    continueToHit = false;
                }


                if (continueToHit)
                {
                    hand.Add(DrawOneCard());
                    RevealHand(hand);
                }
            } while (continueToHit);

                if (!continueToHit)
            {
                Console.WriteLine($"Your final points are: {points}");
            }

        }

        public void CheckForWinner (int totalPoints)
        {
            if (totalPoints == 21)
            {
                Console.WriteLine("You Win!");
            }
            else if(totalPoints <21)
            {
                Console.WriteLine($"You've got {21 - totalPoints} points to go till 21 points");
            }
            else if (totalPoints > 21)
            {
                Console.WriteLine("Lost due to > 21 points");
            }
        }


    }


}

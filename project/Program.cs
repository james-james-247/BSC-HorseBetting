using System;
using System.Collections.Generic;
using System.Linq;

namespace project
{
    static class variables
    {
        //Array Containing Current Competitors
        public static string[,] competitors = new string[,] { { "Greywind", "0" }, { "Hallifax", "0" }, { "Ghost", "0" } };
        //Number of Competitors
        public static int NumCompetitors = competitors.Length;

        //Money User Has
        public static decimal wallet = 0;
        //Bet the User Made
        public static decimal currentBet = 0;

        //Risk of Injuries Changing Over Time
        public static int risk = 7;

        //Month Array and Weather Controller
        public static string[,] months = new string[,] { { "Jan", "High" }, { "Feb", "High" }, { "Mar", "High" }, { "Apr", "Mild" }, { "May", "Low" }, { "Jun", "Low" }, { "Jul", "Low" }, { "Aug", "Low" }, { "Sep", "Mild" }, { "Oct", "Mild" }, { "Nov", "High" }, { "Dec", "High" } };
        //Controllers of Months
        public static int monthCont = 1;
        public static int monthMax = 12;
        //Max Races Per Month 
        public static int perMonth = 3;
    }


    class Program
    {
        //Starting Class
        static void Main(string[] args)
        {
            game();
        }


        //
        //GAME
        //This is the Main Class That Controls The Game
        static void game()
        {
            //Sets the Weather
            int weather = setWeather();
            switch (weather)
            {
                case int n when (weather > 7):
                    Console.WriteLine("- - - The Weather Is Looking Bad! Lowering Odds - - -");
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                case int n when (weather > 4 && weather < 7):
                    Console.WriteLine("- - - The Weather Is Looking Average! Potentionally Lowering Odds - - -");
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
                case int n when (weather > 0 && weather < 4):
                    Console.WriteLine("- - - The Weather Is Looking Good! Odds Likley Wont Change - - -");
                    Console.WriteLine();
                    Console.WriteLine();
                    break;
            }



            //Dispalays the Current Odds
            oddsCalc(weather);



            //Asks for Users Bets
            Console.WriteLine("- - - Welcome! Please Choose a Competitor To Bet On: - - -");
            string chosenComp = Console.ReadLine().Trim();

            Console.WriteLine("- - - Brilliant! Now How Much Would You Like To Bet? - - -");            decimal chosenAmount = decimal.Parse(Console.ReadLine().Trim());
            decimal finalAmount = Math.Round(chosenAmount, 2);

            Console.WriteLine("- - - Top Class! Thats " + finalAmount + " On " + chosenComp + " - - -");
            Console.WriteLine();
            Console.WriteLine();



            //Checks For Injuries
            injuries(weather);



            //Runs the Race
            string winner = race();



            //Handling the Money
            if(chosenComp == winner)
            {
                money(true, finalAmount, "10/1") ;
            }
            else
            {
                money(false, finalAmount, "10/1");
            }



            //Calculates New Risk
            if(variables.risk == 10)
            {
                variables.risk = 1;
                variables.monthCont += 1;
                if(variables.monthCont > 12)
                {
                    variables.monthCont = 0;
                }
                Console.WriteLine("- ! - ! - ! Risk Got Too High! One Month Break Was Given - ! - ! - !");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                variables.risk += 1;
            }



            //Calcualtes Months
            if (variables.perMonth == 3)
            {
                variables.monthCont += 1;
                variables.perMonth = 1;
                if (variables.monthCont > 12)
                {
                    variables.monthCont = 0;
                }
            }
            else
            {
                variables.perMonth += 1;
            }



            //Begins the Cycle Again
            Console.WriteLine("Type anything to continue! Type exit to exit");
            string answer = Console.ReadLine().Trim();
            if(answer == "exit") { }
            else { game(); }
        }

            
        //
        //SETWEATHER
        //This Class Defines the Harshness of the Weather and Returns an INT
        static int setWeather()
        {
            //Getting the Chance from Months
            string chance = variables.months[variables.monthCont, 1];
            //Preparing INT for Storage
            int randNum;
            switch (chance)
            {
                //The Following Calculate Random Numbers
                case "High":
                    randNum = new Random().Next(7, 10);
                    break;
                case "Mild":
                    randNum = new Random().Next(4, 7);
                    break;
                case "Low":
                    randNum = new Random().Next(0, 4);

                    break;
                //Incase System Doesnt Work Properly
                default:
                    randNum = new Random().Next(0, 10);
                    break;
            }
            //Returning Final Value
            return randNum;
        }


        //
        //INJURIES
        //This Class Defines if an Injury Occours
        static void injuries(int weather)
        {
            //Collecting Our Needed Variables
            int risk = variables.risk;
            string[,] array = variables.competitors;
            int comptNum = variables.NumCompetitors;
            int chance = weather + risk;
            string competitorName;

            //For Loop for Every Competitor
            for(int i = 0; i < array.Length / 3; i++)
            {
                //Calculating a Final Random Number
                int randNum = new Random().Next(0, chance);
                if(randNum >= chance - 2)
                {

                    //Removing the Competitor From The Array
                    array[i, 0].Remove(2);//To be finished

                    //Displaying That the Competitor Has Been Removed
                    competitorName = variables.competitors[i, 0];
                    Console.WriteLine("- - - " + competitorName + " Injured and DNF - - -");
                }
            }
        }


        //
        //MONEY HANDLING
        //This Class Handles Money
        static void money(bool victory, decimal bet, string odds)
        {
            //Splitting the Odds
            string[] finalOdds = odds.Split('/');
            int firstOdd = Int32.Parse(finalOdds[0]);
            decimal moneyMade;

            if (victory == true)
            {
                //Takes the Odds and Calculates Winnings
                moneyMade = bet * firstOdd;
                //Adds Winnings To Wallet
                variables.wallet += moneyMade;
                Console.WriteLine("- - - Congrats Up To: £" + variables.wallet + " - - -");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                //Removes Bet From Wallet
                variables.wallet -= bet;
                Console.WriteLine("- - - Ouch Down To: £" + variables.wallet + " - - -");
                Console.WriteLine();
                Console.WriteLine();
            }
        }


        //
        //CALCULATING ODDS
        //This Class Will Determine the Odds For Every Competitors
        static void oddsCalc(int weather)
        {
            //Collecting Variables
            int numComp = variables.NumCompetitors;
            int risk = variables.risk;

            int weatherRisk = risk + weather;
            string[,] arrayCopy = variables.competitors;

            int[,] ordered = new int[,] { };

            for(int i = 0; i < arrayCopy.Length / 2; i++)
            {
            
            }
        }


        //
        //RACE
        //This Class is Where the Race is Held
        static string race()
        {
            int oldWeight = 0;
            string oldName = "default";

            string[,] array = new string[,] { { "James", "10" }, { "Jeff", "5" } };

            for (int i = 0; i < array.Length / 2; i++)
            {
                //Take the odds
                int odd = Int32.Parse(array[i, 1]);

                int weight;
                string name = array[i, 0];

                //Apply generic number based on odds
                switch (odd)
                {
                    case 1 when odd > 8:
                        weight = new Random().Next(70, 100);
                        break;
                    case 2 when odd >= 5 && odd < 8:
                        weight = new Random().Next(60, 90);
                        break;
                    case 3 when odd >= 3 && odd < 5:
                        weight = new Random().Next(40, 70);
                        break;
                    case 4 when odd >= 1 && odd < 3:
                        weight = new Random().Next(20, 60);
                        break;
                    case 5 when odd == 1:
                        weight = new Random().Next(1, 50);
                        break;
                    default:
                        weight = new Random().Next(1, 100);
                        break;
                }

                //All competitors generate a random number with the higher the odds increasing the chance of getting a higher numebr
                weight = weight * new Random().Next(2, 4);

                if(weight > oldWeight)
                {
                    oldWeight = weight;
                    oldName = name;
                }
            }
            Console.WriteLine("- - - Winner: " + oldName + " - - -");


            //Returning the winning name
            return oldName;
        }
    }
}

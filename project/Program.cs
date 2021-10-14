using System;
using System.Collections.Generic;
using System.Linq;

namespace project
{
    class Information
    {
        public List<string> name = new List<string>() { "Greywind", "Halifax", "Pegasus", "Ghost", "Nymeria", "Lady", "Roach", "Shaggy"};
        public List<int> wins = new List<int> () {1, 2, 3, 4, 5, 6, 7, 8};

        public void adding(string newName)
        {
            name.Add(newName);
            wins.Add(0);
            Console.WriteLine("- - - New Competitor Added: " + newName + " - - -");
            Console.WriteLine();
        }

        public void minusing(string removeName) 
        {
            int position = name.IndexOf(removeName);
            wins.RemoveAt(position);
            foreach(int x in wins)
            {
                Console.WriteLine(x);
            }
            name.Remove(removeName);
            foreach(string x in name)
            {
                Console.WriteLine(x);
            }
        }

        //Risk of Injuries Changing Over Time
        public int risk = 0;

        //Money User Has
        public decimal wallet = 0;
        //Bet the User Made
        public decimal currentBet = 0;


        //Month Array and Weather Controller
        public string[,] months = new string[,] { { "Jan", "High" }, { "Feb", "High" }, { "Mar", "High" }, { "Apr", "Mild" }, { "May", "Low" }, { "Jun", "Low" }, { "Jul", "Low" }, { "Aug", "Low" }, { "Sep", "Mild" }, { "Oct", "Mild" }, { "Nov", "High" }, { "Dec", "High" } };
        //Controllers of Months
        public int monthCont = 1;
        //Max Races Per Month 
        public int perMonth = 3;
    }



    class Program
    {
        //Starting Class
        static void Main(string[] args)
        {
            var information = new Information();
            game(information);
        }


        //
        //GAME
        //This is the Main Class That Controls The Game
        static void game(Information information)
        {
            //Sets the Weather
            int weather = setWeather(information);
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
            string[] oddsArray = oddsCalc(weather, information);



            //Asks for Users Bets
            Console.WriteLine("- - - Welcome! Please Choose a Competitor To Bet On: - - -");
            string chosenComp = Console.ReadLine().Trim();

            Console.WriteLine("- - - Brilliant! Now How Much Would You Like To Bet? - - -");            
            decimal chosenAmount = decimal.Parse(Console.ReadLine().Trim());
            decimal finalAmount = Math.Round(chosenAmount, 2);

            Console.WriteLine("- - - Top Class! Thats " + finalAmount + " On " + chosenComp + " - - -");
            Console.WriteLine();
            Console.WriteLine();



            //Checks For Injuries
            injuries(weather, information);



            //Runs the Race
            string winner = race(information, oddsArray);



            //Handling the Money
            if(chosenComp == winner)
            {
                money(true, finalAmount, "10/1", information) ;
            }
            else
            {
                money(false, finalAmount, "10/1", information);
            }



            //Calculates New Risk
            if(information.risk == 10)
            {
                information.risk = 1;
                information.monthCont += 1;
                if(information.monthCont > 12)
                {
                    information.monthCont = 0;
                }
                Console.WriteLine("- ! - ! - ! Risk Got Too High! One Month Break Was Given - ! - ! - !");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                information.risk += 1;
            }



            //Calcualtes Months
            if (information.perMonth == 3)
            {
                information.monthCont += 1;
                information.perMonth = 1;
                if (information.monthCont > 12)
                {
                    information.monthCont = 0;
                }
            }
            else
            {
                information.perMonth += 1;
            }



            //Begins the Cycle Again
            Console.WriteLine("Type anything to continue! Type exit to exit");
            string answer = Console.ReadLine().Trim();
            if(answer == "exit") { }
            else { game(information); }
        }

            
        //
        //SETWEATHER
        //This Class Defines the Harshness of the Weather and Returns an INT
        static int setWeather(Information information)
        {
            //Getting the Chance from Months
            string chance = information.months[information.monthCont, 1];
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
        static void injuries(int weather, Information information)
        {
            //Collecting Our Needed Variables
            int risk = information.risk;
            int comptNum = information.name.Count;
            int chance = weather + risk;
            string competitorName;

            //For Loop for Every Competitor
            for(int i = 0; i < comptNum; i++)
            {
                //Calculating a Final Random Number
                int randNum = new Random().Next(0, chance);
                if(randNum >= chance - 2)
                {
                    //Displaying That the Competitor Has Been Removed
                    competitorName = information.name[i];

                    information.minusing(information.name[i]);

                    
                    Console.WriteLine("- - - " + competitorName + " Injured and DNF - - -");


                    //Adding a New Competitor To Replace The Removed One
                    newCompetitor(information);
                }
            }
        }


        //
        //MONEY HANDLING
        //This Class Handles Money
        static void money(bool victory, decimal bet, string odds, Information information)
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
                information.wallet += moneyMade;
                Console.WriteLine("- - - Congrats Up To: £" + information.wallet + " - - -");
                Console.WriteLine();
                Console.WriteLine();
            }
            else
            {
                //Removes Bet From Wallet
                information.wallet -= bet;
                Console.WriteLine("- - - Ouch Down To: £" + information.wallet + " - - -");
                Console.WriteLine();
                Console.WriteLine();
            }
        }


        //
        //CALCULATING ODDS
        //This Class Will Determine the Odds For Every Competitors
        static string[] oddsCalc(int weather, Information information)
        {
            //Collecting Variables
            int numComp = information.name.Count;
            int risk = information.risk;

            //Calculating the Final Risk
            int weatherRisk = risk + weather;


            //Bringing in All Lists and Converting To Arrays 
            string[] compName = information.name.ToArray();
            int[] compWins = information.wins.ToArray();

            //Ordering this List Based on Number of Wins 
            int[] orderedWins = information.wins.ToArray();
            Array.Sort(orderedWins);
            Array.Reverse(orderedWins);

            //Creating the Final Array
            string[] oddsArray = new string[8]{"", "", "", "", "", "", "", "" };

            for (int i = 0; i < numComp; i++)
            {
                int finalOdd = 0;
                string finalName = "";

                //Adding Higher Odds if Weather is Better
                if (weatherRisk > 12)
                {
                    finalOdd += 5;
                }
                else
                {
                    finalOdd += 10;
                }


                //Minusing 5 Just For Smaller Numbers
                finalOdd -= 5;
                   
                //Adding the Counter in Increasing Order to Make Worse Competitors Have Higher Odds
                finalOdd += i;

                //If the Odd Ends At 1 Then Add One So The Player Can Win Something 
                if (finalOdd == 1)
                {
                    finalOdd++;
                }

                //Converting it To String Because C# Arrays Have Turned Me Crazy and I Gave Up
                string finalFinalOdd = finalOdd.ToString();

                //Finding Who The New Odd Represents
                foreach(int value in compWins)
                {
                    if(value == orderedWins[i])
                    {
                        finalName = compName[information.wins.IndexOf(orderedWins[i])];
                    }
                }

                //Adding the Odd and Name To The Array
                oddsArray[i] = finalName + "," + finalFinalOdd;

                //Displaying the Odds To The Player
                Console.WriteLine();
                Console.WriteLine(finalName + " " + finalFinalOdd);
                Console.WriteLine();
            }

            //Returning the Odds Array
            return oddsArray;
        }


        //
        //RACE
        //This Class is Where the Race is Held
        static string race(Information information, string[] oddsArray)
        {
            int oldWeight = 0;
            string oldName = "default";

            for (int i = 0; i < oddsArray.Length; i++)
            {
                //Taking Each Value in the Array
                string split = oddsArray[i];
                string[] newSplit = split.Split(",", 2);

                //Take the Odds
                int odd = Int32.Parse(newSplit[1]);

                //Taking the Name and Weight  
                int weight;
                string name = newSplit[0];

                //Apply Generic Number Based on Odds
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

                //All Competitors Generate a Random Number With the Higher the Odds Increasing the Chance of Getting a Higher Numebr
                weight = weight * new Random().Next(2, 4);

                if(weight > oldWeight)
                {
                    //Replacing Current Winner With New Winner
                    oldWeight = weight;
                    oldName = name;
                }
            }

            //Displaying Winner to the Player
            Console.WriteLine("- - - Winner: " + oldName + " - - -");


            //Returning the Winning Name
            return oldName;
        }


        //
        //NEW COMPETITOR
        //This Class Adds A New Competitor When One Gets Injured
        static void newCompetitor(Information information)
        {
            int length = new Random().Next(4, 7);

            Random random = new Random();
            //Consonants Plus Some Special Specific Sounds Like Sh 
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            //Vowels Plus y 
            string[] vowels = { "a", "e", "i", "o", "u", "y" };
            string compName = "";

            //This Gets The First Two Letters of The New Name
            compName += consonants[random.Next(consonants.Length)].ToUpper();
            compName += vowels[random.Next(vowels.Length)];

            //This ensures the systems knows weve already added two letters
            int started = 2; 

            //length is Provided and Is Random
            while (started < length)
            {
                compName += consonants[random.Next(consonants.Length)];
                started++;
                compName += vowels[random.Next(vowels.Length)];
                started++;
            }

            //Sending New Value to be Added
            information.adding(compName);
        }
    }
}

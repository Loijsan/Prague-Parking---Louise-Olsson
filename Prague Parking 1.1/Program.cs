using System;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;

namespace Prague_Parking_1._0
{
    class Program
    {
        public static string[] ParkingList = new string[100];

        static void Main()
        {
            MainMenu();
        }
        static void MainMenu()
        {
            Console.WriteLine("Hello and welcome to Pague Parking, what would you like to do? Type the number of your menu choice" +
                "\n" +
                "\n 1. Availability and overview" +
                "\n" +
                "\n 2. Park vehicle" +
                "\n" +
                "\n 3. Check out vehicle" +
                "\n" +
                "\n 4. Search for and Move vehicle" +
                "\n" +
                "\n 5. Optimize the mc:s in the parkinglot" +
                "\n");
            Console.Write("Number: ");
            string menuChoice = Console.ReadLine();
            int a;
            bool choice = int.TryParse(menuChoice, out a);

            if (a >= 1 && a <= 5)
            {
                switch (a)
                {
                    case 1: Available(); break;
                    case 2: ParkVehicle(); break;
                    case 3: CheckOut(); break;
                    case 4: MoveVehicle(); break;
                    case 5: OptimizeSpace(); break;
                    default:
                        break;
                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Invalid input, try again");
                MainMenu();
            }
        }
        static void Available()
        {
            Console.Clear();
            Console.WriteLine("1. Availability and overview");

            int count = Availability();

            Console.WriteLine("\n\n\nThere are {0} parking spaces available", count);

            Console.WriteLine("\n\n\nHere are all the parking spots:");

            AllSpaces();

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        static int Availability()
        {
            int counter = 0;
            foreach (string space in ParkingList)
            {
                if (space == null)
                {
                    counter = counter + 1;
                }
            }
            return counter;
        }
        static void AllSpaces()
        {
            int i = 0;
            string[] vehicle = new string[200];
            foreach (string element in ParkingList)
            {
                i++;
                Console.WriteLine(" _____________");
                if (element == null)
                {
                    Console.WriteLine("|" + i + "\n|_____________");
                }
                else if (element.Contains("mc") && element.Contains(", "))
                {
                    vehicle[i] = " " + element.Replace('!', ' ');
                    string[] twoVehicle = vehicle[i].Split(", ");
                    Console.WriteLine("|" + i + twoVehicle[0] + "\n|  " + twoVehicle[1] + "\n|_____________");
                }
                else
                {
                    vehicle[i] = " " + element.Replace('!', ' ');
                    Console.WriteLine("|" + i + vehicle[i] + "\n|_____________");
                }
            }
        }
        static void ParkVehicle()
        {
            Console.Clear();
            Console.Write("2.Park vehicle \n \nPlease enter vehicle type, car / mc: ");
            string park = Console.ReadLine();
            string carPark;
            string mcPark;

            if (park == "car")
            {
                carPark = ParkCar();
                Console.WriteLine(carPark);
            }
            else if (park == "mc")
            {
                mcPark = ParkMc();
                Console.WriteLine(mcPark);
            }
            else { ParkVehicle(); }

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        static string ParkCar()
        {
            string carReg = "";
            DateTime now = DateTime.Now;
            int empty = EmptySpace();
            string spotReceipt = "";
            string notCorrect = "\nPlease enter a valid registration number";
            Console.Clear();
            Console.Write("\nPlease enter the cars registration number in one string: ");
            carReg = Console.ReadLine().ToUpper();
            string checker = CheckReg(carReg);
            if (checker == "Ok")
            {
                if (carReg.Length <= 10)
                {
                    if (!carReg.Contains(" "))
                    {
                        string receipt = "\nYou have parked a car \nwith the registration number: " + carReg + "\nat: " + now + "\nin spot: " + empty;
                        spotReceipt = "car!" + carReg;
                        SpotAllocation(empty, spotReceipt);
                        return receipt;
                    }
                    else
                    {
                        return notCorrect;
                    }
                }
                else
                {
                    return notCorrect;
                }
            }
            else
            {
                Console.WriteLine(checker);
                return notCorrect;
            }
        }
        static int EmptySpace()
        {
            int counter = 0;
            int emptySpot = 0;
            foreach (string element in ParkingList)
            {
                counter = counter + 1;
                if (element == null)
                {
                    break;
                }
            }
            emptySpot = counter;
            return emptySpot;
        }
        static void SpotAllocation(int spot, string vehicle)
        {
            spot = spot - 1;
            ParkingList[spot] = vehicle;
        }
        static string CheckReg(string search)
        {
            string clone = "\nThere is already a vehicle parked with this registration number, please try again";
            string noClone = "Ok";

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {
                        if (vehicles.Length == 2)
                        {
                            string[] atoms = splits.Split("!");

                            if (atoms[1] == search)
                            {
                                return clone;
                            }
                        }
                        else
                        {
                            string[] atoms = splits.Split("!");
                            if (atoms[1] == search)
                            {
                                return clone;
                            }
                        }
                    }
                }
            }
            return noClone;
        }
        static string ParkMc()
        {
            string mcReg1 = "";
            string mcReg2 = "";
            DateTime now = DateTime.Now;
            int empty = EmptySpace();
            string receipt = "";
            string spotReceipt = "";

            Console.Write("\nis it one or two motorcycles you would like to park?: ");
            string mcSum = Console.ReadLine();
            if (mcSum == "one" || mcSum == "1")
            {
                Console.Write("\nPlease enter the motorcycles registration number in one string: ");
                mcReg1 = Console.ReadLine().ToUpper();
                string checker = CheckReg(mcReg1);

                if (checker == "Ok")
                {
                    if (mcReg1.Length <= 10)
                    {
                        if (!mcReg1.Contains(" "))
                        {
                            receipt = "\nYou have parked a motorcycle \nwith the registration number: " + mcReg1 + "\nat: " + now + "\nin spot: " + empty;
                            spotReceipt = "mc!" + mcReg1;
                            SpotAllocation(empty, spotReceipt);

                        }
                        else
                        {
                            string notCorrect = "\nPlease enter a valid registration number";
                            return notCorrect;
                        }
                    }
                    else
                    {
                        string notCorrect = "\nPlease enter a valid registration number";
                        return notCorrect;
                    }
                }
                else
                {
                    Console.WriteLine("One of the registration numbers are already parked! Check menu option 1 to get an overview.");
                }
            }
            else if (mcSum == "two" || mcSum == "2")
            {
                Console.Write("\nPlease enter the first motorcycles registration number in one string: ");
                mcReg1 = Console.ReadLine().ToUpper();
                Console.Write("\nPlease enter the second motorcycles registration number in one string: ");
                mcReg2 = Console.ReadLine().ToUpper();
                string checker = CheckReg(mcReg1);
                string doubleChecker = CheckReg(mcReg2);
                if (mcReg1 == mcReg2)
                {
                    Console.WriteLine("\n\nEach vehicle has its own registation number, please try again and enter those.");
                }
                else
                {
                    if (checker == "Ok" && doubleChecker == "Ok")
                    {
                        if (mcReg1.Length <= 10 || mcReg2.Length <= 10)
                        {
                            if (!mcReg1.Contains(" ") || !mcReg2.Contains(" "))
                            {
                                receipt = "\nYou have parked two motorcycles \nwith the registration numbers: " + mcReg1 + " and " + mcReg2 + "\nat: " + now + "\nin spot: " + empty;
                                spotReceipt = "mc!" + mcReg1 + ", mc!" + mcReg2;
                                SpotAllocation(empty, spotReceipt);

                            }
                            else
                            {
                                string notCorrect = "\nPlease enter a valid registration number";
                                return notCorrect;
                            }
                        }
                        else
                        {
                            string notCorrect = "\nPlease enter a valid registration number";
                            return notCorrect;
                        }
                    }
                    else
                    {
                        Console.WriteLine("One of the registration numbers are already parked! Check menu option 1 to get an overview.");
                    }
                }
            }
            else { ParkVehicle(); }

            return receipt;
        }
        static void CheckOut()
        {
            Console.Clear();
            Console.Write("3. Check out vehicle \n\nPlease enter the registration number of the vehicle you would like to check out: ");
            string regNr = Console.ReadLine().ToUpper();
            string answer = RemoveVehicle(regNr);
            Console.WriteLine(answer);

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        static string RemoveVehicle(string search)
        {
            string isThere = "\nThe vehicle has been checked out!";
            string notThere = "\nThere is no vehicle parked with this registration number, please try again";

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {
                        if (vehicles.Length == 2)
                        {
                            string[] atoms = splits.Split("!");

                            if (atoms[1] == search)
                            {
                                int nr = Array.IndexOf(ParkingList, parkingSpot);
                                if (vehicles[0].Contains(atoms[1]))
                                {
                                    ParkingList[nr] = vehicles[1];
                                    return isThere;
                                }
                                else
                                {
                                    ParkingList[nr] = vehicles[0];
                                    return isThere;
                                }
                            }
                        }
                        else
                        {
                            string[] atoms = splits.Split("!");
                            if (atoms[1] == search)
                            {
                                int nr = Array.IndexOf(ParkingList, parkingSpot);
                                ParkingList[nr] = null;
                                return isThere;
                            }
                        }

                    }
                }
            }
            return notThere;
        }
        static void MoveVehicle()
        {
            Console.Clear();
            Console.Write("4. Search for and Move vehicle \n\nPlease enter the registration number of the vehicle that you would like to search for and move: ");
            string move = Console.ReadLine().ToUpper();
            int search = CheckForVehicle(move);

            if (search > 0)
            {
                Console.WriteLine("This vehicle is parked in spot {0}", search);
            }
            else
            {
                Console.WriteLine("There is no vehicle parked with that registration number");
                Console.WriteLine("\n\n\nPress any key to return to the main menu");
                Console.ReadKey();
                Console.Clear();
                MainMenu();
            }
            Console.WriteLine("\n(If you don´t want to move the vehicle, just type in its current spot, don´t mind the error message)");
            Console.Write("\n\nWhich spot would you like to move it to?: ");

            string spotSugestion = (Console.ReadLine());
            int newSpot;
            bool noNumber = int.TryParse(spotSugestion, out newSpot);
            if (newSpot <= 100 && newSpot >= 1)
            {
                string answer = ChangeVehicleSpot(move, newSpot);
                Console.WriteLine(answer);
            }
            else
            {
                Console.WriteLine("\nSpot search is out of range or the input is invalid");
            }

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();
        }
        static int CheckForVehicle(string search)
        {
            int currentSpot = 0;
            int notThere = 0;

            foreach (string parkingSpot in ParkingList)
            {
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {

                        if (vehicles.Length == 2)
                        {
                            string[] atoms = splits.Split("!");

                            if (atoms[1] == search)
                            {
                                currentSpot = Array.IndexOf(ParkingList, parkingSpot);
                                currentSpot = currentSpot + 1;
                                return currentSpot;
                            }
                        }
                        else
                        {
                            string[] atoms = splits.Split("!");
                            if (atoms[1] == search)
                            {
                                currentSpot = Array.IndexOf(ParkingList, parkingSpot);
                                currentSpot = currentSpot + 1;
                                return currentSpot;
                            }
                        }
                    }
                }
            }
            return notThere;
        }
        static string ChangeVehicleSpot(string searchVehicle, int spotSuggestion)
        {
            int spotFinder = spotSuggestion - 1;
            string notAvailable = "\n\nThis spot is not available! Please start over";
            string available = "\n\nThe change has been made! Your vehicle is now in the new spot";
            string nada = "\n\nEither the vehicle type or registration number is incorrect, start over";
            int counter = 0;

            foreach (string parkingSpot in ParkingList)
            {
                counter++;
                if (parkingSpot != null)
                {
                    string[] vehicles = parkingSpot.Split(", ");

                    foreach (var splits in vehicles)
                    {
                        if (vehicles.Length == 2)
                        {
                            string[] atoms = splits.Split("!");

                            if (atoms[1] == searchVehicle)
                            {
                                int nr = Array.IndexOf(ParkingList, parkingSpot);
                                if (ParkingList[spotFinder] == null)
                                {
                                    if (vehicles[0].Contains(atoms[1]))
                                    {
                                        ParkingList[nr] = vehicles[1];
                                        ParkingList[spotFinder] = vehicles[0];
                                        return available;
                                    }
                                    else
                                    {
                                        ParkingList[nr] = vehicles[0];
                                        ParkingList[spotFinder] = vehicles[1];
                                        return available;
                                    }
                                }
                                else if (ParkingList[spotFinder].Contains("mc") && !ParkingList[spotFinder].Contains(", "))
                                {
                                    if (vehicles[0].Contains(atoms[1]))
                                    {
                                        ParkingList[nr] = vehicles[1];
                                        ParkingList[spotFinder] = ParkingList[spotFinder] + ", " + splits;
                                        return available;
                                    }
                                    else
                                    {
                                        ParkingList[nr] = vehicles[0];
                                        ParkingList[spotFinder] = ParkingList[spotFinder] + ", " + splits;
                                        return available;
                                    }
                                }
                                else
                                {
                                    return notAvailable;
                                }
                            }
                        }
                        else
                        {
                            string[] atoms = splits.Split("!");
                            if (atoms[1] == searchVehicle)
                            {
                                int nr = Array.IndexOf(ParkingList, parkingSpot);
                                if (ParkingList[spotFinder] == null)
                                {
                                    ParkingList[nr] = null;
                                    ParkingList[spotFinder] = vehicles[0];
                                    return available;
                                }
                                else if (ParkingList[spotFinder].Contains("mc") && atoms[0].Contains("mc"))
                                {
                                    ParkingList[spotFinder] = ParkingList[spotFinder] + ", " + splits;
                                    counter--;
                                    ParkingList[counter] = null;
                                    return available;
                                }
                                else
                                {
                                    return notAvailable;
                                }
                            }
                        }
                    }
                }
            }
            return nada;
        }
        static void OptimizeSpace()
        {
            Console.Clear();
            Console.Write("5. Optimize the mc:s in the parkinglot  \n\nWould you like to move together the singleparked motorcycles of the parking lot? ");
            string optimize = Console.ReadLine();
            int occupied = 0;
            int i;
            int spaces;
            string[] moveThese = new string[100];
            int counter = 0;

            if (optimize == "Yes" || optimize == "yes" || optimize == "y" || optimize == "YES")
            {
                for (i = 100; i > 0; i--)
                {
                    spaces = i - 1;
                    occupied = SpaceSearcher(spaces);
                    int something;

                    if (occupied == 1)
                    {
                        counter = counter + 1;

                        if (ParkingList[spaces].Contains("mc") && !ParkingList[spaces].Contains(", "))
                        {
                            string[] splitter = ParkingList[spaces].Split("!");
                            string mcReg = splitter[1];

                            for (int j = 0; j <= 99; j++)
                            {
                                if (ParkingList[j] == null)
                                {
                                    something = 1;
                                    break;
                                }
                                else if (ParkingList[j].Contains("mc") && !ParkingList[j].Contains(", "))
                                {
                                    if (ParkingList[j] != ParkingList[spaces])
                                    {
                                        ParkingList[j] = ParkingList[j] + ", " + ParkingList[spaces];
                                        ParkingList[spaces] = null;
                                        j = j + 1;
                                        moveThese[counter] = "\nMove mc " + mcReg + " to parking spot " + j;
                                        break;
                                    }
                                    else
                                    {
                                        something = 2;
                                        Console.WriteLine(something);
                                    }
                                }

                                else
                                {
                                    something = 1;
                                }
                            }
                        }

                    }
                }
                int content = 0;
                foreach (var move in moveThese)
                {
                    if (move != null)
                    {
                        Console.WriteLine(move);
                        content = content + 1;
                    }
                }
                if (content == 0)
                {
                    Console.WriteLine("\nThere is no need for optimization");
                }
            }
            else
            {
                Console.WriteLine("Ok, no changes have been made!");
            }

            Console.WriteLine("\n\n\nPress any key to return to the main menu");
            Console.ReadKey();
            Console.Clear();
            MainMenu();

            static int SpaceSearcher(int placeRequest)
            {
                int free;

                if (ParkingList[placeRequest] == null)
                {
                    free = 0;
                    return free;
                }
                else
                {
                    free = 1;
                    return free;
                }
            }
        }
    }
}
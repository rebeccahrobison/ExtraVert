using System.ComponentModel.Design;
using System.IO.Compression;
using System.Security.Authentication;
using System.Xml.Serialization;

List<Plant> plants = new List<Plant>()
{
  new Plant()
  {
    Species = "Golden Pothos",
    LightNeeds = 3,
    AskingPrice = 24.00M,
    City = "Nashville",
    ZIP = 37205,
    Sold = false,
    AvailableUntil = new DateTime(2023, 11, 17)
  },
  new Plant()
  {
    Species = "Spider Plant",
    LightNeeds = 3,
    AskingPrice = 15.75M,
    City = "New York",
    ZIP = 10001,
    Sold = true,
    AvailableUntil = new DateTime(2023, 11, 10)
  },
  new Plant()
  {
    Species = "Peace Lily",
    LightNeeds = 4,
    AskingPrice = 30.00M,
    City = "Chicago",
    ZIP = 60601,
    Sold = false,
    AvailableUntil = new DateTime(2023, 12, 17)
  },
  new Plant()
  {
    Species = "ZZ Plant",
    LightNeeds = 2,
    AskingPrice = 22.50M,
    City = "Miami",
    ZIP = 33101,
    Sold = false,
    AvailableUntil = new DateTime(2023, 12, 17)
  },
  new Plant()
  {
    Species = "Snake Plant",
    LightNeeds = 2,
    AskingPrice = 18.50M,
    City = "Los Angeles",
    ZIP = 90001,
    Sold = true,
    AvailableUntil = new DateTime(2024, 1, 3)
  }
};

Random randomNumber = new Random();
Plant randomPlant = null;
while (randomPlant == null)
{
  var randomPlantIndex = randomNumber.Next(plants.Count);
  Plant possibleRandomPlant = plants[randomPlantIndex];
  if (!possibleRandomPlant.Sold)
  {
    randomPlant = possibleRandomPlant;
  }
  
}


string greeting = @"Welcome to ExtraVert!";
Console.WriteLine(greeting);

string userChoice = null;
while (userChoice != "0")
{
  Console.WriteLine(@"Choose an option:
                    0. Exit
                    1. Display all plants
                    2. Post a plant to be adopted
                    3. Adopt a plant
                    4. Delist a plant
                    5. Display a random plant
                    6. Search for a plant by light needs
                    7. View app statistics");
  userChoice = Console.ReadLine();
  Console.Clear();
  switch (userChoice)
  {
    case "0":
      Console.WriteLine("Thanks for visiting ExtraVert! Goodbye!");
      break;
    case "1":
      ListAllPlants();
      break;
    case "2":
      PostPlant();
      break;
    case "3":
      AdoptPlant();
      break;
    case "4":
      DelistPlant();
      break;
    case "5":
      Console.WriteLine(@$"Your random plant is:
                        {PlantDetails(randomPlant)}");
      break;
    case "6":
      SearchForPlant();
      break;
    case "7":
      ViewStatistics();
      break;
    default:
      Console.WriteLine(greeting);
      break;
  }
}
  

void ListAllPlants()
{
  Console.WriteLine("Plants:");
  for (int i = 0; i < plants.Count; i++)
  {
    Console.WriteLine($"{i + 1}. {PlantDetails(plants[i])}");
      // Console.WriteLine($"{i + 1}. {plants[i].Species} in {plants[i].City} {(plants[i].Sold ? "was sold." : $"is available until {(plants[i].AvailableUntil).ToString("d")} for {plants[i].AskingPrice} dollars.")}");
  }
}

void ListUnsoldPlants()
{
  for (int i = 0; i < plants.Count; i++)
  {
    if (!plants[i].Sold && plants[i].AvailableUntil > DateTime.Now)
    {
      Console.WriteLine($"Plant Id {i + 1} {PlantDetails(plants[i])}");
      // Console.WriteLine($"Plant id {i + 1} {plants[i].Species} in {plants[i].City} {(plants[i].Sold ? "was sold" : $"is available until {(plants[i].AvailableUntil).ToString("d")}")} for {plants[i].AskingPrice} dollars.");
    }
  }
}

void PostPlant()
{
  Console.WriteLine("Please enter the details of the plant to be posted:");
  Console.WriteLine("What species is the plant?");
  string plantToPostSpecies = Console.ReadLine();
  Console.WriteLine("On a scale of 1-5, how much light does the plant need?");
  int plantToPostLightNeeds = int.Parse(Console.ReadLine());
  Console.WriteLine("What is the asking price for the plants?");
  decimal plantToPostAskingPrice = decimal.Parse(Console.ReadLine());
  Console.WriteLine("What city is the plant located in?");
  string plantToPostCity = Console.ReadLine();
  Console.WriteLine("What zip code is the plant located in?");
  int plantToPostZIP = int.Parse(Console.ReadLine());
  Console.WriteLine("What date should the post expire? Enter MM/DD/YYYY");
  DateTime plantToPostAvailableUntil = DateTime.Now;
  while (plantToPostAvailableUntil < DateTime.Now)
  {
    try 
    {
      plantToPostAvailableUntil = DateTime.Parse(Console.ReadLine());
    }
    catch (FormatException)
    {
      Console.WriteLine("Date format was invalid. Returning to main menu.");
      return;
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      Console.WriteLine("Date Entry Error");
    }
  }

  Plant plantToPost = new Plant();
  plantToPost.Species = plantToPostSpecies;
  plantToPost.LightNeeds = plantToPostLightNeeds;
  plantToPost.AskingPrice = plantToPostAskingPrice;
  plantToPost.City = plantToPostCity;
  plantToPost.ZIP = plantToPostZIP;
  plantToPost.Sold = false;
  plantToPost.AvailableUntil = plantToPostAvailableUntil;
  plants.Add(plantToPost);
  Console.WriteLine($"Your {plantToPostSpecies} has been posted for sale.");
}

void AdoptPlant()
{
  Plant chosenPlant = null;
  Console.WriteLine("Which plant would you like to adopt? Enter an id number.");
  ListUnsoldPlants();

  while (chosenPlant == null)
  {
    try{
      int response = int.Parse(Console.ReadLine());
      chosenPlant = plants[response - 1];
      plants[response - 1].Sold = true;
    }
    catch (FormatException)
    {
      Console.WriteLine("Please type only integers!");
    }
    catch (ArgumentOutOfRangeException)
    {
      Console.WriteLine("Please choose an existing item only!");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      Console.WriteLine("Error on your end!");
    }
  }

  Console.WriteLine($"You chose {chosenPlant.Species}, sold to you for {chosenPlant.AskingPrice} dollars.");
}

void DelistPlant()
{
  Plant chosenPlant = null;
  Console.WriteLine("Which plant would you like to delist?");
  ListAllPlants();

  while (chosenPlant == null)
  {
    try{
      int response = int.Parse(Console.ReadLine());
      chosenPlant = plants[response - 1];
      plants.RemoveAt(response - 1);
    }
    catch (FormatException)
    {
      Console.WriteLine("Please type only integers!");
    }
    catch (ArgumentOutOfRangeException)
    {
      Console.WriteLine("Please choose an existing item only!");
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
      Console.WriteLine("Error on your end!");
    }
  }

  Console.WriteLine($"You delisted the {chosenPlant.Species}.");
}

void SearchForPlant()
{
  Console.WriteLine("Enter the max light needs number between 1 and 5:");
  int userInput = int.Parse(Console.ReadLine());
  List<Plant> lightPlants = new List<Plant>();
  for (int i = 0; i < plants.Count; i++)
  {
    if (plants[i].LightNeeds <= userInput)
    {
      lightPlants.Add(plants[i]);
    }
  }
  Console.WriteLine("Plants within your light needs parameter:");
  foreach (Plant plant in lightPlants)
  {
    Console.WriteLine($"{lightPlants.IndexOf(plant) + 1}. {plant.Species}");
  }
}

void ViewStatistics()
{
  string lowestPricePlantName = "";
  decimal lowestPrice = decimal.MaxValue;
  int plantsAvailable = 0;
  string highestLightNeedsPlant = "";
  int highestLightNeeds = 0;
  decimal totalLightNeeds = 0;
  int plantsAdopted = 0;
  foreach (Plant plant in plants)
  {
    if (plant.AskingPrice < lowestPrice)
    {
      lowestPrice = plant.AskingPrice;
      lowestPricePlantName = plant.Species;
    }
    if (!plant.Sold && plant.AvailableUntil > DateTime.Now)
    {
      plantsAvailable++;
    }
    if (plant.LightNeeds > highestLightNeeds)
    {
      highestLightNeeds = plant.LightNeeds;
      highestLightNeedsPlant = plant.Species;
    }
    if (plant.Sold)
    {
      plantsAdopted++;
    }
    totalLightNeeds += plant.LightNeeds;
  }
  Console.WriteLine(@$"ExtraVert Statistics:
                    1. Lowest price plant: {lowestPricePlantName} (${lowestPrice})
                    2. Number of plants available: {plantsAvailable}
                    3. Plant with highest light needs: {highestLightNeedsPlant}
                    4. Average light needs: {(double)totalLightNeeds/plants.Count}
                    5. Percentage of plants adopted: {(double)plantsAdopted/plants.Count * 100}%");
}

string PlantDetails(Plant plant)
{
  string plantString = $"{plant.Species} in {plant.City} {(plant.Sold ? "was sold." : $"is available until {(plant.AvailableUntil).ToString("d")} for {plant.AskingPrice} dollars.")}";
  return plantString;
}
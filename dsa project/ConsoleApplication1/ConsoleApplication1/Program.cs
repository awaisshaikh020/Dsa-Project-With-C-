using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Car
{
    public string Name;
    public string Brand;
    public string Color;
    public int ModelYear;
    public double Mileage; 
    public string FuelType; 

    public Car(string name, string brand, string color, int modelYear, double mileage, string fuelType)
    {
        Name = name;
        Brand = brand;
        Color = color;
        ModelYear = modelYear;
        Mileage = mileage;
        FuelType = fuelType;
    }

    public Car()
    {
    }

    public void DisplayCarInfo()
    {
        Console.WriteLine("---------");
        Console.WriteLine();
        Console.WriteLine("Car Information:");
        Console.WriteLine($"Name: {Name}");
        Console.WriteLine($"Brand: {Brand}");
        Console.WriteLine($"Color: {Color}");
        Console.WriteLine($"Model Year: {ModelYear}");
        Console.WriteLine($"Mileage: {Mileage} km/l");
        Console.WriteLine($"Fuel Type: {FuelType}");
    }
}

class Program
{
    private const int MaxCars = 10;
    private static Car[] cars = new Car[MaxCars];
    private static int carCount = 0;
    private static Car[] stack = new Car[MaxCars];
    private static int stackCount = 0;

    static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("*WELCOME TO CAR-REGISTRY-SYSTEM*");
        AddSampleCars();

        while (true)
        {
            DisplayMenu();

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            if (choice == 1)
            {
                AddCarOption();
            }
            else if (choice == 2)
            {
                ShowSortedCarsOlderThanYear();
            }
            else if (choice == 3)
            {
                SearchCarByBrandBinarySearch();
            }
            else if (choice == 4)
            {
                AddCarToStack();
            }
            else if (choice == 5)
            {
                RemoveCarFromStack();
            }
            else if (choice == 6)
            {
                DisplayCarsInStack();
            }
            else if (choice == 7)
            {
                Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select from 1 to 7.");
            }
        }
    }

    private static void DisplayMenu()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Add a new car");
        Console.WriteLine("2. Show cars older than a specific year (sorted)");
        Console.WriteLine("3. Search for a car by brand (binary search)");
        Console.WriteLine("4. Add car to stack (push)");
        Console.WriteLine("5. Remove car from stack (pop)");
        Console.WriteLine("6. Display the current cars in the stack");
        Console.WriteLine("7. Exit");
        Console.Write("Enter your choice (1-7): ");
    }

    private static void AddSampleCars()
    {
        cars[carCount++] = new Car { Name = "safwan", Brand = "BMW", Color = "Red", ModelYear = 1995, Mileage = 12.5, FuelType = "Petrol" };
        cars[carCount++] = new Car { Name = "ali", Brand = "Ford", Color = "Blue", ModelYear = 2005, Mileage = 15.2, FuelType = "Diesel" };
        cars[carCount++] = new Car { Name = "raza", Brand = "Cultus", Color = "Green", ModelYear = 1990, Mileage = 11.8, FuelType = "Electric" };
    }

    private static void AddCarOption()
    {
        Console.Write("Enter owner name: ");
        string name = Console.ReadLine();

        Console.Write("Enter vehicle brand name: ");
        string brand = Console.ReadLine();

        Console.Write("Enter vehicle color name: ");
        string color = Console.ReadLine();

        Console.Write("Enter model year: ");
        int modelYear;
        if (!int.TryParse(Console.ReadLine(), out modelYear))
        {
            Console.WriteLine("Invalid input for model year. Car not added.");
            return;
        }

        Console.Write("Enter mileage: ");
        double mileage;
        if (!double.TryParse(Console.ReadLine(), out mileage))
        {
            Console.WriteLine("Invalid input for mileage. Car not added.");
            return;
        }

        Console.Write("Enter fuel type: ");
        string fuelType = Console.ReadLine();

        if (carCount < MaxCars)
        {
            cars[carCount++] = new Car { Name = name, Brand = brand, Color = color, ModelYear = modelYear, Mileage = mileage, FuelType = fuelType };
            Console.WriteLine("Car added successfully.");
        }
        else
        {
            Console.WriteLine("Maximum car limit reached.");
        }
    }

    private static void ShowSortedCarsOlderThanYear()
    {
        Array.Sort(cars, 0, carCount, new CarComparer());

        Console.Write("Enter a year to show cars older than: ");
        int year;
        if (!int.TryParse(Console.ReadLine(), out year))
        {
            Console.WriteLine("Invalid input for year.");
            return;
        }

        Console.WriteLine($"Cars older than {year} (sorted by model year):");
        foreach (var car in cars)
        {
            if (car != null && car.ModelYear < year)
            {
                car.DisplayCarInfo();
            }
        }
    }

    private static void SearchCarByBrandBinarySearch()
    {
        Array.Sort(cars, 0, carCount, new CarBrandComparer());

        Console.Write("Enter the brand to search for: ");
        string searchBrand = Console.ReadLine();

        int index = Array.BinarySearch(cars, 0, carCount, new Car { Brand = searchBrand }, new CarBrandComparer());
        if (index >= 0)
        {
            Console.WriteLine("Car Found (using binary search):");
            cars[index].DisplayCarInfo();
        }
        else
        {
            Console.WriteLine("No car found with that brand.");
        }
    }

    private static void AddCarToStack()
    {
        Console.Write("Enter car owner name to add to the stack: ");
        string carName = Console.ReadLine();

        Car foundCar = Array.Find(cars, car => car != null && car.Name == carName);
        if (foundCar != null)
        {
            if (stackCount < MaxCars)
            {
                stack[stackCount++] = foundCar;
                Console.WriteLine("Car added to the stack successfully.");
            }
            else
            {
                Console.WriteLine("Stack is full. Cannot add more cars.");
            }
        }
        else
        {
            Console.WriteLine("Car not found or invalid name. Please try again.");
        }
    }

    private static void RemoveCarFromStack()
    {
        if (stackCount > 0)
        {
            Car removedCar = stack[--stackCount];
            Console.WriteLine($"Removed car: {removedCar.Name}");
            stack[stackCount] = null;
        }
        else
        {
            Console.WriteLine("Stack is empty. No car to remove.");
        }
    }

    private static void DisplayCarsInStack()
    {
        Console.WriteLine("Cars in the stack:");
        foreach (var car in stack)
        {
            if (car != null)
            {
                car.DisplayCarInfo();
            }
        }
    }

    class CarComparer : IComparer<Car>
    {
        public int Compare(Car x, Car y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return x.ModelYear.CompareTo(y.ModelYear);
        }
    }

    class CarBrandComparer : IComparer<Car>
    {
        public int Compare(Car x, Car y)
        {
            if (x == null || y == null)
            {
                return 0;
            }

            return x.Brand.CompareTo(y.Brand);
        }
    }
}
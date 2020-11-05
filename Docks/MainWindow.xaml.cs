using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Printing;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Docks
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Bool list if a space is free. False = Avaliable
        bool[] LeftDockCheck = new bool[64];
        bool[] RightDockCheck = new bool[64];
        List<Boat> LeftDockList = new List<Boat>();
        List<Boat> RightDockList = new List<Boat>();
        int dayNumber = 1;
        int totalRejects = 0;
        int dailyRejects;
        

        public MainWindow()
        {
            InitializeComponent();
            SetUp();
        }

        void SetUp()
        {
            LeftDock.Items.Clear();
            RightDock.Items.Clear();
            LeftDock.Background = System.Windows.Media.Brushes.LightGray;
            RightDock.Background = System.Windows.Media.Brushes.LightGray;
            for (int i = 1; i <= 32; i++)
            {
                LeftDock.Items.Add(i + ":");
                LeftDock.Items.Add(i + ":");
                RightDock.Items.Add(i + ":");
                RightDock.Items.Add(i + ":");
            }
            LoadFromFile();
        }

        private void NextDayButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateDepart();

            dailyRejects = 0;

            int numBoats = 0;
            try
            {
                numBoats = int.Parse(NumberOfBoats.Text);
            }
            catch
            {
                NumberOfBoats.Text = "0";
            }
            List<Boat> boatList = GenerateBoats(numBoats);
            var newBoats = boatList.OrderByDescending(b => b.Size);
            foreach(Boat boat in newBoats)
            {
                Dock(boat);
            }

            UpdateRejectWindow();
            UpdateStatusWindow();
            SaveToFile();
        }

        #region Save and Load

        void LoadFromFile()
        {
            //Index:Name:Size:Weight:MaxSpeed:SpecialTrait:DockedDays

            try
            {
                string leftText = File.ReadAllText("LeftDock.txt");

                string[] textList = leftText.Split("\n");

                foreach(String text in textList)
                {
                    if(text == "")
                    {
                        break;
                    }
                    string[] boatInfo = text.Split(":");

                    int index = int.Parse(boatInfo[0]);
                    string name = boatInfo[1];
                    double size = double.Parse(boatInfo[2]);
                    int weight = int.Parse(boatInfo[3]);
                    int maxSpeed = int.Parse(boatInfo[4]);
                    int special = int.Parse(boatInfo[5]);
                    int dockedDays = int.Parse(boatInfo[6]);

                    Boat boat = new Boat();

                    if(size == 0.5)
                    {
                        RowBoat rowBoat = new RowBoat();
                        rowBoat.Name = name;
                        rowBoat.Weight = weight;
                        rowBoat.MaxSpeed = maxSpeed;
                        rowBoat.MaxPassenger = special;
                        rowBoat.DockedDays = dockedDays;
                        boat = rowBoat;
                    }
                    else if(size == 1)
                    {
                        MotorBoat motorBoat = new MotorBoat();
                        motorBoat.Name = name;
                        motorBoat.Weight = weight;
                        motorBoat.MaxSpeed = maxSpeed;
                        motorBoat.HorsePower = special;
                        motorBoat.DockedDays = dockedDays;
                        boat = motorBoat;
                    }
                    else if (size == 2)
                    {
                        SailBoat sailBoat = new SailBoat();
                        sailBoat.Name = name;
                        sailBoat.Weight = weight;
                        sailBoat.MaxSpeed = maxSpeed;
                        sailBoat.Length = special;
                        sailBoat.DockedDays = dockedDays;
                        boat = sailBoat;
                    }
                    else if (size == 3)
                    {
                        CatamaranBoat catBoat = new CatamaranBoat();
                        catBoat.Name = name;
                        catBoat.Weight = weight;
                        catBoat.MaxSpeed = maxSpeed;
                        catBoat.Bedspaces = special;
                        catBoat.DockedDays = dockedDays;
                        boat = catBoat;
                    }
                    else if (size == 4)
                    {
                        CargoBoat cargoBoat = new CargoBoat();
                        cargoBoat.Name = name;
                        cargoBoat.Weight = weight;
                        cargoBoat.MaxSpeed = maxSpeed;
                        cargoBoat.Cargo = special;
                        cargoBoat.DockedDays = dockedDays;
                        boat = cargoBoat;
                    }

                    UpdateLeftDockCheck(index, size);
                    LeftDockAdd(boat, index, size);
                    LeftDockList.Add(boat);
                }
            }
            catch(IOException e)
            {
                //File does not exist.
            }

            try
            {
                string rightText = File.ReadAllText("RightDock.txt");

                string[] textList = rightText.Split("\n");

                foreach (String text in textList)
                {
                    if (text == "")
                    {
                        break;
                    }
                    string[] boatInfo = text.Split(":");

                    int index = int.Parse(boatInfo[0]);
                    string name = boatInfo[1];
                    double size = double.Parse(boatInfo[2]);
                    int weight = int.Parse(boatInfo[3]);
                    int maxSpeed = int.Parse(boatInfo[4]);
                    int special = int.Parse(boatInfo[5]);
                    int dockedDays = int.Parse(boatInfo[6]);

                    Boat boat = new Boat();

                    if (size == 0.5)
                    {
                        RowBoat rowBoat = new RowBoat();
                        rowBoat.Name = name;
                        rowBoat.Weight = weight;
                        rowBoat.MaxSpeed = maxSpeed;
                        rowBoat.MaxPassenger = special;
                        rowBoat.DockedDays = dockedDays;
                        boat = rowBoat;
                    }
                    else if (size == 1)
                    {
                        MotorBoat motorBoat = new MotorBoat();
                        motorBoat.Name = name;
                        motorBoat.Weight = weight;
                        motorBoat.MaxSpeed = maxSpeed;
                        motorBoat.HorsePower = special;
                        motorBoat.DockedDays = dockedDays;
                        boat = motorBoat;
                    }
                    else if (size == 2)
                    {
                        SailBoat sailBoat = new SailBoat();
                        sailBoat.Name = name;
                        sailBoat.Weight = weight;
                        sailBoat.MaxSpeed = maxSpeed;
                        sailBoat.Length = special;
                        sailBoat.DockedDays = dockedDays;
                        boat = sailBoat;
                    }
                    else if (size == 3)
                    {
                        CatamaranBoat catBoat = new CatamaranBoat();
                        catBoat.Name = name;
                        catBoat.Weight = weight;
                        catBoat.MaxSpeed = maxSpeed;
                        catBoat.Bedspaces = special;
                        catBoat.DockedDays = dockedDays;
                        boat = catBoat;
                    }
                    else if (size == 4)
                    {
                        CargoBoat cargoBoat = new CargoBoat();
                        cargoBoat.Name = name;
                        cargoBoat.Weight = weight;
                        cargoBoat.MaxSpeed = maxSpeed;
                        cargoBoat.Cargo = special;
                        cargoBoat.DockedDays = dockedDays;
                        boat = cargoBoat;
                    }

                    UpdateRightDockCheck(index, size);
                    RightDockAdd(boat, index, size);
                    RightDockList.Add(boat);
                }
            }
            catch(IOException e)
            {
                //File does not exist.
            }
        }

        void SaveToFile()
        {
            StreamWriter lsw = new StreamWriter("LeftDock.txt",false);
            StreamWriter rsw = new StreamWriter("RightDock.txt", false);

            //Index:Name:Size:Weight:MaxSpeed:SpecialTrait:DockedDays

            foreach (Boat boat in LeftDockList)
            {
                int index = LeftDock.Items.IndexOf(boat);
                lsw.Write(index + ":" + boat.saveData() + "\n");
            }
            foreach (Boat boat in RightDockList)
            {
                int index = RightDock.Items.IndexOf(boat);
                rsw.Write(index + ":" + boat.saveData() + "\n");
            }

            lsw.Close();
            rsw.Close();
        }

        #endregion

        void UpdateDepart()
        {
            List<Boat> DepartingBoatsLeft = new List<Boat>();
            foreach (Boat boat in LeftDockList)
            {
                if (boat.DockedDays < 1)
                {
                    DepartingBoatsLeft.Add(boat);
                }
                boat.DockedDays -= 1;
            }
            foreach (Boat boat in DepartingBoatsLeft)
            {
                DepartLeft(boat);
            }

            List<Boat> DepartingBoatsRight = new List<Boat>();
            foreach (Boat boat in RightDockList)
            {
                if (boat.DockedDays < 1)
                {
                    DepartingBoatsRight.Add(boat);
                }
                boat.DockedDays -= 1;
            }
            foreach (Boat boat in DepartingBoatsRight)
            {
                DepartRight(boat);
            }
        }

        void UpdateRejectWindow()
        {
            RejectWindow.Items.Clear();
            RejectWindow.Items.Add("Day " + dayNumber);
            dayNumber++;
            RejectWindow.Items.Add("Today " + dailyRejects + " boats got rejected.");
            totalRejects += dailyRejects;
            RejectWindow.Items.Add("In total " + totalRejects + " boats have been rejected.");
            
            RejectWindow.Items.Add("");

            double spotsLeft = 64;
            foreach(Boat boat in LeftDockList)
            {
                spotsLeft -= boat.Size;
            }
            foreach (Boat boat in RightDockList)
            {
                spotsLeft -= boat.Size;
            }
            RejectWindow.Items.Add("There are " + spotsLeft + " spaces left in the harbour."); 
        }

        void UpdateStatusWindow()
        {
            StatusWindow.Items.Clear();

            var combinedList = LeftDockList.Concat(RightDockList);
            if (combinedList.Any())
            {
                //Print Num of each Boat type

                int numRowBoat = combinedList.Count(b => b.Size == 0.5);
                if (numRowBoat > 0)
                {
                    StatusWindow.Items.Add("There are " + numRowBoat + " Rowboats in the Harbour.");
                }

                int numMotorBoat = combinedList.Count(b => b.Size == 1);
                if (numMotorBoat > 0)
                {
                    StatusWindow.Items.Add("There are " + numMotorBoat + " Motorboats in the Harbour.");
                }

                int numSailBoat = combinedList.Count(b => b.Size == 2);
                if (numSailBoat > 0)
                {
                    StatusWindow.Items.Add("There are " + numSailBoat + " Sailboats in the Harbour.");
                }

                int numKataBoat = combinedList.Count(b => b.Size == 3);
                if (numKataBoat > 0)
                {
                    StatusWindow.Items.Add("There are " + numKataBoat + " Catamaran boats in the Harbour.");
                }

                int numCargoBoat = combinedList.Count(b => b.Size == 4);
                if (numCargoBoat > 0)
                {
                    StatusWindow.Items.Add("There are " + numCargoBoat + " Cargoships in the Harbour.");
                }

                //Total Weigt of all Boats


                int totalWeight = combinedList.Sum(b => b.Weight);
                StatusWindow.Items.Add("Total weight of all boats is " + totalWeight + " kg.");

                //Average Max Speed
                double averageSpeed = combinedList.Average(b => b.MaxSpeed);

                StatusWindow.Items.Add("Average speed: " + Math.Round(averageSpeed * 1.852, 2) + " km/h");


                var leftQuery = LeftDockList.OrderBy(boat => boat.Size);

                StatusWindow.Items.Add("-----Left dock:----- ");
                foreach (Boat boat in leftQuery)
                {
                    StatusWindow.Items.Add(boat.statusPrint());
                }

                var rightQuery = RightDockList.OrderBy(boat => boat.Size);

                StatusWindow.Items.Add("-----Right dock:----- ");
                foreach (Boat boat in rightQuery)
                {
                    StatusWindow.Items.Add(boat.statusPrint());
                }
            }

        }

        #region Dock
        void Dock(Boat boat)
        {
            double size = boat.Size;
            if(size == 1)
            {
                DockOne(boat);
            }
            else if(size == 2)
            {
                DockTwo(boat);
            }
            else if(size == 3)
            {
                DockThree(boat);
            }
            else if(size == 4)
            {
                DockFour(boat);
            }
            else if(size == 0.5)
            {
                DockHalf(boat);
            }       
        }

        void DockOne(Boat boat)
        {
            //Starts with Top Left -> Bot Left, then Top Right-> Bot Right
            bool docked = false;
            int i = 0;
            while (!docked)
            {
                if(i < 62)
                {
                    if (CanDockLeft(i, boat.Size))
                    {
                        UpdateLeftDockCheck(i, boat.Size);
                        LeftDockAdd(boat, i, boat.Size);
                        LeftDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else if(i <= 62 * 2)
                {
                    int j = i - 62;
                    if (CanDockRight(j, boat.Size))
                    {
                        UpdateRightDockCheck(j, boat.Size);
                        RightDockAdd(boat, j, boat.Size);
                        RightDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else
                {
                    docked = true;
                    dailyRejects++;
                    //No room!
                }

                i++;
                i++;
            }
        }

        void DockTwo(Boat boat)
        {
            //Starts with Bot Right -> Top Right, then Bot Left-> Top Left
            bool docked = false;
            int i = 60*2;
            while (!docked)
            {
                if (i > 60)
                {
                    int j = i - 60;
                    if (CanDockRight(j, boat.Size))
                    {
                        UpdateRightDockCheck(j, boat.Size);
                        RightDockAdd(boat, j, boat.Size);
                        RightDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else if (i >= 0)
                {
                    if (CanDockLeft(i, boat.Size))
                    {
                        UpdateLeftDockCheck(i, boat.Size);
                        LeftDockAdd(boat, i, boat.Size);
                        LeftDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else
                {
                    docked = true;
                    dailyRejects++;
                    //No room!
                }

                i--;
                i--;
            }
        }

        void DockThree(Boat boat)
        {
            //Starts with Top Right -> Bot Right then Top Left -> Bot Right
            bool docked = false;
            int i = 0;
            while (!docked)
            {
                if (i <= 58)
                {
                    if (CanDockRight(i, boat.Size))
                    {
                        UpdateRightDockCheck(i, boat.Size);
                        RightDockAdd(boat, i, boat.Size);
                        RightDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else if (i <= 58 * 2)
                {
                    int j = i - 58;
                    if (CanDockLeft(j, boat.Size))
                    {
                        UpdateLeftDockCheck(j, boat.Size);
                        LeftDockAdd(boat, j, boat.Size);
                        LeftDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else
                {
                    docked = true;
                    dailyRejects++;
                    //No room!
                }

                i++;
                i++;
            }
        }

        void DockFour(Boat boat)
        {
            //Starts with Bot Right -> Top Right, then Bot Left-> Top Left
            bool docked = false;
            int i = 56 * 2;
            while (!docked)
            {
                if (i > 56)
                {
                    int j = i - 56;
                    if (CanDockLeft(j, boat.Size))
                    {
                        UpdateLeftDockCheck(j, boat.Size);
                        LeftDockAdd(boat, j, boat.Size);
                        LeftDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else if (i >= 0)
                {
                    if (CanDockRight(i, boat.Size))
                    {
                        UpdateRightDockCheck(i, boat.Size);
                        RightDockAdd(boat, i, boat.Size);
                        RightDockList.Add(boat);

                        docked = true;
                        break;
                    }
                }
                else
                {
                    docked = true;
                    dailyRejects++;
                    //No room!
                }

                i--;
                i--;
            }
        }

        void DockHalf(Boat boat)
        {
            bool docked = false;
            int i = 0;
            while (!docked)
            {
                if (CanDockLeft(i, boat.Size))
                {
                    UpdateLeftDockCheck(i, boat.Size);
                    LeftDockAdd(boat, i, boat.Size);
                    LeftDockList.Add(boat);

                    docked = true;
                    break;
                }
                if (CanDockRight(i, boat.Size))
                {
                    UpdateRightDockCheck(i, boat.Size);
                    RightDockAdd(boat, i, boat.Size);
                    RightDockList.Add(boat);

                    docked = true;
                    break;
                }

                if (i >= 63)
                {
                    docked = true;
                    dailyRejects++;
                    //No room!
                }

                i++;
            }
        }

        bool CanDockLeft(int i, double size)
        {
            bool canDock = true;
            for(int j = 0; j < size*2; j++)
            {
                if(LeftDockCheck[i + j])
                {
                    canDock = false;
                    return canDock;
                }
            }
            return canDock;
        }

        bool CanDockRight(int i, double size)
        {
            bool canDock = true;
            for (int j = 0; j < size*2; j++)
            {
                if (RightDockCheck[i + j])
                {
                    canDock = false;
                    return canDock;
                }
            }
            return canDock;
        }

        void UpdateLeftDockCheck(int i, double size)
        {
            for (int j = 0; j < size*2; j++)
            {
                LeftDockCheck[i + j] = true;
            }
        }

        void UpdateRightDockCheck(int i, double size)
        {
            for (int j = 0; j < size*2; j++)
            {
                RightDockCheck[i + j] = true;
            }
        }

        void LeftDockAdd(Boat boat, int i, double size)
        {
            for(int j = 0; j < size*2; j++)
            {
                LeftDock.Items.RemoveAt(i);
            }
            LeftDock.Items.Insert(i, boat);
            for (int j = 1; j < size*2; j++)
            {
                if (j == size*2 - 1)
                {
                    LeftDock.Items.Insert(i + j, "|____|");
                }
                else
                {
                    LeftDock.Items.Insert(i + j, "|      |");
                }
            }       
        }

        void RightDockAdd(Boat boat, int i, double size)
        {
            for (int j = 0; j < size*2; j++)
            {
                RightDock.Items.RemoveAt(i);
            }
            RightDock.Items.Insert(i, boat);
            for (int j = 1; j < size*2; j++)
            {
                if (j == size * 2 - 1)
                {
                    RightDock.Items.Insert(i + j, "|____|");
                }
                else
                {
                    RightDock.Items.Insert(i + j, "|      |");
                }
            }
        }


        #endregion

        void DepartLeft(Boat boat)
        {
            LeftDockList.Remove(boat);
            int i = LeftDock.Items.IndexOf(boat);
            double size = boat.Size*2;

            for (int j = 0; j < size; j++)
            {
                LeftDock.Items.RemoveAt(i);
            }
            for (int j = 0; j < size; j++)
            {
                LeftDock.Items.Insert(i+j, 1+(i + j)/2 + ":");
                LeftDockCheck[i + j] = false;
            }
        }

        void DepartRight(Boat boat)
        {
            RightDockList.Remove(boat);
            int i = RightDock.Items.IndexOf(boat);
            double size = boat.Size*2;

            for (int j = 0; j < size; j++)
            {
                RightDock.Items.RemoveAt(i);
            }
            for (int j = 0; j < size; j++)
            {
                RightDock.Items.Insert(i+j, 1+(i + j) / 2 + ":");
                RightDockCheck[i + j] = false;
            }

        }

        List<Boat> GenerateBoats(int numBoats)
        {
            Random random = new Random();
            List<Boat> boatList = new List<Boat>();
            for(int i = 0; i < numBoats; i++)
            {
                Boat boat = new Boat();
                int rnd = random.Next(1, 6);
                if(rnd == 1)
                {
                    boat = new RowBoat();
                }
                else if (rnd == 2)
                {
                    boat = new MotorBoat();
                }
                else if (rnd == 3)
                {
                    boat = new SailBoat();
                }
                else if (rnd == 4)
                {
                    boat = new CargoBoat();
                }
                else if (rnd == 5)
                {
                    boat = new CatamaranBoat();
                }
               
                boatList.Add(boat);
            }
            return boatList;
        }
    }
    public class Boat
    {
        public string Name;
        public int Weight;
        public int MaxSpeed;
        public int DockedDays;
        public double Size;


        private static Random random = new Random();
        public static string BoatName()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, 3)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public override string ToString()
        {
            return Name;
        }

        virtual public string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + "   Max Speed: " + this.MaxSpeed;
        }

        virtual public string saveData()
        {
            return this.Name + this.Size + this.Weight + this.MaxSpeed;
        }

    }

    public class RowBoat : Boat
    {
        public int MaxPassenger;

        private static Random random = new Random();

        public RowBoat()
        {
            Name = "R-" + BoatName();
            DockedDays = 1;
            Weight = random.Next(100, 301);
            MaxSpeed = random.Next(1, 4);
            MaxPassenger = random.Next(1, 7);
            Size = 0.5;
        }

        public override string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + " kg   Max Speed: " + Math.Round(this.MaxSpeed* 1.852) + " km/h   Max Passengers: " + this.MaxPassenger;
        }

        public override string saveData()
        {
            return this.Name + ":" + this.Size + ":" + this.Weight + ":" + this.MaxSpeed + ":" + this.MaxPassenger + ":" + this.DockedDays;
        }

    }

    public class MotorBoat : Boat
    {
        public int HorsePower;

        private static Random random = new Random();

        public MotorBoat()
        {
            Name = "M-" + BoatName();
            DockedDays = 3;
            Weight = random.Next(200, 3001);
            MaxSpeed = random.Next(1, 61);
            HorsePower = random.Next(10, 1001);
            Size = 1;
        }

        public override string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + " kg   Max Speed: " + Math.Round(this.MaxSpeed * 1.852) + " km/h   Horse Powers: " + this.HorsePower;
        }

        public override string saveData()
        {
            return this.Name + ":" + this.Size + ":" + this.Weight + ":" + this.MaxSpeed + ":" + this.HorsePower + ":" + this.DockedDays;
        }
    }

    public class SailBoat : Boat
    {
        public int Length;

        private static Random random = new Random();

        public SailBoat()
        {
            Name = "S-" + BoatName();
            DockedDays = 4;
            Weight = random.Next(800, 6001);
            MaxSpeed = random.Next(1, 13);
            Length = random.Next(10, 61);
            Size = 2;
        }

        public override string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + " kg   Max Speed: " + Math.Round(this.MaxSpeed * 1.852) + " km/h   Length: " + this.Length + " m";
        }

        public override string saveData()
        {
            return this.Name + ":" + this.Size + ":" + this.Weight + ":" + this.MaxSpeed + ":" + this.Length + ":" + this.DockedDays;
        }
    }

    public class CargoBoat : Boat
    {
        public int Cargo;

        private static Random random = new Random();

        public CargoBoat()
        {
            Name = "L-" + BoatName();
            DockedDays = 6;
            Weight = random.Next(3000, 20001);
            MaxSpeed = random.Next(1, 21);
            Cargo = random.Next(0, 501);
            Size = 4;
        }

        public override string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + " kg   Max Speed: " + Math.Round(this.MaxSpeed * 1.852) + " km/h   Cargo: " + this.Cargo;
        }

        public override string saveData()
        {
            return this.Name + ":" + this.Size + ":" + this.Weight + ":" + this.MaxSpeed + ":" + this.Cargo + ":" + this.DockedDays;
        }

    }

    public class CatamaranBoat : Boat
    {
        public int Bedspaces;

        private static Random random = new Random();

        public CatamaranBoat()
        {
            Name = "K-" + BoatName();
            DockedDays = 3;
            Weight = random.Next(1200, 8001);
            MaxSpeed = random.Next(1, 13);
            Bedspaces = random.Next(1, 5);
            Size = 3;
        }

        public override string statusPrint()
        {
            return "Boat ID: " + this.Name + "   Weight: " + this.Weight + " kg   Max Speed: " + Math.Round(this.MaxSpeed * 1.852) + " km/h   Bedspaces: " + this.Bedspaces;


        }

        public override string saveData()
        {
            return this.Name + ":" + this.Size + ":" + this.Weight + ":" + this.MaxSpeed + ":" + this.Bedspaces + ":" + this.DockedDays;
        }

    }
}

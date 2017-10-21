using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ZacharyFaulk_CECS545_P2
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            //Matrix that define connections between cities
            //1 means there is a connection between city [x][y]
            int[][] cityConnections = new int[][]
            {       // 1  2  3  4  5  6  7  8  9  10 11
            new int[]{ 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 }, //1
            new int[]{ 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 0 }, //2
            new int[]{ 0, 0, 0, 1, 1, 0, 0, 0, 0, 0, 0 }, //3
            new int[]{ 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0 }, //4
            new int[]{ 0, 0, 0, 0, 0, 0, 1, 1, 0, 0, 0 }, //5
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0 }, //6
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 0 }, //7
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1 }, //8
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //9
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 }, //10
            new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, //11
            };

            //Code needed to run the Windows Form Application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string path = "11PointDFSBFS.TSP";    //File name being tested

            //Variables to keep track of execution time
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();  //Start Stopwatch
            float time1 = 0;
            string time2 = null;
            string timer = null;

            int count = 0;
            string shortPath = null;                //Stores a string of the shortest BFS path
            string shortPath2 = null;               //Stores a string of the shortest DFS path

            //Reads file to find # of lines
            //Stores (# of lines  - 7) to get the number of cities in the file
            string[] lines = File.ReadAllLines(path);
            int newLength = lines.Length - 7;

            newCity[] cityArray = new newCity[newLength];   //Array of newCity objects for BFS with size = # of cities
            newCity[] cityArray2 = new newCity[newLength];  //Array of newCity objects for DFS with size = # of cities

            //Start reading the file again line by line
            //Keep reading until a null line is found (end of file)
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path);
            while ((line = file.ReadLine()) != null)
            {
                //Ignore first 7 lines of the file
                //8th line is where the city data begins
                if (count >= 7)
                {
                    string[] data = line.Split(' ');    //Split and store the city data in data string array
                    int j = 0;

                    //Create newCity object using data pulled from data array (ID, x Coordinate, y Coordinate)
                    //Store each newCity object in newCity array
                    cityArray[count - 7] = new newCity(Int32.Parse(data[j]), float.Parse(data[j + 1]), float.Parse(data[j + 2]));
                    cityArray2[count - 7] = new newCity(Int32.Parse(data[j]), float.Parse(data[j + 1]), float.Parse(data[j + 2]));
                }
                count++;
            }
            //Close File
            file.Close();

            //Checks the cityConnections matrix to find the connections between cities
            //When a connections is found, a cityEdge object is added to its newCity object
            for (int matrixX = 0; matrixX < newLength; matrixX++)
            {
                for (int matrixY = 0; matrixY < newLength; matrixY++)
                {
                    if (cityConnections[matrixX][matrixY] == 1)
                    {
                        cityEdge tempEdge1 = new cityEdge(cityArray[matrixY], cityArray[matrixX]);
                        cityEdge tempEdge2 = new cityEdge(cityArray2[matrixY], cityArray2[matrixX]);
                        cityArray[matrixX].edgeList.Add(tempEdge1);
                        cityArray2[matrixX].edgeList.Add(tempEdge2);

                    }
                }
            }

            //Queues for the BFS and DFS functions to use            
            List<newCity> bfsQueue = new List<newCity>();
            List<newCity> dfsStack = new List<newCity>();

            //Call BFS and DFS functions
            BFS.bfs(cityArray, ref bfsQueue);
            DFS.dfs(cityArray2, ref dfsStack);

            stopWatch.Stop();   //Stop Stopwatch

            //Print time/distance/path data for BFS and DFS
            shortPath = string.Join(",", cityArray[10].cityHistory);
            shortPath2 = string.Join(",", cityArray2[10].cityHistory);
            Console.WriteLine("File = " + path);
            Console.WriteLine("The Final Shortest BFS Distance is " + cityArray[10].distance);
            Console.WriteLine("The Final Shortest DFS Distance is " + cityArray2[10].distance);
            Console.WriteLine("The Final Shortest BFS Path is " + shortPath);
            Console.WriteLine("The Final Shortest DFS Path is " + shortPath2);
            time1 = stopWatch.ElapsedMilliseconds;
            time1 = (float)TimeSpan.FromMilliseconds(time1).TotalSeconds;
            time2 = stopWatch.ElapsedMilliseconds.ToString();
            timer = time1.ToString();
            Console.WriteLine("Execution time took " + time2 + " Milliseconds, or " + timer + " Seconds");

            //Run Windows Form Application to graph the shortest path
            Application.Run(new P2_GUI(cityArray, cityArray[10].cityHistory, newLength));

            Console.ReadKey();
        }

    }

    //newCity class that creates newCity objects
    //Each object has an ID, x coordinate, y coordniate,
    //a distance float to keep track of the shortest distance
    //the city is from the starting city, a list of cityEdge objects
    //that define the connections that the city has,
    //and the history of the shortest path it takes to
    //get to the city from the starting city
    public class newCity
    {
        public int id;
        public float xCoordinate;
        public float yCoordinate;
        public float distance;
        public List<cityEdge> edgeList;
        public List<int> cityHistory;
            
        public newCity(int id, float x, float y)
        {
            this.id = id;
            this.xCoordinate = x;
            this.yCoordinate = y;

            //Set distance to one if this is the starting city object
            //Else set to MaxValue
            if (id == 1)
            {
                this.distance = 0;
            }
            else
            {
                this.distance = float.MaxValue;
            }
            
            //Create new lists for its list of cityEdges and history          
            this.edgeList = new List<cityEdge>();
            this.cityHistory = new List<int>();
         }
    }

    //Creates cityEdge object that takes in 2 cities (choosen by the
    //cityConnections maxtrix) and finds the distance of the edge
    //and adds it to the fromCity's edgeList
    public class cityEdge
    {
        public newCity toCity;
        public newCity fromCity;
        public float distance;

        public cityEdge (newCity to, newCity from)
        {
            this.toCity = to;
            this.fromCity = from;
            this.distance = distance = (float)Math.Sqrt((((toCity.xCoordinate - fromCity.xCoordinate)) * (toCity.xCoordinate - fromCity.xCoordinate)) + 
                                                         ((toCity.yCoordinate - fromCity.yCoordinate) * (toCity.yCoordinate - fromCity.yCoordinate)));
        }

    }

}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P2
{
    public class BFS
    {

        public static void bfs(newCity[] cityArray, ref List<newCity> bfsQueue)
        {
            newCity explore;    //temp newCity object used to modify cityArray

            //If the queue is empty, explore city 1
            //Else explore the city at the front of the queue
            //and remove it from the queue
            if (bfsQueue.Count == 0)
            {
                explore = cityArray[0];
                cityArray[0].cityHistory.Add(1);
            }
            else
            {
                explore = bfsQueue[0];
                bfsQueue.Remove(explore);
            }

            //Loop that explores all cities that the current city is connected to
            for (int i = 0; i < explore.edgeList.Count; i++)
            {
                //If the city being explore has an edge that goes to a city
                //that isn't in the queue, add it to the queue
                if (!bfsQueue.Contains(explore.edgeList[i].toCity))
                {
                    bfsQueue.Add(explore.edgeList[i].toCity);
                }
                
                //If the current distance to a city is larger than the new distance to the same city
                //(same city reached by different path), then update the distance and correlating path
                //to the city as the new smaller distance/path
                if (explore.edgeList[i].toCity.distance > (explore.distance + explore.edgeList[i].distance))
                {
                    explore.edgeList[i].toCity.distance = (explore.distance + explore.edgeList[i].distance);
                    explore.edgeList[i].toCity.cityHistory.Clear();
                    explore.edgeList[i].toCity.cityHistory.AddRange(explore.cityHistory);
                    explore.edgeList[i].toCity.cityHistory.Add(explore.edgeList[i].toCity.id);
                }
                
            }

            //If there are still cities left in the queue, call this function again
            //Else return to main
            if(bfsQueue.Count > 0)
            {
                bfs(cityArray, ref bfsQueue);
            }
            else
            {
                return;
            }
        }
    }
}

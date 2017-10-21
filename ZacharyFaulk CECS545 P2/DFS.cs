using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZacharyFaulk_CECS545_P2
{
    public class DFS
    {
        public static void dfs(newCity[] cityArray2, ref List<newCity> dfsStack)
        {
            newCity explore;    //temp newCity object used to modify cityArray

            //If the stack is empty, explore city 1
            //Else explore the next city in the stack
            if (dfsStack.Count == 0)
            {
                dfsStack.Add(cityArray2[0]);
                explore = cityArray2[0];
                cityArray2[0].cityHistory.Add(1);
            }
            else
            {
                explore = dfsStack[dfsStack.Count - 1];
            }

            //If the current distance to a city is larger than the new distance to the same city
            //(same city reached by different path), then update the distance and correlating path
            //to the city as the new smaller distance/path
            for (int i = 0; i < explore.edgeList.Count; i++)
            {
                if (explore.edgeList[i].toCity.distance > (explore.distance + explore.edgeList[i].distance))
                {
                    explore.edgeList[i].toCity.distance = (explore.distance + explore.edgeList[i].distance);
                    explore.edgeList[i].toCity.cityHistory.Clear();
                    explore.edgeList[i].toCity.cityHistory.AddRange(explore.cityHistory);
                    explore.edgeList[i].toCity.cityHistory.Add(explore.edgeList[i].toCity.id);
                }

                //Add the next city to be explored to the stack
                //Call the function again and remove the explored city from the stack
                dfsStack.Add(explore.edgeList[i].toCity);
                dfs(cityArray2, ref dfsStack);
                dfsStack.Remove(explore);
            }
        }
    }
}

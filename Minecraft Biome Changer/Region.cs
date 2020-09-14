using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft_Biome_Changer
{
        class Region
        {
                //region x      //region z      //index         //chunk x and chunk z
                Dictionary<int, Dictionary<int, Dictionary<int, int[]>>> values;

                public Region()
                {

                }
                
                public Dictionary<int, int[]> this[int x, int z]
                {
                        get {
                                return values[x][z];
                        }
                        set {
                                //x_values[i] = value;
                        }
                }
        }
}

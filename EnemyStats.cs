using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame
{       //PlayerStats class inherits from CharacterStats 
    class EnemyStats : CharacterStats
    { 

        ///// <summary>
        // Initiate the stats when a robot is created. These are the stats for the robot.
        ///// </summary>
        protected override void InitStats()  
        {
            base.InitStats();

            health.InitValue(200);
            shield.InitValue(300);
        }
    }
}

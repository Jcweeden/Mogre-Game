using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame
{

    class PlayerStats : CharacterStats  {       //PlayerStats class inherits from CharacterStats 

    
        protected Score score;
        ///// <summary>
        // Read only. Returns player score
        ///// </summary>
        public Score Score
        {   
            get { return score; }
        }


        ///// <summary>
        // Initiate the stats when the class is created. These are the stats for the player.
        ///// </summary>
        protected override void InitStats()  
        {
 	        base.InitStats();

            score = new Score();    //create a new Score object

            //use the InitValue method to initialize the fields score, health, shield, and lives to 0, 100, 100 and 3
            score.InitValue(0);
            health.InitValue(300);
            shield.InitValue(500);
            lives.InitValue(3);
            lives.setMax(5);
        }
    }
}

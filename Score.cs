using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame

    /// <summary>
    /// This class implements the score for the player
    /// </summary>

{   //To implement the score field you should create a new class Score
    class Score : Stat
    {   
        /// <summary>
        /// This class is used to increase the score and override the Increase method defined in Stat
        /// </summary>
        /// <param name="val">The value by which increase the score</param>
        public override void Increase(int val)
        {
            value +=val;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame
{   //PlayerController class inherits from the CharacterController class 
    class PlayerController : CharacterController
    {

        ///// <summary>
        //  sets the speed the player moves at, and takes player that the movement will be applied to
        ///// </summary> 
        public PlayerController(Character player)
        {
            //In the constructor you should initialize the field speed to 100 and the character field to the parameter player.
            speed = 100;
            character = player;
        }
        ///// <summary>
        //  Checks if input is there to move, rotate or shoot
        ///// </summary>        
        public override void Update(/*Mogre.*/FrameEvent evt)
        {
            base.Update(evt);
            MovementsControls(evt);
            MouseControls();
            ShootingControls();
        }

        ///// <summary>
        //  Checks if there is input to move the player, and if so applies the movement
        ///// </summary>
        private void MovementsControls(FrameEvent evt)  
        {
            //move is of type Vector3 and must be initialized to the zero vector (use Vector3.ZERO)
            Vector3 move = Vector3.ZERO;

            /*
             * read the bool fields in the character controller class which represents directions.
             * If the bool is true update the move field adding or subtracting the correct direction 
             * form the character.Model object
             */
            if (forward)
            {
                move += (character.Model.Forward * 2f);
            }

            if (backward)
            {
                move -= (character.Model.Forward * 2f);
            }

            if (left)
            {
                move += (character.Model.Left * 2f);
            }

            if (right)
            {
                move -= (character.Model.Left * 2f);
            }

            //update the move local variable so that it contains a normalized copy multiplied by the speed 
            move = move.NormalisedCopy*speed;

            if (accellerate == true)    //Check also the bool field accelerate 
            {
                move = move * 3f;    //and if true multiply move by two
            }

            if (move != Vector3.ZERO)   //check the move field if it is not zero (use Vector3.ZERO)
            {
                //if false call the Move method from character and pass the move field multiplied by evt.timeSinceLastFrame
                character.Move(move * evt.timeSinceLastFrame);
            }
           // Console.Out.WriteLine("x: " +move.x +"  y: "+ move.y + "  z: " +move.z);

        }           


        ///// <summary>
        // Retrieves the Model.GameNode of the character object and make it yaw about the value contained in angles.y
        ///// </summary>
        private void MouseControls()
        {
            //retrieve the Model.GameNode of the character object and make it yaw about the value contained in angles.y using the Yaw method
            //using Mogre.Math.AngleUnitsToRadians(angles.y)
            character.Model.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(angles.y));
        }

        private void ShootingControls(){}
    }
}

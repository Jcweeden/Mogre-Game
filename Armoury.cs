using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class Armoury
    {
        SceneManager mSceneMgr;

        private bool gunChanged;    //have a bool field named gunChanged with get and set property
        /// <summary>
        // Returns bool val gunChanged / sets it to passed in bool parameter.
        /// </summary>
        public bool GunChanged
        {
            get { return gunChanged;}
            set { gunChanged = value;}
        }

        private Gun activeGun;  //Gun field named activeGun with get and set property
        /// <summary>
        // Returns the active equipped gun / sets it to passed in gun parameter.
        /// </summary>
        public Gun ActiveGun
        {
            get { return activeGun; }
            set { activeGun = value; }
        }

        List<Gun> collectedGuns;    //list of gun object List<Gun> named collectedGuns
        /// <summary>
        // Read only. Returns the list of collected guns.
        /// </summary>
        public List<Gun> CollectedGuns  
        {
            get { return collectedGuns; }
        }

        /// <summary>
        // Creates a new list to store collected guns
        /// </summary>
        public Armoury(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr; 
            collectedGuns = new List<Gun>();    //The constructor should initialize the collectedGuns list (new List<Gun>();)
        }

        /// <summary>
        // Disposes of the physObj and gamenode for each gun in the armoury
        /// </summary>
        public void Dispose()
        {
            foreach (Gun g in collectedGuns)    //dispose of each gun in the collectedGuns list 
            {
                g.Dispose();
            }

            if (activeGun != null)  //and if the activeGun is not null disposes of it 
            {
                activeGun.Dispose();
            }
        }

        /// <summary>
        // Changes the active equipped gun to the gun passed in as a parameter
        /// </summary>
        public void ChangeGun(Gun gun)
        { 
            activeGun = gun;    //stores it in activeGun
            gunChanged = true;   //set the bool gunChanged to true.
        }

        /// <summary>
        // Swaps the gun for the gun in the guns list at the given index
        /// </summary>
        public void SwapGun(int index)
        {
            if (collectedGuns != null && activeGun != null) //if the collectedGuns and the activeGun are not null 
            {
                if (index < collectedGuns.Count && index >= 0)  //use the .Count property of the list to make sure that the index stays in the limits
                {
                    ChangeGun(collectedGuns[index]);    //call the ChangeGun method passing the gun in the collectedGuns in the list which has the index 
                }
            }
        }

        /// <summary>
        // Swaps the gun to the next indexed gun in the armoury
        /// </summary>
        public void SwapToNextGun()
        {
            if (collectedGuns != null && activeGun != null) //if the collectedGuns and the activeGun are not null 
            {
                //get index of ActiveGun
                int index = 0;
                for (int i = 0; i < collectedGuns.Count; i++)
                {
                    if (collectedGuns[i].Equals(ActiveGun))
                    {
                        index = i;
                        break;
                    }
                }

                if (index+1 < collectedGuns.Count && index >= 0)  //use the .Count property of the list to make sure that the index stays in the limits
                {
                    ChangeGun(collectedGuns[index+1]);    //call the ChangeGun method passing the gun in the collectedGuns in the list which has the index 
                }
                else    //else change back to first gun
                {
                    ChangeGun(collectedGuns[0]);
                }
            }
        }



        //Finally implement a public void AddGun method which takes a Gun object named gun as parameter
        /// <summary>
        // Adds the gun to the armoury, and equips it
        /// </summary>
        public void AddGun(Gun gun)
        {
            bool add = true;    //set a local bool variable named add to true

            foreach (Gun g in collectedGuns)
            {   //for each gun g in the collectedGuns list check add is true and the type of gun passing is in the collected gun list (g.GetType()==gun.GetType())
                if (add == true && g.GetType() == gun.GetType())
                {
                    g.ReloadAmmo(); //if they are both true then it calls the reloadAmmo method for g
                    ChangeGun(g);   //call the ChangeGun method passing g to it
                    add = false;    //set add to false.
                }
            }
                if (add == true)    //Once the for each loop finished check the add variable
                {
                    ChangeGun(gun); //if true then call ChangeGun method, pass gun to it 
                    collectedGuns.Add(gun); //and add the gun to the collectedGun list
                }
                else
                {
                    gun.Dispose();  //else call the Dispose method from gun
                }
            
        }
    }
}

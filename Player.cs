using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class Player : Character

    {
        float timeElapsed = 1f;
        float reloadCooldownTime = 0f;

        List<Projectile> projectiles;
        List<Projectile> projectilesToRemove;

        /// <summary>
        // Constructor. Creates a new model, controller, stats and armoury for the player. Lists to store shot projectiles are also created.
        /// </summary>
        public Player(SceneManager mSceneMgr)
        {
            model = new PlayerModel(mSceneMgr);         //model to an instance of the PlayerModel
            controller = new PlayerController(this);    //As parameter for the constructor of the PlayerController class pass  this
            stats = new PlayerStats();                  //stats to an instance of PlayerStats 
            playerArmoury = new Armoury(mSceneMgr);     //create a new playerArmoury for the player to hold collected guns

            projectiles = new List<Projectile>();           //create a list to hold newly instantiated bullets shot by the player
            projectilesToRemove = new List<Projectile>();   //list to hold projectiles to be removed
        }

        //create a new field playerArmoury with a get property and initialize the field in the player constructor. 
        public Armoury playerArmoury;
        /// <summary>
        // Read only. Returns the player armoury.
        /// </summary>
        public Armoury PlayerArmoury
        { 
            get { return playerArmoury; }
        }

        private bool gunReloading = false;
        /// <summary>
        // Read only. Returns whether the current gun is reloading.
        /// </summary>
        public bool GunReloading
        {
            get { return gunReloading; }
        }

        private PlayerStats stats;
        /// <summary>
        // Read only. Returns the player Stats.
        /// </summary>
        public PlayerStats Stats
        { 
            get { return stats; }
        }

        /// <summary>
        //The player class overrides the Update method. Update handles the changing of guns, reloading, and projectiles in the game.
        /// </summary>
        public override void Update(Mogre.FrameEvent evt)
        {
            model.Animate(evt);     //call the methods Animate from the model object
            controller.Update(evt); //in the Update method in the Player class call the Update method from the controller object

            timeElapsed += evt.timeSinceLastFrame;


            if (playerArmoury.GunChanged == true)
            {
                ((PlayerModel)model).AttachGun(playerArmoury.ActiveGun);

                playerArmoury.GunChanged = false;
            }

            if (controller.SwapGun == true && gunReloading == false) //if the player presses 'e'to swap gun and the current gun is not reloading
            {
                playerArmoury.SwapToNextGun();  //swap to the next gun

                controller.setSwapGun(false);
            }
            else
            {
                controller.setSwapGun(false);
            }

            if (playerArmoury.ActiveGun != null && gunReloading == false)    //if a gun has been picked up, and is not currently reloading
            {
                if (controller.Shoot && timeElapsed > playerArmoury.ActiveGun.ReloadTime() && playerArmoury.ActiveGun.Ammo.Value > 0)  //if the player has ammo, presses shoot, and the gun is not cooling down between shots
                    {
                        Shoot();            //shoot the gun
                        timeElapsed = 0f;   //and reset the cooldown timer of when the gun can be fired next
                    }
                
                   else if (playerArmoury.ActiveGun.Ammo.Value == 0)    //else if the gun has no ammo
                        {
                            gunReloading = true;    //set gunReloading to true so can be displayed in GUI and begins the reload timer     
                        }
            }

            if (gunReloading == true) {
               reloadCooldownTime += evt.timeSinceLastFrame;
            }

            if (reloadCooldownTime > 2.5f) 
            {
                playerArmoury.ActiveGun.ReloadAmmo();
                gunReloading = false; //reset gunReloading to false
                reloadCooldownTime = 0; //reset the reload timer
            }
           
            foreach (RaceGame.Projectile proj in projectiles)
            {
               // Console.Out.WriteLine(projectiles.Count);

                if (proj.TimeElapsed > 1)   //if ab
                {
                    //projectiles.Remove(proj);
                    projectilesToRemove.Add(proj);
                }
                else
                {
                    proj.Update(evt);
                }
            }
            

            foreach (RaceGame.Projectile proj in projectilesToRemove)
            {
                projectiles.Remove(proj);
            }
            projectilesToRemove.Clear();
        }

  

        /// <summary>
        //Override the public void method Shoot() 
        /// </summary>
        public override void Shoot () 
        {

            if (playerArmoury.ActiveGun != null)    //if the player has a gun equipped
            {
                projectiles.Add(playerArmoury.ActiveGun.Fire()); //in its body call the Fire method from the ActiveGun property of the playerArmoury.
            }
        }
    }
}

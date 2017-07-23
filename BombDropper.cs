using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class BombDropper : Gun
    {
        ModelElement Model;
        SceneNode bombDropperNode;

        float timeElapsed;

        ///// <summary>
        // Sets the max ammo value for the gun and loads the model
        ///// </summary> 
        public BombDropper(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;  //Initialize the scene manager to the parameter you just passed;
            maxAmmo = 3;                //Initialize the maxAmmo field to a certain value
            ammo = new Stat();           //Initialize the field ammo to a new Stat object
            ammo.InitValue(maxAmmo);     //Set the initial value of ammo to maxAmmo using the InitValue method of ammo
            LoadModel();                 //Call the LoadModel method
    }

        float reloadTime = 1f;
        ///// <summary>
        // Getter for reloadTime
        ///// </summary> 
        public override float ReloadTime()
        {
            return reloadTime;
        }


        ///// <summary>
        // Loads the bombdropper mesh and applies material
        ///// </summary> 
        protected override void LoadModel()
        {
 	        base.LoadModel();

            Model = new ModelElement(mSceneMgr, "BombDropperGun.mesh");
            gameNode = Model.GameNode;
            Model.GameEntity.SetMaterialName("HeartHMD");

        }

        ///// <summary>
        // If the gun has ammo spawns a new Bomb and sets its position. -1 ammo
        ///// </summary> 
        public override Projectile Fire()
        {
            if (ammo.Value >= 0) {  //check whether ammo.Value is zero
                    projectile = new Bomb(mSceneMgr, GunDirection());//if it isn't create a new projectile of the correct type for the gun 
                    int spawnDistanceFromCannon = 25;
                    projectile.SetPosition(/*-*/GunPosition()+ -spawnDistanceFromCannon * GunDirection() +10);
                    ammo.Decrease(1);   //decrease ammo by 1 using the Decrease function
                    return projectile;
            }
            else
            {
                return null;
            }
        }

        ///// <summary>
        // Check whether the ammo.Value is less than maxAmmo, and if so then reloads
        ///// </summary>                
        public override void ReloadAmmo() {
            if (ammo.Value < maxAmmo) { //if it is it increase the value to a value 
                ammo.Reset();   //if it is it increase the value 
            }
        }

    }
}

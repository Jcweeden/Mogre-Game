using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEng;
using Mogre;

namespace RaceGame
{
    class Bomb : Projectile
    {
        Entity bombEntity;
        SceneNode bombNode;
        ModelElement Model;

        float timeElapsed = 0f;
        ///// <summary>
        // Read Only. Get value for timeElapsed, used to time how the bomb has been instantiated for, and if it needs to be removed
        ///// </summary>
        public float TimeElapsed
        {
            get { return timeElapsed; }
        }

        ///// <summary>
        // Constructor. Set the damage values for the object, along with its speed. apply velocity to the cannonball
        ///// </summary>
        public Bomb(SceneManager mSceneMgr, Vector3 gundirection)
        {
            this.mSceneMgr = mSceneMgr; //initialize the scene manager field to the parameter
            healthDamage = 10;  //set the value for healthDamage
            shieldDamage = 10;  //set the value for shieldDamage
            speed = 0; //set the speed of the projectile - 0 as the bomb does not move

            Load(); //Call the Load method
            physObj.Velocity = speed * -gundirection;
        }

        /// <summary>
        // The colour material is attached to the bomb mesh. The model is repositioned again next to the player in the scene later on. The PhysObj attached here
        /// </summary>
        protected override void Load()  //In the class override the Load method
        {
            base.Load();


            //in its body initialize the gameEntity, and the gameNode for the projectile
            Model = new ModelElement(mSceneMgr, "Bomb.mesh");
            gameNode = Model.GameNode;
            mSceneMgr.RootSceneNode.AddChild(gameNode);


            gameNode.Scale(new Vector3(1.4f, 1.4f, 1.4f));    //scale down the size of the projectile models

            gameNode.SetPosition(200, 200, 20);

            physObj = new PhysObj(3f, "Bomb", 0.1f, 0.7f, 25f);
            physObj.SceneNode = gameNode;   //set its sceneNode property to gameNode 
            physObj.Position = gameNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));   //add a weight force
            physObj.AddForceToList(new FrictionForce(physObj));


            //physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);
        }

        /// <summary>
        // Disposes of the physObj and gamenode
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Physics.RemovePhysObj(physObj);
            gameNode.DetachAllObjects();
            gameNode.Dispose();
        }


        /// <summary>
        // Checks for collisions, and disposes of the bomb if it exists for 20 seconds
        /// </summary>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);

            timeElapsed += evt.timeSinceLastFrame;
            if (timeElapsed > 20f)
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }        
        }
    }
}

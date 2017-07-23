using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class Cannonball : Projectile   //inherit from the Projectile class
    {

        Entity cannonballEntity;
        SceneNode cannonballNode;
        ModelElement Model;

        float timeElapsed = 0f;
        ///// <summary>
        // Read Only. Get value for timeElapsed, used to time how the cannonball has been instantiated for, and if it needs to be removed
        ///// </summary> 
        public float TimeElapsed
        {
            get { return timeElapsed; }
        }

        ///// <summary>
        // Constructor. Set the damage values for the object, along with its speed. apply velocity to the cannonball
        ///// </summary>
        public Cannonball(SceneManager mSceneMgr, Vector3 gundirection)
        {
            this.mSceneMgr = mSceneMgr; //initialize the scene manager field to the parameter
            healthDamage = 25;  //set the value for healthDamage
            shieldDamage = 50;  //set the value for shieldDamage
            speed = 200; //set the speed of the projectile

            Load(); //Call the Load method
            physObj.Velocity = speed * gundirection;
           // Console.Out.WriteLine("initialDirection: " + initialDirection);

        }


        /// <summary>
        // The colour material is attached to the bomb mesh. The model is repositioned again next to the player in the scene later on. The PhysObj attached here
        /// </summary>
        protected override void Load()  //In the class override the Load method
        {
            base.Load();


            //in its body initialize the gameEntity, and the gameNode for the projectile
            Model = new ModelElement(mSceneMgr, "Sphere.mesh");
            gameNode = Model.GameNode;
            mSceneMgr.RootSceneNode.AddChild(gameNode);

            Model.GameEntity.SetMaterialName("red");
            
            gameNode.Scale(new Vector3(0.5f, 0.5f, 0.5f));    //scale down the size of the projectile models

            gameNode.SetPosition(200, 200, 20);

            physObj = new PhysObj(5f, "Cannonball", 0.1f, 0.7f, 1.5f);
            physObj.SceneNode = gameNode;   //set its sceneNode property to gameNode 
            physObj.Position = gameNode.Position;
            //physObj.AddForceToList(new WeightForce(physObj.InvMass));   //add a weight force


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
        // Checks for collisions, and disposes of the cannonball if it hits the boundary edges of the arena. Disposes if the cannonball exists for longer than a second
        /// </summary>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);

            if (gameNode.Position.x > 493 || gameNode.Position.x < -493 || gameNode.Position.z > 493 || gameNode.Position.z < -493)
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }

            timeElapsed += evt.timeSinceLastFrame;
            if (timeElapsed > 1f)
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }

            if(remove != true) {
            if (IsCollidingWith("Robot"))
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                physObj = null;
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }
            }
        }

        /// <summary>
        /// Returns true if the cannonball is colliding with the object passed in
        /// </summary>
        public bool IsCollidingWith(string collider)
        {
            bool isColliding = false;
            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == collider || c.collidingObj.ID == "Robot")
                {
                    isColliding = true;
                    physObj.CollisionList.Remove(c);
                    break;
                }
            }
            return isColliding;
        }

    }
}

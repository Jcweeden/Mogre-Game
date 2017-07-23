using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEng;
using Mogre;

namespace RaceGame
{
    class EnemyProjectile : Projectile
    {

        ModelElement Model;
        Vector3 spawnPosition;
        Vector3 gunDirection;

        float timeElapsed = 0f;
        ///// <summary>
        // Read Only. Get value for timeElapsed, used to time how the projectile has been instantiated for, and if it needs to be removed
        ///// </summary> 
        public float TimeElapsed
        {
            get { return timeElapsed; }

        }

        ///// <summary>
        // Constructor. Set the damage values for the object, along with its speed. apply velocity to the cannonball
        ///// </summary>
        public EnemyProjectile(SceneManager mSceneMgr, Vector3 gundirection, Vector3 position)
        {
            this.mSceneMgr = mSceneMgr; //initialize the scene manager field to the parameter
            healthDamage = 10;  //set the value for healthDamage
            shieldDamage = 10;  //set the value for shieldDamage
            speed = 120; //set the speed of the projectile

            gunDirection = gundirection;
            spawnPosition = position;

            Load(); //Call the Load method
            physObj.Velocity = speed * gundirection;
           // Console.Out.WriteLine("initialDirection: " + initialDirection);

        }


        /// <summary>
        // The colour material is attached to the projectile mesh. The model is repositioned again next to the robot in the scene later on. The PhysObj attached here
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

            Vector3 spawn = spawnPosition + 50 * gunDirection;  //setPosition only accepts x,y,z - not a vector
            gameNode.SetPosition(spawn.x, spawn.y, spawn.z);

            physObj = new PhysObj(5f, "EnemyProjectile", 0.1f, 0.7f, 1.5f);
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
            Physics.RemovePhysObj(physObj);
            gameNode.DetachAllObjects();
            gameNode.Dispose();
            remove = true;
        }

        /// <summary>
        // Checks for collisions. Disposes if the projectile exists for longer than a second
        /// </summary>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);

            timeElapsed += evt.timeSinceLastFrame;
            if (timeElapsed > 0.6f)
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }

            if (IsCollidingWith("Robot"))
            {
                Dispose();
                Physics.RemovePhysObj(physObj);
                gameNode.DetachAllObjects();
                gameNode.Dispose();
                remove = true;
            }
        }

        /// <summary>
        /// Returns true if the projectile is colliding with the object passed in
        /// </summary>
        public bool IsCollidingWith(string collider)
        {
            bool isColliding = false;

            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == collider || c.collidingObj.ID == collider)
                {
                    isColliding = true;

                    physObj.CollisionList.Remove(c);
                    break;
                }
            }
            return isColliding;
        }

        /// <summary>
        /// Checks if the projectile is at the edge of the arena boundary and disposes it if so.
        /// </summary>
        public void checkDispose()
        {
            if (gameNode.Position.x > 493 || gameNode.Position.x < -493 || gameNode.Position.z > 493 || gameNode.Position.z < -493)
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

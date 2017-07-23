using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class CollectableGun : Collectable
    {

        bool toRemove = false;


        Gun gun;
        ///// <summary>
        ///// Read only. Returns the gun associated with the collectable.
        ///// </summary>
        public Gun Gun
        {
            get { return gun; }
        }

        Armoury playerArmoury;
        ///// <summary>
        ///// Write only. Allows a playerArmoury to be attached to the gun.
        ///// </summary>
        public Armoury PlayerArmoury
        {
            set { playerArmoury = value; }
        }


        //Initialize the mSceneMgr, the gun and the playerArmoury fields to the values passed as parameters
        public CollectableGun(SceneManager mSceneMgr, Gun g, Armoury pArmoury)
        {
            // Initialize here the mSceneMgr, the gun and the playerArmoury fields to the values passed as parameters
            this.mSceneMgr = mSceneMgr;
            this.gun = g;
            this.playerArmoury = pArmoury;

            gameNode = mSceneMgr.CreateSceneNode();         //Initialize the gameNode here
            gameNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));

            gameNode.AddChild(gun.GameNode);                //add as its child the gameNode contained in the Gun object
            mSceneMgr.RootSceneNode.AddChild(gameNode);     //Finally attach the gameNode to the sceneGraph.

            //initialize the physics object and add a weight and friction force to it 
            physObj = new PhysObj(14f, "CollectableGun", 0.1f, 0.3f, 15f);
            physObj.SceneNode = gameNode;
            physObj.Position = gameNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);
        }

        ///// <summary>
        ///// Update checks for collisions and rotates the collectable gun on the spot using Animate(evt)
        ///// </summary>
        public override void Update(FrameEvent evt)
        {
            Animate(evt);

            if (IsCollidingWith("Player"))  //collision detection with the player

            {
                toRemove = true;

                (gun.GameNode.Parent).RemoveChild(gun.GameNode.Name);   //detach the gun model from the current node 

                playerArmoury.AddGun(gun);  //add it to the player sub-scene-graph

                Physics.RemovePhysObj(PhysObj);
                Dispose();  //dispose of the pickup as it has been collected
            }
        }

        ///// <summary>
        ///// Rotates the collectable gun on the spot
        ///// </summary>
        public override void Animate(FrameEvent evt)
        {
            gameNode.Rotate(new Quaternion(Mogre.Math.AngleUnitsToRadians(evt.timeSinceLastFrame * 20), Vector3.UNIT_Y));
        }

        /// <summary>
        /// Returns true if the collectable gun is colliding with the object passed in
        /// </summary>
        private bool IsCollidingWith(string objName)
        {
            bool isColliding = false;

            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == objName || c.collidingObj.ID == objName)
                {
                    isColliding = true;
                    break;
                }
            }
            return isColliding;
        }

        ///// <summary>
        ///// Read only. This property gets whether the collectableGun should be removed from the game
        ///// </summary>
        public bool RemoveMe
        {
            get { return toRemove; }
        }
    }
}

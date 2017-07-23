using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class Gem : Collectable
    {
        public bool removeMe;
        protected Stat score;
        protected int increase;
        ModelElement gemModel;


        protected Gem(SceneManager mSceneMgr, Stat score)
        {
            this.mSceneMgr = mSceneMgr;
            this.score = score;
            removeMe = false;
        }

        ///// <summary>
        // Creates the modelElement and gamenode for the gem
        ///// </summary>
        protected virtual void LoadModel()
        {
            gemModel = new ModelElement(mSceneMgr);

            GameNode = mSceneMgr.CreateSceneNode();
        }

        /// <summary>
        /// Returns true if the gem is colliding with the object passed in
        /// </summary>
        public bool IsCollidingWith(string objName)
        {
            bool isColliding = false;
            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == objName)
                {
                    isColliding = true;

                    break;
                }
            }
            return isColliding;
        }

        /// <summary>
        // Update checks whether the gem is colliding with a player
        /// </summary>
        public override void Update(FrameEvent evt)
        {
            Animate(evt);
            if (IsCollidingWith("Player"))
            {
                removeMe = true;
            }           
            base.Update(evt);
        }

        ///// <summary>
        ///// Rotates the gem on the spot.
        ///// </summary>
        public override void Animate(FrameEvent evt)
        {
            gameNode.Yaw(Mogre.Math.AngleUnitsToRadians(20) * evt.timeSinceLastFrame);
        }


        ///// <summary>
        ///// Read only. This property gets whether the bomb should be removed from the game
        ///// </summary>
        public bool RemoveMe
        {
            get { return removeMe; }
        }

        ///// <summary>
        ///// Read only. This property gets the int to increase a stat by upon collecting a pick up
        ///// </summary>
        public int Increase
        {
            get { return increase; }
        }
    

    }
}

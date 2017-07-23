using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PhysicsEng;
using Mogre;

namespace RaceGame
{
    abstract class PowerUp : Collectable
    {
        ModelElement powerUpModel;
        public ModelElement Model
        {
            get { return powerUpModel; }
        }

        public bool removeMe = false;
        public bool RemoveMe
        {
            get { return removeMe; }
        }

        public string pickUpType;
        public string PickUpType
        {
            get { return pickUpType; }
            set { pickUpType = value; }
        }

        protected Stat stat;
        public Stat Stat
        {
            set { stat = value; }
        }

        protected PowerUp(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            LoadModel();



        }

        protected int increase;
        public int Increase
        {
            get { return increase; }
        }

        protected virtual void LoadModel() 
        {
            removeMe = false;
            powerUpModel = new ModelElement(mSceneMgr);

            // The link with to physics engine goes here
            GameNode = mSceneMgr.CreateSceneNode();
        
        }

        public override void Update(FrameEvent evt)
        {
            Animate(evt);

            if (IsCollidingWith("Player"))
            {
                Console.Out.WriteLine("bopp");

                removeMe = true;
            }           

        }


        public bool IsCollidingWith(string objName)
        {
            bool isColliding = false;
            // Console.Out.WriteLine("Red Gem: " + RemoveMe);

            foreach (Contacts c in physObj.CollisionList)
            {
                // Console.Out.WriteLine(c.colliderObj.ID);

                if (c.colliderObj.ID == objName || c.collidingObj.ID == objName)
                {
                    isColliding = true;

                    break;
                }
            }
            return isColliding;
        }

        public override void Animate(FrameEvent evt)
        {
            Model.GameNode.Yaw(Mogre.Math.AngleUnitsToRadians(20) * evt.timeSinceLastFrame);
        }




    }
}

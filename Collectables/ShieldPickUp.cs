using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class ShieldPickUp : PowerUp
    {
        Entity shieldPickUpEntity;
        SceneNode shieldPickUpNode;

        /// <summary>
        // Constructor. Set the pickup type, and value for how much it will increment that stat.
        /// </summary>    
        public ShieldPickUp(SceneManager mSceneMgr)
            :base(mSceneMgr)    //call the base constructor using :base(mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

            Stat = new Stat();
            increase = 500;

            PickUpType = "shield";

        }

        /// <summary>
        // The material is attached to the powerup mesh. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModel() //The class should override the LoadModel method from PowerUp
        {
            //load the geometry for the power up and the scene graph nodes
            base.LoadModel();
            shieldPickUpEntity = mSceneMgr.CreateEntity("shield.mesh");
            //timePickUpEntity.SetMaterialName("HeartHMD");
            shieldPickUpEntity.SetMaterialName("shieldPickup");


            shieldPickUpNode = mSceneMgr.CreateSceneNode();
            shieldPickUpNode.Scale(new Vector3(0.1f, 0.1f, 0.1f));
            shieldPickUpNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));

            shieldPickUpNode.Position -= new Vector3(0f, -8f, 0f);

            shieldPickUpNode.AttachObject(shieldPickUpEntity);


            mSceneMgr.RootSceneNode.AddChild(shieldPickUpNode);



            shieldPickUpEntity.GetMesh().BuildEdgeList();

            physObj = new PhysObj(9f, "ShieldPickUp", 0.1f, 0.3f, 20f);
            physObj.SceneNode = shieldPickUpNode;
            physObj.Position = shieldPickUpNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);

        }

        /// <summary>
        // Disposes of the physObj and gamenode
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Physics.RemovePhysObj(PhysObj);

            physObj = null;
            shieldPickUpNode.DetachAllObjects();
            shieldPickUpNode.Dispose();
        }


    }
}

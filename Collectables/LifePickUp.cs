using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class LifePickUp : PowerUp    //The class should inherit from the PowerUp class.
    {

        Entity lifePickUpEntity;
        SceneNode lifePickUpNode;

        /// <summary>
        // Constructor. Set the pickup type, and value for how much it will increment that stat.
        /// </summary>    
        public LifePickUp(SceneManager mSceneMgr)
            :base(mSceneMgr)    //call the base constructor using :base(mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

            Stat = new Stat();
            increase = 1;

            PickUpType = "life";
        }
        /// <summary>
        // The material is attached to the powerup mesh. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModel() //The class should override the LoadModel method from PowerUp
        {
            //load the geometry for the power up and the scene graph nodes
            base.LoadModel();
            lifePickUpEntity = mSceneMgr.CreateEntity("Heart.mesh");
            lifePickUpEntity.SetMaterialName("HeartHMD");


            lifePickUpNode = mSceneMgr.CreateSceneNode();
            lifePickUpNode.Scale(new Vector3(5f, 5f, 5f));
            lifePickUpNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));

            lifePickUpNode.Position += 3 * Vector3.UNIT_Y ;
            lifePickUpNode.Position -= new Vector3(0f, -8f, 0f);
            //Model.GameNode.Position -= (3 * Vector3.UNIT_Y);
            //Model.GameNode.Position -= new Vector3(0f, -80f, 0f);

            lifePickUpNode.AttachObject(lifePickUpEntity);


            mSceneMgr.RootSceneNode.AddChild(lifePickUpNode);



            lifePickUpEntity.GetMesh().BuildEdgeList();

            physObj = new PhysObj(9f, "LifePickUp", 0.1f, 0.3f, 20f);
            physObj.SceneNode = lifePickUpNode;
            physObj.Position = lifePickUpNode.Position;
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
            lifePickUpNode.DetachAllObjects();
            lifePickUpNode.Dispose();
        }

    }
}

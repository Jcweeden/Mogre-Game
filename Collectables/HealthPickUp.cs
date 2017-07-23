using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class HealthPickUp : PowerUp
    {
        Entity healthPickUpEntity;
        SceneNode healthPickUpNode;

        /// <summary>
        // Constructor. Set the pickup type, and value for how much it will increment that stat.
        /// </summary>    
        public HealthPickUp(SceneManager mSceneMgr)
            :base(mSceneMgr)    //call the base constructor using :base(mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

            increase = 300;


            PickUpType = "health";
        }

        /// <summary>
        // The material is attached to the powerup mesh. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModel() //The class should override the LoadModel method from PowerUp
        {
            //load the geometry for the power up and the scene graph nodes
            base.LoadModel();
            healthPickUpEntity = mSceneMgr.CreateEntity("Main.mesh");
            healthPickUpEntity.SetMaterialName("HeartHMD");


            healthPickUpNode = mSceneMgr.CreateSceneNode();
            healthPickUpNode.Scale(new Vector3(0.7f, 0.7f, 0.7f));
            healthPickUpNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));

            healthPickUpNode.AttachObject(healthPickUpEntity);


            mSceneMgr.RootSceneNode.AddChild(healthPickUpNode);



            healthPickUpEntity.GetMesh().BuildEdgeList();

            physObj = new PhysObj(6f, "HealthPickUp", 0.1f, 0.3f, 20f);
            physObj.SceneNode = healthPickUpNode;
            physObj.Position = healthPickUpNode.Position;
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
            healthPickUpNode.DetachAllObjects();
            healthPickUpNode.Dispose();
        }

    }
}

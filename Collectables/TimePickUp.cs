using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class TimePickUp : PowerUp
    {
        
        Entity timePickUpEntity;
        SceneNode timePickUpNode;

        /// <summary>
        // Constructor. Set the pickup type.
        /// </summary>        
        public TimePickUp(SceneManager mSceneMgr)
            :base(mSceneMgr)    //call the base constructor using :base(mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;

            PickUpType = "time";
        }

        /// <summary>
        // The material is attached to the powerup mesh. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModel() //The class should override the LoadModel method from PowerUp
        {
            //load the geometry for the power up and the scene graph nodes
            base.LoadModel();
            timePickUpEntity = mSceneMgr.CreateEntity("Watch.mesh");
            timePickUpEntity.SetMaterialName("clockPickup");


            timePickUpNode = mSceneMgr.CreateSceneNode();
            timePickUpNode.Scale(new Vector3(0.3f, 0.3f, 0.3f));
            timePickUpNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));

            timePickUpNode.AttachObject(timePickUpEntity);


            mSceneMgr.RootSceneNode.AddChild(timePickUpNode);



            timePickUpEntity.GetMesh().BuildEdgeList();

            physObj = new PhysObj(6f, "TimePickUp", 0.1f, 0.3f, 20f);
            physObj.SceneNode = timePickUpNode;
            physObj.Position = timePickUpNode.Position;
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
            timePickUpNode.DetachAllObjects();
            timePickUpNode.Dispose();
        }

    }
}

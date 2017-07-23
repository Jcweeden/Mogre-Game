using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class RedGem : Gem  //The class should inherit from the Gem class.
    {

        Entity gemEntity;
        SceneNode gemNode;


        /// <summary>
        // In the constructor the gem is attached to the scene, loaded and its score value determined
        /// </summary>
        public RedGem(SceneManager mSceneMgr, Stat score)
            : base(mSceneMgr, score) //. The class constructor should call the base constructor using :base(mSceneMgr, score)
        {
            //In the body of the constructor you should initialize this.mSceneMgr field to the parameter just passed in 
            this.mSceneMgr = mSceneMgr;

            increase = 10;  //initialize the increase field inherited from Gem 
            LoadModel();    //and call the LoadModel method

        }

        /// <summary>
        // The colour material is attached to the gem mesh. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModel() //The class should override the LoadModel method from Gem
        {
            base.LoadModel();   //you first call the LoadModel from the base class, base.LoadModel()
            gemEntity = mSceneMgr.CreateEntity("Gem.mesh");
            gemEntity.SetMaterialName("red");
             
      
            gemNode = mSceneMgr.CreateSceneNode();
            gemNode.Scale(new Vector3(2f, 2f, 2f));
            gemNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 150, Mogre.Math.RangeRandom(-450, 450));
            gemNode.AttachObject(gemEntity);
            mSceneMgr.RootSceneNode.AddChild(gemNode);

            gemEntity.GetMesh().BuildEdgeList();

            physObj = new PhysObj(11f, "Gem", 1f, 0.3f, 1f);
            physObj.SceneNode = gemNode;
            physObj.Position = gemNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);
        }

        public override void Update(FrameEvent evt)
        {
            base.Update(evt);

        }

        /// <summary>
        // Disposes of the physObj and gamenode
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            Physics.RemovePhysObj(PhysObj);
            physObj = null;
            gemNode.DetachAllObjects();
            gemNode.Dispose();
        }
    }
}

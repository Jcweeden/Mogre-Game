using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class PlayerModel : CharacterModel  //The PlayerModel class inherits from the CharacterModel class
    { 
        //geometry as listed in Lab1 Part 2: Loading Geometries
        private ModelElement Model;
        private ModelElement PowerCells;
        private ModelElement Hull;
        private ModelElement Sphere;

        private ModelElement HullGroupNode;
        private ModelElement WheelGroupNode;
        private ModelElement GunGroupNode;

        PhysObj physObj;
        public PhysObj PhysObj
        {
            get { return physObj; }
        }
        SceneNode controlNode;

        //The PlayerModel class should have a constructor which take as parameter of type SceneManager named mSceneMgr.
        public PlayerModel(SceneManager mSceneMgr)
        {

            //In the body of the constructor you should initialize this.mSceneMgr field to the parameter just passed in 
            this.mSceneMgr = mSceneMgr;

            //Call the methods LoadModelElements and AssembleModelElements.
            LoadModelElements();
            AssembleModel();
        }

        /// <summary>
        // After defining the ModelElements they are initialized in the LoadModelElements method. The model is positioned and PhysObj attached here
        /// </summary>
        protected override void LoadModelElements()
        {
            Model = new ModelElement(mSceneMgr);

            HullGroupNode = new ModelElement(mSceneMgr);
            WheelGroupNode = new ModelElement(mSceneMgr);
            GunGroupNode = new ModelElement(mSceneMgr);
            PowerCells = new ModelElement(mSceneMgr, "PowerCells.mesh");
            Hull = new ModelElement(mSceneMgr, "Main.mesh");

            Sphere = new ModelElement(mSceneMgr, "Sphere.mesh");

            float radius = 9;

            //physics for player
            controlNode = mSceneMgr.CreateSceneNode();
            controlNode.Position += radius * Vector3.UNIT_Y ;
            Model.GameNode.Position -= (radius * Vector3.UNIT_Y);
            Model.GameNode.Position -= new Vector3(0f, -8f, 0f);    //set model to be above the ground

           //controlNode.SetPosition(0, 140, 0);


            physObj = new PhysObj(radius, "Player", 1f, 0.3f, 0.7f);
            physObj.SceneNode = controlNode;
            physObj.Position = controlNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);

        }

        /// <summary>
        // Attach the objects of the player model and add to the scenemanager
        /// </summary>
        protected override void AssembleModel()
        {
            HullGroupNode.AddChild(PowerCells.GameNode);
            HullGroupNode.AddChild(Hull.GameNode);


            WheelGroupNode.AddChild(Sphere.GameNode);


            HullGroupNode.AddChild(WheelGroupNode.GameNode);
            HullGroupNode.AddChild(GunGroupNode.GameNode);

            Model.AddChild(HullGroupNode.GameNode);

            controlNode.AddChild(Model.GameNode);

            Hull.GameEntity.GetMesh().BuildEdgeList();  //add shadows

            //you should initialize the gameNode of the PlayerModel class to the gameNode in the ModelElement model object, i.e. gameNode=model.GameNode;
            gameNode = controlNode;

            //The assembling of the scene manager should happen in the AssembleModelElements 
            mSceneMgr.RootSceneNode.AddChild(gameNode);

        }

        //You should also override the DisposeModel method form the CharacterModel class

        /// <summary>
        // Calls the Dispose method from each one of the ModelElements created in this class
        /// </summary>
        public override void DisposeModel()
        {

        Model.Dispose();
        PowerCells.Dispose();
        Hull.Dispose();
        Sphere.Dispose();

        HullGroupNode.Dispose();
        WheelGroupNode.Dispose();
        GunGroupNode.Dispose();
        }

        
        /// <summary>
        /// Attaches the gun mesh to the player model
        /// </summary>
        public void AttachGun(Gun gun)
        {
            if (GunGroupNode.GameNode.NumChildren() != 0)   //in the body checks whether the gunGroupNode has any child 
            {
                GunGroupNode.GameNode.RemoveAllChildren();     //if it has children call the RemoveAllChildren() 
            }
            GunGroupNode.AddChild(gun.GameNode);   //add the GameNode of the gun you passed as parameter as child of the GameNode of the gunGroupNode.
        }


        /// <summary>
        /// Returns true if the player is colliding with the object passed in
        /// </summary>
        public bool IsCollidingWith(string collider)
        {
            bool isColliding = false;

            foreach (Contacts c in physObj.CollisionList)
            {
                if (c.colliderObj.ID == collider || c.collidingObj.ID == collider)
                {
                    isColliding = true;
                    break;
                }
            }
            return isColliding;
        }
    }
}

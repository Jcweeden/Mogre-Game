using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;


namespace RaceGame
{
    /// <summary>
    /// This class implements the ground of the environment
    /// </summary>
    class Ground
    {
        SceneManager mSceneMgr;

        Entity groundEntity;
        SceneNode groundNode;

        int groundWidth = 10;
        int groundHeight = 10;
        int groundXSegs = 1;
        int groundZSegs = 1;
        int uTiles = 10;
        int vTiles = 10;


        //the implementation of the GroundPlane method - create two local variables
        Plane plane;            //One of type Plane plane 
        MeshPtr groundMeshPtr;  //the other of type MeshPtr groundMeshPtr


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference of the scene manager</param>
        public Ground(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            groundWidth = 1000;
            groundHeight = 1000;
            CreateGround();
        }

        /// <summary>
        /// This method initializes the ground mesh and node
        /// </summary>
        private void CreateGround()
        {
            GroundPlane();
            groundNode = mSceneMgr.CreateSceneNode();
            groundNode.AttachObject(groundEntity);
            mSceneMgr.RootSceneNode.AddChild(groundNode);
        }


        /// <summary>
        /// Getter for plane object
        /// </summary>
        public Plane GetPlane
        {
            get { return plane; }

        }

        /// <summary>
        /// This method generate a plane in an Entity which will be used as ground plane
        /// </summary>
        private void GroundPlane()
        {
            //plane should be initialized to a plane object in the same fashion of the plane used for the sky plane
            plane = new Plane(Vector3.UNIT_Y, 0);  //set to height zero

            //groundMeshPtr should be initialized using the MeshManager.Singleton.CreatePlane 
            groundMeshPtr = MeshManager.Singleton.CreatePlane("ground", ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, plane, 
                                        groundWidth, groundHeight, groundXSegs, groundZSegs, true, 1, uTiles, vTiles, Vector3.UNIT_Z);


            groundEntity = mSceneMgr.CreateEntity("ground");
            groundEntity.SetMaterialName("Ground");
        }

        /// <summary>
        /// This method disposes of the scene node and enitity
        /// </summary>
        public void Dispose()
        {
            groundNode.DetachAllObjects();
            groundNode.Parent.RemoveChild(groundNode);
            groundNode.Dispose();
            groundEntity.Dispose();
        }
    }
}
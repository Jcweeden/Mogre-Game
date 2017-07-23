using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class Wall
    {

        SceneManager mSceneMgr;             // This field will contain a reference of the scene manages

        ManualObject manualObj;
        Entity boundaryEntity;
        SceneNode boundaryNode;

        /// <summary>
        /// Constructor. Simply calls the load method.
        /// </summary>
        public Wall(SceneManager mSceneMgr)
        {
            this.mSceneMgr = mSceneMgr;
            Load();                               
        }

        /// <summary>
        /// Calls to create the walls.
        /// </summary>
        private void Load()
        {
            CreateBoundaries();
        }


        /// <summary>
        /// Places invisible walls at the edge of each side of the arena so that nothing can pass through, but rather bounces off. Calls to create the walls first
        /// </summary>
        private void CreateBoundaries()
        {
            BoundaryCubes();
            boundaryNode = mSceneMgr.CreateSceneNode();
            boundaryNode.AttachObject(boundaryEntity);
            mSceneMgr.RootSceneNode.AddChild(boundaryNode);

            Plane pNorth = new Plane(Vector3.UNIT_Z, -500);  //set to height zero
            Physics.AddBoundary(pNorth);

            Plane pSouth = new Plane(-Vector3.UNIT_Z, -500);  //set to height zero
            Physics.AddBoundary(pSouth);

            Plane pEast = new Plane(Vector3.UNIT_X, -500);  //set to height zero
            Physics.AddBoundary(pEast);

            Plane pWest = new Plane(-Vector3.UNIT_X, -500);  //set to height zero
            Physics.AddBoundary(pWest);

        }

        /// <summary>
        /// The vertexs for the walls around the edge of the arena are defined, and placed into the buffer to create the walls.
        /// </summary>
        private void BoundaryCubes()
        {
            manualObj = mSceneMgr.CreateManualObject("a");
            manualObj.Begin("void", RenderOperation.OperationTypes.OT_TRIANGLE_LIST);    // Starts filling the manual object as a triangle list


            // ---- Vertex Buffer ----
            manualObj.Position(new Vector3(500, 45, -500));//SW        // --- Vertex 0 ---
            manualObj.Position(new Vector3(-500, 45, -500));//SE       // --- Vertex 1 ---
            manualObj.Position(new Vector3(500, 0, -500));//SW         // --- Vertex 2 ---
            manualObj.Position(new Vector3(-500, 0, -500));//SE        // --- Vertex 3 ---

            manualObj.Position(new Vector3(500, 45, 500));//NW         // --- Vertex 4 ---
            manualObj.Position(new Vector3(-500, 45, 500));//NE        // --- Vertex 5 ---
            manualObj.Position(new Vector3(500, 0, 500));//NW          // --- Vertex 6 ---
            manualObj.Position(new Vector3(-500, 0, 500));//NE         // --- Vertex 7 ---

            // ---- Index Buffer ----

            //south wall
            // --- Triangle 0 ---
            manualObj.Index(0);
            manualObj.Index(1);
            manualObj.Index(2);
            // --- Triangle 1 ---
            manualObj.Index(1);
            manualObj.Index(3);
            manualObj.Index(2);

            //north wall
            // --- Triangle 2 ---
            manualObj.Index(6);
            manualObj.Index(5);
            manualObj.Index(4);
            // --- Triangle 3 ---
            manualObj.Index(6);
            manualObj.Index(7);
            manualObj.Index(5);

            //east wall
            // --- Triangle 4 ---
            manualObj.Index(7);
            manualObj.Index(3);
            manualObj.Index(5);
            // --- Triangle 5 ---
            manualObj.Index(3);
            manualObj.Index(1);
            manualObj.Index(5);

            //west wall
            // --- Triangle 6 ---
            manualObj.Index(4);
            manualObj.Index(2);
            manualObj.Index(6);

            // --- Triangle 7 ---
            manualObj.Index(4); //NWT
            manualObj.Index(0); //SWT
            manualObj.Index(2); //SWB

            manualObj.End();                                        // Closes the manual objet

            //return manualObj.ConvertToMesh("Quad");      

            manualObj.ConvertToMesh("Quad");
            boundaryEntity = mSceneMgr.CreateEntity("maualObjQuad", "Quad");
            boundaryEntity.SetMaterialName("Ground");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;

namespace RaceGame
{
    class RobotModel : CharacterModel
    {
        private ModelElement Model;
        SceneNode controlNode;
        public SceneNode ControlNode
        {
            get { return controlNode; }
        }
        Player player;
        Radian angle;           // Angle for the mesh rotation
        Vector3 direction;      // Direction of motion of the mesh for a single frame
        float radius;           // Radius of the circular trajectory of the mesh
        
        const float maxTime = 2000f;        // Time when the animation have to be changed
        Timer time;                         // Timer for animation changes
        AnimationState animationState;      // Animation state, retrieves and store an animation from an Entity
        bool animationChanged;              // Flag which tells when the mesh animation has changed
        string animationName;               // Name of the animation to use
        bool isBoss;

        /// <summary>
        // Loads the model, assembles it, and plays the animation
        /// </summary>
        public RobotModel(SceneManager mSceneMgr, Player player, bool boss)
        {
            this.mSceneMgr = mSceneMgr;
            isBoss = boss;
            LoadModelElements();
            AssembleModel();
            this.player = player;
            AnimationSetup();
        }

        /// <summary>
        // The robot mesh is loaded. The model is positioned randomly in the scene and PhysObj attached here
        /// </summary>
        protected override void LoadModelElements()
        {
            base.LoadModelElements();
            Model = new ModelElement(mSceneMgr, "robot.mesh");

            controlNode = mSceneMgr.CreateSceneNode();

            float radius = 30;
            controlNode.Position += radius * Vector3.UNIT_Y;
            Model.GameNode.Position -= (radius * Vector3.UNIT_Y);
            if (isBoss)
            {
                Model.GameNode.Position -= new Vector3(0f, -9f, 0f);    //set model to be above the ground
            }

            controlNode.SetPosition(Mogre.Math.RangeRandom(-450, 450), 0, Mogre.Math.RangeRandom(-450, 450));

            GameNode = controlNode;

            if (isBoss)
            {
                controlNode.Scale(new Vector3(1.7f, 1.7f, 1.7f));
            }

            physObj = new PhysObj(30f, "Robot", 10f, 0.3f, 5f);
            physObj.SceneNode = controlNode;
            physObj.Position = controlNode.Position;
            physObj.AddForceToList(new WeightForce(physObj.InvMass));
            physObj.AddForceToList(new FrictionForce(physObj));
            Physics.AddPhysObj(physObj);
        }

        /// <summary>
        // Attaches gamenode to the SceneMgr
        /// </summary>
        protected override void AssembleModel()
        {
            base.AssembleModel();


            controlNode.AddChild(Model.GameNode);

            gameNode = controlNode;

            mSceneMgr.RootSceneNode.AddChild(gameNode);

        }

        /// <summary>
        /// Returns true if the robot is colliding with the object passed in
        /// </summary>
        public bool IsCollidingWith(string collider)
        {
            bool isColliding = false;
            //Console.Out.WriteLine("CAlled");

            foreach (Contacts c in physObj.CollisionList)
            {
                // Console.Out.WriteLine(c.colliderObj.ID);

                if (c.colliderObj.ID == collider || c.collidingObj.ID == collider)
                {
                    isColliding = true;
                    //Console.WriteLine("colliding with " + collider);

                    physObj.CollisionList.Remove(c);
                    break;
                }
            }
            //Console.WriteLine("is colliding: " + isColliding);
            return isColliding;
        }

        /// <summary>
        // Disposes of the physObj and gamenode
        /// </summary>
        public override void DisposeModel()
        {
            //base.Dispose();
            Physics.RemovePhysObj(physObj);
            gameNode.DetachAllObjects();
            gameNode.Dispose();
            Model.Dispose();
        }

        /// <summary>
        /// This method set up all the field needed for animation
        /// </summary>
        private void AnimationSetup()
        {
            radius = 0.09f;
            direction = Vector3.ZERO;
            angle = 0f;

            time = new Timer();
            PrintAnimationNames();
            animationChanged = false;
            animationName = "Walk";
            LoadAnimation();
        }

        /// <summary>
        /// This method this method makes the mesh move in circle
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        private void CircularMotion(FrameEvent evt)
        {
            angle += (Radian)evt.timeSinceLastFrame;
            direction = radius * new Vector3(Mogre.Math.Cos(angle), 0, Mogre.Math.Sin(angle));
            controlNode.Translate(direction);
            controlNode.Yaw(-evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method sets the animationChanged field to true whenever the animation name changes
        /// </summary>
        /// <param name="newName"> The new animation name </param>
        private void HasAnimationChanged(string newName)
        {
            if (newName != animationName)
                animationChanged = true;
        }

        /// <summary>
        /// This method prints on the console the list of animation tags
        /// </summary>
        private void PrintAnimationNames()
        {
            AnimationStateSet animStateSet = Model.GameEntity.AllAnimationStates;                    // Getd the set of animation states in the Entity
            AnimationStateIterator animIterator = animStateSet.GetAnimationStateIterator();     // Iterates through the animation states

            while (animIterator.MoveNext())                                                     // Gets the next animation state in the set
            {
                //Console.WriteLine(animIterator.CurrentKey);                                     // Print out the animation name in the current key
            }
        }

        /// <summary>
        /// This method deternimes whether the name inserted is in the list of valid animation names
        /// </summary>
        /// <param name="newName">An animation name</param>
        /// <returns></returns>
        private bool IsValidAnimationName(string newName)
        {
            bool nameFound = false;

            AnimationStateSet animStateSet = Model.GameEntity.AllAnimationStates;
            AnimationStateIterator animIterator = animStateSet.GetAnimationStateIterator();

            while (animIterator.MoveNext() && !nameFound)
            {
                if (newName == animIterator.CurrentKey)
                {
                    nameFound = true;
                }
            }

            return nameFound;
        }

        /// <summary>
        /// This method changes the animation name randomly
        /// </summary>
        private void changeAnimationName()
        {
            switch ((int)Mogre.Math.RangeRandom(0, 4.5f))       // Gets a random number between 0 and 4.5f
            {
                case 0:
                    {
                        animationName = "Walk";
                        break;
                    }
                case 1:
                    {
                        animationName = "Shoot";
                        break;
                    }
                case 2:
                    {
                        animationName = "Idle";
                        break;
                    }
                case 3:
                    {
                        animationName = "Slump";
                        break;
                    }
                case 4:
                    {
                        animationName = "Die";
                        break;
                    }
            }
        }

        /// <summary>
        /// This method loads the animation from the animation name
        /// </summary>
        private void LoadAnimation()
        {
            animationState = Model.GameEntity.GetAnimationState(animationName);
            animationState.Loop = true;
            animationState.Enabled = true;
        }

        /// <summary>
        /// This method puts the mesh in motion
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        private void AnimateMesh(FrameEvent evt)
        {
            animationState.AddTime(evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method animates the robot mesh
        /// </summary>
        /// <param name="evt">A frame event which can be used to tune the animation timings</param>
        public void AnimateAI(FrameEvent evt)
        {
            float movementX = player.Position.x - controlNode.Position.x;
            float movementZ = player.Position.z - controlNode.Position.z;
            Vector3 moveDir = new Vector3(movementX, 0, movementZ); //direction to walk towards the player
            moveDir = moveDir.NormalisedCopy;        //normalised to remain at same speed regardless of distance between robot and player


            Vector3 lookDirection = new Vector3(player.Position.x, controlNode.Position.y, player.Position.z);  //position of the player, but at the robot's height so it does not tip to look at the player lower down

            MoveRobot(moveDir, lookDirection, evt);
        }


        /// <summary>
        /// Applies velocity to the robot to travel towards the player, and rotates the robot to look at the player
        /// </summary>
        public void MoveRobot(Vector3 movement, Vector3 look, FrameEvent evt)
        {
            controlNode.LookAt(look, Node.TransformSpace.TS_WORLD); //rotate robot to look 
            controlNode.Yaw((Degree)90);    //the robot mesh does not face forwards, and must be rotated 90 degrees

            physObj.Velocity = movement * 14f; //add velocity to the robot to make it move , and set the speed in which the AI moves towards the player
            AnimateMesh(evt);   //plays the animation
        }
    }
}

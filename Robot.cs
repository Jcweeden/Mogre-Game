using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using PhysicsEng;


namespace RaceGame
{
    class Robot : Character
    {
       // EnemyController robotController;
        public bool playerCollision;
        public bool PlayerCollision
        {
            get { return playerCollision; }
        }

        public void setPlayercol(bool val)
        {
            playerCollision = val;
        }


        public bool bombCollision;
        public bool BombCollision
        {
            get { return bombCollision; }
        }

        public void setBombCol(bool val)
        {
            bombCollision = val;
        }


        public bool cannonballCollision;
        public bool CannonCollision
        {
            get { return cannonballCollision; }
        }

        public void setCannonCol(bool val)
        {
            cannonballCollision = val;
        }

        float timeElapsed;
        public float TimeElapsed
         {
            set { timeElapsed = value; }
            get { return timeElapsed; }
        }


        private SceneManager mSceneMgr;

        private Player player;
        public Player Player
        {
            get { return player; }
        }

        bool shooting;
        public bool Shooting
        {
            get { return shooting; }
        }

        private RobotModel model;
        public RobotModel Model
        {
            get { return model; }
        }

        private bool boss;
        public bool isBoss
        {
            get { return boss; }
        }

        /// <summary>
        /// This class implements a robot
        /// </summary>
        /// 
        public Robot(SceneManager mSceneMgr, Player playerr, bool isBoss) {
            this.mSceneMgr = mSceneMgr;  //Initialize the scene manager to the parameter you just passed;
            boss = isBoss;  //determines whether the robot is a boss
            model = new RobotModel(mSceneMgr, playerr, boss);
            stats = new EnemyStats(); //stats to an instance of EnemyStats 
            this.player = playerr;
            playerCollision = false;
            //robotController = new EnemyController(this);
            shooting = false;
        }


        /// <summary>
        // Animates the robot and checks for collisions
        /// </summary>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);
            model.AnimateAI(evt);
            //robotController.Update(evt);
            timeElapsed += evt.timeSinceLastFrame;

            if (model.IsCollidingWith("Player"))
            {
                playerCollision = true;
            }
            if (model.IsCollidingWith("Cannonball"))
            {
                    cannonballCollision = true;                
            }
           if (model.IsCollidingWith("Bomb"))
            {
                    bombCollision = true;                
            }
        }

        /// <summary>
        // Disposes of the model
        /// </summary>
        public void Dispose()
        {
            model.DisposeModel();
        }


    }
}

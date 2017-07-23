using Mogre;
using Mogre.TutorialFramework;
using System;
using System.Collections.Generic;

using PhysicsEng;

namespace RaceGame  //could change namespace to RaceGame to stop 
{
    class Level : BaseApplication
    {

        Physics physics;

        Player player;                        // This fields is goning to contain an instance of the Robot class
        SceneNode cameraNode;
        InputsManager inputsManager;          //define and initialize the inputsManager field

        Environment environment;
        GameInterface gameHMD;

        List<Gem> gems;
        List<Gem> gemsToRemove;

        List<CollectableGun> collectableGuns;
        List<CollectableGun> collectableGunsToRemove;

        List<PowerUp> powerUps;
        List<PowerUp> powerUpsToRemove;

        List<Robot> enemies;
        List<Robot> enemiesToRemove;


        List<Projectile> enemyProjectiles;
        List<Projectile> enemyProjectilesToRemove;

        int totalTime;  //timer value displayed as countdown timer in GUI

        bool gamePlaying;    //if true then physics and player input are enabled

        private int level;  //the current level the user is on

        public static void Main()
        {
            new Level().Go();            // This method starts the rendering loop
        }


        /// <summary>
        /// This method create a new camera
        /// </summary>
        protected override void CreateCamera()
        {
            mCamera = mSceneMgr.CreateCamera("PlayerCam"); //create camera with name
            mCamera.Position = new Vector3(0, 40,-85);      //define camera position
            mCamera.LookAt(new Vector3(0, 0, 0));           //position it is looking at
            mCamera.NearClipDistance = 5;
            mCamera.FarClipDistance = 1000;                 //limits furthest distance camera can see
            mCamera.FOVy = new Degree(90);                  //field of view - how narrow or wide shot is

            mCameraMan = new CameraMan(mCamera);            //required by tutorial framework 
            mCameraMan.Freeze = true;                       //defines whether camera can move
        }

        /// <summary>
        /// This method create a new viewport
        /// </summary>
        protected override void CreateViewports()
        {
            Viewport viewport = mWindow.AddViewport(mCamera);   //creating using handler - add camera to window
            viewport.BackgroundColour = ColourValue.Black;      //set background colour
            mCamera.AspectRatio = viewport.ActualWidth / viewport.ActualHeight; //define aspect ratio
        }

        /// <summary>
        /// This method destroys the scene & components within it
        /// </summary>
        protected override void DestroyScene()
        {
            base.DestroyScene();
            gameHMD.Dispose();
            cameraNode.DetachAllObjects();
            cameraNode.Dispose();
            environment.Dispose();
            physics.Dispose();
         }
        

        /// <summary>
        /// This method update the scene after a frame has finished rendering
        /// The method calls an update to every non-static gameobject, updating its physics, collisions etc.
        /// </summary>
        /// <param name="evt"></param>
        protected override void UpdateScene(FrameEvent evt)
        {
            //timeElapsed += evt.timeSinceLastFrame;



            player.Update(evt);

            if (gameHMD.gameTimeHasReachedZero(totalTime)) //if still time left on clock
            {
                gameHMD.updateTimerText(totalTime); //update the clock
            }
            else    //else game timer has run out so end game
            {
                gameHMD.displayGameLoseText();//display game completed text
                gamePlaying = false; //disable physics and input
                inputsManager.DisableInputs();  //disable the player inputs
            }

            if (gamePlaying == true)    //if the game has not been won / lost
            {
                physics.UpdatePhysics(0.01f);   //update the physics every 0.01 seconds. This value was tweaked until physics appeared realistic

            }

            base.UpdateScene(evt);

            if (player.GunReloading == true)    //if the player is reloading
            {
                gameHMD.updateAmmoTextReloading();  //display info saying so in the GUI
            }
            else if (player.playerArmoury.ActiveGun != null)    //else if the player has a gun equipped
            {
                gameHMD.updateAmmoText(player.playerArmoury.ActiveGun.Ammo.Value);  //display the ammo count on the gun
            }


            if (inputsManager != null && gamePlaying == true)   //if the game is playing (not won or lost) and the inputsManager has been created
            {
                inputsManager.ProcessInput(evt);    // then process player inputs
            }


            if ( gems.Count > 0)    //if gems are instantiated in the game
            {

                foreach (Gem gem in gems)   //for each gem
                {
                    gem.Update(evt);    //update the gem
                    if (gem.RemoveMe == true)   //and if a collision has been detected and the gem must be removed
                    {
                        gemsToRemove.Add(gem);  //add the gem to gems to be removed 
                        player.Stats.Score.Increase(gem.Increase);  //and update the score
                    }
                }

                foreach (Gem gem in gemsToRemove)   //for each gem that needs to be removed
                {
                    gems.Remove(gem);   //remove the gem from the list of active gems
                    gem.Dispose();      //and dispose of it
                }
                gemsToRemove.Clear();   //clear the list now it has been disposed
            }

            if (powerUps.Count > 0) //if power up collectables are instantiated in the game
            {
                foreach (PowerUp pow in powerUps) //for each powerup
                {
                    pow.Update(evt); //update the powerup
                    if (pow.RemoveMe == true) //and if a collision has been detected and the power up must be removed
                    {
                        switch (pow.pickUpType)     //determine the type of the powerup and apply the necessary buff to the player
                        {
                            case "life":
                                player.Stats.Lives.Increase(pow.Increase);  //add a life
                                break;
                            case "time":
                                totalTime += 30;    //add 30 seconds to the counter
                                break;
                            case "shield":
                                player.Stats.Shield.Increase(pow.Increase); //add health
                                break;
                            case "health":
                                player.Stats.Health.Increase(pow.Increase); //add shields
                                break;
                        }
                        powerUpsToRemove.Add(pow);  //then add the power up to be removed
                    }
                }

                foreach (PowerUp pow in powerUpsToRemove)//for each power up that needs to be removed
                {
                    powerUps.Remove(pow);   //remove the power up from the list of active power ups
                    pow.Dispose();          //and dispose of it
                }
                powerUpsToRemove.Clear();   //clear the list now it has been disposed
            }


            if (collectableGuns.Count > 0)  //if collectables are instantiated in the game
            {
                foreach (CollectableGun gun in collectableGuns)  //for each collectable
                {
                    gun.Update(evt);            //update the collectable
                    if (gun.RemoveMe == true)   //and if a collision has been detected and the collectable must be removed
                    {
                        collectableGunsToRemove.Add(gun);   //the addition of the gun to the armoury is handled in the Player class, so add the gun to be removed
                    }
                }

                foreach (CollectableGun gun in collectableGunsToRemove) //for each gun that needs to be removed
                {
                    collectableGuns.Remove(gun);    //remove the power up from the list of active gun collectables in the game
                    gun.Dispose();                  //and dispose of it
                }
                collectableGunsToRemove.Clear();    //clear the list now it has been disposed
            }

    

            if (enemies.Count > 0) {            //if robots are instantiated in the game
                foreach (Robot rob in enemies)  //for each robot that needs to be removed
                {
                    rob.Update(evt);            //update the robot

                    if (rob.TimeElapsed > 1f) //if the robot has not shot in the last 0.5seconds
                    {
                        rob.TimeElapsed = 0;    //reset the timer until shooting again
                        Vector3 position = new Vector3(rob.Model.ControlNode.Position.x, 10, rob.Model.ControlNode.Position.z); //position shooting from
                        Vector3 direction = new Vector3(player.Position.x - rob.Model.ControlNode.Position.x, 0, player.Position.z - rob.Model.ControlNode.Position.z); //direction in which to apply velocity to enemy projectiles towards the player
                        direction = direction.NormalisedCopy;
                        //Console.WriteLine(position);

                        EnemyProjectile projectile = new EnemyProjectile(mSceneMgr, direction, position);
                        projectile.SetPosition(position + 50 * direction);
                        enemyProjectiles.Add(projectile);   //add the new projectile to the list of enemy projectiles
                    }

                    if (rob.PlayerCollision)    //if the robot collides with the player
                    {
                        if (player.Stats.Shield.Value != 0) //if the player has shields remaining
                        {
                            player.Stats.Shield.Decrease(1);    //deduct shields
                        }
                        else                                    //else the player has no shields
                        {
                            player.Stats.Health.Decrease(1);    //so lower their health
                        }
                        rob.setPlayercol(false);    //and reset that the player and robot are colliding to false
                    }

                    if (rob.BombCollision)  //if the robot has collided with a player bomb
                    {
                        rob.Stats.Shield.Decrease(500); //damage both health and shields so the robot dies immediately 
                        rob.Stats.Health.Decrease(500);
                        rob.setBombCol(false);          //and reset the robot colliding with a bomb to false
                    }


                    if (rob.CannonCollision)                //if the robot has collided with a player cannonball
                    {

                        if (rob.Stats.Shield.Value > 0)     //if the player has shields
                        {
                            rob.Stats.Shield.Decrease(25);  //deduct shields
                        }
                        else                                //else the player has no shields
                        {
                            rob.Stats.Health.Decrease(50);  //so lower their health
                        }
                        rob.setCannonCol(false);            //and reset that the robot and cannonball are colliding to false
                    }

                    gameHMD.updateBossText(enemies[0].Stats.Shield.Value, enemies[0].Stats.Health.Value, enemies[0]); //in the third level the health values of the boss are displayed on screen - send these vals to the GUI

                    if (rob.Stats.Health.Value <= 0)    //if the robot has no health
                    {
                        enemiesToRemove.Add(rob);       //then add it to the remove list
                    }
                }


                foreach (Robot rob in enemiesToRemove)  //for each robot that needs to be removed
                {
                    enemies.Remove(rob);    //remove it from the current robots list
                    rob.Dispose();          //and dispose of it
                }
                enemiesToRemove.Clear();

            }

            foreach (EnemyProjectile proj in enemyProjectiles)
            {
                proj.checkDispose();    //check whether the projectile has existed for longer than it's lifetime

                if(proj.TimeElapsed > 1)    //if it has then dispose of it
                {
                    proj.Dispose();
                }

                if (proj.IsCollidingWith("Player")) //if the robot projectile is colliding with the player
                {
                    if (player.Stats.Shield.Value != 0) //if the player has shields
                    {
                        player.Stats.Shield.Decrease(100);  //deduct shields
                    }
                    else                                    //else the player has no shields
                    {
                        player.Stats.Health.Decrease(50);   //so reduce their health
                    }
                    proj.Dispose();                         //then dispose of the projectile
                }

            }


            if (player.Stats.Health.Value <= 0 && player.Stats.Lives.Value <= 0)    //if no lives or health
            {
                gameHMD.displayGameLoseTextLives();  //display game loss text
                gamePlaying = false;            //disable physics and input
                inputsManager.DisableInputs();  //set all current inputs to false
            }
                else if (player.Stats.Health.Value <= 0 && player.Stats.Lives.Value >= 1)   //if no health but still has remaining lifes
                    {
                        player.Stats.Health.Reset();    //reset the player's health and shields
                        player.Stats.Shield.Reset();
                        player.Stats.Lives.Decrease(1); //and reduce their lives by one
                    }
            
                

            if (checkIfLevelComplete())
            {
                if (level <= 2) //if there still is another level to play (e.g. level 3 has not been completed)
                {
                    initNextLevel();    //init the next level
                    gameHMD.updateLevelText(level); //and update the GUI to represent the new level number
                }
                else   //else the final level and game has been completed
                {
                    level++;
                    gameHMD.updateBossText(0, 0, null);   //ensure the display shows the boss is dead
                    gameHMD.displayGameWinText();   //display game completed text
                    gamePlaying = false;            //disable physics and input
                    inputsManager.DisableInputs();  //set all current inputs to false
                }
            }
            gameHMD.Update(evt);    //updte the GUI last after all data has been processed that will be displayed

        }

        /// <summary>
        /// Checks if all gems have been collected and all enemies defeated before advancing to the next level
        /// </summary>
        protected bool checkIfLevelComplete()
        {
            bool levelCompleted = false;
            
                if (gems.Count == 0 && enemies.Count == 0)  //and all gems have been collected and enemies defeated
                {
                    return true;                            //return true to finish game
                }
            
            return levelCompleted;
        }


        /// <summary>
        /// Instantiates gems, guns, collectables, robots according to the level starting
        /// </summary>
        protected void initNextLevel()
        {
            level++;    //the level number is iterated
                
            if (level == 1) //and for each level the objects instantiated defined
            {
                for (int i = 0; i < (int)Mogre.Math.RangeRandom(5, 8); i++)     //number of gems are spawned randomly between two numbers
                    gems.Add(new RedGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(3, 5); i++)
                    gems.Add(new YellowGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(2, 3); i++)
                    gems.Add(new BlueGem(mSceneMgr, new Stat()));


                powerUps.Add(new TimePickUp(mSceneMgr));


            }

            else if (level == 2)
            {
                for (int i = 0; i < (int)Mogre.Math.RangeRandom(4, 7); i++)
                    gems.Add(new RedGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(3, 5); i++)
                    gems.Add(new YellowGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(2, 4); i++)
                    gems.Add(new BlueGem(mSceneMgr, new Stat()));

                powerUps.Add(new TimePickUp(mSceneMgr));
                powerUps.Add(new LifePickUp(mSceneMgr));

                collectableGuns.Add(new CollectableGun(mSceneMgr, new BombDropper(mSceneMgr), player.PlayerArmoury));
                collectableGuns.Add(new CollectableGun(mSceneMgr, new Cannon(mSceneMgr), player.PlayerArmoury));

            }

            else if (level == 3)
            {
                for (int i = 0; i < (int)Mogre.Math.RangeRandom(4, 6); i++)
                    gems.Add(new RedGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(3, 6); i++)
                    gems.Add(new YellowGem(mSceneMgr, new Stat()));

                for (int i = 0; i < (int)Mogre.Math.RangeRandom(3, 5); i++)
                    gems.Add(new BlueGem(mSceneMgr, new Stat()));

                enemies.Add(new Robot(mSceneMgr, player, true));
                enemies.Add(new Robot(mSceneMgr, player, false));
                enemies.Add(new Robot(mSceneMgr, player, false));

                powerUps.Add(new HealthPickUp(mSceneMgr));
                powerUps.Add(new ShieldPickUp(mSceneMgr));
            }

        }

        /// <summary>
        /// This method creates the initial scene. Data structures holding collectables are instantiated, the camera is attached to the player, GUI instantiated etc.
        /// </summary>
        protected override void CreateScene()
        {
            physics = new Physics();
            player = new Player (mSceneMgr);


            level = 0;          //iterated to level 1 upon calling initNextLevel() at bottom of this method
            totalTime = 45;     //the timer initially starts with 45seconds on the clock
            gamePlaying = true; //the game is set to start playing, ticking the physics and accepting controller input

            environment = new Environment(mSceneMgr, mWindow);  //create the environment including wall, floor, lighting etc.

            inputsManager = InputsManager.Instance;    //The class should be used in the CreateScene method of the Mogre tutorial framework
            inputsManager.PlayerController = (PlayerController)player.Controller; //set the PlayerController property of the inputsManager object to player.Controller
            InitializeInput();  //init inputs to read from keyboard and mouse

            cameraNode = mSceneMgr.CreateSceneNode();
            cameraNode.AttachObject(mCamera);
            player.Model.GameNode.AddChild(cameraNode); //attach the camera to the player


            gameHMD = new GameInterface(mSceneMgr, mWindow, player.Stats); //set to update player.stats



            //instantiate lists to hold and remove enemies, gems, powerups and projectiles
            enemies = new List<Robot>();
            enemiesToRemove = new List<Robot>();

            gems = new List<Gem>();
            gemsToRemove = new List<Gem>();

            powerUps = new List<PowerUp>();
            powerUpsToRemove = new List<PowerUp>();

            collectableGuns = new List<CollectableGun>();
            collectableGunsToRemove = new List<CollectableGun>();

            enemyProjectiles = new List<Projectile>();
            enemyProjectilesToRemove = new List<Projectile>();


            initNextLevel();    //run to spawn the first level of the game

            physics.StartSimTimer();    //must be last line after creating scene - starts physics
        }



       

        /// <summary>
        /// This method set create a frame listener to handle events before, during or after the frame rendering
        /// </summary>
        protected override void CreateFrameListeners()
        {
            base.CreateFrameListeners();
        }

        /// <summary>
        /// This method initilize the inputs reading from keyboard adn mouse
        /// </summary>
        protected override void InitializeInput()
        {
            base.InitializeInput();

            int windowHandle;
            mWindow.GetCustomAttribute("WINDOW", out windowHandle);
            inputsManager.InitInput(ref windowHandle);
        }

    }
}
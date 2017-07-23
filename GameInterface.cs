using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mogre;

namespace RaceGame
{
    /// <summary>
    /// This class implements an example of interface
    /// </summary>
    class GameInterface:HMD     // Game interface inherits form the Head Mounted Dispaly (HMD) class
    {
        private PanelOverlayElement panel;
        private OverlayElement scoreText;
        private OverlayElement levelText;
        private OverlayElement timerText;
        private OverlayElement timeRemainingText;
        private OverlayElement gameWinText;
        private OverlayElement gunLoadedText;
        private OverlayElement bossText;

        private OverlayElement healthBar;
        private OverlayElement shieldBar;
        private Overlay overlay3D;
        private Entity lifeEntity;
        private List<SceneNode> lives;
        public int winningTimeRemaining;
        public bool gameWon;
        private Timer time;
        public Timer Time
        {
            set { time = value; }
        }

        private float hRatio;
        private float sRatio;
        private string score = "Score: ";

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mSceneMgr">A reference of a scene manager</param>
        /// <param name="playerStats">A reference to a character stats</param>
        public GameInterface(SceneManager mSceneMgr,
            RenderWindow mWindow, CharacterStats playerStats)
            : base(mSceneMgr, mWindow, playerStats)  // this calls the constructor of the parent class
        {
            Load("GameInterface");
            time = new Timer();
        }

        /// <summary>
        /// This method initializes the element of the interface
        /// </summary>
        /// <param name="name"> A name to pass to generate the overaly </param>
        protected override void Load(string name)
        {
            base.Load(name);

            winningTimeRemaining = 0;
            lives = new List<SceneNode>();
            gameWon = false;

            healthBar = OverlayManager.Singleton.GetOverlayElement("HealthBar");
            hRatio = healthBar.Width / (float)characterStats.Health.Max;

            shieldBar = OverlayManager.Singleton.GetOverlayElement("ShieldBar");
            sRatio = shieldBar.Width / (float)characterStats.Shield.Max;

            scoreText = OverlayManager.Singleton.GetOverlayElement("ScoreText");
            scoreText.Caption = score;
            scoreText.Left = mWindow.Width * 0.6f;

            levelText = OverlayManager.Singleton.GetOverlayElement("LevelText");
            levelText.Caption = "Level 1";
            levelText.Left = mWindow.Width * 0.37f;

            timeRemainingText = OverlayManager.Singleton.GetOverlayElement("TimeRemainingText");
            //timeRemainingText.Caption = "Time remaining: ";
            timeRemainingText.Left = mWindow.Width * 0.5f;

            timerText = OverlayManager.Singleton.GetOverlayElement("TimerText");
            timerText.Left = mWindow.Width * 0.5f;

            gameWinText = OverlayManager.Singleton.GetOverlayElement("GameWinText");
            gameWinText.Left = mWindow.Width * 0.5f;

            gunLoadedText = OverlayManager.Singleton.GetOverlayElement("GunLoadedText");
            gunLoadedText.Caption = "No Gun Loaded";
            gunLoadedText.Left = mWindow.Width * 0.04f;


            bossText = OverlayManager.Singleton.GetOverlayElement("BossText");
            bossText.Caption = "";
            bossText.Left = mWindow.Width * 0.3f;

            panel =
           (PanelOverlayElement)OverlayManager.Singleton.GetOverlayElement("GreenBackground");
            panel.Width = mWindow.Width;
            LoadOverlay3D();
        }

        /// <summary>
        /// Displayed in level 3, showing the boss health and shields
        /// </summary>
        public void updateBossText(int shield, int health, Robot boss)
        {
            if (boss == null)
            {
                bossText.Caption = "";

            }
            else if (boss.isBoss == true)
            {
                if (health != 0)
                {
                    bossText.Caption = "Boss Health: Shields: " + shield + "  Health: " + health;
                }
                else
                {
                    bossText.Caption = "";
                }
            }
            
        }

        /// <summary>
        /// Displays the current level
        /// </summary>
        public void updateLevelText(int level)
        {
            levelText.Caption = "Level " + level;
        }

        /// <summary>
        /// Displays the remaining ammo count
        /// </summary>
        public void updateAmmoText(int ammo)
        {
            gunLoadedText.Caption = "Ammo Remaining: " + ammo;
        }

        /// <summary>
        /// Displayed when the gun is reloading
        /// </summary>
        public void updateAmmoTextReloading()
        {
            gunLoadedText.Caption = "Gun Reloading...";
        }

        /// <summary>
        /// Updates the time shown on screen, or if the timer has run out calls to display a message
        /// </summary>
        public void updateTimerText(int timeGathered)
        {
            if (winningTimeRemaining == 0)
            {    //if game has not been won
                if ((timeGathered * 1000) - time.Milliseconds > 0)
                {
                    timerText.Caption = "Time remaining: " + convertTime(((timeGathered * 1000) - time.Milliseconds)); //total time gathered - time elapsed
                }
                else    //timer has reached zero - display static text to stop from going into negative
                {
                    timerText.Caption = "Time remaining: 0:00";
                }

            }
            else
            {
                timerText.Caption = "Time remaining: " + convertTime((timeGathered * 1000) - winningTimeRemaining);
            }
        }

        /// <summary>
        /// A test checking whether time has run out
        /// </summary>
        public bool gameTimeHasReachedZero(int timeGathered)
        {
            if((timeGathered * 1000) - time.Milliseconds <= 0)
            { 
                return false; 
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Displayed upon winning the game
        /// </summary>
        public void displayGameWinText()
        {
            gameWinText.Caption = "You Win!";
            if (!gameWon)
            {
                winningTimeRemaining = (int)time.Milliseconds;//stop timer
                gameWon = true;
            }
        }

        /// <summary>
        /// Displayed when the player runs out of time
        /// </summary>
        public void displayGameLoseText()
        {
            gameWinText.Caption = "Time Ran Out - You Lose!";
        }

        /// <summary>
        /// Displayed when the player runs out of lives
        /// </summary>
        public void displayGameLoseTextLives()
        {
            gameWinText.Caption = "No More Lives - You Lose!";
        }

        /// <summary>
        /// This method initalize a 3D overlay
        /// </summary>
        private void LoadOverlay3D()
        {
            overlay3D = OverlayManager.Singleton.Create("3DOverlay");
            overlay3D.ZOrder = 15000;

            CreateHearts();

            overlay3D.Show();
        }

        /// <summary>
        /// This method generate as many hearts as the number of lives left
        /// </summary>
        private void CreateHearts()
        {
            for (int i = 0; i < characterStats.Lives.Value; i++)
                AddHeart(i);
        }

        /// <summary>
        /// This method add an heart to the 3D overlay
        /// </summary>
        /// <param name="n"> A numeric tag</param>
        private void AddHeart(int n)
        {
            SceneNode livesNode = CreateHeart(n);
            lives.Add(livesNode);
            overlay3D.Add3D(livesNode);
        }

        /// <summary>
        /// This method remove from the 3D overlay and destries the passed scene node
        /// </summary>
        /// <param name="life"></param>
        private void RemoveAndDestroyLife(SceneNode life)
        {
            overlay3D.Remove3D(life);
            lives.Remove(life);
            MovableObject heart = life.GetAttachedObject(0);
            life.DetachAllObjects();
            life.Dispose();
            heart.Dispose();
        }

        /// <summary>
        /// This method initializes the heart node and entity
        /// </summary>
        /// <param name="n"> A numeric tag used to determine the heart postion on sceen </param>
        /// <returns></returns>
        private SceneNode CreateHeart(int n)
        {
            lifeEntity = mSceneMgr.CreateEntity("Heart.mesh");
            lifeEntity.SetMaterialName("HeartHMD");
            SceneNode livesNode;
            livesNode = new SceneNode(mSceneMgr);
            livesNode.AttachObject(lifeEntity);
            livesNode.Scale(new Vector3(0.15f, 0.15f, 0.15f));
            livesNode.Position = new Vector3(7.2f, 7.2f, -8) - n * 0.5f * Vector3.UNIT_X; ;
            livesNode.SetVisible(true);
            return livesNode;
        }

        /// <summary>
        /// This method converts milliseconds in to minutes and second format mm:ss
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private string convertTime(float time)
        {
            string convTime;
            float secs = time / 1000f;
            int min = (int)(secs / 60);
            secs = (int)secs % 60f;
            if (secs < 10)
                convTime = min + ":0" + secs;
            else
                convTime = min + ":" + secs;
            return convTime;
        }

        /// <summary>
        /// This method updates the interface
        /// </summary>
        /// <param name="evt"></param>
        public override void Update(FrameEvent evt)
        {
            base.Update(evt);

            Animate(evt);

            //time.Reset();
           // Console.Out.WriteLine(time.Microseconds);
          //  timerText.Caption = convertTime((time.Milliseconds)); //total time gathered - time elapsed

            if (lives.Count > characterStats.Lives.Value && characterStats.Lives.Value >= 0)
            {
                SceneNode life = lives[lives.Count - 1];
                RemoveAndDestroyLife(life);

            }
            if (lives.Count < characterStats.Lives.Value)
            {
                AddHeart(characterStats.Lives.Value);
            }

            //need to modify this for final game
            healthBar.Width = hRatio * characterStats.Health.Value;
            shieldBar.Width = sRatio * characterStats.Shield.Value;
            scoreText.Caption = score + ((PlayerStats)characterStats).Score.Value;
        }

        /// <summary>
        /// This method animates the heart rotation
        /// </summary>
        /// <param name="evt"></param>
        protected override void Animate(FrameEvent evt)
        {
            foreach (SceneNode sn in lives)
                sn.Yaw(evt.timeSinceLastFrame);
        }

        /// <summary>
        /// This method disposes of the elements generated in the interface
        /// </summary>
        public override void Dispose()
        {
            List<SceneNode> toRemove = new List<SceneNode>();
            foreach (SceneNode life in lives)
            {
                toRemove.Add(life);
            }
            foreach (SceneNode life in toRemove)
            {
                RemoveAndDestroyLife(life);
            }
            lifeEntity.Dispose();
            toRemove.Clear();
            shieldBar.Dispose();
            healthBar.Dispose();
            scoreText.Dispose();
            panel.Dispose();
            overlay3D.Dispose();
            base.Dispose();
        }
    }
}

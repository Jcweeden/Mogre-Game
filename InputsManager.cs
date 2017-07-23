using System;
using Mogre;
using System.Collections;

namespace RaceGame
{
    /// <summary>
    /// This class manages the inputs from keyboard and mouse
    /// </summary>
    class InputsManager
    {
        // Keyboard, mouse and inputs managers
        MOIS.Keyboard mKeyboard;
        MOIS.Mouse mMouse;
        MOIS.InputManager mInputMgr;

        PlayerController playerController;                // Reference to an istance of the robot
        /// <summary>
        /// Read only. This property allow to set a reference to an istance of the robot
        /// </summary>
        public PlayerController PlayerController
        {
            set { playerController = value; }
        }

        /// <summary>
        /// Private constructor (for singleton pattern)
        /// </summary>
        private InputsManager() { }

        private static InputsManager instance; // Private instance of the class
        /// <summary>
        /// Gives back a new istance of the class if the instance field is null
        /// otherwise it gives back the istance already created
        /// </summary>
        public static InputsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputsManager();
                }
                return instance;
            }
        }

        /// <summary>
        /// This method set the reaction to each key stroke
        /// The ProcessInput method should not call the functions to move or rotate the player, 
        /// it should just set the values of how much movement or rotation the player should do.
        /// </summary>
        /// <param name="evt">Can be used to tune the reaction timings</param>
        /// <returns></returns>
        public bool ProcessInput(FrameEvent evt)
        {
            Vector3 displacements = Vector3.ZERO;
            Vector3 angles = Vector3.ZERO;
            mKeyboard.Capture();
            mMouse.Capture();

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_A))
                playerController.Left = true;
            else
                playerController.Left = false;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_D))
                playerController.Right = true;
            else
                playerController.Right = false;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_W))
                playerController.Forward = true;
            else
                playerController.Forward = false;


            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_S))
                playerController.Backward = true;
            else
                playerController.Backward = false;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_SPACE))
                playerController.Shoot = true;
            else
                playerController.Shoot = false;

            if (mKeyboard.IsKeyDown(MOIS.KeyCode.KC_LSHIFT))
                playerController.Accellerate = true;
            else
                playerController.Accellerate = false;

            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Left))
            {
                angles.y/*z*/ = -mMouse.MouseState.X.rel;   //change angles.z into angles.y
            }
            if (mMouse.MouseState.ButtonDown(MOIS.MouseButtonID.MB_Right))
            {
                angles.y/*x*/ = -mMouse.MouseState.Y.rel;   //change angles.z into angles.y
            }
            playerController.Angles = angles;   //the pass the angle vector to the PlayerController as playerController.Angles = angles after the if statements for the mouse
            return true;
        }

        /// <summary>
        /// This method sets all inputs to false
        /// The ProcessInput method should not call the functions to move or rotate the player, 
        /// it should just set the values of how much movement or rotation the player should do.
        /// </summary>
        public void DisableInputs()
        {
            Vector3 displacements = Vector3.ZERO;
            Vector3 angles = Vector3.ZERO;
            mKeyboard.Capture();
            mMouse.Capture();
       
            playerController.Left = false;
            playerController.Right = false;
            playerController.Forward = false;
            playerController.Backward = false;
            playerController.Shoot = false;
            playerController.Accellerate = false;
            playerController.Angles = angles;   //the pass the angle vector to the PlayerController as playerController.Angles = angles after the if statements for the mouse
        }

        /// <summary>
        /// Initializes the keyboad, mouse and the input manager systems
        /// </summary>
        /// <param name="windowHandle">An handle to the game windonw</param>
        public void InitInput(ref int windowHandle)
        {
            mInputMgr = MOIS.InputManager.CreateInputSystem((uint)windowHandle);
            mKeyboard = (MOIS.Keyboard)mInputMgr.CreateInputObject(MOIS.Type.OISKeyboard, true);
            mMouse = (MOIS.Mouse)mInputMgr.CreateInputObject(MOIS.Type.OISMouse, false);

            mKeyboard.KeyPressed += new MOIS.KeyListener.KeyPressedHandler(OnKeyPressed);
        }

        /// <summary>
        /// Buffered key listener
        /// </summary>
        /// <param name="arg">A keyboard event</param>
        /// <returns></returns>
        private bool OnKeyPressed(MOIS.KeyEvent arg)
        {
            switch (arg.key)
            {
                //case MOIS.KeyCode.KC_SPACE:
                //    {   //You should use the spacebar to shoot 
                //        playerController.Shoot = true;
                //        break;
                //    }
                case MOIS.KeyCode.KC_E:
                    {   //shoot and the E key to swap weapons.
                        playerController.SwapGun = true;
                        break;
                    }
                case MOIS.KeyCode.KC_ESCAPE:
                    {
                        return false;   //exit game
                    }
            }
            return true;
        }
    }
}

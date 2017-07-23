using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame
{
    abstract class Projectile : MovableElement
    {
        protected int maxTime = 1000;
        protected float speed =150;



        protected float healthDamage;
        ///// <summary>
        // Read only. Returns health damage of the projectile
        ///// </summary>
        public float HealthDamage
        {
            get { return healthDamage; }
        }

        protected float shieldDamage;
        ///// <summary>
        // Read only. Returns shield damage of the projectile
        ///// </summary>
        public float ShieldDamage
        {
            get { return shieldDamage; }
        }

        float timeElapsed;
        ///// <summary>
        // Read only. Returns time projectile has existed
        ///// </summary>
        public float TimeElapsed
        {
            get { return timeElapsed; }
        }

        virtual protected void Load() { }



        ///// <summary>
        // Constructor. Returns health damage of the projectile
        ///// </summary>
        protected Projectile()
        {
            timeElapsed = 0f;
        }


        ///// <summary>
        // Disposes of the model
        ///// </summary>
        public override void Dispose()
        {
            base.Dispose();
            this.remove = true;
        }


        ///// <summary>
        // Updates time the projectile has been instantiated
        ///// </summary>
        virtual public void Update(FrameEvent evt)
        {
            timeElapsed += evt.timeSinceLastFrame;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace RaceGame
{
    class Gun : MovableElement
    {
        protected int maxAmmo;

        protected Projectile projectile;
        /// <summary>
        /// Setter. sets the ammo type of the gun
        /// </summary>
        public Projectile Projectile
        {
            set { projectile = value; }
        }

        protected Stat ammo;
        /// <summary>
        /// Read only. returns the ammo of the gun
        /// </summary>
        public Stat Ammo
        {
            get { return ammo; }
        }


        /// <summary>
        /// Returns the Vector3 position of the gun
        /// </summary>
        public Vector3 GunPosition()
        {
            SceneNode node = gameNode;
            try
            {
                while (node.ParentSceneNode.ParentSceneNode != null)
                {
                    node = node.ParentSceneNode;
                }
            }
            catch (System.AccessViolationException)
            { }

            return node.Position;
        }


        /// <summary>
        /// Returns the Vector3 direction the gun is facing
        /// </summary>
        public Vector3 GunDirection()
        {
            SceneNode node = gameNode;
            try
            {
                while (node.ParentSceneNode.ParentSceneNode != null)
                {
                    node = node.ParentSceneNode;
                }
            }
            catch (System.AccessViolationException)
            { }

            Vector3 direction = node.LocalAxes * gameNode.LocalAxes.GetColumn(2);

            return direction;
        }

        virtual protected void LoadModel() { }
        virtual public void ReloadAmmo() { }
        virtual public Projectile Fire() { return null; }

        virtual public float ReloadTime() { return 0; }

    }
}

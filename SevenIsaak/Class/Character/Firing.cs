using SevenIsaak.Class.Attacks;
using SharpDX.Direct3D9;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SevenIsaak.Class.Sprite;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SevenIsaak.Class.Character
{
    internal class Firing : Stats
    {
        Texture2D _projectileTexture;

        public float damageMultiplier = 1;
        float _damage;
        public float damage
        {
            get { return _damage; }
            set
            {
                if (value > _damage)
                {
                    float dif = value - _damage;
                    _damage = value;
                    damageMultiplier -= dif / 20;
                }
                else if (value < _damage)
                {
                    float dif = _damage - value;
                    _damage = value;
                    damageMultiplier += dif / 20;
                }
            }
        }

        public float fireRateMultiplier = 1;
        float _fireRate;
        public float fireRate
        {
            get { return _fireRate; }
            set
            {
                if (value > _fireRate)
                {
                    float dif = value - _fireRate;
                    _fireRate = value;
                    fireRateMultiplier -= dif / 20;
                }
                else if (value < _fireRate)
                {
                    float dif = _fireRate - value;
                    _fireRate = value;
                    fireRateMultiplier += dif / 20;
                }
            }
        }
        private float _timeSinceLastShot = 0f;

        public float rangeMultiplier = 1;
        float _range;
        public float range
        {
            get { return _range; }
            set
            {
                if (value > _range)
                {
                    float dif = value - _range;
                    _range = value;
                    rangeMultiplier -= dif / 20;
                }
                else if (value < _range)
                {
                    float dif = _range - value;
                    _range = value;
                    rangeMultiplier += dif / 20;
                }
            }
        }

        public float shotSpeedMultiplier = 1;
        float _shotSpeed;
        public float shotSpeed
        {
            get { return _shotSpeed; }
            set
            {
                if (value > _shotSpeed)
                {
                    float dif = value - _shotSpeed;
                    _shotSpeed = value;
                    shotSpeedMultiplier -= dif / 20;
                }
                else if (value < _shotSpeed)
                {
                    float dif = _shotSpeed - value;
                    _shotSpeed = value;
                    shotSpeedMultiplier += dif / 20;
                }
            }
        }

        Microsoft.Xna.Framework.Vector2 size;

        public bool canFire = true;

        public int projectileNumber = 4;//number of projectile that are simultaneously fire
        public int gabDegree = 15;//number of projectile that are simultaneously fire
        public Firing(Texture2D projectileTexture, float damage, float fireRate, float range, float shotSpeed)
        {
            _projectileTexture = projectileTexture;
            _damage = damage;
            _fireRate = fireRate;
            _range = range;
            _shotSpeed = shotSpeed;
        }
        public void Update(GameTime gameTime)
        {
            if (!canFire)
            {
                _timeSinceLastShot += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (_timeSinceLastShot >= 1f / fireRate)
                {
                    canFire = true;
                    _timeSinceLastShot = 0f;
                }
            }
        }
        
        //chatGPT powered
        /// <summary>
        /// generate a given number of projectile(projectileNumber) that will handle themself 
        /// </summary>
        /// <param name="position">from where porj gonna be fire</param>
        /// <param name="direction"> in wich direction</param>
        /// <param name="shooter">who fire the projectile</param>
        public void Fire(Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 direction, Character shooter, Microsoft.Xna.Framework.Vector2 shooterVelocity)
        {
            if (!canFire) return;

            direction.X += shooterVelocity.X * 0.4f;
            direction.Y += shooterVelocity.Y * 0.4f;

            // Define the spread angle (in radians) for the shotgun effect.
            float spreadAngle = MathHelper.ToRadians(gabDegree); // Total spread in degrees, converted to radians

            // Calculate half the spread to distribute projectiles evenly
            float halfSpread = spreadAngle / 2f;

            if (projectileNumber > 1)
            {
                // Calculate the angle increment based on how many projectiles there are
                float angleStep = spreadAngle / (projectileNumber - 1);

                // Loop through and create each projectile
                for (int i = 0; i < projectileNumber; i++)
                {
                    // Calculate the current angle offset from the base direction
                    float angleOffset = -halfSpread + (i * angleStep); // Start from the leftmost point in the arc

                    // Rotate the base direction by the angleOffset
                    Vector2 projectileDirection = RotateVector(direction, angleOffset);

                    // Create and fire a new projectile in this direction
                    new Projectile(_projectileTexture, position, projectileDirection, shooter, this, shotSpeed);
                }
            }
            else
            {
                new Projectile(_projectileTexture, position, direction, shooter, this, shotSpeed);
            }
            canFire = false;
        }

        // Helper method to rotate a vector by a given angle (in radians)
        private Vector2 RotateVector(Vector2 direction, float angle)
        {
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            return new Vector2(
                cos * direction.X - sin * direction.Y,
                sin * direction.X + cos * direction.Y
            );
        }

    }
}

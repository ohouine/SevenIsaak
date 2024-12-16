using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SevenIsaak.Class.Sprite;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenIsaak.Class.Attacks;
using SevenIsaak.Class.Decor;
using SevenIsaak.Class.Utility;

namespace SevenIsaak.Class.Character
{
    abstract class Character
    {

        bool _isDead;
        public virtual bool isDead
        {
            get { return _isDead; }
            set
            {
                _isDead = value;
            }
        }
        float _life;
        public float life { get => _life; set {
                if (value <= 0)
                {
                    isDead = true;
                }
                _life = value; 
            } }

        protected float speed;
        protected float speedMultiplier;

        public Vector2 position;
        protected Vector2 previousPosition;
        protected Vector2 velocity;

        //percent of untouchable mass inside rectangle
        protected int? colisionZoneX = null;
        protected int? colisionZoneY = null;

        protected AnimationManager animationManager = new AnimationManager();
        public Rectangle rectangle { get => new Rectangle((int)position.X, (int)position.Y, animationManager.currentAnimeSprite.frameWidth, animationManager.currentAnimeSprite.frameHeight); }
        public bool existInRoom;
        public Character(float life, float speed, bool existInRoom = true)
        {
            this.life = life;
            this.speed = speed;
            this.existInRoom = existInRoom;
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        DrawForDebug drawForDebug = new DrawForDebug();
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationManager.Draw(gameTime, spriteBatch, position);

            //drawForDebug.DrawRectangle(rectangle,5,Color.Black,spriteBatch);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptedPosition"></param>
        /// <returns></returns>
        protected void HandleColision()
        {
            foreach (var obstacle in Manager.obstacles)
            {

                while (rectangle.Intersects(obstacle.rectangle))
                {

                    // Calculate the depth of intersection on the X-axis
                    float intersectionDepthX = Math.Min(rectangle.Right, obstacle.rectangle.Right) - Math.Max(rectangle.Left, obstacle.rectangle.Left);

                    // Calculate the depth of intersection on the Y-axis
                    float intersectionDepthY = Math.Min(rectangle.Bottom, obstacle.rectangle.Bottom) - Math.Max(rectangle.Top, obstacle.rectangle.Top);

                    // Determine which axis the collision is primarily on
                    if (Math.Abs(intersectionDepthX) < Math.Abs(intersectionDepthY))
                    {
                        position.X = (position.X > obstacle.rectangle.X) ? obstacle.rectangle.Right + 5 : obstacle.rectangle.Left - rectangle.Width;
                    }
                    else
                    {
                        position.Y = (position.Y < obstacle.rectangle.Y) ? obstacle.rectangle.Top - rectangle.Height : obstacle.rectangle.Bottom;
                    }
                }
            }
        }
    }
}

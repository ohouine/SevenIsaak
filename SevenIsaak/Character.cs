using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenIsaak
{
    abstract class Character
    {
        public static List<Character> characters = new List<Character>();
        protected bool isDead;
        protected int life;
        protected float speed;
        protected Vector2 position;
        protected AnimationManager animationManager = new AnimationManager();
        public Rectangle rectange { get => new Rectangle((int)position.X, (int)position.Y, animationManager.currentAnimeSprite.frameWidth, animationManager.currentAnimeSprite.frameHeight); }

        public Character(int life, float speed)
        {
            this.life = life;
            this.speed = speed;
            characters.Add(this);
        }

        public virtual void Update(GameTime gameTime)
        {

        }

        public virtual void GetHit(Attack attack)
        {

        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}

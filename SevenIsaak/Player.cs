using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SpriteBatch = Microsoft.Xna.Framework.Graphics.SpriteBatch;

namespace SevenIsaak
{
    class Player : Character
    {

        public static List<Player> players = new List<Player>();   

        Dictionary<string, int> controlls = new Dictionary<string, int>()
        {
            { NameMapping.MOVE_UP , (int)Keys.W },
            { NameMapping.MOVE_DOWN , (int)Keys.S },
            { NameMapping.MOVE_UP , (int)Keys.W },
            { NameMapping.MOVE_UP , (int)Keys.W },

        };
        public Player(int life, float speed, Texture2D texture2D, Dictionary<string, int> optControll = null) : base(life, speed)
        {
            animationManager.Add(NameMapping.MOVE_UP, new AnimeSprite(texture2D, 9, 50, 62, 62, 2, 9));
            animationManager.Add(NameMapping.MOVE_DOWN, new AnimeSprite(texture2D, 9, 50, 62, 62, 2, 11));
            animationManager.Add(NameMapping.MOVE_RIGHT, new AnimeSprite(texture2D, 9, 50, 62, 62, 2, 12));
            animationManager.Add(NameMapping.MOVE_LEFT, new AnimeSprite(texture2D, 9, 50, 62, 62, 2, 10));
            if (optControll != null)
            {
                controlls = optControll;
            }
            
            players.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            //handle player moves
            if (Keyboard.GetState().IsKeyDown((Microsoft.Xna.Framework.Input.Keys)controlls[NameMapping.MOVE_UP]))
            {
                animationManager.changeAnimation(NameMapping.MOVE_UP);
                this.position.Y -= this.speed;
            }
            if (Keyboard.GetState().IsKeyDown((Microsoft.Xna.Framework.Input.Keys)controlls[NameMapping.MOVE_DOWN]))
            {
                animationManager.changeAnimation(NameMapping.MOVE_DOWN);
                this.position.Y += this.speed;
            }
            if (Keyboard.GetState().IsKeyDown((Microsoft.Xna.Framework.Input.Keys)controlls[NameMapping.MOVE_RIGHT]))
            {
                animationManager.changeAnimation(NameMapping.MOVE_RIGHT);
                this.position.X += this.speed;
            }
            if (Keyboard.GetState().IsKeyDown((Microsoft.Xna.Framework.Input.Keys)controlls[NameMapping.MOVE_LEFT]))
            {
                animationManager.changeAnimation(NameMapping.MOVE_LEFT);
                this.position.X -= this.speed;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
    }
}

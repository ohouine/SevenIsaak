using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.ActiveDirectory;
using SevenIsaak.Class.Character;

namespace SevenIsaak.Class.Decor
{
    internal class Obstacle
    {

        Texture2D _image;
        Microsoft.Xna.Framework.Vector2 _position;
        Microsoft.Xna.Framework.Vector2 _size;
        public Rectangle rectangle { get => new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y); }
        public bool existInRoom;

        public Obstacle(Texture2D image, Microsoft.Xna.Framework.Vector2 position, Microsoft.Xna.Framework.Vector2 size, bool existInRoom = true)
        {
            _image = image;
            _position = position;
            _size = size;
            this.existInRoom = existInRoom;
            Manager.obstacles.Add(this);
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!existInRoom) return;
            spriteBatch.Draw(_image, _position, Color.White);
        }

    }
}

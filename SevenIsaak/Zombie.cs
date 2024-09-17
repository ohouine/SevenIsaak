using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenIsaak
{
    class Zombie : Enemy
    {
        List<Zombie> zombies = new List<Zombie>();

        public Zombie(int life, float speed, Texture2D texture2D, float scale, int nbFrame, int holdFrmTime) : base(life, speed, texture2D, scale, nbFrame, holdFrmTime)
        {
            zombies.Add(this);

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenIsaak
{
    class Enemy : Character
    {
        static public List<Enemy> enemys = new List<Enemy>();

        public Enemy(int life, float speed, Texture2D texture2D, float scale, int nbFrame, int holdFrmTime) : base(life, speed)
        {
            enemys.Add(this);
        }
    }
}

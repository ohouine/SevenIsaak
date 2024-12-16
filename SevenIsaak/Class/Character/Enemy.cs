using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SevenIsaak.Class.Decor;
using SevenIsaak.Class.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SevenIsaak.Class.Character
{
    class Enemy : Character
    {
        protected Pathfinding pathfinding;
        protected int pathCalculatingLatency = new Random().Next(20,100);
        int currentLatency;
        Vector2 direction;
        List<Vector2> nodes = new List<Vector2>();

        public Enemy(int life, float speed, float scale, bool existInRoom) : base(life, speed, existInRoom)
        {
            
            currentLatency = pathCalculatingLatency;
        }
        DrawForDebug drawForDebug = new DrawForDebug();
        protected void targetPlayer(Player player, GameTime gameTime)
        {
            if (pathfinding != null)
            {
                //calcule nodes every pathCalculatingLatency
                if (currentLatency >= pathCalculatingLatency || nodes.Count <= 0)
                {
                    nodes = pathfinding.FindPath(position, player.position);
                    currentLatency = 0;
                }
                else currentLatency++;

                //follow player if there is no node
                direction = (nodes.Count > 0 && nodes[0] != position) ? nodes[0] - position : player.position - position;

                // Normalize the direction (so it doesn't depend on distance)
                if (direction != Vector2.Zero)
                {
                    direction.Normalize();
                }

                position += direction * speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (nodes.Count > 0)
                {
                    if (Vector2.Distance(position, nodes[0]) < speed * gameTime.ElapsedGameTime.TotalSeconds) // Example threshold of 10 pixels
                    {
                        nodes.RemoveAt(0);
                    }
                }
            }
        }
    }
}

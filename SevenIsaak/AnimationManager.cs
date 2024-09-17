using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace SevenIsaak
{
    class AnimationManager
    {
        Dictionary<string, AnimeSprite> sprites = new Dictionary<string, AnimeSprite>();
        //set currentAnimeSprite on change
        string currentAnimation { get { return currentAnimation; } set { _currentAnimeSprite = sprites[value]; currentAnimation = value; } }
        AnimeSprite _currentAnimeSprite;
        public AnimeSprite currentAnimeSprite { get => _currentAnimeSprite;}

        public void changeAnimation(string animation)
        {
            sprites[currentAnimation].Stop();
            if (sprites.ContainsKey(animation))
            {
                currentAnimation = animation;
                sprites[currentAnimation].Reset();
                sprites[animation].Start();
            }
        }

        public void Add(string key, AnimeSprite animation) => sprites.Add(key, animation);
    }
}

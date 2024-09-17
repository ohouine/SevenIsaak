using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;

namespace SevenIsaak
{
    class AnimeSprite
    {
        private float _scale;

        private int _time;
        private int _threshold;

        private int _frameWidth;
        private int _frameHeight;
        private int _gap;
        private int _nbFrames;
        private Rectangle[] _spriteFrames;
        private int _selectedSpriteFrame;
        private int _ypositionOnSpriteBatsh;

        private bool _stop = true;

        public Texture2D texture2D;

        public int frameWidth { get => _frameWidth; set => _frameWidth = value; }
        public int frameHeight { get => _frameHeight; set => _frameHeight = value; }

        public AnimeSprite(Texture2D texture2D, int nbFrame, int holdFrmTime, int height, int width, int gab, int row, float scale = 1)
        {
            this.texture2D = texture2D;
            _scale = scale;
            _nbFrames = nbFrame;

            _frameWidth = width;
            _frameHeight = height;
            _gap = gab;

            _threshold = holdFrmTime;
            _spriteFrames = new Rectangle[_nbFrames];
            _ypositionOnSpriteBatsh = row * height + _gap * row;

            this.SplitImage();
        }

        /// <summary>
        /// Sépare chaque frame du SpriteBatch
        /// </summary>
        private void SplitImage()
        {
            for (int i = 0; i < _nbFrames; i++)
            {
                _spriteFrames[i] = new Rectangle(i * _frameWidth + _gap * i, _ypositionOnSpriteBatsh , _frameWidth, _frameHeight);
            }
        }

        public void SelectStartingSprite(int sprite)
        {
            _selectedSpriteFrame = sprite;
        }
        public void Update(GameTime gameTime)
        {
            if (_time >= _threshold)
            {
                if (_selectedSpriteFrame == _spriteFrames.Length - 1)
                {
                    _selectedSpriteFrame = 0;
                }
                else
                {
                    _selectedSpriteFrame += 1;
                }

                _time = 0;
            }
            else
            {
                if (_stop == false)
                {
                    _time += (int)gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        public void Reset() { 
            _selectedSpriteFrame = 0; 
            _time = 0;
        }
        public void Start() => _stop = false;
        public void Stop() => _stop = true;

        /// <summary>
        /// Retourne la hauteur d'une frame en pixels
        /// </summary>
        /// <returns>Hauteur du Sprite en pixels</returns>
        public int GetCurrentHeigt()
        {
            return Convert.ToInt32(Math.Floor(_frameHeight * _scale));
        }

        /// <summary>
        /// Retourne la largeur d'une frame en pixels
        /// </summary>
        /// <returns>Largeur du Sprite en pixels</returns>
        public int GetCurrentWidht()
        {
            return Convert.ToInt32(Math.Floor(_frameWidth * _scale));
        }
    }

}

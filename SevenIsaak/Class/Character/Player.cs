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
using SevenIsaak.Class.Utility;
using SevenIsaak.Class.Sprite;
using SevenIsaak.Class.Decor;


namespace SevenIsaak.Class.Character
{
    class Player : Character
    {

        Dictionary<string, Stats> _stats = new Dictionary<string, Stats>();

        Dictionary<string, int> _controlls = new Dictionary<string, int>()
        {
            { NameMapping.MOVE_UP , (int)Keys.W },
            { NameMapping.MOVE_DOWN , (int)Keys.S },
            { NameMapping.MOVE_RIGHT , (int)Keys.D },
            { NameMapping.MOVE_LEFT , (int)Keys.A },
            { NameMapping.SHOOT_UP , (int)Keys.Up },
            { NameMapping.SHOOT_DOWN , (int)Keys.Down },
            { NameMapping.SHOOT_RIGHT , (int)Keys.Right },
            { NameMapping.SHOOT_LEFT , (int)Keys.Left },
        };

        private string _lastMovement = NameMapping.NONE;
        string _currentMovement;
        const string FIRING = "firing";

        public Player(int life, float speed, Texture2D texture2D, Texture2D projectileTexture, Dictionary<string, int> optControll = null) : base(life, speed)
        {
            animationManager.Add(NameMapping.MOVE_UP, new AnimeSprite(texture2D, 9, 50, 64, 64, 8));
            animationManager.Add(NameMapping.MOVE_DOWN, new AnimeSprite(texture2D, 9, 50, 64, 64, 10));
            animationManager.Add(NameMapping.MOVE_RIGHT, new AnimeSprite(texture2D, 9, 50, 64, 64, 11));
            animationManager.Add(NameMapping.MOVE_LEFT, new AnimeSprite(texture2D, 9, 50, 64, 64, 9));

            animationManager.changeAnimation(NameMapping.MOVE_DOWN);
            _currentMovement = NameMapping.MOVE_DOWN;
            _lastMovement = NameMapping.MOVE_DOWN;

            if (optControll != null) _controlls = optControll;

            _stats.Add(FIRING, new Firing(projectileTexture, 50, 2, 300, 200));

            position = new Vector2(0, 0);

            Manager.players.Add(this);
            Manager.characters.Add(this);
        }

        public override void Update(GameTime gameTime)
        {
            if (!existInRoom) return;

            Firing firing = (Firing)_stats[FIRING]; firing.Update(gameTime);

            HandleMovement((float)gameTime.ElapsedGameTime.TotalSeconds);
            HandelShooting((float)gameTime.ElapsedGameTime.TotalSeconds);
            animationManager.currentAnimeSprite.Update(gameTime);
        }

        public void HandleMovement(float deltaTime)
        {

            bool move = false;
            var keyboardState = Keyboard.GetState(); // Cache the keyboard state to avoid multiple calls

            if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_UP]))
            {
                _currentMovement = NameMapping.MOVE_UP;
                velocity.Y = -1;
                move = true;
            }
            else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_DOWN]))
            {
                _currentMovement = NameMapping.MOVE_DOWN;
                velocity.Y = 1;
                move = true;
            }
            else
            {
                velocity.Y = 0;
            }

            if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_RIGHT]))
            {
                _currentMovement = NameMapping.MOVE_RIGHT;
                velocity.X = 1;
                move = true;
            }
            else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.MOVE_LEFT]))
            {
                _currentMovement = NameMapping.MOVE_LEFT;
                velocity.X = -1;
                move = true;
            }
            else
            {
                velocity.X = 0;
            }

            if (velocity.Length() > 0)
            {
                velocity.Normalize();
            }

            // Apply movement to position based on speed and deltaTime
            position += velocity * speed * deltaTime;


            if (move)
            {
                animationManager.currentAnimeSprite.Start();
            }
            else
            {
                animationManager.currentAnimeSprite.Stop();
            }
            previousPosition = position;
            HandleColision();

            // Only perform movement if the current movement is different from the last one
            if (_currentMovement != NameMapping.NONE && _currentMovement != _lastMovement)
            {
                animationManager.changeAnimation(_currentMovement);
                // Update the last movement
                _lastMovement = _currentMovement;
            }

        }

        public void HandelShooting(float deltaTime)
        {
            if (_stats[FIRING] != null)
            {
                Firing firing = (Firing)_stats[FIRING];
                if (!firing.canFire) return;

                var keyboardState = Keyboard.GetState(); // Cache the keyboard state to avoid multiple calls
                Vector2 direction = new Vector2(0, 0);
                if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.SHOOT_UP]))
                {
                    direction.Y = -1;
                }
                else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.SHOOT_DOWN]))
                {
                    direction.Y = 1;

                }
                else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.SHOOT_RIGHT]))
                {
                    direction.X = 1;
                }
                else if (keyboardState.IsKeyDown((Keys)_controlls[NameMapping.SHOOT_LEFT]))
                {
                    direction.X = -1;
                }
                else
                {
                    return;
                }


                if (direction.Length() > 0)
                {
                    direction.Normalize();
                }

                firing.Fire(position, direction, this, velocity);
            }
        }
    }
}

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SevenIsaak.Class.Sprite;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenIsaak.Class.Character;
using SevenIsaak.Class.Decor;

namespace SevenIsaak.Class.Attacks
{
    class Projectile : Attack
    {

        Texture2D _texture;

        Character.Character _shooter;
        Firing _info;
        float _speed;

        bool _Spectral = false;//mean it can move trough obstacle
        int _piercing = 1;//number of ennemy he can go trough

        Vector2 _direction;
        Vector2 _position;
        Vector2 _size = new Vector2(25, 25);
        public Rectangle rectangle { get => new Rectangle((int)_position.X, (int)_position.Y, (int)_size.X, (int)_size.Y); }
        public bool destroyed = false;
        public Projectile(Texture2D texture, Vector2 position, Vector2 direction, Character.Character shooter, Firing info, float speed)
        {
            this._texture = texture;
            this._position = position;
            this._direction = direction;
            this._shooter = shooter;
            _info = info;

            Manager.projectiles.Add(this);
            _speed = speed;
        }

        float distance = 0;//distance in pixel that have been traveled
        public void Update(GameTime gameTime)
        {
            this._position.X += _speed * _direction.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
            this._position.Y += _speed * _direction.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;

            distance += (_speed * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (distance >= _info.range)
            {
                destroyed = true;
            }

            CheckColision();
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, new Vector2(0, 0), new Vector2(0.01f, 0.01f), SpriteEffects.None, 1);
        }

        public void CheckColision()
        {
            if (_shooter is Player)
            {

                foreach (Enemy enemy in Manager.enemys)
                {
                    if (enemy.existInRoom)
                    {

                        if (rectangle.Intersects(enemy.rectangle))
                        {
                            enemy.life -= _info.damage;
                            _piercing--;
                            if (_piercing <= 0) destroyed = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Player player in Manager.players)
                {
                    if (player.existInRoom)
                    {
                        if (rectangle.Intersects(player.rectangle))
                        {
                            player.life -= _info.damage;
                            _piercing--;
                            if (_piercing <= 0) destroyed = true;
                        }
                    }
                }
            }

            foreach (var obstacle in Manager.obstacles)
            {
                if (obstacle.existInRoom)
                {
                    if (rectangle.Intersects(obstacle.rectangle))
                    {

                        if (!_Spectral)
                        {
                            destroyed = true;
                        }
                    }
                }
            }
        }

    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MySql.Data.MySqlClient;
using SevenIsaak.Class.Decor;
using SevenIsaak.Class.Sprite;
using SevenIsaak.Class.Utility;
using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mysqlx.Crud;

namespace SevenIsaak.Class.Character
{
    class Slime : Enemy
    {

        public int remainingLife = 0;
        public int division = 2;
        int maxLife;
        Texture2D texture2D;
        public override bool isDead
        {
            get => base.isDead;
            set
            {
                if (value == true)
                {

                    if (remainingLife > 0)
                    {
                        Random random = new Random();
                        for (int i = 0; i < division; i++)
                        {
                            Slime slime = new Slime((int)(maxLife * 0.70), (int)(speed + speed * 0.3), texture2D);
                            slime.position.X = position.X + random.Next(-50, 50);
                            slime.position.Y = position.Y + random.Next(-50, 50);
                            slime.remainingLife = remainingLife - 1;
                        }
                    }

                    string connectionString = "Server=localhost;Database=SevenIsaac;User ID=root;Password=Kiouma2.2;";
                    string query = "SELECT Score FROM User WHERE UserName = '" + File.ReadAllText("localData.txt") + "'";

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();

                            int score = 0;
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {
                                using (MySqlDataReader reader = command.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        score = Convert.ToInt32(reader["Score"]);
                                    }

                                };
                            }
                            query = $"UPDATE User SET Score = {score + 1} " +
                                $"WHERE UserName = '{File.ReadAllText("localData.txt")}'; ";
                            using (MySqlCommand command = new MySqlCommand(query, connection))
                            {

                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                    Debug.WriteLine("Data inserted successfully!");
                                else
                                    Debug.WriteLine("No rows were inserted.");
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"An error occurred: {ex.Message}");
                        }
                    }

                }
                base.isDead = value;
            }
        }
        public Slime(int life, float speed, Texture2D texture2D, bool existInRoom = true) : base(life, speed, life / 100 * 3, existInRoom)
        {
            float scale = life / 100 * 2;
            maxLife = life;
            this.texture2D = texture2D;

            Manager.toAddSlime.Add(this);


            animationManager.Add(NameMapping.MOVE_UP, new AnimeSprite(texture2D, 6, 150, 32, 32, 2, scale));
            animationManager.Add(NameMapping.MOVE_DOWN, new AnimeSprite(texture2D, 6, 150, 32, 32, 0, scale));
            animationManager.Add(NameMapping.MOVE_RIGHT, new AnimeSprite(texture2D, 6, 150, 32, 32, 1, scale, true));
            animationManager.Add(NameMapping.MOVE_LEFT, new AnimeSprite(texture2D, 6, 150, 32, 32, 1, scale));

            animationManager.changeAnimation(NameMapping.MOVE_DOWN);
            position = new Vector2(300, 300);
            pathfinding = new Pathfinding(Manager.obstacles, 57);
            //HandleColision();
        }

        int currentTargetIndex = 0;
        public override void Update(GameTime gameTime)
        {
            if (!existInRoom) return;

            targetPlayer(Manager.players[currentTargetIndex], gameTime);
            HandleColision();
            animationManager.currentAnimeSprite.Update(gameTime);
        }
    }
}

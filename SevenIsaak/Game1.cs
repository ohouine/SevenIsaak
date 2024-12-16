using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MySql.Data.MySqlClient;
using SevenIsaak.Class.Attacks;
using SevenIsaak.Class.Character;
using SevenIsaak.Class.Decor;
using SharpDX.Direct3D9;
using System;
using System.IO;
using System.Drawing;
using Color = Microsoft.Xna.Framework.Color;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace SevenIsaak
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Manager _manager = new Manager();

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        void CreateUserInDB()
        {
            string connectionString = "Server=localhost;Database=SevenIsaac;User ID=root;Password=Kiouma2.2;";
            string query = "INSERT INTO User(UserName,Score) VALUES('" + File.ReadAllText("localData.txt") + "',0)";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

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

        /// <summary>
        /// use a username randomly uniq store in a file. if file doesnt exist or user in doesnt exit in db it is created and store in db.
        /// </summary>
        void HandleUser()
        {

            string connectionString = "Server=localhost;Database=SevenIsaac;User ID=root;Password=Kiouma2.2;";
            string username;

            if (!File.Exists("localData.txt") || File.ReadAllText("localData.txt") == "")
            {
                username = Guid.NewGuid().ToString() + "_" + DateTime.UtcNow.ToString("yyyyMMddHHmmss");
                File.WriteAllText("localData.txt", username);
                CreateUserInDB();
            }
            else
            {
                string query = "SELECT COUNT(*) FROM User WHERE UserName = '" + File.ReadAllText("localData.txt")+"'";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        Debug.WriteLine("Connected to the database.");

                        using (MySqlCommand command = new MySqlCommand(query, connection))
                        {

                            int userCount = Convert.ToInt32(command.ExecuteScalar());

                            if (userCount < 1)
                            {
                                CreateUserInDB();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred: {ex.Message}");
                    }
                }
            }
        }

        protected override void LoadContent()
        {
            HandleUser();

            _graphics.PreferredBackBufferWidth = 1920;    // Set the desired width
            _graphics.PreferredBackBufferHeight = 1080;  // Set the desired height
            _graphics.IsFullScreen = false;           // Set fullscreen mode
            _graphics.ApplyChanges();

            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //new Room(2,Content).Show();
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(100, 100), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(150, 150), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(30, 70), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(10, 75), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(200, 500), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(50, 50), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(65, 72), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(250, 250), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(500, 500), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(346, 745), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(90, 240), new Vector2(32, 32));
            new Obstacle(Content.Load<Texture2D>("Rock"), new Vector2(400, 400), new Vector2(32, 32));
            new Player(100, 150, Content.Load<Texture2D>("PlayerSpriteBatsh"), Content.Load<Texture2D>("ninjaStar"));
            new Slime(150, 70f, Content.Load<Texture2D>("Slime")).position = new Vector2(800, 800);
            new Slime(100, 70f, Content.Load<Texture2D>("Slime")).position = new Vector2(700, 500);
            new Slime(200, 70f, Content.Load<Texture2D>("Slime")).position = new Vector2(20, 150);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _manager.UpdateCharacter(gameTime);
            _manager.UpdateProjectile(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            _manager.DrawCharacter(gameTime, _spriteBatch);
            _manager.DrawProjectile(gameTime, _spriteBatch);
            _manager.DrawObstacle(gameTime, _spriteBatch);

            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}

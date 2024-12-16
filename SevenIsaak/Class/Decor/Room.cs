using SevenIsaak.Class.Character;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json.Serialization;
using Microsoft.Xna.Framework;
using System.Xml.Linq;
using System.Numerics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace SevenIsaak.Class.Decor
{
    class Room
    {
        public List<Room> RoomList;
        public List<Enemy> EnemyList;
        public List<Obstacle> ObstacleList;

        int _roomNumber;
        public bool isClear = false;
        ContentManager _content;

        int height = 32;
        int width = 32;

        public Room(int roomNumber, ContentManager Content)
        {
            this._roomNumber = roomNumber;
            this._content = Content;
        }

        //NEED : find a way to get the texture(image) into this class

        public void Show()
        {
            string filePath = Path.Combine("../../../../", "Maps", "Maps", $"Map{_roomNumber}.json");


            // Read the JSON data from the file
            string json = File.ReadAllText(filePath);

            // Parse the JSON
            JObject jsonObject = JObject.Parse(json);

            // Extract the "data" array from the first layer
            JArray dataArray = (JArray)jsonObject["layers"][0]["data"];

            // Convert to int array
            List<int> dataList = new List<int>();
            foreach (var item in dataArray)
            {
                dataList.Add(item.Value<int>());
            }

            Vector2 position = new Vector2(0, 0);
            int x = 0;
            int y = 0;
            for (int i = 0; i < dataList.Count; i++, x++)
            {
                if (i % 60 == 0) y++;

                if (dataList[i] == 19)
                    new Obstacle(_content.Load<Texture2D>("Rock"), new Vector2(x * width, y * height), new Vector2(32, 32));
                else if (dataList[i] == 1)
                {
                    Slime slime = new Slime(200, 70f, _content.Load<Texture2D>("Slime"));
                    slime.position = new Vector2(x * width, y * height);
                }

            }

        }

        public void Hide()
        {

        }
    }
}

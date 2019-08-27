using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace DownwellClone
{
    class Cloud
    {
        public static Texture2D cloudTexture;

        float startX = 20;
        public float startY;
        float targetY;

        Vector2 scale;

        float scaleFactor = 2;

        Random rand = new Random();

        public bool Hit(CharacterEntity player)
        {
            if (this.X <= (player.X + cloudTexture.Width /2) && this.X >= (player.X - cloudTexture.Width /2) && this.Y <= (player.Y + 30) && this.Y >= (player.Y + 10))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool Reset(float currentHeight)
        {
            if (currentHeight <= (targetY - cloudTexture.Height))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float X
        {
            get;
            set;
        }

        public float Y
        {
            get;
            set;
        }

        public Cloud(GraphicsDevice graphicsDevice, Texture2D texture)
        {
            cloudTexture = texture;

            scale = new Vector2(cloudTexture.Width / (cloudTexture.Width * scaleFactor), cloudTexture.Height / (cloudTexture.Height * scaleFactor));

            startX = rand.Next(0, graphicsDevice.Viewport.Width - cloudTexture.Width);
            startY = graphicsDevice.Viewport.Height + cloudTexture.Height;

            this.X = startX;
            this.Y = startY;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, float scrollSpeed)
        {
            //The height in the game that the cloud has to travel to
            targetY = 0;

            this.X = startX;
            this.Y -= ((float)gameTime.ElapsedGameTime.TotalSeconds * graphicsDevice.Viewport.Height) * scrollSpeed;

            if (this.Y <= targetY - cloudTexture.Height)
            {
                this.Y = startY;
                startX = rand.Next(0, graphicsDevice.Viewport.Width - cloudTexture.Width);
                this.X = startX;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.X, this.Y);
            Color tintColor = Color.DarkBlue;

            spriteBatch.Draw(cloudTexture, position: position, scale: scale, color: tintColor);
        }
    }
}

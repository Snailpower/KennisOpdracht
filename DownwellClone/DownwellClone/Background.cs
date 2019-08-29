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
    class Background
    {
        public static Texture2D BGTexture;

        float startX = 0;
        float startY = 0;
        float targetY = 0;

        Vector2 scale;

        float scaleFactor = 2;

        public bool Reset(float currentHeight)
        {
            if (currentHeight <= (0 - (BGTexture.Height / 2)))
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

        public Background(GraphicsDevice graphicsDevice, Texture2D texture)
        {
            BGTexture = texture;

            this.X = startX;
            this.Y = startY;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, float scrollSpeed)
        {
            //The height in the game that the bg has to travel to for a reset
            targetY = 0 - (BGTexture.Height);

            //Move the BG upwards
            this.Y -= ((float)gameTime.ElapsedGameTime.TotalSeconds * graphicsDevice.Viewport.Height) * scrollSpeed;

            if (this.Y <= targetY)
            {
                this.Y = startY + (BGTexture.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(this.X, this.Y);

            spriteBatch.Draw(BGTexture, position: position);
        }
    }
}

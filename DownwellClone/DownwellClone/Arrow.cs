using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input.Touch;

namespace DownwellClone
{
    class Arrow
    {
        static Texture2D arrowTexture;

        float targetX = 50;
        float targetY;

        public float startY = 16;

        Vector2 scale;

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

        public bool Hit(Enemy enemy)
        {
            if (this.X <= (enemy.X + 16) && this.X >= (enemy.X - 16) && this.Y <= (enemy.Y + 16) && this.Y >= (enemy.Y - 16))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public Arrow(GraphicsDevice graphicsDevice, Texture2D texture, float StartX)
        {
            arrowTexture = texture;

            scale = new Vector2(2, 2);

            X = StartX;

            Y = startY;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, Enemy enemy)
        {
            this.Y += (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.WhiteSmoke;

            spriteBatch.Draw(arrowTexture, position: topLeftOfSprite, color: tintColor, scale: scale);
        }
    }
}

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
    class Pickup
    {
        static Texture2D pickupTexture;

        float targetX = 50;
        float targetY;

        public float startY = 840;

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

        public bool Hit(CharacterEntity player)
        {
            if (this.X <= (player.X + 40) && this.X >= (player.X - 40) && this.Y <= (player.Y + 40) && this.Y >= (player.Y - 40))
            {
                if (player.Ammo < 5)
                {
                    player.Ammo++;
                }
                
                if (player.Health < 3)
                {
                    player.Health++;
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public Pickup(GraphicsDevice graphicsDevice, Texture2D texture, float StartX)
        {
            pickupTexture = texture;

            scale = new Vector2(1.5f, 1.5f);

            X = StartX;

            Y = startY;
        }

        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.WhiteSmoke;

            spriteBatch.Draw(pickupTexture, position: topLeftOfSprite, color: tintColor, scale: scale);
        }
    }
}

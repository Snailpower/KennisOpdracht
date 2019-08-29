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
            //Detect if the pickup is close enough to the player for a hit
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

        //Starting values of a pickup
        public Pickup(GraphicsDevice graphicsDevice, Texture2D texture, float StartX)
        {
            pickupTexture = texture;

            scale = new Vector2(1.5f, 1.5f);

            X = StartX;

            Y = startY;
        }

        //Make the pickup fly upwards
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            this.Y -= (float)gameTime.ElapsedGameTime.TotalSeconds * 200;
        }

        //Drawing code for pickup
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.WhiteSmoke;

            spriteBatch.Draw(pickupTexture, position: topLeftOfSprite, color: tintColor, scale: scale);
        }
    }
}

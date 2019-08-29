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
    public class Enemy
    {
        static Texture2D characterSheetTexture;

        float targetX = 500;
        float targetY;

        public float startY = 840;

        Vector2 scale;

        Animation moveLeft;
        Animation moveRight;
        Animation moveUp;

        Animation currentAnimation;

        //Detect a hit with the top of the screen
        public bool Hit(float currentHeight)
        {
            if (currentHeight <= (targetY))
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

        //Get position of the player to move towards with a velocity, then normalize it
        Vector2 GetDesiredVelocityFromPlayerPos(CharacterEntity player)
        {
            Vector2 desiredVelocity = new Vector2();

            desiredVelocity.X = player.X - this.X;
            desiredVelocity.Y = player.Y - this.Y;

            if (desiredVelocity.X != 0 || desiredVelocity.Y != 0)
            {
                desiredVelocity.Normalize();
                const float desiredSpeed = 200;
                desiredVelocity *= desiredSpeed;
            }

            return desiredVelocity;
        }

        public Enemy(GraphicsDevice graphicsDevice, Texture2D spritesheet, string type)
        {
            characterSheetTexture = spritesheet;

            scale = new Vector2(targetX / characterSheetTexture.Width, targetX / characterSheetTexture.Width);

            Y = startY;

            //Defining which parts of the spritesheet need to be put in per animation and per enemy type (every individual frame is 16x16)
            
            if (type == "snake")
            {
                moveLeft = new Animation();
                moveLeft.AddFrame(new Rectangle(48, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(64, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(48, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(80, 96, 16, 16), TimeSpan.FromSeconds(.25));

                moveRight = new Animation();
                moveRight.AddFrame(new Rectangle(96, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(112, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(96, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(128, 96, 16, 16), TimeSpan.FromSeconds(.25));

                moveUp = new Animation();
                moveUp.AddFrame(new Rectangle(144, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(160, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(144, 96, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(176, 96, 16, 16), TimeSpan.FromSeconds(.25));
            }
            else if (type == "shroom")
            {
                moveLeft = new Animation();
                moveLeft.AddFrame(new Rectangle(48, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(64, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(48, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(80, 64, 16, 16), TimeSpan.FromSeconds(.25));

                moveRight = new Animation();
                moveRight.AddFrame(new Rectangle(96, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(112, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(96, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(128, 64, 16, 16), TimeSpan.FromSeconds(.25));

                moveUp = new Animation();
                moveUp.AddFrame(new Rectangle(144, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(160, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(144, 64, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(176, 64, 16, 16), TimeSpan.FromSeconds(.25));
            }
            else if (type == "blob")
            {
                moveLeft = new Animation();
                moveLeft.AddFrame(new Rectangle(48, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(64, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(48, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveLeft.AddFrame(new Rectangle(80, 112, 16, 16), TimeSpan.FromSeconds(.25));

                moveRight = new Animation();
                moveRight.AddFrame(new Rectangle(96, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(112, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(96, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveRight.AddFrame(new Rectangle(128, 112, 16, 16), TimeSpan.FromSeconds(.25));

                moveUp = new Animation();
                moveUp.AddFrame(new Rectangle(144, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(160, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(144, 112, 16, 16), TimeSpan.FromSeconds(.25));
                moveUp.AddFrame(new Rectangle(176, 112, 16, 16), TimeSpan.FromSeconds(.25));
            }
            currentAnimation = moveUp;
        }

        /// <summary>
        /// Moves enemies towards the player when entering the screen based on the player's position
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsDevice"></param>
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice, CharacterEntity player)
        {
            targetY = player.Y;

            var vp = graphicsDevice.Viewport;

            var velocity = GetDesiredVelocityFromPlayerPos(player);

            if (this.Y >= player.Y + 40)
            {
                this.X += velocity.X * (float)gameTime.ElapsedGameTime.TotalSeconds;
                this.Y += velocity.Y * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                this.Y -= 200 * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            

            currentAnimation.Update(gameTime, true);
        }

        /// <summary>
        /// Renders or "Draws" the player sprite
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 topLeftOfSprite = new Vector2(this.X, this.Y);
            Color tintColor = Color.WhiteSmoke;

            var sourceRectangle = currentAnimation.CurrentRectangle;

            spriteBatch.Draw(characterSheetTexture, position: topLeftOfSprite, sourceRectangle: sourceRectangle, color: tintColor, scale: scale);
        }


    }
}

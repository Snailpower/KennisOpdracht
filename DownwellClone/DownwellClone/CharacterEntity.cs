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
    public class CharacterEntity
    {
        static Texture2D characterSheetTexture;

        float targetX = 500;
        float targetY;
        Vector2 scale;

        Animation walkLeft;
        Animation walkRight;
        Animation standStill;

        Animation currentAnimation;

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

        public CharacterEntity(GraphicsDevice graphicsDevice, Texture2D spritesheet)
        {
            if (characterSheetTexture == null)
            {
                characterSheetTexture = spritesheet;
            }

            scale = new Vector2(targetX / characterSheetTexture.Width, targetX / characterSheetTexture.Width);
            targetY = characterSheetTexture.Height * scale.Y;

            //Defining which parts of the spritesheet need to be put in per animation (every individual frame is 16x16)
            walkLeft = new Animation();
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(64, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(48, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkLeft.AddFrame(new Rectangle(80, 0, 16, 16), TimeSpan.FromSeconds(.25));

            walkRight = new Animation();
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(112, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(96, 0, 16, 16), TimeSpan.FromSeconds(.25));
            walkRight.AddFrame(new Rectangle(128, 0, 16, 16), TimeSpan.FromSeconds(.25));

            standStill = new Animation();
            standStill.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standStill.AddFrame(new Rectangle(16, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standStill.AddFrame(new Rectangle(0, 0, 16, 16), TimeSpan.FromSeconds(.25));
            standStill.AddFrame(new Rectangle(32, 0, 16, 16), TimeSpan.FromSeconds(.25));
        }

        /// <summary>
        /// Makes sure that the player moves inbetween the game borders on left and right keypress and changes animation accordingly
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="graphicsDevice"></param>
        public void Update(GameTime gameTime, GraphicsDevice graphicsDevice)
        {
            var vp = graphicsDevice.Viewport;

            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Keys.Left) && this.X > vp.X)
            {
                this.X -= (float)gameTime.ElapsedGameTime.TotalSeconds * vp.Width;
                if (currentAnimation != walkLeft)
                {
                    currentAnimation = walkLeft;
                }
            }
                
            else if (state.IsKeyDown(Keys.Right) && this.X < (vp.Width - (currentAnimation.CurrentRectangle.Width * scale.X)))
            {
                this.X += (float)gameTime.ElapsedGameTime.TotalSeconds * vp.Width;
                if (currentAnimation != walkRight)
                {
                    currentAnimation = walkRight;
                }
            }

            else if (state.GetPressedKeys().Length == 0)
            {
                if (currentAnimation != standStill)
                {
                    currentAnimation = standStill;
                }
                
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
            Color tintColor = Color.White;

            if (currentAnimation == null)
            {
                currentAnimation = standStill;
            }
            var sourceRectangle = currentAnimation.CurrentRectangle;

            spriteBatch.Draw(characterSheetTexture, position: topLeftOfSprite, sourceRectangle: sourceRectangle, color: tintColor, scale: scale);
        }

        
    }
}

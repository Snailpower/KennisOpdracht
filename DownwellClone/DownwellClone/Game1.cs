using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D;
using System;

namespace DownwellClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int state = 0;

        Texture2D startScreen;
        CharacterEntity character;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 420;
            graphics.PreferredBackBufferHeight = 840;

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            character = new CharacterEntity(this.GraphicsDevice, this.Content.Load<Texture2D>("charactersheet"));

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            startScreen = this.Content.Load<Texture2D>("TitleScreen");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && state == 0)
                Exit();

            //Introscreen state of the game
            if (state == 0)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    state += 1;
                }
            }

            //Play state of the game
            else if (state == 1)
            {
                character.Update(gameTime, GraphicsDevice);
            }

            //Death or Game Over state of the game
            else if (state == 2)
            {

            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here

            //Introscreen state of the game
            if (state == 0)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(startScreen, position: Vector2.Zero);
                spriteBatch.End();
            }
            //Play state of the game
            else if (state == 1)
            {
                spriteBatch.Begin();
                character.Draw(spriteBatch);
                spriteBatch.End();
            }
            //Death or Game Over state of the game
            else if (state == 2)
            {

            }
            

            base.Draw(gameTime);
        }
    }
}

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using tainicom.Aether.Physics2D;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;

namespace DownwellClone
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //Monogame basic vars
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Music vars
        Song song;
        List<SoundEffect> soundEffects = new List<SoundEffect>();

        //Game state vars
        int state = 0;
        bool spacebarDown = false;
        bool pickupActive = false;
        float score;
        private SpriteFont font;

        //Vars for UI and menus
        Texture2D startScreen;
        Texture2D endScreen;
        UI userInterface;

        //Vars for all entities
        CharacterEntity character;
        List<Cloud> clouds = new List<Cloud>();
        List<Enemy> enemies = new List<Enemy>();
        List<Background> SkyBackground = new List<Background>();
        List<Arrow> arrows = new List<Arrow>();
        Arrow arrow;
        Pickup pickup;

        //Change pickuptimer to make powerups spawn at an interval of that float amount of seconds
        float pickupTimer = 8f;
        float currentPTime;

        //Set cloud amount here
        int cloudAmount = 4;
        Random randC = new Random();
        int currentCloud = 0;

        //Set enemy amount here
        int enemyAmount = 3;
        Random randE = new Random();
        int currentEnemy;
        List<String> types = new List<string>();

        int backgroundAmount = 2;
        int currentBG = 1;


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
            //Add your initialization logic here
            character = new CharacterEntity(this.GraphicsDevice, this.Content.Load<Texture2D>("charactersheet"));

            userInterface = new UI(this.GraphicsDevice, this.Content.Load<Texture2D>("healthbarFill"), this.Content.Load<Texture2D>("arrowFill"));

            //Give enemy types and load them up
            types.Add("snake");
            types.Add("shroom");
            types.Add("blob");

            for (int i = 0; i < types.Count; i++)
            {
                var newEnemy = new Enemy(this.GraphicsDevice, this.Content.Load<Texture2D>("charactersheet"), types[i]);

                newEnemy.X = randE.Next(16, 404);

                enemies.Add(newEnemy);
            }

            //Load up cloudamount of clouds
            for (int i = 0; i < cloudAmount; i++)
            {
                var newCloud = new Cloud(this.GraphicsDevice, this.Content.Load<Texture2D>("Cloud"));

                newCloud.X = randC.Next(100, 600);

                clouds.Add(newCloud);
            }

            for (int i = 0; i < backgroundAmount; i++)
            {
                var newBG = new Background(this.GraphicsDevice, this.Content.Load<Texture2D>("BGSky"));

                SkyBackground.Add(newBG);
            }

            pickup = new Pickup(this.GraphicsDevice, this.Content.Load<Texture2D>("lemon"), randE.Next(80, 300));

            currentPTime = pickupTimer;

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

            // Use this.Content to load your game content here
            startScreen = this.Content.Load<Texture2D>("TitleScreen");

            endScreen = this.Content.Load<Texture2D>("EndScreen");

            font = Content.Load<SpriteFont>("Segoe");

            this.song = Content.Load<Song>("Friends Theme Song (8 Bit Remix Cover Version) - 8 Bit Universe");

            soundEffects.Add(Content.Load<SoundEffect>("shoot"));
            soundEffects.Add(Content.Load<SoundEffect>("enemy"));
            soundEffects.Add(Content.Load<SoundEffect>("hit"));
            soundEffects.Add(Content.Load<SoundEffect>("gameover"));
            soundEffects.Add(Content.Load<SoundEffect>("pickup"));

            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.5f;
            //Looping the music here
            MediaPlayer.IsRepeating = true;
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        void Soundeffect(bool loop, int index)
        {
            if (!loop)
            {
                // Fire and forget play
                soundEffects[index].Play();
            }
            else
            {
                // Play that can be manipulated after the fact
                var instance = soundEffects[index].CreateInstance();
                instance.IsLooped = true;
                instance.Play();
            }
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //Close the game on escape press when in main menu
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) && state == 0)
                Exit();

            //Introscreen state of the game
            if (state == 0)
            {
                //Unpause the music if the game is restarted
                if (MediaPlayer.State == MediaState.Paused)
                {
                    MediaPlayer.Resume();
                }

                score = 0;

                //Start the game on spacebar
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spacebarDown)
                {
                    state += 1;
                    spacebarDown = true;
                }
                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    spacebarDown = false;
                }
            }

            //Play state of the game
            else if (state == 1)
            {
                //Listen for animation and button presses and update UI according to character values
                character.Update(gameTime, GraphicsDevice);
                userInterface.Update(character);

                //Increase score based on time playing
                score += (float)gameTime.ElapsedGameTime.TotalSeconds;

                //Kill the player and go to game over when hit too much
                if (character.Health <= 0)
                {
                    Soundeffect(false, 3);
                    state = 2;
                }

                //Give the player health and ammo when picking up the powerup, then reset the position and wait for the timer
                if (pickup.Hit(character) || !pickup.Hit(character) && pickup.Y <= -20)
                {
                    if (pickup.Hit(character))
                    {
                        Soundeffect(false, 4);
                    }
                    pickupActive = false;
                    pickup.Y = 840;
                    pickup.X = randE.Next(80, 300);
                }

                //Activate a powerup when the timer reaches zero
                currentPTime -= (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (pickupActive)
                {
                    pickup.Update(gameTime, GraphicsDevice);
                }

                if (currentPTime <= 0)
                {
                    pickupActive = true;
                    currentPTime = pickupTimer;
                }
                
                //Put one background slightly above the other to connect the seems to create a scrolling effect
                if (SkyBackground[0].Y <= 0)
                {
                    SkyBackground[1].Y = SkyBackground[0].Y + this.Content.Load<Texture2D>("BGSky").Height;
                }
                else if (SkyBackground[0].Reset(SkyBackground[0].Y))
                {
                    SkyBackground[0].Y = SkyBackground[1].Y + this.Content.Load<Texture2D>("BGSky").Height;
                }

                SkyBackground[0].Update(gameTime, GraphicsDevice, 0.3f);
                SkyBackground[1].Update(gameTime, GraphicsDevice, 0.3f);

                //Spawn clouds and detect a hit with the player's hitbox.
                //Play a sound when hit, and spawn a new cloud when the cloud reaches the top of the screen
                for (int i = 0; i < clouds.Count; i++)
                {
                    if (i == currentCloud)
                    {
                        clouds[i].Update(gameTime, GraphicsDevice, 1f);
                    }
                    else if (clouds[currentCloud].Hit(character))
                    {
                        Soundeffect(false, 2);
                        character.Health--;

                        clouds[currentCloud].X = randC.Next(100, 600);
                        clouds[currentCloud].Y = clouds[currentCloud].startY;

                        if (currentCloud < cloudAmount - 1)
                        {
                            currentCloud++;
                        }
                        else
                        {
                            currentCloud = 0;
                        }
                    }
                    else if (clouds[currentCloud].Reset(clouds[currentCloud].Y))
                    {
                        clouds[currentCloud].X = randC.Next(100, 600);

                        if (currentCloud < cloudAmount - 1)
                        {
                            currentCloud++;
                        }
                        else
                        {
                            currentCloud = 0;
                        }
                    }
                }

                //Spawn enemies and detect a hit with the player if they are within the hitbox
                //Spawn new enemy when one reaches the top of the screen
                for (int i = 0; i < enemies.Count; i++)
                {
                    if (i == currentEnemy)
                    {
                        enemies[i].Update(gameTime, GraphicsDevice, character);
                        
                    }
                    else if (enemies[currentEnemy].Hit(enemies[currentEnemy].Y))
                    {

                        if (enemies[currentEnemy].X <= (character.X + 50) && enemies[currentEnemy].X >= (character.X - 50))
                        {
                            Soundeffect(false, 2);
                            character.Health--;
                        }

                        enemies[currentEnemy].X = randE.Next(16, 404);
                        enemies[currentEnemy].Y = enemies[currentEnemy].startY;
                        

                        if (currentEnemy < enemyAmount - 1)
                        {
                            currentEnemy++;
                        }
                        else
                        {
                            currentEnemy = 0;
                        }
                    }
                }

                //Shoot an arrow with a sound effect
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spacebarDown && character.Ammo > 0)
                {
                    Soundeffect(false, 0);

                    arrows.Add(new Arrow(this.GraphicsDevice, this.Content.Load<Texture2D>("arrow"), character.X));

                    spacebarDown = true;

                    character.Ammo--;
                }

                if (Keyboard.GetState().IsKeyUp(Keys.Space))
                {
                    spacebarDown = false;
                }

                //When an arrow hits or gets outside of the screen, remove it
                for (int i = 0; i < arrows.Count; i++)
                {
                    arrows[i].Update(gameTime, this.GraphicsDevice, enemies[currentEnemy]);

                    if (arrows[i].Y >= 840)
                    {
                        arrows.Remove(arrows[i]);
                    }

                    //Play a sound effect and kill the current enemy + spawn a new enemy
                    if (arrows.Count > 0 && arrows[i].Hit(enemies[currentEnemy]))
                    {
                        Soundeffect(false, 2);
                        arrows.Remove(arrows[i]);

                        enemies[currentEnemy].X = randE.Next(16, 404);
                        enemies[currentEnemy].Y = enemies[currentEnemy].startY;

                        if (currentEnemy < enemyAmount - 1)
                        {
                            currentEnemy++;
                        }
                        else
                        {
                            currentEnemy = 0;
                        }
                    }
                }

                
            }

            //Death or Game Over state of the game
            else if (state == 2)
            {
                //Pause the music
                if (MediaPlayer.State == MediaState.Playing)
                {
                    MediaPlayer.Pause();
                }
                
                //Reset the game with spacebar
                if (Keyboard.GetState().IsKeyDown(Keys.Space) && !spacebarDown)
                {
                    character.Health = 3;
                    character.Ammo = 5;
                    state = 0;
                    spacebarDown = true;
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //Background color
            GraphicsDevice.Clear(Color.CadetBlue);

            // Add your drawing code here

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
                for (int i = 0; i < SkyBackground.Count; i++)
                {
                    SkyBackground[i].Draw(spriteBatch);
                }
                for (int i = 0; i < enemies.Count; i++)
                {
                    enemies[i].Draw(spriteBatch);
                }

                character.Draw(spriteBatch);

                for (int i = 0; i < clouds.Count; i++)
                {
                    clouds[i].Draw(spriteBatch);
                }

                pickup.Draw(spriteBatch);

                for (int i = 0; i < arrows.Count; i++)
                {
                    arrows[i].Draw(spriteBatch);
                }

                userInterface.Draw(spriteBatch);

                spriteBatch.End();
                
            }
            //Death or Game Over state of the game
            else if (state == 2)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(endScreen, position: Vector2.Zero);
                spriteBatch.DrawString(font, "" + (float)Math.Floor(score), new Vector2(GraphicsDevice.Viewport.Width /2 - 10, GraphicsDevice.Viewport.Height / 2 - 20), Color.White);
                spriteBatch.End();
            }
            

            base.Draw(gameTime);
        }
    }
}

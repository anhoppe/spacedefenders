using System;
using System.Collections.Generic;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using SpaceDefender.src.GameObjects;
using SpaceDefender.src;

namespace SpaceDefender
{
   /// <summary>
   /// This is the main type for your game
   /// </summary>
   public class Game1 : Microsoft.Xna.Framework.Game
   {
      GraphicsDeviceManager graphics;
      SpriteBatch spriteBatch;

      /// <summary>
      /// The player Object
      /// </summary>
      Player m_player;
      ArrayList m_playerList;

      /// <summary>
      /// The enemy container
      /// </summary>
      EnemyContainer m_enemy;

      /// <summary>
      /// The Background
      /// </summary>
      Texture2D m_background;
      Rectangle m_backgroundRect;
      Texture2D m_ground;
      Rectangle m_groundRect;

      public Game1()
      {
         graphics = new GraphicsDeviceManager(this);
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
         //////////////////////////////////////////////////////////////////////////
         // make content manager public (I'm a newbie)
         GameObject.M_content = Content;
         GameObject.M_screenWidth = graphics.GraphicsDevice.Viewport.Width;
         GameObject.M_screenHeight = graphics.GraphicsDevice.Viewport.Height;
         GameObject.M_random = new Random();
         //          graphics.ToggleFullScreen();
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

         //////////////////////////////////////////////////////////////////////////
         // Initialize background
         m_background = Content.Load<Texture2D>("Sprites\\background");
         m_backgroundRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
         m_ground = Content.Load<Texture2D>("Sprites\\ground");
         m_groundRect = new Rectangle(0, graphics.GraphicsDevice.Viewport.Height - m_ground.Height, 
            graphics.GraphicsDevice.Viewport.Width, m_ground.Height);

         //////////////////////////////////////////////////////////////////////////
         // Initialize the player
         m_player = new Player(Content.Load<Texture2D>("Sprites\\Player"),
            new Vector2(graphics.GraphicsDevice.Viewport.Width / 2, graphics.GraphicsDevice.Viewport.Height - m_ground.Height));
         m_playerList = new ArrayList();
         m_playerList.Add(m_player);

         //////////////////////////////////////////////////////////////////////////
         // Initialize the enemies
         m_enemy = new EnemyContainer();
         m_enemy.init();
      }

      /// <summary>
      /// UnloadContent will be called once per game and is the place to unload
      /// all content.
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

         KeyboardState a_state = Keyboard.GetState();

         if(a_state.IsKeyDown(Keys.Escape))
         {
            this.Exit();
         }

         //////////////////////////////////////////////////////////////////////////
         // Propagate game time
         GameObject.M_gameTime = gameTime.TotalGameTime;

         //////////////////////////////////////////////////////////////////////////
         // Update the player
         m_player.update(m_enemy.m_enemies);

         //////////////////////////////////////////////////////////////////////////
         // Update all enemies
         m_enemy.update(m_playerList);

         //////////////////////////////////////////////////////////////////////////
         // Check if the player died
         if(m_player.m_killed)
         {
            this.Exit();
         }

         base.Update(gameTime);
      }

      /// <summary>
      /// This is called when the game should draw itself.
      /// </summary>
      /// <param name="gameTime">Provides a snapshot of timing values.</param>
      protected override void Draw(GameTime gameTime)
      {
         graphics.GraphicsDevice.Clear(Color.CornflowerBlue);

         //////////////////////////////////////////////////////////////////////////
         // Initialize drawing
         spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
         
         // Draw the background
         spriteBatch.Draw(m_background, m_backgroundRect, Color.White);
         spriteBatch.Draw(m_ground, m_groundRect, Color.White);

         // Draw the player
         m_player.draw(spriteBatch);

         // Draw all enemies
         m_enemy.draw(spriteBatch);

         // End drawing
         spriteBatch.End();

         base.Draw(gameTime);
      }
   }
}

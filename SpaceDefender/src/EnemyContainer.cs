using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

using SpaceDefender.src.GameObjects;

namespace SpaceDefender.src
{
   class EnemyContainer
   {
      /// <summary>
      /// The dynamic array that contains all enemies
      /// </summary>
      public ArrayList m_enemies;

      private bool m_direction;

      /// <summary>
      /// Constuctor
      /// </summary>
      public EnemyContainer()
      {
         m_enemies = new ArrayList();

         m_direction = true;
      }

      /// <summary>
      /// Initializes enemies for a new level
      /// </summary>
      public void init()
      {
         m_enemies.Clear();
         
         //////////////////////////////////////////////////////////////////////////
         // Create the 'Classic' level
         int a_x = 0;
         int a_y = 0;
         Enemy a_enemy;
         Texture2D a_texture;

         for(a_x = 0; a_x < 8; a_x++)
         {
            for(a_y = 0; a_y < 4; a_y++)
            {
               switch(a_y)
               {
                  case 0:
                     a_texture = GameObject.M_content.Load<Texture2D>("Sprites\\alien1");
                     break;
                  case 1:
                     a_texture = GameObject.M_content.Load<Texture2D>("Sprites\\alien2");
                     break;
                  case 2:
                     a_texture = GameObject.M_content.Load<Texture2D>("Sprites\\alien3");
                     break;
                  case 3:
                     a_texture = GameObject.M_content.Load<Texture2D>("Sprites\\alien4");
                     break;
                  default:
                     a_texture = GameObject.M_content.Load<Texture2D>("Sprites\\alien4");
                     break;
               }
               a_enemy = new Enemy(a_texture, new Vector2(2*a_x * a_texture.Width + a_texture.Width, 
                  a_y * a_texture.Height + a_texture.Height));
               a_enemy.m_velocity = new Vector2(1, 0);

               m_enemies.Add(a_enemy);

               
            }
         }
      }

      /// <summary>
      /// Updates all enemies
      /// </summary>
      public void update(ArrayList t_combatants)
      {
         
         //////////////////////////////////////////////////////////////////////////
         // here the 'classic' behavior

         // get the highest or lowest X value (depending on the direction)

         //////////////////////////////////////////////////////////////////////////
         // update all game objects
         ArrayList a_killedEnemies = new ArrayList();

         IEnumerator a_it = m_enemies.GetEnumerator();
         bool a_result = true;

         while (a_it.MoveNext())
         {
            Enemy a_enemy = (Enemy)a_it.Current;

            if (m_direction)
            {
               a_enemy.m_velocity.X = 1;
            }
            else
            {
               a_enemy.m_velocity.X = -1;
            }

            a_result &= a_enemy.update(t_combatants);

            if(a_enemy.m_killed)
            {
               a_killedEnemies.Add(a_enemy);
            }
         }

         if(!a_result)
         {
            m_direction = !m_direction;
            
            a_it = m_enemies.GetEnumerator();

            while (a_it.MoveNext())
            {
               Enemy a_enemy = (Enemy)a_it.Current;
               Rectangle a_rect = new Rectangle();
               a_enemy.getRectangle(ref a_rect);

               a_enemy.m_position.Y += a_rect.Height;
            }
         }

         //////////////////////////////////////////////////////////////////////////
         // Remove killed enemies
         a_it = a_killedEnemies.GetEnumerator();
         while(a_it.MoveNext())
         {
            m_enemies.Remove(a_it.Current);
         }
      }

      /// <summary>
      /// Draw all enemies
      /// </summary>
      /// <param name="t_spriteBatch"></param>
      public void draw(SpriteBatch t_spriteBatch)
      {
         IEnumerator a_it = m_enemies.GetEnumerator();
         while(a_it.MoveNext())
         {
            Enemy a_enemy = (Enemy)a_it.Current;
            a_enemy.draw(t_spriteBatch);
         }
      }
   }
}

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

namespace SpaceDefender.src.GameObjects
{
   struct ShotStruct
   {
      public Rectangle m_rect;
      public GameObject m_object;
   };

   class GameObject
   {
      /// <summary>
      /// Give public access to the content manager
      /// </summary>
      public static ContentManager M_content;

      /// <summary>
      /// I may suck
      /// </summary>
      public static TimeSpan M_gameTime;

      public static int M_screenWidth;

      public static int M_screenHeight;

      public static Random M_random;

      /// <summary>
      /// Objects graphic
      /// </summary>
      private Texture2D m_texture;

      /// <summary>
      /// objects position
      /// </summary>
      public Vector2 m_position;
      
      /// <summary>
      /// Objects rotation center
      /// </summary>
      private Vector2 m_center;
      
      /// <summary>
      ///  angle of the object
      /// </summary>
      protected float m_angle;

      /// <summary>
      /// The shots fired by the object
      /// </summary>
      protected ArrayList m_shots;

      /// <summary>
      /// Velocity of the object
      /// </summary>
      public Vector2 m_velocity;

      /// <summary>
      /// maximal number of shots
      /// </summary>
      protected int m_maxShots;

      /// <summary>
      /// The shot frequency of the game object in milliseconds
      /// </summary>
      protected int m_shotFrequency;

      /// <summary>
      /// Point in time when the last fire event occurred
      /// </summary>
      private TimeSpan m_lastFireEventTime;

      /// <summary>
      /// Indicates if the object was killed
      /// </summary>
      public bool m_killed;


      public GameObject(Texture2D t_texture, Vector2 t_position)
      {
         m_texture = t_texture;
         m_position = t_position;
         m_angle = 0.0f;

         m_center = new Vector2(m_texture.Width / 2, m_texture.Height / 2);

         m_velocity = new Vector2(0, 0);

         m_shots = new ArrayList();
         m_lastFireEventTime = new TimeSpan();

         m_killed = false;
      }

      public void fire(Vector2 t_velocity)
      {
         TimeSpan a_diffTime = GameObject.M_gameTime.Subtract(m_lastFireEventTime);

         if((m_shots.Count < m_maxShots) &&
            (a_diffTime.Milliseconds > m_shotFrequency))
         {
            GameObject a_shot = new GameObject(GameObject.M_content.Load<Texture2D>("Sprites\\playerbullet"), m_position);

            a_shot.m_position = m_position;
            a_shot.m_velocity = t_velocity;

            m_shots.Add(a_shot);

            m_lastFireEventTime = GameObject.M_gameTime;
         }
      }

      public virtual bool update(ArrayList t_combatants)
      {
         bool r_result = false;

         // delete list
         ArrayList a_deleteList = new ArrayList();
         ArrayList a_bulletRects = new ArrayList();
         
         // update all bullets
         IEnumerator a_it = m_shots.GetEnumerator();

         while(a_it.MoveNext())
         {
            GameObject a_shot = (GameObject)a_it.Current;

            // update the shot
            a_shot.update(null);

            // get bullet rect for collision detection
            Rectangle a_bulletRect = new Rectangle();
            a_shot.getRectangle(ref a_bulletRect);

            ShotStruct a_shotStruct = new ShotStruct();
            a_shotStruct.m_rect = a_bulletRect;
            a_shotStruct.m_object = a_shot;

            a_bulletRects.Add(a_shotStruct);

            // remove shots
            if((a_shot.m_position.Y < 0) || (a_shot.m_position.Y > GameObject.M_screenHeight) || (a_shot.m_killed))
            {
               a_deleteList.Add(a_shot);
            }
         }

         //////////////////////////////////////////////////////////////////////////
         // Check for collision with an enemy
         GameObject a_combatant;
         Rectangle a_combRect = new Rectangle();
         IEnumerator a_rectIt;

         if(null != t_combatants)
         {
            a_it = t_combatants.GetEnumerator();
            while (a_it.MoveNext())
            {
               a_combatant = (GameObject)a_it.Current;
               a_combatant.getRectangle(ref a_combRect);

               a_rectIt = a_bulletRects.GetEnumerator();
               while (a_rectIt.MoveNext())
               {
                  ShotStruct a_shotStruct = (ShotStruct)a_rectIt.Current;

                  if (a_shotStruct.m_rect.Intersects(a_combRect))
                  {
                     a_combatant.m_killed = true;
                     a_shotStruct.m_object.m_killed = true;
                  }
               }
            }
         }

         //////////////////////////////////////////////////////////////////////////
         // Remove shots
         a_it = a_deleteList.GetEnumerator();
         while(a_it.MoveNext())
         {
            m_shots.Remove(a_it.Current);
         }
         a_deleteList.Clear();

         //////////////////////////////////////////////////////////////////////////
         // update the position of the game object
         m_position += m_velocity;

         if(m_position.X >= 0 && m_position.X < GameObject.M_screenWidth &&
            m_position.Y >= 0 && m_position.Y < GameObject.M_screenHeight)
         {
            r_result = true;
         }

         return r_result;
      }

      public void getRectangle(ref Rectangle t_rect)
      {
         t_rect.X = (int)m_position.X;
         t_rect.Y = (int)m_position.Y;

         t_rect.Width = m_texture.Width;
         t_rect.Height = m_texture.Height;
      }

      public void draw(SpriteBatch t_spriteBatch)
      {
         t_spriteBatch.Draw(m_texture, m_position, null, Color.White, m_angle, m_center, 1.0f, SpriteEffects.None, 0.0f);

         // draw all shots fired by the game object
         IEnumerator a_it = m_shots.GetEnumerator();

         while (a_it.MoveNext())
         {
            GameObject a_shot = (GameObject)a_it.Current;
            a_shot.draw(t_spriteBatch);
         }

      }
   }
}

using System;
using System.Collections.Generic;
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
   class GameObject
   {
      private Texture2D m_texture;
      protected Vector2 m_position;
      private Vector2 m_center;
      protected float m_angle;

      public GameObject(Texture2D t_texture, Vector2 t_position)
      {
         m_texture = t_texture;
         m_position = t_position;
         m_angle = 0.0f;

         m_center = new Vector2(m_texture.Width / 2, m_texture.Height / 2);
      }

      public void draw(SpriteBatch t_spriteBatch)
      {
         t_spriteBatch.Draw(m_texture, m_position, null, Color.White, m_angle, m_center, 1.0f, SpriteEffects.None, 0.0f);
      }
   }
}

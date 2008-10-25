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
   class Player : GameObject
   {
      bool m_isOnGound;

      /// <summary>
      /// 
      /// </summary>
      /// <param name="t_texture"></param>
      /// <param name="t_position"></param>
      public Player(Texture2D t_texture, Vector2 t_position) : base(t_texture, t_position)
      {
         m_isOnGound = true;
         m_maxShots = 2;
         m_shotFrequency = 300;
      }

      public override bool update(ArrayList t_combatants)
      {
         KeyboardState a_state = Keyboard.GetState();

         if(a_state.IsKeyDown(Keys.Left))
         {
            if(m_isOnGound)
            {
               m_position.X -= 1;
            }
            else
            {
               m_angle -= -0.1f;
               m_angle = MathHelper.Clamp(m_angle, MathHelper.PiOver2 * 4, 0);
            }
         }

         if (a_state.IsKeyDown(Keys.Right))
         {
            if (m_isOnGound)
            {
               m_position.X += 1;
            }
            else
            {
               m_angle += -0.1f;
               m_angle = MathHelper.Clamp(m_angle, MathHelper.PiOver2 * 4, 0);
            }
         }

         if(a_state.IsKeyDown(Keys.LeftControl))
         {
            fire(new Vector2(0, -10));
         }

         // call base method
         return base.update(t_combatants);
      }
   }
}

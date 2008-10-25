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
   class Enemy : GameObject
   {

      public Enemy(Texture2D t_texture, Vector2 t_position) : base(t_texture, t_position)
      {
         m_maxShots = 1;
      }

      public override bool update(ArrayList t_combatants)
      {
         int a_val = GameObject.M_random.Next(1000);

         if(a_val > 995)
         {
            fire(new Vector2(0, 5));
         }

         return base.update(t_combatants);
      }
   }
}

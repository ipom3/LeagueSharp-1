#region LICENSE

// Copyright 2015-2015 Support
// Urgot.cs is part of Support.
// 
// Support is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// Support is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with Support. If not, see <http://www.gnu.org/licenses/>.
// 
// Filename: Support/Support/Urgot.cs
// Created:  10/01/2015
// Date:     20/01/2015/11:20
// Author:   h3h3

#endregion

#region

#endregion

namespace Support.Plugins
{
    #region

    using System;
    using System.Linq;
    using LeagueSharp;
    using LeagueSharp.Common;
    using Support.Util;

    #endregion

    public class Urgot : PluginBase
    {
        private Spell Q2;
        public Urgot()
        {
            Q = new Spell(SpellSlot.Q, 1000);
            Q2 = new Spell(SpellSlot.Q, 1200);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 900);
            R = new Spell(SpellSlot.R);

            Q.SetSkillshot(0.10f, 100f, 1600f, true, SkillshotType.SkillshotLine);
            Q2.SetSkillshot(0.10f, 100f, 1600f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.283f, 0f, 1750f, false, SkillshotType.SkillshotCircle);
            R.SetTargetted(250, 550);
            
            /*Q = new Spell(SpellSlot.Q, 650);
            W = new Spell(SpellSlot.W, 625);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 600);

            Q.SetTargetted(250, 1400);
            W.SetSkillshot(600, (float) (50 * Math.PI / 180), float.MaxValue, false, SkillshotType.SkillshotCone);
            R.SetSkillshot(250, 200, float.MaxValue, false, SkillshotType.SkillshotCircle);*/
        }

        public override void OnUpdate(EventArgs args)
        {
            if (ComboMode)
            {
                SpellSecondQ();
                SpellQ(Target);
                
                if (E.CastCheck(Target, true))
                {
                    E.Cast(Target, true);
                }
                
                W.Cast(Player, true);
            }
        }
        
        internal void SpellSecondQ()
        {
            foreach (var obj in
                ObjectManager.Get<Obj_AI_Hero>()
                    .Where(obj => obj.IsValidTarget(Q2.Range) && obj.HasBuff("urgotcorrosivedebuff", true)))
            {
                Q2.Cast(obj.ServerPosition, true);
            }
        }

        internal void SpellQ(Obj_AI_Base t)
        {
            if (t.HasBuff("urgotcorrosivedebuff", true))
                return;

            if (Q.IsReady() && Q.IsInRange(t))
            {
                Q.Cast(t, true);
            }
        }


 /*       internal static void SpellE(Obj_AI_Base t)
        {
            var hitchance = (HitChance)(ComboMenu.Item("preE").GetValue<StringList>().SelectedIndex + 3);

            if (SpellClass.E.IsInRange(t) && SpellClass.E.IsReady())
            {
                SpellClass.E.CastIfHitchanceEquals(t, hitchance, PacketCast);
            }
        }*/


        
        public override void OnPossibleToInterrupt(Obj_AI_Base unit, InterruptableSpell spell)
        {
            if (spell.DangerLevel < InterruptableDangerLevel.High || unit.IsAlly)
            {
                return;
            }
       
            R.Cast(unit);
        }


        public override void ComboMenu(Menu config)
        {
            config.AddBool("ComboQ", "Use Q", true);
            config.AddBool("ComboW", "Use W", true);
            config.AddBool("ComboR", "Use R", true);
        }

        public override void InterruptMenu(Menu config)
        {
            config.AddBool("Interrupt.Q", "Use Q to Interrupt Spells", true);
            config.AddBool("Interrupt.W", "Use W to Interrupt Spells", true);
        }
    }
}

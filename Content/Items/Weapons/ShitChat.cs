using Microsoft.Xna.Framework;
using SpeechAttacker.Content.Items.Skills;
using SpeechAttacker.Content.Projs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;

namespace SpeechAttacker.Content.Items.Weapons
{
    public class ShitChat : BasicChatItem, IColor,IBoom,IPenetrate
    {
        public Color Color => new(173,80,3);
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 2;
            DamageBones += 1;
            AddTextLength = 0;
            Item.rare = ItemRarityID.Master;
        }
        public override string ShootText(string Text) => StringToUnicode(Text);
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (source is EntitySource_ItemUse_WithAmmo_SpeechAttacker speechAttacker && speechAttacker.Text.Contains(StringToUnicode("shit")) && speechAttacker.ShootIndex < 4)
            {
                for (int i = 0; i < speechAttacker.Text.Length; i++)
                {
                    var proj = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(0.6) * 5, type, damage, knockback, player.whoAmI, -1);
                    (proj.ModProjectile as ChatProj).Text = speechAttacker.Text[i].ToString();
                    proj.timeLeft /= 2;
                }
                return false;
            }
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
        public static string StringToUnicode(string source)
        {
            char[] cs = source.ToCharArray();
            StringBuilder sb = new();
            for (int i = 0; i < cs.Length; i++)
            {
                sb.AppendFormat("\\u{0:x4}", (int)cs[i]);
            }
            return sb.ToString();
        }
    }
}

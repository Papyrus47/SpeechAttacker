using SpeechAttacker.Content.Items.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ID;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using SpeechAttacker.Content.Projs;

namespace SpeechAttacker.Content.Items.Weapons
{
    public class VolcanoChat : BasicChatItem, IBoom, IColor
    {
        public Color Color => Color.OrangeRed;
        public override void SetDefaults()
        {
            base.SetDefaults();
            Item.damage = 3;
            DamageBones += 0.2f;
            DamageBones *= 1.1f;
            AddTextLength = 500;
            Item.rare = ItemRarityID.Orange;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.HellstoneBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            var proj = Projectile.NewProjectileDirect(source, position, velocity.RotatedByRandom(0.6) * -5, type, damage, knockback, player.whoAmI, -1);
            (proj.ModProjectile as ChatProj).Text = "<=>";
            (proj.ModProjectile as ChatProj).Trace = true;
            proj.timeLeft *= 10;
            return base.Shoot(player, source, position, velocity, type, damage, knockback);
        }
    }
}

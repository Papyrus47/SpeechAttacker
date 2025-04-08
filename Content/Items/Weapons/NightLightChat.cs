using Microsoft.Xna.Framework;
using SpeechAttacker.Content.Items.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;

namespace SpeechAttacker.Content.Items.Weapons
{
    public class NightLightChat : BasicChatItem, ITrace, IColor
    {
        public Color Color => Color.Purple;
        public override void SetDefaults()
        {
            base.SetDefaults();
            DamageBones += 0.5f;
            AddTextLength = 450;
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.DemoniteBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

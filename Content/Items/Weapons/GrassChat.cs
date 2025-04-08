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
    public class GrassChat : BasicChatItem, ISummon, IColor
    {
        public Color Color => Color.GreenYellow;
        public override void SetDefaults()
        {
            base.SetDefaults();
            AddTextLength = 100;
            Item.rare = ItemRarityID.Green;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.Stinger, 5);
            recipe.AddIngredient(ItemID.JungleSpores, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

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
    public class BloodChat : BasicChatItem,IBoom,IColor
    {
        public Color Color => Color.Red;
        public override void SetDefaults()
        {
            base.SetDefaults();
            AddTextLength = 450;
            Item.damage = 2;
            Item.rare = ItemRarityID.Blue;
        }
        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.CrimtaneBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();
        }
    }
}

using Terraria;
using Terraria.DataStructures;

namespace SpeechAttacker
{
    public class EntitySource_ItemUse_WithAmmo_SpeechAttacker : EntitySource_ItemUse_WithAmmo
    {
        public string Text;
        public int ShootIndex;
        public EntitySource_ItemUse_WithAmmo_SpeechAttacker(Player player, Item item, int ammoItemId, string Text, int shootIndex, string context = null) : base(player, item, ammoItemId, context)
        {
            this.Text = Text;
            ShootIndex = shootIndex;
        }
    }
}

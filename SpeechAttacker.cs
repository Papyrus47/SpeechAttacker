using SpeechAttacker.Content.Items;
using SpeechAttacker.Content.Projs;
using System;
using Terraria;
using Terraria.Chat;
using Terraria.ModLoader;

namespace SpeechAttacker
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class SpeechAttacker : Mod
    {

    }
    public class SpeechAttackerSystem : ModSystem
    {
        public override void Load()
        {
            On_ChatCommandProcessor.ProcessIncomingMessage += SpeechAttack;
        }
        public static void SpeechAttack(On_ChatCommandProcessor.orig_ProcessIncomingMessage orig, ChatCommandProcessor self, ChatMessage message, int clientId)
        {
            orig.Invoke(self, message, clientId);
            if (message.IsConsumed)
            {
                Player player = Main.LocalPlayer;
                for (int i = 0; i < message.Text.Length; i++)
                {
                    Microsoft.Xna.Framework.Vector2 vector2 = (Main.MouseWorld - player.Center);
                    int damage = System.Text.Encoding.UTF8.GetByteCount(message.Text);
                    int crit = 0;
                    if (player.HeldItem.ModItem is BasicChatItem chatItem)
                    {
                        damage = (int)chatItem.DamageBones.ApplyTo(damage + player.GetWeaponDamage(player.HeldItem));
                        crit = player.GetWeaponCrit(player.HeldItem);
                    }
                    for (int j = 0;j < message.Text.Length; j++)
                    {
                        if (message.Text[j] == message.Text[i])
                        {
                            string t = message.Text[i].ToString();
                            damage -= System.Text.Encoding.UTF8.GetByteCount(t);
                            damage = (int)(damage * 0.75f);
                        }
                    }
                    var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center, vector2 / 30f + (vector2.SafeNormalize(default) * (2 * i)), ModContent.ProjectileType<ChatProj>(), damage, 0f, player.whoAmI);
                    (proj.ModProjectile as ChatProj).Text = message.Text[i].ToString();
                    proj.CritChance = crit;
                }
            }
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}

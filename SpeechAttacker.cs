using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
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
        public static int UseCtrlVCount = 0;
        public override void Load()
        {
            On_Main.DoUpdate_HandleChat += HandleChat;
            On_Main.GetInputText += GetInputText;
            On_ChatCommandProcessor.ProcessIncomingMessage += SpeechAttack;
        }

        public static string GetInputText(On_Main.orig_GetInputText orig, string oldString, bool allowMultiLine)
        {
            if (UseCtrlVCount >= 2 && Main.keyState.PressingControl() && Main.keyState.IsKeyDown(Keys.V))
                return oldString;
            if (UseCtrlVCount < 2 && Main.keyState.PressingControl() && Main.keyState.IsKeyDown(Keys.V))
                UseCtrlVCount++;
            Player player = Main.LocalPlayer;
            if (player.HeldItem.ModItem is BasicChatItem chatItem && Main.oldInputText.GetPressedKeys().Length == 0 && Main.inputText.GetPressedKeys().Length > 0)
            {
                for (int i = 0; i < chatItem.AddTextNum; i++)
                {
                    oldString += (char)Main.rand.Next('!', 'Z' + 1);
                }
            }
            return orig.Invoke(oldString, allowMultiLine);
        }
        public static void HandleChat(On_Main.orig_DoUpdate_HandleChat orig)
        {
            int width = Main.screenWidth;
            Main.screenWidth *= 2;
            orig.Invoke();
            Main.screenWidth = width;
        }
        public static void SpeechAttack(On_ChatCommandProcessor.orig_ProcessIncomingMessage orig, ChatCommandProcessor self, ChatMessage message, int clientId)
        {
            orig.Invoke(self, message, clientId);
            if (message.IsConsumed)
            {
                Player player = Main.LocalPlayer;
                for (int i = 0; i < message.Text.Length; i++)
                {
                    Vector2 vector2 = (Main.MouseWorld - player.Center);
                    int damage = System.Text.Encoding.UTF8.GetByteCount(message.Text) * 3;
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
                    damage /= UseCtrlVCount + 1;
                    var proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(player.HeldItem), player.Center, vector2 / 30f + (vector2.SafeNormalize(default) * (0.2f * i)), ModContent.ProjectileType<ChatProj>(), damage, 0f, player.whoAmI);
                    (proj.ModProjectile as ChatProj).Text = message.Text[i].ToString();
                    proj.CritChance = crit;
                }
            }
            UseCtrlVCount = 0;
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
    public class SpeechAttackerPlayer : ModPlayer
    {
        /// <summary>
        /// 站立在文字上
        /// </summary>
        public bool StandOnSpeech;
        /// <summary>
        /// 是否与文字碰撞X轴
        /// </summary>
        public bool CollideSpeechX;
        public override void PreUpdateMovement()
        {
            if(StandOnSpeech)
            {
                StandOnSpeech = false;
                Player.position.Y -= Player.velocity.Length() * 0.1f;
                if(!Player.controlJump)
                    Player.velocity.Y = 0;
            }
            if (CollideSpeechX)
            {
                CollideSpeechX = false;
                Player.position.Y -= 8;
            }
        }
    }
}

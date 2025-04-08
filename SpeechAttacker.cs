using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using SpeechAttacker.Content.Items.Weapons;
using SpeechAttacker.Content.Projs;
using System;
using System.Text;
using Terraria;
using Terraria.Chat;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpeechAttacker
{
    // Please read https://github.com/tModLoader/tModLoader/wiki/Basic-tModLoader-Modding-Guide#mod-skeleton-contents for more information about the various files in a mod.
    public class SpeechAttacker : Mod
    {
        public static Asset<DynamicSpriteFont> PixelFont;
        public override void Load()
        {
            PixelFont = ModContent.Request<DynamicSpriteFont>("SpeechAttacker/Assets/Fonts/PixelFont");
        }
        public override void Unload()
        {
            PixelFont = null;
        }
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
                string text = chatItem.OnInputText(oldString);
                if (text != null)
                    oldString = text;
            }
            chatItem = player.HeldItem.ModItem as BasicChatItem;
            if (chatItem != null)
            {
                string text = chatItem.GetInputText(oldString);
                if (text != null)
                    oldString = text;
            }
            return orig.Invoke(oldString, allowMultiLine);
        }
        public static void HandleChat(On_Main.orig_DoUpdate_HandleChat orig)
        {
            Main.autoPause = false;
            int width = Main.screenWidth;
            Main.screenWidth = 470; // 调整屏幕宽度(打字长度)
            Player player = Main.LocalPlayer;
            if (player.HeldItem.ModItem is BasicChatItem chatItem)
            {
                Main.screenWidth += chatItem.AddTextLength; // 调整屏幕宽度(打字长度)
                chatItem.OnUpdateHandleChat();
            }
            orig.Invoke();
            Main.screenWidth = width;
        }
        public static void SpeechAttack(On_ChatCommandProcessor.orig_ProcessIncomingMessage orig, ChatCommandProcessor self, ChatMessage message, int clientId)
        {
            orig.Invoke(self, message, clientId);
            if (message.IsConsumed)
            {
                Player player = Main.LocalPlayer;
                int oldProjWhoAmI = -1;
                string text = message.Text;
                if (player.HeldItem.ModItem is BasicChatItem changeText)
                {
                    text = changeText.ShootText(text);
                }
                for (int i = 0; i < text.Length; i++) // 遍历所有 ChatMessage 的字符
                {
                    Vector2 vector2 = (Main.MouseWorld - player.Center); // 计算鼠标位置与玩家中心的向量
                    int damage = System.Text.Encoding.UTF8.GetByteCount(text) * 6; // 计算伤害
                    int crit = 0; // 暴击率
                    if (player.HeldItem.ModItem is BasicChatItem chatItem)
                    {
                        damage = (int)chatItem.DamageBones.ApplyTo(damage + player.GetWeaponDamage(player.HeldItem));
                        crit = player.GetWeaponCrit(player.HeldItem);
                    }
                    for (int j = 0; j < text.Length; j++) // 伤害衰减部分
                    {
                        if (text[j] == text[i])
                        {
                            string t = text[i].ToString();
                            damage -= System.Text.Encoding.UTF8.GetByteCount(t);
                            //damage = (int)(damage * 0.75f);
                        }
                    }
                    chatItem = player.HeldItem.ModItem as BasicChatItem;
                    damage /= UseCtrlVCount * 2 + 5; // 根据 Ctrl+V 的次数调整伤害
                    Vector2 pos = player.Center;
                    Vector2 vel = vector2 / 120f + (vector2.SafeNormalize(default) * (0.6f * i));
                    int type = ModContent.ProjectileType<ChatProj>();
                    float kn = 0f;
                    bool shoot = true;
                    EntitySource_ItemUse_WithAmmo entitySource = new EntitySource_ItemUse_WithAmmo_SpeechAttacker(player, player.HeldItem, ItemID.None, text,i);
                    if (chatItem != null && chatItem.CanShoot(player)) // 自定义射击
                    {
                        chatItem.ModifyShootStats(player, ref pos, ref vel, ref type, ref damage, ref kn);
                        shoot = chatItem.Shoot(player, entitySource, pos, vel, type, damage, kn);
                    }
                    if (shoot) // 允许发射
                    {
                        var proj = Projectile.NewProjectileDirect(entitySource, pos, vel, type, damage, kn, player.whoAmI, oldProjWhoAmI);
                        (proj.ModProjectile as ChatProj).Text = text[i].ToString();
                        //proj.CritChance = crit;
                        oldProjWhoAmI = proj.whoAmI;
                    }
                }
            }
            UseCtrlVCount = 0; // 重置 Ctrl+V 计数
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}

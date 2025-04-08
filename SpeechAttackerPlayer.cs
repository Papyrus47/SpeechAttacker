using Terraria;
using Terraria.ModLoader;

namespace SpeechAttacker
{
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

using Terraria;
using Terraria.ModLoader;

namespace SpeechAttacker
{
    public class SpeechAttackerPlayer : ModPlayer
    {
        /// <summary>
        /// վ����������
        /// </summary>
        public bool StandOnSpeech;
        /// <summary>
        /// �Ƿ���������ײX��
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

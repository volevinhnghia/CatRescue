using UnityEngine;

namespace Watermelon
{
    public class PlayerAnimationHandler : MonoBehaviour
    {
        private PlayerBehavior playerBehavior;

        public void Inititalise(PlayerBehavior playerBehavior)
        {
            this.playerBehavior = playerBehavior;
        }

        public void LeftStepCallback()
        {
            playerBehavior.LeftFootParticle();
        }

        public void RightStepCallback()
        {
            playerBehavior.RightFootParticle();
        }
    }
}
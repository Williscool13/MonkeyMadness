
using UnityEngine;

namespace Woodchop
{
    public class WoodchopPlayer : MonoBehaviour
    {
        [SerializeField] Animator anim;


        public float LockoutTimer { get; private set; } = 0;


        public void SetLockoutTimer(float val) {
            LockoutTimer = val;
        }

        public void ReduceLockoutTimer(float val) {
            LockoutTimer -= val;
        }

        public void SetAnimationTrigger(string id) {
            if (anim != null) {
                anim.SetTrigger(id);
            }
        }

        [SerializeField] ParticleSystem missParticleEffect;
        public void PlayMissParticleEffect() {
            if (missParticleEffect != null) {
                missParticleEffect.Play();
            }
        }

        [SerializeField] ParticleSystem hitParticleEffect;
        public void PlayHitParticleEffect() {
            if (hitParticleEffect != null) {
                hitParticleEffect.Play();
            }
        }

        [SerializeField] AudioSource swingSound;
        public void PlaySwingSound() {
            swingSound.PlayOneShot(swingSound.clip);
        }


        [SerializeField] AudioSource hitSound;
        public void PlayHitSound() {
            hitSound.PlayOneShot(hitSound.clip);
        }
    }
}

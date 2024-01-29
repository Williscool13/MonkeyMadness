using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderPlayer : MonoBehaviour
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

    public void SetAnimationBool(string id, bool val) {
        if (anim != null) {
            anim.SetBool(id, val);
        }
    }

    [SerializeField] SpriteRenderer playerMove;
    public void FaceLeft() {
        playerMove.flipX = false;
    }

    public void FaceRight() {
        playerMove.flipX = true;
    }

    [SerializeField] ParticleSystem starEffect;
    public void StarEffect() {
        starEffect.Play();
    }
}

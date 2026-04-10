using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class SoldierAnimHandler : MonoBehaviour
{

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAttackAnimation()
    {
        animator.SetTrigger(Constants.SOLDIER_ATTACK);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger(Constants.SOLDIER_DEATH);
    }
    public void PlayTakeDamageAnimation()
    {
        animator.SetTrigger(Constants.TAKE_DAMAGE);
    }

}

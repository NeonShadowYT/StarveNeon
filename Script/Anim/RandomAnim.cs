using UnityEngine;

public class RandomAnim : StateMachineBehaviour
{
    override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        animator.SetInteger("RandomAnim", Random.Range(0, 2));
    }
}

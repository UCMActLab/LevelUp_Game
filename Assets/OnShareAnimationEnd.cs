using UnityEngine;

public class OnShareAnimationEnd : StateMachineBehaviour
{
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= 0.95f)
        {
            animator.GetComponent<ArticleGameObject>().OnShare.Invoke();
        }
    }
}

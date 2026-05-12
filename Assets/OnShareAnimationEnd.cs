using UnityEngine;

public class OnShareAnimationEnd : StateMachineBehaviour
{
    //// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (stateInfo.normalizedTime >= 0.99f)
    //    {
    //        animator.GetComponent<ArticleGameObject>().VerifyArticleSharing();
    //    }
    //}

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.GetComponent<ArticleGameObject>().VerifyArticleSharing();
    }

}

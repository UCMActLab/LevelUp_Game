using UnityEngine;

public class ShowNextArticleOnAnimationEnd : StateMachineBehaviour
{
    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LevelManager.Instance.ShowNextArticle();

        animator.transform.parent.gameObject.SetActive(false);
    }
}

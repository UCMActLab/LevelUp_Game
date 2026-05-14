using UnityEngine;

public class OnBadActionAnimationEnd : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        ArticleGameObject article = animator.GetComponentInParent<ArticleGameObject>();

        if (!article.IsTrue)
        {
            article.VerifyArticleSharing();
        }
        else
        {
            article.InvokeOnSkip();
        }
    }
}
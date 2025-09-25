using UnityEngine;
using System.Collections;

public class notificationScript : MonoBehaviour
{
    [SerializeField] private float NotiTime = 3.0f;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(WaitAndNoti());
    }

    // Update is called once per frame
    IEnumerator WaitAndNoti()
    {
        // suspend execution for 5 seconds
        yield return new WaitForSeconds(NotiTime);
        animator.Play("NotiAnimation");
    }
}

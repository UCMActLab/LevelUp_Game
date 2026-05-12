using DA_Assets.DAG;
using UnityEngine;

public class ElectionVFX : MonoBehaviour
{
    [SerializeField]
    GradientObject grCorrect;
    [SerializeField]
    GradientObject grIncorrect;


    private void Start()
    {
    }

    public void setGradient(bool g)
    {
        Debug.Log("que pasa???????????");
        if(g) GetComponent<DAGradient>().Gradient = grCorrect.gradient;
        else GetComponent<DAGradient>().Gradient = grIncorrect.gradient;
    }
}

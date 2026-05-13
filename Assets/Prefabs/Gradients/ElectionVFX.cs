using DA_Assets.DAG;
using UnityEngine;

public class ElectionVFX : MonoBehaviour
{
    [SerializeField]
    GradientObject grCorrect;
    [SerializeField]
    GradientObject grIncorrect;
    [SerializeField]
    GameObject particles;


    private void Start()
    {
    }

    public void setGradient(bool g)
    {
        if(g) GetComponent<DAGradient>().Gradient = grCorrect.gradient;
        else GetComponent<DAGradient>().Gradient = grIncorrect.gradient;
    }

    public void setParticles()
    {
        if(particles)
            particles.SetActive(true);
    }

}

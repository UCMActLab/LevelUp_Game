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
    [SerializeField]
    GameObject electionObject;


    private void Start()
    {
    }

    public void setGradient(bool g)
    {
        if(electionObject == null)
        {
            if (g) GetComponent<DAGradient>().Gradient = grCorrect.gradient;
            else GetComponent<DAGradient>().Gradient = grIncorrect.gradient;
        }
        else
        {
            if (g) electionObject.GetComponent<DAGradient>().Gradient = grCorrect.gradient;
            else electionObject.GetComponent<DAGradient>().Gradient = grIncorrect.gradient;
        }
        
    }

    public void setParticles()
    {
        if(particles)
            particles.SetActive(true);
    }

    public Gradient getGradientA()
    {
        return grCorrect.gradient;
    }
    public Gradient getGradientB()
    {
        return grIncorrect.gradient;
    }

}

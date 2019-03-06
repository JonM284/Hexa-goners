using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesBlast : MonoBehaviour {

    public bool shoot = false;
    public ParticleSystem[] particles;

    void Update()
    {
        if (shoot)
        {
            Blast();
        }
    }

    public void Blast()
    {
        
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].Play();
        }
        shoot = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleLauncher : MonoBehaviour{

    public ParticleSystem pLauncher;
  
    // Start is called before the first frame update
    void Start()
    {

        var em = pLauncher.emission;
        em.enabled = true;
        em.type = ParticleSystemEmissionType.Time;
        em.SetBursts(
           new ParticleSystem.Burst[]{
                new ParticleSystem.Burst(2.0f, 100),
           });

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void startParticle() {
        pLauncher.Play();
    }
  
}

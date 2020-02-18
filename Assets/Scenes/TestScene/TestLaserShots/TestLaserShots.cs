using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARBattleTest
{
    public class TestLaserShots : MonoBehaviour
    {
        public Transform laserShotPosition;
        private Animator anim;
        public ParticleSystem startWavePS;
        public ParticleSystem startParticles;
        public int startParticlesCount = 100;
        public GameObject laserShotPrefab;
        private ConLaser conLaser;


        void Awake()
        {
            conLaser = GetComponentInChildren<ConLaser>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                startWavePS.Emit(1);
                startParticles.Emit(startParticlesCount);
                conLaser.GenerateHitEffect();
            }

            if (Input.GetKey(KeyCode.K))
            {
                anim.SetBool("Fire", true);
                conLaser.SetGlobalProgress(0);
            }
            else
            {
                anim.SetBool("Fire", false);
            }
        }

        public void GenerateLaser()
        {
            startWavePS.Emit(1);
            startParticles.Emit(startParticlesCount);
            conLaser.GenerateHitEffect();
        }

        public void SetFire(bool fire)
        {
            if (fire)
            {
                anim.SetBool("Fire", true);
                conLaser.SetGlobalProgress(0);
            }
            else
            {
                anim.SetBool("Fire", false);
            }       
        }

        public void SetChlidActive(bool active)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
               var child = transform.GetChild(i);
                child.gameObject.SetActive(active);
            }      
        }
    }
}

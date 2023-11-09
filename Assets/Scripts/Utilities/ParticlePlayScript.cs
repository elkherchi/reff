using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayScript : MonoBehaviour {

	ParticleSystem particle;

	void Start()
	{
		particle = GetComponentInChildren<ParticleSystem> ();
	}

	public void PlayParticle()
	{
		particle.Play ();
	}
}

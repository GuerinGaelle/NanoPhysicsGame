using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ChangingCurrent : MonoBehaviour {

    public float TimeStopped = 1;
    public float TimeFlowing = 1;

    private AreaEffector2D ae2d;
    private float Power;

    private List<SpriteRenderer> ObjectsToFade;
    private List<ParticleSystem> ParticleSystems;

    private Color transparent = new Color(1, 1, 1, 0);

	void Start ()
    {
        ObjectsToFade = new List<SpriteRenderer>();
        ParticleSystems = new List<ParticleSystem>();

        foreach (SpriteRenderer spr in transform.parent.GetComponentsInChildren<SpriteRenderer>())
        {
            if (spr.transform != transform)
                ObjectsToFade.Add(spr);
        }

        foreach (ParticleSystem ps in transform.parent.GetComponentsInChildren<ParticleSystem>())
        {
                ParticleSystems.Add(ps);
        }

        ae2d = GetComponent<AreaEffector2D>();
        Power = ae2d.forceMagnitude;
        Flow();
	}

    void Flow()
    {
        Invoke("Stop", TimeFlowing);

        DOTween.To(() => ae2d.forceMagnitude, x => ae2d.forceMagnitude = x, Power, 0.25f);
        ChangeColor(Color.white);
    }

    void Stop()
    {
        Invoke("Flow", TimeStopped);

        DOTween.To(() => ae2d.forceMagnitude, x => ae2d.forceMagnitude = x, 0, 0.25f);
        ChangeColor(transparent);
    }
	
	void Update ()
    {
	
	}

    void ChangeColor(Color color)
    {
        foreach (SpriteRenderer spr in ObjectsToFade)
        {
            spr.DOColor(color, 0.25f);
        }

        if(color != Color.white)
        {
            foreach (ParticleSystem ps in ParticleSystems)
            {
                ps.Stop();
                ps.transform.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (ParticleSystem ps in ParticleSystems)
            {
                ps.Play();
                ps.transform.gameObject.SetActive(true);
            }
        }     
    }
}
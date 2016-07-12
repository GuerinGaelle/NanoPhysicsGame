using UnityEngine;
using System.Collections;

public class AutoDestroy : MonoBehaviour {

    public float timer = 1;

	void Update ()
    {
        timer -= Time.deltaTime;
        if(timer < 0)
        {
            Destroy(this.gameObject);
        }
	}
}

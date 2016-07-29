using UnityEngine;
using System.Collections;

public class Parallax : MonoBehaviour {

    private Transform _camera;
    private Transform _parallaxForeGround;

	void Start ()
    {
        _camera = GameObject.Find("Main Camera").transform;
	}
	
	void Update ()
    {
        _parallaxForeGround.transform.position = _camera.transform.position;

    }
}

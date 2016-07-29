using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DontDestroy : MonoBehaviour {
    
    public static DontDestroy instance;

	void Start()
	{
        //Causes UI object not to be destroyed when loading a new scene. If you want it to be destroyed, destroy it manually via script.
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }    
	}

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0 && !transform.GetChild(0).gameObject.activeSelf)
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }
}

using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    //-------------------------------------------------//
    //------------------- VARIABLES -------------------//
    //-------------------------------------------------//

    public static GameManager Instance;
    public CharacterBehaviour Player;

    //-------------------------------------------------//

    void Awake()
    {
        if(Instance == null)
            Instance = this;

        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterBehaviour>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Player.IsStuck = true;
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Player.IsStuck = false;
        }
    }
}
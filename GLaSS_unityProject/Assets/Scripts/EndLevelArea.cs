using UnityEngine;
using System.Collections;

public class EndLevelArea : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Debug.Log("trigg");
            GameManager.Instance.LevelFinished();
        }
    }
}

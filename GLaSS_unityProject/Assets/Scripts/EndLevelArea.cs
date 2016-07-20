using UnityEngine;
using System.Collections;

public class EndLevelArea : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            GameManager.Instance.LevelFinished();
        }
    }
}

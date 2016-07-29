using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemMenuPrincipal : MonoBehaviour
{

	void Update ()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(GameObject.Find("Start").gameObject);
    }
}

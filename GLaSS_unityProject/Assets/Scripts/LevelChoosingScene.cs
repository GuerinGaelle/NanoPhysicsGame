using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelChoosingScene : MonoBehaviour
{
    public int nblevels = 6;
    private List<Button> levelButtons;
    private GameObject levelLoaderButton;
    private Canvas canvas;

    void Start ()
    {
        LevelManager.LoadData();
        levelLoaderButton = Resources.Load<GameObject>("Prefabs/LevelLoaderButton");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        levelButtons = new List<Button>();

        for (int i = 1; i <= nblevels; i++)
        {
            GameObject _obj = Instantiate<GameObject>(levelLoaderButton) as GameObject;
            _obj.transform.SetParent(canvas.transform.FindChild("ButtonContainer"));
            Button _btn = _obj.GetComponent<Button>();
            _btn.GetComponentInChildren<Text>().text = "Level " + i;

            levelButtons.Add(_btn);

            if (LevelManager.UnlockedLevels < i)
            {
                _btn.interactable = false;
            }

            _btn.onClick.AddListener(delegate { LoadScene(_btn); });
        }
	}
	
    public void LoadScene(Button btn)
    {
        int nb = levelButtons.IndexOf(btn) + 1;
        int _realNb = nb + 2;

        canvas.gameObject.SetActive(false); // we hide the menu

        MovieTexture _movTexture = Resources.Load<MovieTexture>("Movies/LEVEL" + nb) as MovieTexture;
        GameObject.Find("MoviePlayer").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture = _movTexture;

        ((MovieTexture)GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture).Play();
        _movTexture.Play();

        StartCoroutine(LoadScene(_realNb));
    }

    IEnumerator LoadScene(int nb)
    {
        Debug.Log("Coroutine mon gars");
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(nb);
        yield return null;
    }
}
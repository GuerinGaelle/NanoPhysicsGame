using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelChoosingScene : MonoBehaviour
{
    public int nblevels = 6;
    private List<Button> levelButtons;
    private GameObject levelLoaderButton;
    private Canvas canvas;

    private List<GameObject> CarteUnlockImages;
    private int CurrentSelectedImage;
    private bool bLoad = false;

    // Set-up the scene and load the right data
    void Start ()
    {
        LevelManager.LoadData();
        levelLoaderButton = Resources.Load<GameObject>("Prefabs/LevelLoaderButton");
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        // Carte
        CarteUnlockImages = new List<GameObject>();
        int nbCartes = GameObject.Find("Map_Selectors").transform.childCount;
        for (int i = 0; i < nbCartes; i++)
        {
            CarteUnlockImages.Add(GameObject.Find("Map_Selectors").transform.GetChild(i).gameObject);
        }

        // Create the menu
        levelButtons = new List<Button>();
        for (int i = 1; i <= nblevels; i++)
        {
            GameObject _obj = GameObject.Find("LevelLoaderButton_" + i);
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

        for (int i = 0; i < levelButtons.Count; i++)
        {
            CarteUnlockImages[i].SetActive(levelButtons[i].interactable);
        }


        // Select a button
        EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);
        InvokeRepeating("FadeOutIn", 1, 1);
    }

    void Update()
    {
        if (bLoad)
            return;

        if(EventSystem.current.currentSelectedGameObject == null)
            EventSystem.current.SetSelectedGameObject(levelButtons[0].gameObject);

        string selectedButton = EventSystem.current.currentSelectedGameObject.name;
        int nb = levelButtons.IndexOf(GameObject.Find(selectedButton).GetComponent<Button>());

        if(CurrentSelectedImage != nb)
        {
            CurrentSelectedImage = nb;
            foreach (GameObject spr in CarteUnlockImages)
            {
                spr.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    void FadeOutIn()
    {
        StartCoroutine(FadeInOutCoroutine(CurrentSelectedImage));
    }

    IEnumerator FadeInOutCoroutine(int nb)
    {
        CarteUnlockImages[nb].GetComponent<SpriteRenderer>().color = Color.cyan;
        CarteUnlockImages[nb].GetComponent<SpriteRenderer>().DOFade(0, 0.5f);
        yield return new WaitForSeconds(0.5f);
        CarteUnlockImages[nb].GetComponent<SpriteRenderer>().DOFade(1, 0.5f);
        yield return new WaitForSeconds(0.5f);
        yield return null;
    }


    public void LoadScene(Button btn)
    {
        bLoad = true;

        int nb = levelButtons.IndexOf(btn) + 1;
        int _realNb = nb + 2;

        canvas.gameObject.SetActive(false); // we hide the menu

        MovieTexture _movTexture = Resources.Load<MovieTexture>("Movies/LEVEL" + nb) as MovieTexture;
        GameObject.Find("MoviePlayer").GetComponent<MeshRenderer>().enabled = true;
        GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture = _movTexture;

        ((MovieTexture)GameObject.Find("MoviePlayer").GetComponent<Renderer>().material.mainTexture).Play();
        _movTexture.Play();

        StartCoroutine(LoadSceneCoroutine(_realNb));
    }

    IEnumerator LoadSceneCoroutine(int nb)
    {
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(nb);
        yield return null;
    }
}
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MusicManager : MonoBehaviour {

    public AudioClip introMusic;
    public AudioClip loopingMusic;

    private AudioSource source;

    private AudioClip intro_123;
    private AudioClip intro_45;
    private AudioClip Level1;
    private AudioClip Level2;
    private AudioClip Level3;
    private AudioClip Level4;
    private AudioClip Level5;

    void Start ()
    {
        source = GetComponent<AudioSource>();
        source.clip = introMusic;
        source.Play();


        intro_123 = Resources.Load<AudioClip>("Music/level_music/intro_1_2_3");
        intro_45 = Resources.Load<AudioClip>("Music/level_music/intro_4_5");

        Level1 = Resources.Load<AudioClip>("Music/level_music/Level 1 FINAL");
        Level2 = Resources.Load<AudioClip>("Music/level_music/Level 2 FINAL");
        Level3 = Resources.Load<AudioClip>("Music/level_music/Level 3 FINAL");
        Level4 = Resources.Load<AudioClip>("Music/level_music/Level 4 FINAL");
        Level5 = Resources.Load<AudioClip>("Music/level_music/Level 5 FINAL");
    }

	void Update ()
    {
        if(SceneManager.GetActiveScene().name == "Level 1" && source.clip != Level1)
        {
            source.clip = Level1;
            source.Play();
            source.loop = true;
        } else if (SceneManager.GetActiveScene().name == "Level 2" && source.clip != Level2)
        {
            source.clip = Level2;
            source.Play();
            source.loop = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level 3 version Adrien" && source.clip != Level3)
        {
            source.clip = Level3;
            source.Play();
            source.loop = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level 4" && source.clip != Level4)
        {
            source.clip = Level4;
            source.Play();
            source.loop = true;
        }
        else if (SceneManager.GetActiveScene().name == "Level 5 s" && source.clip != Level5)
        {
            source.clip = Level5;
            source.Play();
            source.loop = true;
        }

        if (source.clip == introMusic && !source.isPlaying)
        {
            source.clip = loopingMusic;
            source.Play();
            source.loop = true;
        }
	}
}
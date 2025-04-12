using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    float wait = 0;

    private AudioSource audioSource;

    [SerializeField] float TransitionTime;

    [SerializeField] AudioClip MenuMusic;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = MenuMusic;
        audioSource.volume = 1;
        audioSource.Play();
    }

    public void StartLevel()
    {
        wait = TransitionTime;
        GameObject.Find("Menu").GetComponent<Animator>().Play("MenuDisappear");
    }

    // Update is called once per frame
    void Update()
    {
        if(wait > 0)
        {
            wait = Mathf.Max(0, wait - Time.deltaTime);

            if(wait == 0)
                SceneManager.LoadScene("Level");
        }
    }
}

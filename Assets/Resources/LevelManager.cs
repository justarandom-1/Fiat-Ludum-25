using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;



public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int gold;
    public int souls;

    public int phase;
    public GameObject Base = null;
    public GameObject LowerMenu;
    public List<Button> TowerButtons = new List<Button>();

    public TMP_Text GoldDisplay;
    public TMP_Text SoulsDisplay;

    public Button ExchangeButton;

    private List<(float, SpawnPattern)> schedule;

    float timer = 0;

    private AudioSource audioSource;

    int nextSpawn;

    // Start is called before the first frame update
    void Start()
    {
        phase = 0;
        instance = this;

        gold = 500;
        souls = PlayerPrefs.GetInt("souls", 0);

        SaveManager.instance.LoadData();

        TowerButtons.Add(null);

        Transform TowerBar = GameObject.Find("TowerBar").transform;

        for(int i = 0; i < 5; i++)
            TowerButtons.Add(TowerBar.GetChild(i).gameObject.GetComponent<Button>());

        GoldDisplay = GameObject.Find("GoldText").GetComponent<TMP_Text>();
        SoulsDisplay = GameObject.Find("SoulsText").GetComponent<TMP_Text>();
        
        GoldDisplay.text = "" + gold;
        SoulsDisplay.text = "" + souls;

        ExchangeButton = GameObject.Find("Exchange").GetComponent<Button>();

        ExchangeButton.interactable = souls > 0;
        
        audioSource = GetComponent<AudioSource>();

        nextSpawn = 0;


        schedule = new List<(float, SpawnPattern)>();

        // schedule.Add((1, new SpawnPattern(4, 4, 1, 0)));


        // return;

        //          Time                  S  T  N  R 
        schedule.Add((1, new SpawnPattern(3, 1, 1, 0)));
        schedule.Add((1.5f, new SpawnPattern(0, 1, 5, 1)));
        schedule.Add((2, new SpawnPattern(4, 1, 2, 2)));
        schedule.Add((3, new SpawnPattern(5, 1, 1, 0)));



        schedule.Add((10, new SpawnPattern(1, 1, 5, 1)));
        schedule.Add((10, new SpawnPattern(6, 1, 5, 1)));
        schedule.Add((10, new SpawnPattern(1, 1, 5, 1)));
        schedule.Add((10, new SpawnPattern(10, 1, 5, 1)));


        schedule.Add((25, new SpawnPattern(3, 1, 5, 1)));
        schedule.Add((25, new SpawnPattern(4, 1, 8, 1)));
        schedule.Add((25, new SpawnPattern(5, 1, 5, 1)));
        schedule.Add((30, new SpawnPattern(4, 2, 3, 3)));


        schedule.Add((45, new SpawnPattern(7, 2, 3, 1)));
        schedule.Add((45, new SpawnPattern(1, 1, 4, 1)));
        schedule.Add((45, new SpawnPattern(5, 1, 4, 1)));
        schedule.Add((45, new SpawnPattern(9, 1, 4, 1)));
        schedule.Add((47, new SpawnPattern(6, 3, 1, 0)));

        schedule.Add((60, new SpawnPattern(0, 1, 5, 1)));
        schedule.Add((60, new SpawnPattern(6, 2, 2, 1)));
        schedule.Add((65, new SpawnPattern(1, 3, 1, 0)));
        schedule.Add((66, new SpawnPattern(8, 3, 1, 0)));

        schedule.Add((81, new SpawnPattern(0, 2, 1, 0)));
        schedule.Add((81, new SpawnPattern(1, 2, 1, 0)));
        schedule.Add((81, new SpawnPattern(10, 2, 1, 0)));
        schedule.Add((81, new SpawnPattern(7, 2, 1, 0)));
        schedule.Add((81, new SpawnPattern(8, 2, 1, 0)));
        schedule.Add((81, new SpawnPattern(9, 2, 1, 0)));
        schedule.Add((85, new SpawnPattern(10, 3, 1, 0)));
        schedule.Add((86, new SpawnPattern(4, 3, 1, 0)));
        schedule.Add((88, new SpawnPattern(6, 3, 1, 0)));


        schedule.Add((99, new SpawnPattern(4, 4, 1, 0)));

    }

    private void UpdateUI()
    {
        TowerButtons[1].interactable = Base == null; 

        if(phase != 0)    
            for(int i = 2; i < TowerButtons.Count; i++)
                TowerButtons[i].interactable = TowerSelector.TowerCosts[i] <= gold;
        
        GoldDisplay.text = "" + gold;
        SoulsDisplay.text = "" + souls;

        ExchangeButton.interactable = souls > 0;
    }

    public void ExchangeSoul()
    {
        souls--;
        gold += 100;
        LevelManager.instance.UpdateUI();
        PlaySound(Resources.Load<AudioClip>("SFX/soulForCash"));
    }

    public void AddGold(int value)
    {
        gold += value;
        UpdateUI();
    }

    public void AddSouls(int value)
    {
        souls += value;
        UpdateUI();
    }

    public void PlaySound(AudioClip a)
    {
        audioSource.PlayOneShot(a);
    }

    // Update is called once per frame
    void Update()
    {        
        if(phase == 1)
        {
            if(Base == null){
                phase = 2; 
                GameObject.Find("black").GetComponent<Animator>().Play("endLevel");
            }

            timer+=Time.deltaTime;
            
            if(nextSpawn < schedule.Count && timer >= schedule[nextSpawn].Item1)
            {
                SpawnPattern s = schedule[nextSpawn].Item2;

                nextSpawn++;

                GameObject.Find(s.source).GetComponent<EnemySpawn>().addToQueue(s);
            }
        }

        if (Input.GetKeyDown(KeyCode.S) && ExchangeButton.interactable)
            ExchangeSoul();

        if(phase == 2){
            audioSource.volume = Mathf.Max(0, audioSource.volume - Time.deltaTime);
            if(audioSource.volume == 0){
                PlayerPrefs.SetInt("souls", souls);
                SaveManager.instance.SaveData();
                SceneManager.LoadScene("Menu");
            }
        }
    }
}

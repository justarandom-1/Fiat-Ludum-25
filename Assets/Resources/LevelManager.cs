using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static int gold;
    public static int souls;

    public static int phase;
    public static GameObject Base;
    public static GameObject LowerMenu;
    public static List<Button> TowerButtons = new List<Button>();

    [SerializeField] List<GameObject> TowerButtonObjects;
    public static TMP_Text GoldDisplay;
    public static TMP_Text SoulsDisplay;

    [SerializeField] GameObject GoldText;
    [SerializeField] GameObject SoulsText;

    public static Button ExchangeButton;
    [SerializeField] GameObject ExchangeButtonObject;

    private List<(int, SpawnPattern)> schedule;

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

        TowerButtons.Add(null);

        foreach(GameObject button in TowerButtonObjects)
            if(button && button.GetComponent<Button>())
                TowerButtons.Add(button.GetComponent<Button>());

        GoldDisplay = GoldText.GetComponent<TMP_Text>();
        SoulsDisplay = SoulsText.GetComponent<TMP_Text>();
        
        GoldDisplay.text = "" + gold;
        SoulsDisplay.text = "" + souls;

        ExchangeButton = ExchangeButtonObject.GetComponent<Button>();

        ExchangeButton.interactable = souls > 0;
        
        audioSource = GetComponent<AudioSource>();

        schedule = new List<(int, SpawnPattern)>();

        //          Time                  S  T  N  R 
        schedule.Add((1, new SpawnPattern(3, 1, 1, 0)));

        schedule.Add((2, new SpawnPattern(4, 1, 2, 2)));

        schedule.Add((3, new SpawnPattern(5, 1, 1, 0)));

        nextSpawn = 0;

    }

    public void UpdateUI()
    {
        TowerButtons[1].interactable = Base == null;        
        for(int i = 2; i < TowerButtons.Count; i++)
        {
            TowerButtons[i].interactable = TowerSelector.TowerCosts[i] <= gold;
        }
        GoldDisplay.text = "" + gold;
        SoulsDisplay.text = "" + souls;

        ExchangeButton.interactable = souls > 0;
    }

    public void ExchangeSoul()
    {
        souls--;
        gold += 200;
        UpdateUI();
    }

    public void PlaySound(AudioClip a)
    {
        audioSource.PlayOneShot(a);
    }

    // Update is called once per frame
    void Update()
    {        
        if(phase > 0)
        {
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
    }
}

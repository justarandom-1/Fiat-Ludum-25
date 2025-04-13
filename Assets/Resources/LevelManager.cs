using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public static int gold;
    public static int souls;
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

    // Start is called before the first frame update
    void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

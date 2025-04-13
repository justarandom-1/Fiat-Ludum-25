using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class LevelManager : MonoBehaviour
{
    public static int gold;
    public static int souls;
    public static GameObject LowerMenu;
    public static List<Button> TowerButtons = new List<Button>();

    [SerializeField] List<GameObject> TowerButtonObjects;
    public static Text GoldDisplay;
    public static Text SoulsDisplay;

    [SerializeField] GameObject GoldText;
    [SerializeField] GameObject SoulsText;

    public static Button ExchangeButton;
    [SerializeField] GameObject ExchangeButtonObject;

    // Start is called before the first frame update
    void Start()
    {
        gold = 500;
        souls = PlayerPrefs.GetInt("souls", 0);

        foreach(GameObject button in TowerButtonObjects)
            if(button && button.GetComponent<Button>())
                TowerButtons.Add(button.GetComponent<Button>());

        GoldDisplay = GoldText.GetComponent<Text>();
        SoulsDisplay = SoulsText.GetComponent<Text>();
    }

    public void UpdateButtons()
    {
        for(int i = 1; i < TowerButtons.Count; i++)
        {
            TowerButtons[i].interactable = TowerSelector.TowerCosts[i] <= gold;
        }
    }

    public void UpdateCurrency()
    {
        GoldDisplay.text = "" + gold;
        SoulsDisplay.text = "" + souls;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public enum Tower
{
    None,
    Base,
    Wall,
    WallRight,
    Shooter,

    DoubleShooter
}

public class TowerSelector : MonoBehaviour
{
    public static Tower selectedTower = Tower.None;

    public static List<int> TowerCosts;

    public static Vector2 MousePosition;
    [SerializeField] List<Sprite> PreviewImgs;
    [SerializeField] List<int> Costs;

    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        TowerCosts = Costs;
    }

    public void SelectTower(int n)
    {
        Debug.Log("Selected" + n);
        selectedTower = (Tower)n;
        spriteRenderer.sprite = PreviewImgs[n];        
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i <= 5; i++)
        {
            if (Input.GetKeyDown("" + i))
            {
                Debug.Log(LevelManager.TowerButtons[i].interactable);
                if((int)selectedTower == i)
                    SelectTower(0);
                else if(LevelManager.TowerButtons[i].interactable){
                    SelectTower(i);
                }
            }
        }
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(MousePosition.x, MousePosition.y, transform.position.z);
    }
}

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

    [SerializeField] List<GameObject> TowerObjects;

    private SpriteRenderer spriteRenderer;
    [SerializeField] int overlaps;
    private Color preview;
    private Color warning;
    private Collider2D collider_;
    private Animator animator;

    [SerializeField] AudioClip buildSFX;

    // Start is called before the first frame update
    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();
        TowerCosts = Costs;
        overlaps = 0;

        preview = spriteRenderer.color;
        warning = new Color(1, 0, 0, 0.5f);

        collider_ = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Tower" || other.gameObject.tag == "Environment" || other.gameObject.tag == "Ruin" || other.gameObject.tag == "Enemy"){
            overlaps++;
            spriteRenderer.color = warning;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Tower" || other.gameObject.tag == "Environment" || other.gameObject.tag == "Ruin" || other.gameObject.tag == "Enemy")
            if(overlaps <= 0)
                overlaps = 0;
            else
                overlaps--;
                if(overlaps == 0)
                    spriteRenderer.color = preview;
    }

    public void SelectTower(int n)
    {
        selectedTower = (Tower)n;

        animator.Play(n.ToString()); 

        if(n > 0)
            GetComponent<Collider2D>().enabled = true;
        else
            GetComponent<Collider2D>().enabled = false;      
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 1; i <= 5; i++)
            if (Input.GetKeyDown("" + i))
                if(LevelManager.TowerButtons[i].interactable && (int)selectedTower != i)
                    SelectTower(i);
                
                else 
                    SelectTower(0);

        
        
        MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(!Mathf.Approximately(MousePosition.x, transform.position.x) || !Mathf.Approximately(MousePosition.y, transform.position.y))
            transform.position = new Vector3(MousePosition.x, MousePosition.y, transform.position.z);

        if(Input.GetMouseButtonDown(0) && (int)selectedTower > 0 && overlaps == 0)
        {
            LevelManager.instance.PlaySound(buildSFX);
            GameObject n = Instantiate(TowerObjects[(int)selectedTower], 
                                       new Vector3(transform.position.x, transform.position.y, -1), 
                                       Quaternion.identity * TowerObjects[(int)selectedTower].transform.localRotation);
            if(selectedTower == Tower.Base){
                LevelManager.Base = n;
                if(LevelManager.phase == 0)
                    LevelManager.phase = 1;
            }
            LevelManager.AddGold(-1 * TowerCosts[(int)selectedTower]);
            SelectTower(0);
        }
    }
}

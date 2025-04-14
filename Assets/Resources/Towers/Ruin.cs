using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ruin : MonoBehaviour
{
    [SerializeField] int type;
    [SerializeField] int value;

    [SerializeField] AudioClip collectSFX;

    private SpriteRenderer spriteRenderer;
    private Color selected = new Color(1, 0.8215f, 0);
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnMouseEnter()
    {
        // Debug.Log("over");
        spriteRenderer.color = selected;
    }

    void OnMouseExit()
    {
        // Debug.Log("off");
        spriteRenderer.color = Color.white;
    }

    void OnMouseDown()
    {
        LevelManager.instance.PlaySound(collectSFX);
        LevelManager.instance.AddGold(value);
        // Destroy the gameObject after clicking on it
        Destroy(gameObject);
    }

    public int getValue()
    {
        return value;
    }

    public int getType()
    {
        return type;
    }

    public float getX()
    {
        return transform.position.x;
    }

    public float getY()
    {
        return transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

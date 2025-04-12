using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Tower
{
    None,
    Base,
    Wall,
    Shooter
}

public class TowerSelector : MonoBehaviour
{
    public static Tower selectedTower = Tower.None;
    [SerializeField] List<Sprite> PreviewImgs;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 3);
    }
}

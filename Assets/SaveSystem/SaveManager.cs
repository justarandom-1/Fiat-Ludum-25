using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] List<GameObject> Towers;
    [SerializeField] List<GameObject> Ruins;
    public static SaveManager instance; 

    private List<TowerData> savedData = new List<TowerData>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    struct TowerData
    {
        public int type;
        public float x;
        public float y;
        public int hp;
    }

    // runs when save button is clicked
    public void SaveData()
    {
        savedData = new List<TowerData>();

        var allRuins = FindObjectsByType<Ruin>(FindObjectsSortMode.None);

        foreach(Ruin r in allRuins)
            savedData.Add(new TowerData{
                            type = r.getType(),
                            x = r.getX(),
                            y = r.getY(),
                            hp = 0
                         });

        var allTowers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

        foreach(TowerController t in allTowers)
            savedData.Add(new TowerData{
                            type = t.getType(),
                            x = t.getX(),
                            y = t.getY(),
                            hp = t.GetHealth()
                         });
    }

    // called when load button is clicked
    public void LoadData()
    {
        foreach(TowerData t in savedData){
            if(t.hp == 0)
                Instantiate(Ruins[t.type], 
                            new Vector3(t.x, t.y, -1), 
                            Quaternion.identity * Ruins[t.type].transform.localRotation);
            else
            {
                GameObject newTower = Instantiate(Towers[t.type], 
                                                  new Vector3(t.x, t.y, -1), 
                                                  Quaternion.identity * Towers[t.type].transform.localRotation);
                newTower.GetComponent<TowerController>().setHealth(t.hp);
            }
        }
    }

}

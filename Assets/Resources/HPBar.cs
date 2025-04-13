using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPBar : MonoBehaviour
{
    // Start is called before the first frame update
    private RectTransform healthbar;
    private Transform parent;
    private GameEntity parentScript;

    private Vector3 offset;
    void Start()
    {
        healthbar = transform.GetChild(0).gameObject.GetComponent<RectTransform>();

        parent = transform.parent;
        parentScript = parent.gameObject.GetComponent<GameEntity>();

        offset = transform.position - transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        healthbar.sizeDelta = new Vector2(60 * parentScript.getHealth(), healthbar.sizeDelta.y);

        transform.position = parent.position + offset;

        transform.rotation = Quaternion.Euler (0.0f, 0.0f, parent.rotation.z * -1.0f);

    }
}

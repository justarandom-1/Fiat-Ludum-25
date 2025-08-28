using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LichSoul : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;

    private Vector2 direction;

    private Image fade;

    private float initDistance;

    [SerializeField] float speed;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        Vector2 distance = new Vector2(-11 - transform.position.x, -1 * transform.position.y);

        direction = distance.normalized;

        initDistance = distance.magnitude;

        rb.velocity = direction * speed;

        transform.Rotate(0, 0, VectorToAngle(direction) - 90);

        fade = GameObject.Find("black (1)").GetComponent<Image>();

        var allEnemies = FindObjectsByType<EnemyMovement>(FindObjectsSortMode.None);

        foreach(EnemyMovement enemy in allEnemies)
        {
            Destroy(enemy.gameObject);
        }
    }

    float VectorToAngle(Vector2 v)
    {
        if(v.magnitude == 0) return 0;

        float r = Mathf.Asin(v.y / v.magnitude) * 180 / Mathf.PI;

        if(v.x < 0)
            return 180 - r;

        return r ;            
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 distance = new Vector2(-11 - transform.position.x, -1 * transform.position.y);

        fade.color = new Color(0, 0, 0, 1 - Mathf.Max(distance.magnitude, 0) / initDistance);

        if(transform.position.x < -11)
        {
            if(LevelManager.instance.Base != null)
                SceneManager.LoadScene("WinningScene");
            Destroy(gameObject);
        }
        
    }
}

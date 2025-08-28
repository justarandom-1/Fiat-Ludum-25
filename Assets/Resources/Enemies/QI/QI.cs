using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Pathfinding;


public class QI : EnemyMovement
{
    // [SerializeField] protected float speed;
    // [SerializeField] protected int value;
    // protected bool isMoving;

    // protected Transform Base;

    // protected float nextWayPointDistance = 1f;

    // protected Path path;
    // protected int currentWaypoint = 0;
    // bool reachedTarget = false;

    // Seeker seeker;

    float xDirection;

    [SerializeField] protected GameObject projectilePrefab;


    [SerializeField] float fireRate;

    [SerializeField] float teleportRate;

    [SerializeField] AudioClip teleportSFX;
    private Transform hand;

    [SerializeField] float timer;

    private float teleportTimer = 0;

    private GameObject beam = null;

    private int state;

    [SerializeField] List<Vector2> teleportCoordinates;

    int prevTeleport = 3;

    private List<GameObject> allTowers = new List<GameObject>();

    [SerializeField] protected GameObject skeletonPrefab;

    [SerializeField] protected float spawnRate;

    [SerializeField] protected float spawnTimer;

    [SerializeField] protected GameObject soul;

    [SerializeField] GameObject starsPrefab;

    [SerializeField] protected GameObject crocodilePrefab;

    private Shield shield;
    private int summonedCrocodiles = 0;

    private bool usedWave = false;
    private bool usedStars = false;

    private bool justUsedSpecial = false;

    [SerializeField] bool isUnderHaste = false;

    private float hasteTimer = 0;

    [SerializeField] float hasteDuration;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        hand = transform.GetChild(3);        

        state = 0;

        var data = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

        foreach(TowerController t in data)
            allTowers.Add(t.gameObject);

        shield = transform.GetChild(9).gameObject.GetComponent<Shield>();
    }

    public override void Push(Vector2 other)
    {
        if(shield.getState() == 1)
            return;
        LevelManager.instance.PlaySound(hitSFX, 0.4f);
        Vector2 force = ((Vector2)transform.position - other).normalized;
        rb.AddForce(force * 100);
        UpdatePath();
    }

    protected override void kill()
    {

        Instantiate(soul, new Vector3(transform.position.x, transform.position.y, -2), Quaternion.identity);

        base.kill();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if(shield.getState() == 1)
            return;
        base.OnTriggerEnter2D(other);
    }


    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if(shield.getState() == 1)
            return;
        base.OnCollisionEnter2D(collision);
    }


    public override void takeDamage(int dmg)
    {
        if(shield.getState() != 1)
            base.takeDamage(dmg);
    }

    protected void haste()
    {
        animator.speed = 3;
        isUnderHaste = true;
        hasteTimer = hasteDuration;
    }

    protected void unHaste()
    {
        animator.speed = 1;
        isUnderHaste = false;
    }

    protected override void FixedUpdate()
    {

        allTowers.RemoveAll(item => item == null);

        if(Base == null)
            return;

        xDirection = Base.position.x - transform.position.x;
        xDirection = xDirection / Mathf.Abs(xDirection);
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * -1 * xDirection, transform.localScale.y, transform.localScale.z);


        if(state == 1)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("MidTeleport")){
                state = 0;
                animator.Play("Appear");
                UpdatePath();
                teleportTimer = teleportRate;

                justUsedSpecial = false;
            }

            return;
        }

        teleportTimer = Mathf.Max(teleportTimer - Time.deltaTime, 0);

        if(teleportTimer == 0 && beam == null && !isUnderHaste)
        {
            rb.velocity = new Vector2(0, 0);
            state = 1;
            animator.Play("Teleport");

            int t = Random.Range(0, teleportCoordinates.Count);

            while(t == prevTeleport)
                t = Random.Range(0, teleportCoordinates.Count);

            Vector2 target = teleportCoordinates[t];

            var allTowers = FindObjectsByType<TowerController>(FindObjectsSortMode.None);

            for(int i = 0; i < allTowers.Length; i++)
            {
                if((target - (Vector2)allTowers[i].getPos()).magnitude < 2f)
                    allTowers[i].takeDamage(666);
            }

            transform.position = new Vector3(target.x, target.y, transform.position.z);
            prevTeleport = t;
            LevelManager.instance.PlaySound(teleportSFX);

            
            
            return;
        }

        if(animator.GetCurrentAnimatorStateInfo(0).IsName("Attacking") && timer == -1){
            if(!isUnderHaste)
                timer = fireRate;
            else
                timer = 0.1f;
            FireProjectile();
        }

        if((!animator.GetCurrentAnimatorStateInfo(0).IsName("QI") && !animator.GetCurrentAnimatorStateInfo(0).IsName("Attack")))
        {
            rb.velocity = new Vector2(0, 0);
            return;
        }

        if(isUnderHaste)
        {
            hasteTimer = Mathf.Max(hasteTimer - Time.deltaTime, 0);
            if(hasteTimer == 0)
                unHaste();
        }

        if(path != null)
        {
            if(currentWaypoint >= path.vectorPath.Count){
                reachedTarget = true;
                return;
            }

            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

            Vector2 force = direction * speed * Time.deltaTime;

            rb.AddForce(force);

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if (distance < nextWayPointDistance)
                currentWaypoint++;
        }

        if(timer > 0)
        {
            timer = Mathf.Max(timer - Time.deltaTime, 0);
        }
        else if(animator.GetCurrentAnimatorStateInfo(0).IsName("QI") && beam == null)
        {
            timer = -1;
            animator.Play("Attack");
            beam = hand.gameObject;
        }

        if(curHealth < maxHealth / 2)
            spawnTimer = Mathf.Max(spawnTimer - Time.deltaTime, 0);

        if(!(teleportTimer > 1 && teleportTimer < teleportRate -1) || justUsedSpecial || beam != null || shield.getState() == 1)
            return;
        
        if(curHealth < maxHealth / 3 && shield.getState() != 1 && !usedWave)
        {
            usedWave = true;
            animator.Play("Wave");
            LevelManager.instance.PlaySound(Resources.Load<AudioClip>("SFX/firing"));
            teleportTimer = 5;
            justUsedSpecial = true;
        }

        else if(curHealth < maxHealth * 0.5f && Random.Range(0, 100) == 0)
        {
            haste();
            justUsedSpecial = true;
        }

        else if(summonedCrocodiles < 2 && Random.Range(0, 666) == 0)
        {
            summonedCrocodiles++;
            Instantiate(crocodilePrefab, new Vector3(transform.position.x + xDirection, transform.position.y + 1, -1), Quaternion.identity);
            justUsedSpecial = true;
        }
        
        else if(!usedStars && Random.Range(0, 666) == 0)
        {
            usedStars = true;
            animator.Play("ThrowStars");
            Instantiate(starsPrefab, new Vector3(transform.position.x, transform.position.y + 0.41f, -1), Quaternion.identity);
            justUsedSpecial = true;
        }

        else if(curHealth < maxHealth * 0.9f && shield.getState() == 0 && Random.Range(0, 300) == 0)
        {
            shield.activate();
            justUsedSpecial = true;
        }

        else if(curHealth < maxHealth / 2 && spawnTimer == 0)
        {
            spawnTimer = spawnRate;

            Instantiate(skeletonPrefab, new Vector3(transform.position.x + 1, transform.position.y + 1, -1), Quaternion.identity);

            Instantiate(skeletonPrefab, new Vector3(transform.position.x - 1, transform.position.y + 1f, -1), Quaternion.identity);

            Instantiate(skeletonPrefab, new Vector3(transform.position.x - 1, transform.position.y - 1, -1), Quaternion.identity);

            Instantiate(skeletonPrefab, new Vector3(transform.position.x + 1, transform.position.y - 1, -1), Quaternion.identity);

            justUsedSpecial = true;
        }
    
    }

    protected void FireProjectile()
    {
        beam = Instantiate(projectilePrefab, hand.position, Quaternion.identity);
        beam.GetComponent<QIBeam>().Initialize(hand, allTowers.Count);
    }
}

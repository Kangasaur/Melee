using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Knight : MonoBehaviour
{
    NavMeshAgent nav;
    Animator anim;
    public Transform goal;
    [SerializeField] GameObject healthBar;
    [SerializeField] string enemyTag;
    [SerializeField] float spotDist = 10f;
    [SerializeField] float hitDist = 5f;
    float maxHitTime = 7f;
    float hitTime;
    float currHitTime = 0;
    float _health = 0;
    [HideInInspector] public bool isDead = false;
    float healthBarMax;
    float healthBarPos;

    [HideInInspector] public GameObject[] enemyTeam;

    float health
    {
        get { return _health; }
        set { _health = Mathf.Clamp(value, 0, totalHealth); }
    }
    float totalHealth = 20f;

    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        nav.SetDestination(goal.position);
        anim.SetBool("isWalking", true);
        health = totalHealth;
        nav.speed = 3f;
        hitTime = Random.Range(1f, maxHitTime);
        healthBarMax = healthBar.transform.localScale.x;
        healthBarPos = healthBar.transform.localPosition.x;
        nav.isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead && !nav.isStopped)
        {
            Transform nearest = GetNearestEnemy();
            currHitTime += Time.deltaTime;
            if (nearest != null && Vector3.Distance(transform.position, nearest.position) < spotDist)
            {
                nav.SetDestination(nearest.position);
                if (Vector3.Distance(transform.position, nearest.position) < hitDist)
                {
                    nav.speed = 3f;
                    if (currHitTime > hitTime)
                    {
                        hitTime = Random.Range(1f, maxHitTime);
                        currHitTime = 0f;
                        anim.SetTrigger("attack");
                        nearest.gameObject.GetComponent<Knight>().GetHit();
                    }
                }
                else nav.speed = 8f;
            }
            else
            {
                nav.SetDestination(goal.position);
                nav.speed = 3f;
            }
            anim.SetFloat("velocity", nav.velocity.magnitude);
        }
        

        if (health == 0 && !isDead)
        {
            nav.isStopped = true;
            anim.SetTrigger("dead");
            isDead = true;
        }
    }

    Transform GetNearestEnemy()
    {
        Transform nearest = null;
        float distance = Mathf.Infinity;
        foreach (GameObject enemy in enemyTeam)
        {
            if (Vector3.Distance(transform.position, enemy.transform.position) < distance && !enemy.GetComponent<Knight>().isDead)
            {
                distance = Vector3.Distance(transform.position, enemy.transform.position);
                nearest = enemy.transform;
            }
        }

        return nearest;
    }

    public void GetHit()
    {
        health -= Random.Range(1f, 5f);
        healthBar.transform.localScale = new Vector3(health / 20f * healthBarMax, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        healthBar.transform.localPosition = new Vector3(health / 20f * healthBarPos, 0f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform == goal)
        {
            FindObjectOfType<GameManager>().EndGame(enemyTag == "BlueTeam" ? "Red" : "Blue");
        }
    }
}

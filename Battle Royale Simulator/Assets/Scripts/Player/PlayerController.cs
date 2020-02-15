using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    Vector3 centerPosition;
    [SerializeField] float sightDistance = 10f;
    float interactRange;
    Animator animator;

    Chest nextChest;
    [SerializeField] PlayerController nextPlayer;
    [SerializeField] PlayerStatsTXT stats;
    float distanceToNextPlayer = Mathf.Infinity;
    float distanceToNextChest = Mathf.Infinity;
    float turnSpeed = 10;
    //PlayerIdentity identity;
    PlayerStatsTXT identity;

    [SerializeField] int damage = 2;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        interactRange = agent.stoppingDistance;
        centerPosition = new Vector3(0, 0, 0);
        identity = GetComponent<PlayerStatsTXT>();
    }
  
    void Update()
    {
        LookForChest();
        MoveToTarget();
    }

    void LookForChest()
    {
        float distanceToClosestChest = Mathf.Infinity;
        Chest[] allChests = GameObject.FindObjectsOfType<Chest>();

        foreach (Chest currentChest in allChests)
        {
            float distanceToChest = (currentChest.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToChest < distanceToClosestChest)
            {
                distanceToClosestChest = distanceToChest;
                nextChest = currentChest;
            }
        }
    }

    private void MoveToTarget()
    {
        if(nextPlayer != null)
        {
            distanceToNextPlayer = Vector3.Distance(nextPlayer.transform.position, transform.position);
            agent.SetDestination(nextPlayer.transform.position);
            if (distanceToNextPlayer <= agent.stoppingDistance)
            {
                FacePlayer();
                StartCoroutine(Attack());
            }
        }
        else if(nextPlayer == null && nextChest != null)
        {
            nextPlayer = null;
            distanceToNextChest = Vector3.Distance(nextChest.transform.position, transform.position);
            if(distanceToNextChest <= agent.stoppingDistance)
            {
                //stats.SetWeapon(nextChest.GiveWeapon());
                Destroy(nextChest.gameObject);
            }
            agent.SetDestination(nextChest.transform.position);

        }
        else if(nextPlayer == null && nextChest == null)
        {
            nextPlayer = null;
            agent.SetDestination(centerPosition);
        }
    }

    void LookForPlayer()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(transform.position, Vector3.forward);

        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * sightDistance, Color.blue, 1);

        if (Physics.Raycast(landingRay, out hit, 100))
        {
            Debug.DrawRay(transform.position, Vector3.forward * sightDistance, Color.red, 100);

            if(hit.collider.tag == "Player")
            {
                print("hit player");
                Debug.DrawRay(transform.position, Vector3.forward * sightDistance, Color.red, 100);
                nextPlayer = hit.collider.GetComponent<PlayerController>();
            }
        }
    }

    bool ReturnChestPosition()
    {
        float distanceToChest = Vector3.Distance(transform.position, nextChest.transform.position);

        if (distanceToChest <= interactRange) return true;
        else return false;
    }

    void FacePlayer()
    {
        Vector3 direction = (nextPlayer.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    #region Attack

    IEnumerator Attack()
    {
        float attackDelay = Random.Range(0, 10);
        yield return new WaitForSeconds(attackDelay);
        animator.SetTrigger("attack");
    }

    public void AttackHitEvent()
    {
        if (nextPlayer == null) return;
        Health target = nextPlayer.GetComponent<Health>();

        if (target == null) return;

        if (target.TakeDamage(damage))
        {
            //KillFeed.Instance.AddKillFeedItemIdentity(identity.ReturnPlayerName(), target.GetComponent<PlayerIdentity>().ReturnPlayerName());
            KillFeed.Instance.AddKillFeedItem(identity.ReturnPlayerName(), target.GetComponent<PlayerStatsTXT>().ReturnPlayerName());
            identity.AddKills();
        }
    }
    #endregion

    public PlayerController ReturnNextPlayer()
    {
        return nextPlayer;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            nextPlayer = other.gameObject.GetComponent<PlayerController>();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}

/*private void MoveToTarget()
    {
        if(nextPlayer != null)
        {
            distanceToNextPlayer = Vector3.Distance(nextPlayer.transform.position, transform.position);
            agent.SetDestination(nextPlayer.transform.position);
            if(distanceToNextPlayer <= agent.stoppingDistance)
            {
                print("going to attack" + nextPlayer);
                FacePlayer();
                StartAttack();
            }
        }

        if (nextPlayer == null)
        {
            nextPlayer = null;
        }

        else if (nextChest != null)
        {
            if (!ReturnChestPosition())
            {
                agent.SetDestination(nextChest.transform.position);
            }
            else Destroy(nextChest.gameObject);
        }

        else agent.SetDestination(centerPosition);
    }*/

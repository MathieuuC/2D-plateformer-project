using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class PatrolEnnemy : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float distance = 1f;
    [SerializeField] private float attackRange = 10f;
    [SerializeField] private Transform checkpoint;
    [SerializeField] private Transform player;
    [SerializeField] private Animator animator;
    [SerializeField] private float retrieveDistance = 2.5f;
    [SerializeField] private float chaseSpeed = 4f;

    public LayerMask layerMask;
    
    public bool inRange = false;
    public float attackRadius = 1f;
    public Transform attackPoint;
    public LayerMask attackLayer;



    private bool facingLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector2.Distance(transform.position, player.position) <= attackRange){
            inRange = true;
        } else {
           inRange = false;
        }

        if(inRange) {

            if(player.position.x > transform.position.x && facingLeft){
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;
            } else if (player.position.x < transform.position.x && !facingLeft) {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;

            }
                if(Vector2.Distance(transform.position,player.position) > retrieveDistance) {
                animator.SetBool("Attack1", false);
                transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
            } else {
                animator.SetBool("Attack1", true);
            }
        } else {
            transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

            RaycastHit2D hit = Physics2D.Raycast(checkpoint.position, Vector2.down, distance, layerMask);

            if(!hit && facingLeft) {
                transform.eulerAngles = new Vector3(0, -180, 0);
                facingLeft = false;

            } else if(!hit && !facingLeft) {
                transform.eulerAngles = new Vector3(0, 0, 0);
                facingLeft = true;
            }

        }

        
    }

    private void OnDrawGizmos() {

        if(checkpoint == null) {
            return;
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(checkpoint.position, Vector2.down * distance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        if(attackPoint == null) {
            return; }

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);


    }

    public void Attack() {
        Collider2D collInfo = Physics2D.OverlapCircle(attackPoint.position,attackRadius,attackLayer);   
        
        if(collInfo) {
            if(collInfo.gameObject.GetComponent<Player>() != null) {
                collInfo.gameObject.GetComponent<Player>().TakeDamage(1);

            }
        }
    }
}

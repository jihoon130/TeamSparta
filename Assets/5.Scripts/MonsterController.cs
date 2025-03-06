using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float forceMultiplier = 2000f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float massDefault = 1f;
    [SerializeField] private float massHeavy = 80f;
    [SerializeField] private float detectDistance = 0.03f;
    [SerializeField] private float maxTargetDistance = 2.3f;
    [SerializeField] private float yDistThreshold = 0.5f;
    [SerializeField] private float yDistMax = 1.0f;


    private GameObject target;
    private Rigidbody2D rigidbody;
    private Vector3 targetPosition;
    private bool isGround = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        bool isEnemy = CheckEnemyPresence(Vector2.right, new Vector2(1.0f, 0.5f));

        rigidbody.mass = isEnemy ? massHeavy : (isGround ? massDefault : rigidbody.mass);

        if (target == null)
        {
            MoveLeft();
        }
        else if (target.layer != 3)
        {
            HandleTarget();
        }
        else
        {
            CheckTargetDistance();
        }
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void HandleTarget()
    {
        float yDist = Mathf.Abs(transform.position.y - target.transform.position.y);
        bool isEnemy = CheckEnemyPresence(Vector2.left, new Vector2(-1.0f, 1.3f));

        if (yDist < yDistThreshold && !isEnemy)
        {
            rigidbody.mass = massHeavy;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, jumpForce);
        }
        else if (yDist > yDistMax)
        {
            target = null;
        }
    }

    private void CheckTargetDistance()
    {
        float xDistance = Mathf.Abs(target.transform.position.x - transform.position.x);
        if (xDistance >= maxTargetDistance)
        {
            target = null;
        }
    }

    private bool CheckEnemyPresence(Vector2 direction, Vector2 offset)
    {
        return Physics2D.Raycast((Vector2)transform.position + offset, direction, detectDistance, 1 << gameObject.layer);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            target = collision.gameObject;
        }
        else if (collision.gameObject.layer == gameObject.layer && collision.transform.position.x < transform.position.x && target == null && !collision.gameObject.CompareTag("Ground"))
        {
            target = collision.gameObject;
            targetPosition = new Vector3(target.transform.position.x, target.GetComponent<BoxCollider2D>().bounds.max.y, target.transform.position.z);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGround = collision.gameObject.CompareTag("Ground");
    }
}

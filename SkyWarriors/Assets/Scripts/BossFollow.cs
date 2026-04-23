using UnityEngine;

public class BossFollow : MonoBehaviour
{
    public float moveSpeed = 0.5f;
    public float raycastDistance = 5f;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (player == null) return;

        Vector2 direction = player.position - transform.position;

        Vector2 directionNormalized = direction.normalized;
        Vector2 moveDirection = directionNormalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, directionNormalized, raycastDistance);
        Debug.DrawRay(transform.position, directionNormalized * raycastDistance, Color.red);

        if (hit.collider != null && hit.collider.CompareTag("Obstacle"))
        {
            moveDirection = Quaternion.Euler(0f, 0f, -90f) * directionNormalized;
        }

        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}

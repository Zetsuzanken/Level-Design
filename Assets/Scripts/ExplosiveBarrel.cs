using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    public float triggerDistance = 5f;
    public GameObject explosionPrefab;
    public float explosionYOffset = 2f;
    private bool isTriggered = false;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("No Animator component found on the barrel.");
        }
    }

    void Update()
    {
        if (!isTriggered)
        {
            float distanceToPlayer = Vector3.Distance(Camera.main.transform.position, transform.position);
            if (distanceToPlayer <= triggerDistance)
            {
                isTriggered = true;
                animator.SetTrigger("StartDeformation");
            }
        }
    }

    public void OnDeformationAnimationEnd()
    {
        Vector3 explosionPosition = transform.position + new Vector3(0, explosionYOffset, 0);
        Instantiate(explosionPrefab, explosionPosition, Quaternion.identity);
        PushPlayerAway();
    }

    private void PushPlayerAway()
    {
        FirstPersonMovement playerMovement = FindObjectOfType<FirstPersonMovement>();

        if (playerMovement != null)
        {
            Vector3 direction = playerMovement.transform.position - transform.position;
            direction.y = 0;
            direction.Normalize();

            float pushForce = 50f;

            playerMovement.AddExternalForce(direction * pushForce);
        }
    }
}
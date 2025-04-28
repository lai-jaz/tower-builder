using UnityEngine;

public class CheckCollapsed : MonoBehaviour
{
    [HideInInspector] public TowerPhysics tp;
    [SerializeField] private float maxTiltAngle = 30f;

    private void Update()
    {
        if (tp.collapsed) return;
        float blockTiltAngle = Vector3.Angle(Vector3.up, transform.up); // collapse because of tilt
        if (blockTiltAngle > maxTiltAngle)
        {
            tp.CollapseTower(false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (tp.collapsed) return;
        if(collision.collider.CompareTag("Ground")) // collapse because of collision with ground
        {
            tp.CollapseTower(false);
        }
    }
}

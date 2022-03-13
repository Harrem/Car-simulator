using UnityEngine;

public class Wheel : MonoBehaviour
{
    Rigidbody rb;


    [Header("Suspension")]
    public float restLength;
    public float springTravel;
    public float springStiffness;
    public float damperStiffness;

    float minLength;
    float maxLength;
    float lastLength;
    float springLength;
    float springVelocity;
    float springForce;
    float damperForce;

    Vector3 suspensionForce;

    [Header("Wheel")]
    public float wheelRaduis;

    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    void FixedUpdate()
    {
        if(Physics.Raycast(transform.position, -transform.up, 
            out RaycastHit hit, maxLength + wheelRaduis)){

            lastLength = springLength;

            springLength    = hit.distance - wheelRaduis;
            springLength    = Mathf.Clamp(springLength, minLength, maxLength);
            springVelocity  = (lastLength - springLength) / Time.fixedDeltaTime;
            springForce     = springStiffness * (restLength - springLength);
            damperForce     = damperStiffness * springVelocity;
            suspensionForce = (springForce + damperForce) * transform.up;

            rb.AddForceAtPosition(suspensionForce, hit.point);
        }
    }
}

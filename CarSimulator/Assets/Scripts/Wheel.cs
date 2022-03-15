using UnityEngine;

public class Wheel : MonoBehaviour
{
    Rigidbody rb;

    public bool wheelFrontLeft;
    public bool wheelFrontRight;
    public bool wheelRearLeft;
    public bool wheelRearRight;


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


    [Header("Wheel")]
    public float wheelRaduis;
    public float steerAngle;
    public float steerTime;

    Vector3 suspensionForce;
    Vector3 wheelVelocityLS;
    float fX;
    float fY;
    float wheelAngle;


    void Start()
    {
        rb = transform.root.GetComponent<Rigidbody>();

        minLength = restLength - springTravel;
        maxLength = restLength + springTravel;
    }

    void Update(){
        wheelAngle = Mathf.Lerp(wheelAngle, steerAngle, steerTime * Time.deltaTime);
        transform.localRotation = Quaternion.Euler(Vector3.up * wheelAngle);

        Debug.DrawRay(transform.position, -transform.up *( springLength + wheelRaduis), Color.green);
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

            wheelVelocityLS = transform.InverseTransformDirection(rb.GetPointVelocity(hit.point));
            fX = Input.GetAxis("Vertical") * springForce;
            fY = wheelVelocityLS.x * springForce;

            rb.AddForceAtPosition(suspensionForce + (fX * transform.forward) + (fY * -transform.right), hit.point);
        }
    }
}

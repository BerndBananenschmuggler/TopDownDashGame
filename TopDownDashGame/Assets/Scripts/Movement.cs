using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement
{
    private Rigidbody m_rigidbody;

    public Movement(Rigidbody rigidbody)
    {
        this.m_rigidbody = rigidbody;
    }

    public void ChangeRigidbody(Rigidbody rigidbody)
    {
        this.m_rigidbody = rigidbody;
    }

    public void MoveRigidbody(Vector3 movement, float speed)
    {
        Vector3 velocity = m_rigidbody.velocity.y * Vector3.up +
                           m_rigidbody.transform.forward * movement.magnitude * speed;
        m_rigidbody.velocity = velocity;
    }

    public void StopRigidbody()
    {
        Vector3 velocity = m_rigidbody.velocity.y * Vector3.up;
        m_rigidbody.velocity = velocity;
    }

    public void RotateTransformTowardsForward(Transform transform, Vector3 movement, float rotationSpeed)
    {
        if (movement != Vector3.zero)
        {
            Vector3 skewedMovement = movement.ToIso();

            Quaternion currentRotation = m_rigidbody.transform.rotation;
            Quaternion rotation = Quaternion.LookRotation(movement.ToIso(), Vector3.up);
            //transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);


            Vector3 origin = transform.position;
            Vector3 direction = movement.ToIso().normalized;
            Debug.DrawRay(origin, direction, Color.red);

            //m_rigidbody.transform.rotation = Quaternion.RotateTowards(currentRotation, rotation, rotationSpeed * Time.deltaTime);
            m_rigidbody.transform.rotation = Quaternion.RotateTowards(currentRotation, rotation, float.MaxValue);
        }
    }
}

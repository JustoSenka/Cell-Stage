using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(ConstantForce))]
public class Movement : NetworkBehaviour {

	private Rigidbody rb;
    private ConstantForce cf;
	
    public float horizontalSpeed = 10f;
    public float airSpeed = 5f;
	public float jumpStrength = 500f;

    private bool inAir = false;
    private bool canJump = true;
    
	void Start () 
    {
		rb = gameObject.GetComponent<Rigidbody>();
        cf = gameObject.GetComponent<ConstantForce>();
	}

	void FixedUpdate ()
	{
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        AddConstantHorizontalForce(h);
        AddJumpForce(v);
	}

    private void AddConstantHorizontalForce(float h)
    {
        if (!inAir)
        {
            cf.force = new Vector3(h * horizontalSpeed, 0, 0);
        }
        else
        {
            cf.force = new Vector3(h * airSpeed, 0, 0);
        }
    }

    private void AddJumpForce(float v)
    {
        if (v > 0 && !inAir && canJump)
        {
            if (rb == null)
            {
                Debug.Log("Rigid body null. RigidBody: " + rb); // Strange behavior, bug?
            }
            else
            {
                rb.AddExplosionForce(jumpStrength, transform.position + new Vector3(0, -1, 0), 10f);
                inAir = true;
                canJump = false;
                StartCoroutine(SetCanJumpAfter(0.05f));
            }
        }
    }

    public IEnumerator SetCanJumpAfter(float time)
    {
        yield return new WaitForSeconds(time);
        canJump = true;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (!(inAir = IsInAir(collision)))
        {
            AddJumpForce(Input.GetAxisRaw("Vertical"));
        }
    }

    public void OnCollisionStay(Collision collision)
    {
        inAir = IsInAir(collision);
    }

    public void OnCollisionExit(Collision collision)
    {
        inAir = true;
    }

    private bool IsInAir(Collision collision)
    {
        bool temp = true;
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.otherCollider.tag.Equals("Ground") && contact.point.y < transform.position.y)
            {
                temp = false;
            }
        }
        return temp;
    }
}
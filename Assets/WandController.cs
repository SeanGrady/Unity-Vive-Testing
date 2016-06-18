using UnityEngine;
using System.Collections;

public class WandController : MonoBehaviour {

    public Rigidbody attachPoint;
    public float pullForce;

    SteamVR_TrackedObject trackedObj;
    FixedJoint joint;
    GameObject pickup;

    void Awake () {
        trackedObj = GetComponent<SteamVR_TrackedObject>();
    }

    // TODO: Something better than just GetTouch or GetTouchDown for grabbing object
    //       (GetTouchDown with a bigger sphere maybe? who knows...)
    void FixedUpdate ()
    {
        var device = SteamVR_Controller.Input((int)trackedObj.index);
        if (joint == null && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && pickup == null)
        {
            Debug.Log("pulling on tiny cube?");
            var magnetObj = GameObject.FindWithTag("Planetoid");
            Debug.Log(magnetObj.name);
            var mrb = magnetObj.GetComponent<Rigidbody>();
            var heading = trackedObj.transform.position - mrb.transform.position;
            var direction = heading / heading.magnitude;
            mrb.useGravity = false;
            mrb.AddForce(direction * pullForce);
        }
        if (joint == null && device.GetTouch(SteamVR_Controller.ButtonMask.Trigger) && pickup != null)
        {
            pickup.transform.position = attachPoint.transform.position;
            joint = pickup.AddComponent<FixedJoint>();
            joint.connectedBody = attachPoint;
        }
        else if (joint != null && device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            var rb = pickup.GetComponent<Rigidbody>();
            Object.DestroyImmediate(joint);
            rb.useGravity = true;
            joint = null;

			var origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
			if (origin != null)
			{
				rb.velocity = origin.TransformVector(device.velocity);
				rb.angularVelocity = origin.TransformVector(device.angularVelocity);
			}
			else
			{
				rb.velocity = device.velocity;
				rb.angularVelocity = device.angularVelocity;
			}

			rb.maxAngularVelocity = rb.angularVelocity.magnitude;
        }
    }

    void OnTriggerEnter (Collider collider)
    {
        pickup = collider.gameObject;
        // Debug.Log("wand trigger entered");
    }

    void OnTriggerExit (Collider collider)
    {
        pickup = null;
        // Debug.Log("wand trigger exited");
    }
}

using UnityEngine;
using System.Collections;

public class PlanetGravity : MonoBehaviour {

    GameObject moon;
    GameObject planet;
    public float gravConst;
    public float planetMass;
    Rigidbody rb;

	// Use this for initialization
	void Start () {
        planet = gameObject;
	}
	
	void FixedUpdate () {
        if (moon != null)
        {
            var heading = planet.transform.position - moon.transform.position;
            var distSquared = heading.sqrMagnitude;
            var dist = heading.magnitude;
            Debug.Log("distance is: ");
            Debug.Log(dist);
            var direction = heading / distSquared;
            var forceNumerator = rb.mass * gravConst *planetMass;
            var force = forceNumerator / distSquared;

            rb.useGravity = false;
            rb.AddForce(direction * force);
        }
	}

    private void OnTriggerEnter(Collider collider) {
        moon = collider.gameObject;
        rb = moon.GetComponent<Rigidbody>();
        // Debug.Log("planet trigger entered");
    }

    private void OnTriggerExit(Collider collider) {
        rb.useGravity = true;
        moon = null;
        rb = null;
    }
}

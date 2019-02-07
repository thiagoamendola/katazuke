using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerNumber{
	Player1, Player2
};

public class Player : MonoBehaviour {

    public PlayerNumber playerNumber;
	public bool controlEnabled;
	[HideInInspector]
	public bool softControlEnabled;
	public float speed = 5;
    public int totalClothesQuantity = 10;
    
	[HideInInspector]
	public int clothesQuantity;
	[HideInInspector]
	public Animator animator;

	new Rigidbody rigidbody;
	Collider actionCollider;

	[HideInInspector]
	public GameObject holdingObject;

    [HideInInspector]
	public GameObject holdingPoint;

	InterationSpot currentInteractionSpot;

	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        actionCollider = transform.Find("ActionCollider").gameObject.GetComponent<Collider>();
        holdingObject = null;
        holdingPoint = transform.Find("Character").Find("Rootbone").Find("Bottom").Find("Belly").Find("HoldingPoint").gameObject;
        controlEnabled = true;
        softControlEnabled = true;
        clothesQuantity = totalClothesQuantity;
	}

	void Update() {
		if(controlEnabled && softControlEnabled && Input.GetButtonDown("Action"+GetControllerNumber()) && currentInteractionSpot != null){
            currentInteractionSpot.TriggerInteraction(this);
		}
	}

	void FixedUpdate () {
		if(controlEnabled && softControlEnabled){
			// Get input.
			float rawInputX = Input.GetAxisRaw("Horizontal"+GetControllerNumber());
			float rawInputZ = Input.GetAxisRaw("Vertical"+GetControllerNumber());
			Vector3 rawInput = new Vector3(rawInputX, 0, rawInputZ);
			if (rawInput.magnitude > 1)
				rawInput.Normalize();
			float smoothInputX = Input.GetAxis("Horizontal"+GetControllerNumber());
			float smoothInputZ = Input.GetAxis("Vertical"+GetControllerNumber());
			Vector3 smoothInput = new Vector3(smoothInputX, 0, smoothInputZ);
			if(smoothInput.magnitude > 1)
				smoothInput.Normalize();
			// Get camera basis.
			Transform cameraTransform = Camera.main.transform;
			Vector3 cameraForward = Vector3.ProjectOnPlane(cameraTransform.forward, Vector3.up).normalized;
			Vector3 cameraRight = Vector3.ProjectOnPlane(cameraTransform.right, Vector3.up).normalized;
            // Move player.
            Vector3 velocity = (cameraForward * smoothInput.z) + (cameraRight * smoothInput.x);
			velocity *= speed;
			velocity.y = rigidbody.velocity.y;
			rigidbody.velocity = velocity;
			// Rotate player.
			if(rawInput.magnitude > 0){
				float roty = Vector3.Angle(Vector3.forward,velocity) * Mathf.Sign(velocity.x);
				rigidbody.rotation = Quaternion.Euler(0,roty,0);
			}
			// Animate player.
			if(Mathf.Abs(rawInputX) > 0 || Mathf.Abs(rawInputZ) > 0){
				animator.SetBool("walking", true);
			}else{
				animator.SetBool("walking", false);
			}
		}
    }

	char GetControllerNumber(){
		string numberString = playerNumber.ToString();
		return numberString[numberString.Length-1];
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "InteractionSpot"){
            currentInteractionSpot = other.gameObject.GetComponent<InterationSpot>();
		}
	}

	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "InteractionSpot"){
            if(currentInteractionSpot == other.gameObject.GetComponent<InterationSpot>())
                currentInteractionSpot = null;
		}
    }
}

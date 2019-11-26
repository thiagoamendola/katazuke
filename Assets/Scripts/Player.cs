using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public ControlInput? controlInput;

    [HideInInspector]
	public ThoughtBalloon mistakeThought;

	InterationSpot currentInteractionSpot;
	List<InterationSpot> previousNextSpots;

	void Start () {
        rigidbody = GetComponent<Rigidbody>();
        animator = transform.Find("Character").GetComponent<Animator>();
        actionCollider = transform.Find("ActionCollider").gameObject.GetComponent<Collider>();
        holdingObject = null;
        holdingPoint = transform.Find("Character").Find("Rootbone").Find("Bottom").Find("Belly").Find("HoldingPoint").gameObject;
        controlEnabled = true;
        softControlEnabled = true;
        clothesQuantity = totalClothesQuantity;
        controlInput = (ControlInput?) InputManager.GetControlInput(playerNumber);
		mistakeThought = transform.Find("ThoughtsHolder").Find("Mistake").gameObject.GetComponent<ThoughtBalloon>();
		print("WOW");
		print(transform.name);
		print(GameObject.Find("Room").name);
		print(GameObject.Find("Room").transform.Find("InteractionSpots").gameObject.name);
		previousNextSpots = new List<ClothesPile>(GameObject.Find("Room").transform.Find("InteractionSpots").gameObject.GetComponentsInChildren<ClothesPile>()).Cast<InterationSpot>().ToList();
	}

	void Update() {
        string actionCommand = "Action" + controlInput.ToString();
        if(controlEnabled && softControlEnabled && Input.GetButtonDown(actionCommand) && currentInteractionSpot != null){
            bool success = currentInteractionSpot.TriggerInteraction(this);
			if (!success){
				mistakeThought.Show();
			}else{
				if (previousNextSpots != null)
					foreach(InterationSpot spot in previousNextSpots){
						spot.hint.Hide();
					}
				List<InterationSpot> nextSpots = currentInteractionSpot.GetHintableNextSpots();
				foreach(InterationSpot spot in nextSpots){
					print(spot.name);
					print(spot.hint.name);
					spot.hint.Show();
				}
				previousNextSpots = nextSpots;
			}
		}
	}

	void FixedUpdate () {
		if(controlEnabled && softControlEnabled){
			rigidbody.isKinematic = false;
            string horizontalCommand = "Horizontal" + controlInput.ToString();
            string verticalCommand = "Vertical" + controlInput.ToString();
            // Get input.
            float rawInputX = Input.GetAxisRaw(horizontalCommand);
			float rawInputZ = Input.GetAxisRaw(verticalCommand);
			Vector3 rawInput = new Vector3(rawInputX, 0, rawInputZ);
			if (rawInput.magnitude > 1)
				rawInput.Normalize();
			float smoothInputX = Input.GetAxis(horizontalCommand);
			float smoothInputZ = Input.GetAxis(verticalCommand);
			Vector3 smoothInput = new Vector3(smoothInputX, 0, smoothInputZ);
			if(smoothInput.magnitude > 1)
				smoothInput.Normalize();
			// Get camera basis.
			Transform cameraTransform = ScreenManager.activeCamera.transform;
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
		} else {
			rigidbody.isKinematic = true;
			rigidbody.velocity = Vector3.zero;
		}
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

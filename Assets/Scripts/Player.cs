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

	InteractionSpot currentInteractionSpot;
	List<InteractionSpot> previousNextSpots;

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
		previousNextSpots = new List<ClothesPile>(GameObject.Find("Room").transform.Find("InteractionSpots").gameObject.GetComponentsInChildren<ClothesPile>().Where(r => r.playerNumber == playerNumber)).Cast<InteractionSpot>().ToList();
		MusicController.Instance.UpdatePlayerMusic(this);
	}

	void Update() {
        string actionCommand = "Action" + controlInput.ToString();
        if(controlEnabled && softControlEnabled && Input.GetButtonDown(actionCommand) && currentInteractionSpot != null){
            bool success = currentInteractionSpot.TriggerInteraction(this);
			if (!success){
				mistakeThought.Show();
			}else{
				if (previousNextSpots != null)
					foreach(InteractionSpot spot in previousNextSpots){
						spot.hint.Hide();
					}
				List<InteractionSpot> nextSpots = currentInteractionSpot.GetHintableNextSpots();
				foreach(InteractionSpot spot in nextSpots){
					spot.hint.Show();
				}
				previousNextSpots = nextSpots;
				// If last cloth, turn on arrow.
				if(clothesQuantity <= 0 && currentInteractionSpot is ClothDisposer && holdingObject != null){
					List <ClothDisposer> allSpotsList = new List<ClothDisposer>(GameObject.FindObjectsOfType<ClothDisposer>());
                    allSpotsList = allSpotsList.Where(s => s.playerNumber == playerNumber && !s.requiresJoy).Cast<ClothDisposer>().ToList();
					allSpotsList[0].hint.Show();
				}
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

	public void DestroyHoldingObject(){
		// Release the main variable from its reference
		GameObject destroyingObject = holdingObject;
		holdingObject = null;

		destroyingObject.transform.parent = null;
		destroyingObject.transform.Find("JoyfulParticle")?.GetComponent<ParticleSystem>().Stop();
		destroyingObject.transform.Find("JoylessParticle")?.gameObject.SetActive(false);

		foreach(MeshRenderer mr in destroyingObject.GetComponentsInChildren<MeshRenderer>()){
			mr.enabled = false;
		}
		foreach(SkinnedMeshRenderer smr in destroyingObject.GetComponentsInChildren<SkinnedMeshRenderer>()){
			smr.enabled = false;
		}
		Destroy(destroyingObject, 1.5f);
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "InteractionSpot"){
            currentInteractionSpot = other.gameObject.GetComponent<InteractionSpot>();
			currentInteractionSpot.hint.SetSelectable(true);
		}
	}

	void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "InteractionSpot"){
            if(currentInteractionSpot == other.gameObject.GetComponent<InteractionSpot>()){
				currentInteractionSpot.hint.SetSelectable(false);
                currentInteractionSpot = null;
			}
		}
    }
}

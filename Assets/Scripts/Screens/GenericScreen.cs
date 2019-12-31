using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericScreen : MonoBehaviour{

    public const float TRANSITIONDURATION = 1.75f;

    [Header("Generic Screen")]
    public Camera camera;

    protected virtual void Start(){
        ScreenManager.Create();
        if(!ScreenManager.openedScreen){
            ScreenManager.openedScreen = true;
            Open();
        }
    }

    public abstract void Open();

    public abstract void Close();

    // Uses the global camera to tween to current scene's camera position.
    public virtual Coroutine FocusCamera(){
        Camera cameraToUse = ScreenManager.activeCamera;
        Transform targetTransform = camera.transform;
        if (camera != cameraToUse){
            camera.gameObject.SetActive(false);
        }
        return StartCoroutine(AsyncFocusCamera(cameraToUse, targetTransform, TRANSITIONDURATION));
    }

    public IEnumerator AsyncFocusCamera(Camera cameraToUse, Transform targetTransform, float duration){
        float progress = 0;
        Vector3 initialPosition = cameraToUse.transform.position;
        Quaternion initialRotation = cameraToUse.transform.rotation;
        while (progress < 1){
            cameraToUse.transform.position = Vector3.Lerp(initialPosition,
                targetTransform.position, ApplyEasing(progress));
            cameraToUse.transform.rotation = Quaternion.Lerp(initialRotation,
                targetTransform.rotation, ApplyEasing(progress));
            progress = Mathf.Min(progress + Time.deltaTime/duration, 1.0f);
            yield return null;
        }
    }

    public static float ApplyEasing (float value) {
        return value*value*value * (value * (6f*value - 15f) + 10f); // (Quintic) Smootherstep;
    }	

}

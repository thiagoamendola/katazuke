using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericScreen : MonoBehaviour{

    public const float transitionDuration = 2.0f;

    [Header("Generic Screen")]
    public Camera camera;


    public abstract void Open();

    public abstract void Close();

    // Uses the global camera to tween to current scene's camera position.
    public virtual Coroutine FocusCamera(){
        Camera cameraToUse = ScreenManager.activeCamera;
        Transform targetTransform = camera.transform;
        if (camera != cameraToUse){
            camera.gameObject.SetActive(false);
        }
        return StartCoroutine(AsyncFocusCamera(cameraToUse, targetTransform));
    }

    IEnumerator AsyncFocusCamera(Camera cameraToUse, Transform targetTransform){
        float progress = 0;
        Vector3 initialPosition = cameraToUse.transform.position;
        Quaternion initialRotation = cameraToUse.transform.rotation;
        while (progress < 1){
            cameraToUse.transform.position = Vector3.Lerp(initialPosition, targetTransform.position, progress);
            cameraToUse.transform.rotation = Quaternion.Lerp(initialRotation, targetTransform.rotation, progress);
            progress = Mathf.Min(progress + Time.deltaTime/transitionDuration, 1.0f);
            yield return null;
        }
    }

}

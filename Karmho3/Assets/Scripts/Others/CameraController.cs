using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Camera Follow Settings")]
    [SerializeField] private Transform target; // The target the camera will follow
    [SerializeField] float minX, maxX;
    [SerializeField] float minY, maxY;

    [Header("Camera Shake Settings")]
    [SerializeField] private float shakeDuration = 0.5f; // Duration of the camera shake
    public AnimationCurve shakeCurve;
    public bool startShake = false;
   
    // Update is called once per frame
    void Update()
    {
        CameraFollow();
        if (startShake)
        {
            StartCoroutine(ShakeCamera());
            startShake = false; // Reset the shake trigger
        }
    }

    #region CameraFollow

    void CameraFollow()
    {
        transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), Mathf.Clamp(transform.position.y, minY, maxY), transform.position.z);
    }

    #endregion

    #region ShakeCamera

    IEnumerator ShakeCamera()
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {           
            elapsed += Time.deltaTime;
            float strenght = shakeCurve.Evaluate(elapsed / shakeDuration);
            transform.position = originalPosition + Random.insideUnitSphere * strenght; // Adjust the shake intensity as needed
            yield return null;
        }
        transform.position = originalPosition;
    }



    #endregion
}

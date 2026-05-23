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
            transform.position = originalPosition + Random.insideUnitSphere * 0.5f; // Adjust the shake intensity as needed
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
    }



    #endregion
}

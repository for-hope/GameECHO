using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    public new Camera camera;
    public float sphereRadius;
    public float maxDistance;
    
    private Vector3 origin;
    private Vector3 direction;
    private float currentHitDistance;

    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        origin = ray.origin;
        direction = ray.direction;
        int layerMask = LayerMask.GetMask("VoiceInteractable");
        GameObject objectText = GameObject.Find("objectText");
        if (Physics.SphereCast(origin, sphereRadius, direction ,out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            GameObject gameObject = hit.collider.gameObject;
            string tag = gameObject.tag;
            GameManager.currentExactEnvObject = (EnvObjects)System.Enum.Parse(typeof(EnvObjects), tag);
                    
         

            //get Text from gameobject
         
            objectText.GetComponent<UnityEngine.UI.Text>().text = gameObject.name;
            GameManager.updateCommandsList(gameObject.tag);

            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Hit " + hit.collider.name + " - " + hit.distance + " - " + hit.collider.gameObject.tag);
        } else
        {
            objectText.GetComponent<UnityEngine.UI.Text>().text = "";
            currentHitDistance = maxDistance;
            GameManager.updateCommandsList("");
            
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);

        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Debug.DrawLine(origin, origin + direction * currentHitDistance);
        Gizmos.DrawWireSphere(origin + direction * currentHitDistance, sphereRadius);
    }
}

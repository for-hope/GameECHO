using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraRay : MonoBehaviour
{
    public new Camera camera;
    public float sphereRadius;
    public float maxDistance;

    private Vector3 origin;
    private Vector3 direction;
    float currentHitDistance;


    void FixedUpdate()
    {
        RaycastHit hit;
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        origin = ray.origin;
        direction = ray.direction;
        int layerMask = LayerMask.GetMask("VoiceInteractable");
        GameObject objectText = GameObject.Find("objectText");

        if (Physics.SphereCast(origin, sphereRadius, direction, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            currentHitDistance = hit.distance;
            GameObject gameObject = hit.collider.gameObject;
            string tag = gameObject.tag;
        
            GameManager.currentExactEnvObject = (EnvObjects)System.Enum.Parse(typeof(EnvObjects), tag);



            //get Text from gameobject
            string name = tag.Replace("_", " ");
            //capitalize first letter of each word
            string formattedName = name.ToLower().Split(' ').Select(word => word.First().ToString().ToUpper() + word.Substring(1)).Aggregate((a, b) => a + " " + b);
            objectText.GetComponent<UnityEngine.UI.Text>().text = formattedName;
            GameManager.updateCommandsList(gameObject.tag);
            //GameObject rayObjText = GameObject.Find("rayObj");
            //rayObjText.GetComponent<UnityEngine.UI.Text>().text = "RAY OBJECT:\n " + gameObject.tag;

        }
        else
        {
            objectText.GetComponent<UnityEngine.UI.Text>().text = "";
            currentHitDistance = maxDistance;
            //GameManager.updateCommandsList("");
            //GameObject rayObjText = GameObject.Find("rayObj");
            //rayObjText.GetComponent<UnityEngine.UI.Text>().text = "RAY OBJECT:\n ";
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDetector : MonoBehaviour
{
    GameObject[] gos;
    List<Renderer> m_Renderers = new List<Renderer>();
    // Sta-rt is called before the first frame update

    void Start()
    {
        gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<Renderer>() != null && go.layer==6)
            {
                m_Renderers.Add(go.GetComponent<Renderer>());
            } 
            
        }
    }

    // Update is called once per frame
    void Update()
    {
       
        foreach (Renderer renderer in m_Renderers)
        {
            string tag = renderer.gameObject.tag;
            if (renderer.gameObject.layer != 6 || tag == "Untagged" ) continue;
            EnvObjects envObject = (EnvObjects)System.Enum.Parse(typeof(EnvObjects), tag); 
            if (renderer.isVisible && renderer.gameObject.layer == 6)
            {
                
                GameManager.currentFrameEnvObjects.Add(envObject);
                //Debug.Log(renderer.gameObject.name + "is Visible");
            }

            if (!renderer.isVisible && renderer.gameObject.layer == 6 &&  GameManager.currentFrameEnvObjects.Contains(envObject))
            {
                GameManager.currentFrameEnvObjects.Remove(envObject);
                //Debug.Log(renderer.gameObject.name + "is not Visible");
            }
        }
    }
}


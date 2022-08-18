using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDetector : MonoBehaviour
{

    public Camera cam;
    
    GameObject[] gos;
    List<Renderer> m_Renderers = new List<Renderer>();
    Dictionary<int, string> rendererTags = new Dictionary<int, string>();
    // Sta-rt is called before the first frame update


    void Start()
    {
        gos = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject go in gos)
        {
            if (go.GetComponent<Renderer>() != null && go.layer==6 && go.tag != "Untagged")
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
            EnvObjects envObject = (EnvObjects)System.Enum.Parse(typeof(EnvObjects), tag);
            List<EnvObjects> envObjs = GameManager.currentFrameEnvObjects;
            //UnityEngine.UI.Text frameText = GameObject.Find("frameObjs").GetComponent<UnityEngine.UI.Text>();
            if (IsVisible(renderer))
            {
                if (envObjs.Contains(envObject))
                {
                  // renderer.material.color = renderer.material.color == Color.yellow ? Color.yellow : Color.green;
                    //if (!frameText.text.Contains(envObject.ToString())) frameText.text = frameText.text + "\n" + envObject.ToString();
                    rendererTags[renderer.GetInstanceID()] = envObject.ToString();
                    continue;   
                };
                //frameText.text = frameText.text + "\n" + envObject.ToString();
                GameManager.currentFrameEnvObjects.Add(envObject);
                rendererTags[renderer.GetInstanceID()] = envObject.ToString();

            }
         
            if (!IsVisible(renderer) && GameManager.currentFrameEnvObjects.Contains(envObject))
            {


                Dictionary<int, string> rendererTagsCopy = new Dictionary<int, string>(rendererTags);
                bool isBroken = false;
                
                foreach (var visibleRenderer in rendererTagsCopy)
                {
                    string visibleRendererTag = visibleRenderer.Value;
                    if (renderer.GetInstanceID() == visibleRenderer.Key) {
                        rendererTags.Remove(visibleRenderer.Key);
                        continue;
                    }
                    if (tag == visibleRendererTag)
                    {
                        isBroken = true;
                        break;
                    }
                }
           
                //renderer.material.color = Color.red;
                if (isBroken) continue;
                GameManager.currentFrameEnvObjects.Remove(envObject);
                string s = string.Join("\n", GameManager.currentFrameEnvObjects.ToArray());
                //frameText.text = "Frame Objects:\n" + s;
                //Debug.Log(renderer.gameObject.name + "is not Visible");
            }
        }
    }
    
    private bool IsVisible(Renderer renderer)
    {
        GameObject target = renderer.gameObject;
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);
        var point = renderer.bounds.center;
/*        var leftPoint = renderer.bounds.center + new Vector3(-renderer.bounds.extents.x, 0, 0);
        var rightPoint = renderer.bounds.center + new Vector3(renderer.bounds.extents.x, 0, 0);
        var topPoint = renderer.bounds.center + new Vector3(0, renderer.bounds.extents.y, 0);
        var bottomPoint = renderer.bounds.center + new Vector3(0, -renderer.bounds.extents.y, 0);   */
            
        foreach (var plane in planes)
        {
            if (plane.GetDistanceToPoint(point) < 0 )
            {
                return false;
            }
        }
        return true;
        
    }
    
}


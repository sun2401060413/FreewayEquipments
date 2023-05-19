using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityModify : MonoBehaviour
{
    public float intensity_value;
    public List<GameObject> lampsGroupList;
    public Texture2D t_lighton, t_lightoff;
    public Material m_lighton, m_lightoff;

    public int mode = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnModifyTheLampsValue()
    {
        if (mode == 0)
        {
            foreach (var elem in lampsGroupList)
            {
                foreach (Transform child in elem.transform)
                {
                    LampControl script = child.GetComponent<LampControl>();
                    script.t_lighton = t_lighton;
                    script.t_lightoff = t_lightoff;
                    script.m_lighton = m_lighton;
                    script.m_lightoff = m_lightoff;

                    Light spt = child.GetChild(1).GetComponent<Light>();
                    spt.intensity = intensity_value;
                    //spt.
                    //spt.lightmapBakeType = LightmapBakeType.Mixed;
                    spt.renderMode = LightRenderMode.Auto;
                    spt.color = new Color(234f / 256f, 207f / 256f, 178f / 256f);
                }
            }
        }
        else if (mode == 1)
        {
            foreach (var elem in lampsGroupList)
            {

                LampControl script = elem.GetComponent<LampControl>();
                script.t_lighton = t_lighton;
                script.t_lightoff = t_lightoff;
                script.m_lighton = m_lighton;
                script.m_lightoff = m_lightoff;

                //Light spt = elem.transform.GetChild(1).GetComponent<Light>();
                //spt.intensity = intensity_value;
                //spt.renderMode = LightRenderMode.ForcePixel;
                //spt.color = new Color(234f / 256f, 207f / 256f, 178f / 256f);

            }
        }


    }


}

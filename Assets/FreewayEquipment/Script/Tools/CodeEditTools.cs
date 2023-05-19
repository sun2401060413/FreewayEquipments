using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sz.tools
{

    public class CodeEditTools
    {

        /// <summary>
        /// GetChildTransformByName:通过子物体的名称查找到物体;
        /// </summary>
        /// <param name="check">待查找子物体所在父物体</param>
        /// <param name="name">待查找子物体名称</param>
        /// <returns></returns>
        static public Transform GetChildTransformByName(Transform check, string name)
        {
            Transform forreturn = null;
            foreach (Transform t in check.GetComponentsInChildren<Transform>())
            {
                if (t.name == name)
                {
                    forreturn = t;
                    return t;
                }        
            }
            return forreturn;
        }

        /// <summary>
        /// GetAllChildTransforms:查找所有子物体;
        /// </summary>
        /// <param name="check">待查找子物体所在父物体</param>
        /// <returns></returns>
        static public List<Transform> GetAllChildTransforms(Transform check)
        {
            // Get all children in the next level
            List<Transform> forreturn = new List<Transform>();
            foreach (Transform t in check.GetComponentsInChildren<Transform>())
            {
                if (t.parent == check)
                {
                    //Debug.Log(t.name+";"+t.parent.name);
                    forreturn.Add(t);
                }
            }
            return forreturn;
        }
    }
}


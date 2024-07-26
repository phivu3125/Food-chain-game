using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipDirectionController : MonoBehaviour
{
    // Start is called before the first frame update
    static private int flag = -1; // 0: left, 1: right
    public Animator anim = null;
    // Update is called once per frame
    void Update()
    {
        if (anim != null)
        {
            if (flag == 0)
            {
                anim.SetTrigger("FlipLeft");
            }
            else if (flag == 1)
            {
                anim.SetTrigger("FlipRight");
            }
            setFlag(-1);
        }
    }

    public void setFlag(int flag)
    {
        FlipDirectionController.flag = flag;
    }
}

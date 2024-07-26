using UnityEngine;


public class InactiveObjAfterAnimated : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentObj = null;
    public Animator anim = null;

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
        {
            currentObj.SetActive(false);
        }
    }
}

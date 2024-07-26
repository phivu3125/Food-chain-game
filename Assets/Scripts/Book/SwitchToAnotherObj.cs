using UnityEngine;


public class SwitchToAnotherObj : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject currentObj = null;
    public GameObject nextObj = null;
    public Animator anim = null;

    void Update()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1 && !anim.IsInTransition(0))
        {
            currentObj.SetActive(false);
            nextObj.SetActive(true);
        }
    }
}

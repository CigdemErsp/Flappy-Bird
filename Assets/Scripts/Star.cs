using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Start", true);
    }
}

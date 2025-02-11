using UnityEngine;

public class Background : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    private float animationSpeed = 1f;

    public float AnimationSpeed {  get { return animationSpeed; } set { animationSpeed = value; } }

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        meshRenderer.material.mainTextureOffset += new Vector2(animationSpeed * Time.deltaTime, 0);
    }
}

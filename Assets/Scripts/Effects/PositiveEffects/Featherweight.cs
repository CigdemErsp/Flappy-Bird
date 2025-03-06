using UnityEngine;

[CreateAssetMenu(fileName = "Featherweight", menuName = "Roguelike/Effects/Featherweight")]
public class Featherweight : RoguelikeEffect
{
    private void Awake()
    {
        EffectName.text = "Featherweight";
        Description.text = "Your body feels lighter than air!\n" +
            "Your gravity is reduced, making it easier to stay in the air.";
    }

    public void OnClick()
    {
        ApplyEffect();
    }

}
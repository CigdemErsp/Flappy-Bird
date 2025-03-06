using UnityEngine;

[CreateAssetMenu(fileName = "Superpower", menuName = "Roguelike/Effects/Superpower")]
public class Superpower : RoguelikeEffect
{
    private void Awake()
    {
        EffectName.text = "Superpower";
        Description.text = "A mystical force grants you another chance.\n" +
            "You are invulnerable for 10 seconds.";
    }

    public void OnClick()
    {
        ApplyEffect();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "ShrinkingSerum", menuName = "Roguelike/Effects/ShrinkingSerum")]
public class ShrinkingSerum : RoguelikeEffect
{
    private void Awake()
    {
        EffectName.text = "Shrinking Serum";
        Description.text = "You feel smaller and more agile.\n" +
            "Your character's hitbox shrinks by 15%";
    }

    public void OnClick()
    {
        ApplyEffect();
    }
}

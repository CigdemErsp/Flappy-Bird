using UnityEngine;

[CreateAssetMenu(fileName = "ShrinkingSerum", menuName = "Roguelike/Effects/ShrinkingSerum")]
public class ShrinkingSerum : RoguelikeEffect
{
    //private void Awake()
    //{
    //    EffectName = "Shrinking Serum";
    //    Description = "You feel smaller and more agile.\n" +
    //        "Your character's hitbox shrinks by 15%";
    //}

    public override void OnClick()
    {
        Debug.Log($"GoldMaster Clicked! Name: {EffectName}, Description: {Description}");
        base.ApplyEffect();
    }
}

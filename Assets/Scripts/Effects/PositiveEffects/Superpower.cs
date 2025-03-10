using UnityEngine;

[CreateAssetMenu(fileName = "Superpower", menuName = "Roguelike/Effects/Superpower")]
public class Superpower : RoguelikeEffect
{
    //private void Awake()
    //{
    //    EffectName = "Superpower";
    //    Description = "A mystical force grants you another chance.\n" +
    //        "You are invulnerable for 10 seconds.";
    //}

    public override void OnClick()
    {
        Debug.Log($"GoldMaster Clicked! Name: {EffectName}, Description: {Description}");
        base.ApplyEffect();
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "TimeWarp", menuName = "Roguelike/Effects/TimeWarp")]
public class TimeWarp : RoguelikeEffect
{
    //private void Awake()
    //{
    //    EffectName = "Time Warp";
    //    Description = "Everything feels slightly slower.\n" +
    //        "The game's speed is reduced by 10% for 5 seconds.";
    //}

    public override void OnClick()
    {
        Debug.Log($"GoldMaster Clicked! Name: {EffectName}, Description: {Description}");
        base.ApplyEffect();
    }
}

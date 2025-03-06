using UnityEngine;

[CreateAssetMenu(fileName = "TimeWarp", menuName = "Roguelike/Effects/TimeWarp")]
public class TimeWarp : RoguelikeEffect
{
    private void Awake()
    {
        EffectName.text = "Time Warp";
        Description.text = "Everything feels slightly slower.\n" +
            "The game's speed is reduced by 10% for 5 seconds.";
    }

    public void OnClick()
    {
        ApplyEffect();
    }
}

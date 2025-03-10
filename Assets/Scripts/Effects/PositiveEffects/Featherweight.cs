using UnityEngine;

[CreateAssetMenu(fileName = "Featherweight", menuName = "Roguelike/Effects/Featherweight")]
public class Featherweight : RoguelikeEffect
{
    //private void Awake()
    //{
    //    EffectName = "Featherweight";
    //    Description = "Your body feels lighter than air!\n" +
    //        "Your gravity is reduced, making it easier to stay in the air.";
    //}

    public override void OnClick()
    {
        Debug.Log($"GoldMaster Clicked! Name: {EffectName}, Description: {Description}");
        base.ApplyEffect();
    }
}
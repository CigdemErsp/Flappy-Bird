using UnityEngine;

[CreateAssetMenu(fileName = "GoldMaster", menuName = "Roguelike/Effects/GoldMaster")]
public class GoldMaster : RoguelikeEffect
{
//    private void Awake()
//    {
//        EffectName = "Gold Master";
//        Description = "You will collect double the coins!\n" +
//            "Doubles the coins you collect.";
//    }

    public override void OnClick()
    {
        Debug.Log($"GoldMaster Clicked! Name: {EffectName}, Description: {Description}");
        base.ApplyEffect();
    }
}

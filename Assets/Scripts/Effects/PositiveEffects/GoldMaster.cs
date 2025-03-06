using UnityEngine;

[CreateAssetMenu(fileName = "GoldMaster", menuName = "Roguelike/Effects/GoldMaster")]
public class GoldMaster : RoguelikeEffect
{
    private void Awake()
    {
        EffectName.text = "Gold Master";
        Description.text = "You will collect double the coins!\n" +
            "Doubles the coins you collect.";
    }

    public void OnClick()
    {
        ApplyEffect();
    }
}

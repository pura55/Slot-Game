using UnityEngine;

//リール配列をデータ化するもの
//[CreateAssetMenu(menuName = "Slot/Reel layout", fileName = "ReelLayoutData")]
public class ReelLayoutData : MonoBehaviour
{
    public Symbol[] left = new Symbol[21];
    public Symbol[] center = new Symbol[21];
    public Symbol[] right = new Symbol[21];

    public Symbol[] Get(int reelIndex)
    {
        if (reelIndex == 0) return left;
        if (reelIndex == 1) return center;
        return right;
    }
}
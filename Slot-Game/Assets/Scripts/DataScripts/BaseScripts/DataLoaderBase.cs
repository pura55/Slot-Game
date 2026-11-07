using UnityEngine;

//‘S‚Ä‚ÌDataLoader‚ÉŒp³‚·‚éˆ×‚ÌƒNƒ‰ƒX
public abstract class DataLoaderBase : MonoBehaviour
{
    [SerializeField] protected TextAsset textAsset;
    protected abstract void LoadData();
}

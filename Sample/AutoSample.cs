using Reddot;
using UnityEngine;

public class AutoSample : MonoBehaviour
{
    void Start()
    {
        ReddotManager.Ins.SetCheck(ReddotType.Test1, HasReddot);
        ReddotManager.Ins.SetCheck(ReddotType.Test2, HasReddot);
    }

    public bool HasReddot()
    {
        return true;
    }

    public void RefreshReddot()
    {
        ReddotManager.Ins.MarkDirty(ReddotType.Test1);
        ReddotManager.Ins.MarkDirty(ReddotType.Test2);
    }

}

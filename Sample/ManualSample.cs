using Reddot;
using UnityEngine;

public class ManualSample : MonoBehaviour
{
    ManualReddot reddot;
    void Start()
    {
        reddot = gameObject.AddComponent<ManualReddot>();
        ReddotManager.Ins.SetCheck(reddot, HasReddot);
        reddot.RegisterEvent(ReddotEvent.Test1);
        reddot.RegisterEvent(ReddotEvent.Test2);
    }
    public bool HasReddot()
    {
        return true;
    }

    public void RefreshReddot()
    {
        ReddotManager.Ins.MarkDirty(ReddotEvent.Test1);
        ReddotManager.Ins.MarkDirty(ReddotEvent.Test2);
    }
}

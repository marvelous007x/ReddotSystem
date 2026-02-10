using UnityEngine;

namespace Reddot
{
    [DisallowMultipleComponent]
    public abstract class Reddot : MonoBehaviour
    {
        protected bool dirty;
        private bool active;
        protected virtual void Start()
        {
            MarkDirty();
        }

        protected virtual void OnEnable()
        {
            if (dirty)
                ReddotManager.Ins.AddDirtyComps(this);
        }

        protected void ShowReddot(bool active)
        {
            var pActive = this.active;
            if (pActive == active) return;
            this.active = active;
            //执行红点显示逻辑
        }

        internal abstract void CheckReddot();

        protected void MarkDirty()
        {
            if (dirty) return;
            dirty = true;
            if (isActiveAndEnabled)
                ReddotManager.Ins.AddDirtyComps(this);
        }

        protected void UnmarkDirty()
        {
            dirty = false;
        }

        protected virtual void OnDestroy()
        {
            if (dirty)
                ReddotManager.Ins.RemoveDirtyComps(this);
        }
    }
}
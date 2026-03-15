using System.Collections.Generic;

namespace Reddot
{
    /// <summary>
    /// 用于非全局上下文情况，例如背包里每个道具的红点
    /// </summary>
    public class ManualReddot : Reddot
    {
        private CheckReddot check;
        private List<ReddotEvent> events;

        internal void SetCheck(CheckReddot check)
        {
            this.check = check;
            MarkDirty();
        }

        internal override void CheckReddot()
        {
            bool active;
            if (check != null)
                active = check();
            else
                active = false;
            ShowReddot(active);
            UnmarkDirty();
        }

        public void RegisterEvent(ReddotEvent ev)
        {
            events ??= new List<ReddotEvent>();
            if (events.Contains(ev)) return;
            events.Add(ev);
            ReddotManager.Ins.RegisterOnChange(ev, MarkDirty);
        }

        public void UnregisterEvent(ReddotEvent ev)
        {
            if (events == null)
                return;
            if (!events.Contains(ev)) return;
            events.Remove(ev);
            ReddotManager.Ins.UnregisterOnChange(ev, MarkDirty);
        }

        protected override void OnDestroy()
        {
            if (events != null )
            {
                var manager = ReddotManager.Ins;
                if(manager != null)
                {
                    foreach (var ev in events)
                        manager.UnregisterOnChange(ev, MarkDirty);
                }
                events = null;               
            }
            check = null;
            base.OnDestroy();
        }
    }
}
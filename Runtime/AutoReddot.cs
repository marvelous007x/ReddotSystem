using UnityEngine;
namespace Reddot
{
    public class AutoReddot : Reddot
    {
        [SerializeField] private ReddotType[] listens;

        protected override void Start()
        {
            base.Start();
            foreach (var reddot in listens)
            {
                ReddotManager.Ins.RegisterOnChange(reddot, MarkDirty);
            }
        }

        public void SetReddotTypes(params ReddotType[] types)
        {
            bool enabled = isActiveAndEnabled;
            foreach (var type in listens)
            {
                ReddotManager.Ins.UnregisterOnChange(type, MarkDirty);
                if (enabled)
                    ReddotManager.Ins.RemoveReference(type);
            }

            foreach (var type in types)
            {
                ReddotManager.Ins.RegisterOnChange(type, MarkDirty);
                if (enabled)
                    ReddotManager.Ins.AddReference(type);
            }
            listens = types;
        }


        protected override void OnEnable()
        {
            base.OnEnable();
            foreach (var reddot in listens)
            {
                ReddotManager.Ins.AddReference(reddot);
            }
        }

        internal override void CheckReddot()
        {
            if (!dirty) return;
            bool active = false;
            foreach (var t in listens)
            {
                if (ReddotManager.Ins.IsActive(t))
                {
                    active = true;
                    break;
                }
            }
            ShowReddot(active);
            UnmarkDirty();
        }

        private void OnDisable()
        {
            foreach (var reddot in listens)
            {
                ReddotManager.Ins.RemoveReference(reddot);
            }
        }

        protected override void OnDestroy()
        {
            foreach (var reddot in listens)
            {
                ReddotManager.Ins.UnregisterOnChange(reddot, MarkDirty);
            }
            base.OnDestroy();
        }
    }
}
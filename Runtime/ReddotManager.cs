using System;
using System.Collections.Generic;
using UnityEngine;

namespace Reddot
{
    public delegate bool CheckReddot();

    public class ReddotManager : MonoBehaviour
    {
        private readonly HashSet<ReddotType> activeAutos = new HashSet<ReddotType>();
        private readonly HashSet<ReddotType> dirtyAutosFront = new HashSet<ReddotType>();
        private readonly HashSet<ReddotType> dirtyAutosBack = new HashSet<ReddotType>();
        private readonly Dictionary<ReddotType, int> autoReferences = new Dictionary<ReddotType, int>();
        private readonly Dictionary<ReddotType, CheckReddot> checkReddots = new Dictionary<ReddotType, CheckReddot>();
        private readonly Dictionary<ReddotType, Action> onReddotChanges = new Dictionary<ReddotType, Action>();

        private readonly Dictionary<ReddotEvent, Action> reddotEventActions = new Dictionary<ReddotEvent, Action>();
        private readonly List<Reddot> reddotComps = new List<Reddot>();

        static ReddotManager _ins;
        public static ReddotManager Ins
        {
            get
            {
                if (_ins == null && Application.isPlaying)
                {
                    var go = new GameObject();
                    DontDestroyOnLoad(go);
                    _ins = go.AddComponent<ReddotManager>();
                }
                return _ins;
            }
        }

        private void LateUpdate()
        {
            foreach (var reddot in dirtyAutosFront)
            {
                bool pActive = activeAutos.Contains(reddot);
                bool cActive;
                if (checkReddots.TryGetValue(reddot, out var func))
                    cActive = func();
                else
                    cActive = false;

                if (pActive != cActive)
                {
                    if (cActive) activeAutos.Add(reddot);
                    else activeAutos.Remove(reddot);
                }
            }
            dirtyAutosFront.Clear();
            foreach (var comp in reddotComps)
            {
                comp.CheckReddot();
            }
            reddotComps.Clear();
        }

        internal void AddReference(ReddotType reddot)
        {
            autoReferences.TryGetValue(reddot, out var value);
            autoReferences[reddot] = ++value;

            if (dirtyAutosBack.Contains(reddot)) dirtyAutosFront.Add(reddot);
        }

        internal void RemoveReference(ReddotType reddot)
        {
            var value = autoReferences[reddot] - 1;
            autoReferences[reddot] = value;
            if (value <= 0 && dirtyAutosFront.Contains(reddot))
            {
                dirtyAutosFront.Remove(reddot);
                dirtyAutosBack.Add(reddot);
            }
        }

        internal void RegisterOnChange(ReddotType reddot, Action action)
        {
            if (onReddotChanges.TryGetValue(reddot, out var call))
                call += action;
            else
                call = action;
            onReddotChanges[reddot] = call;
        }

        internal void UnregisterOnChange(ReddotType reddot, Action action)
        {
            var call = onReddotChanges[reddot];
            call -= action;
            if (call == null) onReddotChanges.Remove(reddot);
            else onReddotChanges[reddot] = call;
        }

        internal void RegisterOnChange(ReddotEvent ev, Action action)
        {
            if (reddotEventActions.TryGetValue(ev, out var call))
                call += action;
            else
                call = action;
            reddotEventActions[ev] = call;
        }

        internal void UnregisterOnChange(ReddotEvent ev, Action action)
        {
            if (this == null) return;
            var call = reddotEventActions[ev];
            call -= action;
            if (call == null) reddotEventActions.Remove(ev);
            else reddotEventActions[ev] = call;
        }

        internal bool IsActive(ReddotType reddot)
        {
            return activeAutos.Contains(reddot);
        }

        public void SetCheck(ReddotType reddot, CheckReddot check)
        {
            if (check == null) checkReddots.Remove(reddot);
            else checkReddots[reddot] = check;
            MarkDirty(reddot);
        }

        public void SetCheck(ManualReddot manual, CheckReddot check)
        {
            manual.SetCheck(check);
        }

        internal void AddDirtyComps(Reddot reddot)
        {
            reddotComps.Add(reddot);
        }

        internal void RemoveDirtyComps(Reddot reddot)
        {
            reddotComps.Remove(reddot);
        }

        public void MarkDirty(ReddotType reddot)
        {
            onReddotChanges.TryGetValue(reddot, out var action);
            action?.Invoke();

            autoReferences.TryGetValue(reddot, out var count);
            if (count > 0) dirtyAutosFront.Add(reddot);
            else dirtyAutosBack.Add(reddot);
        }

        public void MarkDirty(ReddotEvent ev)
        {
            reddotEventActions.TryGetValue(ev, out var action);
            action?.Invoke();
        }
    }
}
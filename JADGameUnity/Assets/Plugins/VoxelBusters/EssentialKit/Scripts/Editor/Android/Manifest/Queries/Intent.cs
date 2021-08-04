using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelBusters.EssentialKit.Editor.Android
{
    public class Intent : Element
    {
        public void Add(Action action)
        {
            base.Add(action);
        }

        public void Add(Category category)
        {
            base.Add(category);
        }

        public void Add(Data data)
        {
            base.Add(data);
        }

        protected override string GetName()
        {
            return "intent";
        }
    }
}

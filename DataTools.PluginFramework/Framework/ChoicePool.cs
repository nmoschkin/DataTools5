using Newtonsoft.Json;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
    /// <summary>
    /// Standard implementation of the choice pool.
    /// </summary>
    public class ChoicePool : KeyedCollection<string, ICustomValueCollection>, IChoicePool
    {
        Guid id;

        public Guid Id
        {
            get => id;
        }

        public ChoicePool(Guid? id = null) : base()
        {
            this.id = id ?? IChoicePool.GenerateChoicePoolId();
        }

        ICustomValueCollection IChoicePool.this[string key]
        {
            get
            {
                return this[key];
            }
            set
            {
                this.SetItem(IndexOf(this[key]), value);
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
            info.AddValue("ChoicePool", JsonConvert.SerializeObject(this));
        }

        public ChoicePool(SerializationInfo info, StreamingContext context)
        {
            JsonConvert.PopulateObject(info.GetString("ChoicePool"), this);
        }

        protected override string GetKeyForItem(ICustomValueCollection item) => item.Key;
    }

}

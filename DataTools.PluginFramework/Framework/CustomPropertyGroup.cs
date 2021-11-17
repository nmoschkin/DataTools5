using DataTools.Observable;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
    /// <summary>
    /// Custom property group default implementation.
    /// </summary>
    public class CustomPropertyGroup : ObservableBase, ICustomPropertyGroup
    {

        protected CustomPropertyCollection _properties;
        protected bool _AllowAddRemoveProperties;
        protected string _Description;
        protected int _GroupPosition;

        protected string _Name;
        protected IPlugIn _PlugIn;

        protected IChoicePool _ChoicePool;
        protected bool isDefaultChoicePool;

        public CustomPropertyGroup(IPlugIn plugin)
        {
            IChoicePool.DefaultChoicePoolChanged += IChoicePool_DefaultChoicePoolChanged;

            _PlugIn = plugin;
            _properties = new CustomPropertyCollection();

            _ChoicePool = IChoicePool.DefaultChoicePool;
            isDefaultChoicePool = true;
        }

        private void IChoicePool_DefaultChoicePoolChanged(object sender, EventArgs e)
        {
            if (isDefaultChoicePool)
            {
                ChoicePool = IChoicePool.DefaultChoicePool;
            }
        }

        public CustomPropertyGroup(IPlugIn plugin, IChoicePool choicePool) : this(plugin)
        {
            _ChoicePool = choicePool;
            isDefaultChoicePool = false;
        }

        public CustomPropertyGroup(IPlugIn plugin, Guid choicePoolId) : this(plugin)
        {
            _ChoicePool = IChoicePool.GetChoicePoolById(choicePoolId);
            isDefaultChoicePool = false;
        }

        public virtual IChoicePool ChoicePool
        {
            get => _ChoicePool;
            protected set
            {
                SetProperty(ref _ChoicePool, value);
            }
        }

        public virtual IPlugIn PlugIn
        {
            get => _PlugIn;
            set
            {
                SetProperty(ref _PlugIn, value);
            }
        }

        public virtual bool AllowAddRemoveProperties
        {
            get => _AllowAddRemoveProperties;
            set
            {
                SetProperty(ref _AllowAddRemoveProperties, value);
            }
        }

        public virtual string Description
        {
            get => _Description;
            set
            {
                SetProperty(ref _Description, value);
            }
        }

        public virtual int GroupPosition
        {
            get => _GroupPosition;
            set
            {
                SetProperty(ref _GroupPosition, value);
            }
        }
        public virtual string Name
        {
            get => _Name;
            set
            {
                SetProperty(ref _Name, value);
            }
        }

        public virtual CustomPropertyCollection Properties
        {
            get => _properties;
            set
            {
                SetProperty(ref _properties, value);
            }
        }
        

    }
}

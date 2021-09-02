using DataTools.Observable;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
    /// <summary>
    /// Custom property default implementation.
    /// </summary>
    public abstract class CustomProperty : ObservableBase, ICustomProperty
    {
        protected bool _Changed;
        protected string _Description;
        protected ICustomPropertyGroup _Group;
        protected string _InvalidValueMessage = "Invalid Value";
        protected string _Message;
        protected string _Name;

        protected bool _haveChoice;

        protected ICustomValueCollection _Choices = new CustomValueCollection();
        protected ICustomValueCollection _CustomChoices = new CustomValueCollection();

        protected IPlugIn _PlugIn;

        protected PropertyTypes _Type = PropertyTypes.String;


        protected object _Value;

        
        /// <summary>
        /// Create a new custom property with the specified name and type, and belonging to the specified group.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="group">The group that the property is joining.</param>
        public CustomProperty(string name, PropertyTypes type, ICustomPropertyGroup group)
        {
            _Name = name;
            _Group = group;
            _Type = type;

            if (group.Properties.Where((a) => a.Name == _Name).FirstOrDefault() == null)
            {
                group.Properties.Add(this);
            }
            else
            {
                throw new ArgumentException("That key already exists in the collection.");
            }

            _PlugIn = group.PlugIn;
        }

        public virtual bool Changed
        {
            get => _Changed;
            set
            {
                SetProperty(ref _Changed, value);
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

        public virtual ICustomPropertyGroup Group
        {
            get => _Group;
            protected set
            {
                SetProperty(ref _Group, value);
            }
        }

        public virtual string InvalidValueMessage
        {
            get => _InvalidValueMessage;
            set
            {
                SetProperty(ref _InvalidValueMessage, value);
            }
        }

        public virtual string Message
        {
            get => _Message;
            set
            {
                SetProperty(ref _Message, value);
            }
        }

        public virtual string Name
        {
            get => _Name;
            protected set
            {
                SetProperty(ref _Name, value);
            }
        }
        public virtual IPlugIn PlugIn
        {
            get => _PlugIn;
            protected set
            {
                SetProperty(ref _PlugIn, value);
            }
        }
        public virtual PropertyTypes Type
        {
            get => _Type;
            protected set
            {
                SetProperty(ref _Type, value);
            }
        }
        
        object ICustomProperty.Value
        {
            get => _Value;
            set
            {
                SetProperty(ref _Value, value);
            }
        }

        public abstract bool ShouldAllowCustomChoices { get; }

        public abstract bool HasChoiceKey { get; }

        public abstract string ChoiceKey { get; protected set; }

        ICustomValueCollection ICustomProperty.CustomChoices
        {
            get => _CustomChoices;
            set
            {
                SetProperty(ref _CustomChoices, value); 
            }
        }

        ICustomValueCollection ICustomProperty.Choices
        {
            get => _Choices; 
        }

        public virtual void ClearCustomChoices()
        {
            _CustomChoices?.Clear();
        }

        public virtual bool Validate(object value)
        {
            return value != null;
        }

    }

    public class CustomProperty<T> : CustomProperty, ICustomProperty<T>
    {
        protected ICustomValueCollection<T> choices = new CustomValueCollection<T>();
        protected ICustomValueCollection<T> custChoices;

        protected bool allowCustom;
        protected string choiceKey;

        protected T val;

        /// <summary>
        /// Creates a new custom property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="group">The group that the property is joining.</param>
        /// <param name="choiceKey">Optional global choices choice key.</param>
        /// <param name="allowCustomChoices">True to allow custom choices.</param>
        /// <remarks>
        /// If the property type is not multiple choice, then the <paramref name="choiceKey" /> and <paramref name="allowCustomChoices"/> parameters are ignored.
        /// </remarks>
        public CustomProperty(string name, PropertyTypes type, ICustomPropertyGroup group, string choiceKey = null, bool allowCustomChoices = false) : base(name, type, group)
        {
            allowCustom = allowCustomChoices;
            this.choiceKey = choiceKey;

            if (allowCustomChoices)
            {
                custChoices = new CustomValueCollection<T>();
            }
        }


        /// <summary>
        /// Creates a new custom property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="group">The group that the property is joining.</param>
        /// <param name="choices">The pre-set list of options for the consumer.</param>
        /// <param name="choiceKey">Optional global choices choice key.</param>
        /// <param name="allowCustomChoices">True to allow custom choices.</param>
        /// <remarks>
        /// If the property type is not multiple choice, then the <paramref name="choices"/>, <paramref name="choiceKey" /> and <paramref name="allowCustomChoices"/> parameters are ignored.
        /// </remarks>
        public CustomProperty(string name, PropertyTypes type, ICustomPropertyGroup group, ICustomValueCollection<T> choices, string choiceKey = null, bool allowCustomChoices = false) : this(name, type, group, choiceKey, allowCustomChoices)
        {
            this.choices = choices;

            //switch (type)
            //{
            //    case PropertyTypes.ListBox:
            //    case PropertyTypes.DropDown:
            //    case PropertyTypes.AutoComplete:
            //    case PropertyTypes.RadioButton:
            //    case PropertyTypes.ColorAutoComplete:
            //        throw new InvalidOperationException("Choices has no ")
            //}
        }

        /// <summary>
        /// Creates a new custom property.
        /// </summary>
        /// <param name="name">The name of the property.</param>
        /// <param name="type">The type of the property.</param>
        /// <param name="group">The group that the property is joining.</param>
        /// <param name="choices">The pre-set list of options for the consumer.</param>
        /// <param name="choices">The pre-filled list of custom options for the consumer.</param>
        /// <param name="choiceKey">Optional global choices choice key.</param>
        public CustomProperty(string name, PropertyTypes type, ICustomPropertyGroup group, ICustomValueCollection<T> choices, ICustomValueCollection<T> customChoices, string choiceKey = null) : this(name, type, group, choices, choiceKey, true)
        {
            this.custChoices = customChoices;
        }

        public virtual T Value
        {
            get => val;
            set
            {
                if (SetProperty(ref val, value))
                {
                    base._Value = val;
                }
            }
        }

        object ICustomProperty.Value
        {
            get => _Value;
            set
            {
                if (SetProperty(ref _Value, value))
                {
                    Value = (T)value;
                }
            }
        }

        public virtual ICustomValueCollection<T> Choices
        {
            get => choices;
            protected set
            {
                if (SetProperty(ref choices, value))
                {
                    _Choices = choices;
                }
            }
        }            

        public virtual ICustomValueCollection<T> CustomChoices
        {
            get => custChoices;
            set
            {
                if (SetProperty(ref custChoices, value))
                {
                    _CustomChoices = custChoices;
                }
            }
        }

        ICustomValueCollection ICustomProperty.CustomChoices
        {
            get => _CustomChoices;
            set
            {
                if (SetProperty(ref _CustomChoices, value))
                {
                    CustomChoices = (ICustomValueCollection<T>)value;
                }
            }
        }

        public override bool ShouldAllowCustomChoices => allowCustom;

        public override bool HasChoiceKey => choiceKey != null;

        public override string ChoiceKey
        {
            get => choiceKey;
            protected set
            {
                SetProperty(ref choiceKey, value);  
            }
        }

        public virtual bool Validate(T value)
        {
            return value is object;
        }
    }



}

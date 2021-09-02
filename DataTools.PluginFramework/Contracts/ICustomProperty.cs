using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("DataTools.Chart")]

namespace DataTools.PluginFramework
{

    public enum PropertyTypes
    {
        String,
        MultiString,
        Color,
        Thickness,
        Brush,
        Decimal,
        Double,
        Integer,
        Guid,
        Time,
        Date,
        DateTime,
        DropDown,
        ListBox,
        Checkbox,
        RadioButton,
        AutoComplete,
        ColorAutoComplete
    }

    /// <summary>
    /// PlugIn properties interface
    /// </summary>
    public interface ICustomProperty : INotifyPropertyChanged, INotifyPropertyChanging
    {
        /// <summary>
        /// The property name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Property description.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Current message.
        /// </summary>
        string Message { get; set; }

        /// <summary>
        /// Invalid value message.
        /// </summary>
        string InvalidValueMessage { get; }

        /// <summary>
        /// True if the property has been changed.
        /// </summary>
        bool Changed { get; set; }

        /// <summary>
        /// Gets the group associated with this property.
        /// </summary>
        ICustomPropertyGroup Group { get; }

        /// <summary>
        /// Gets the overlay associated with this property.
        /// </summary>
        IPlugIn PlugIn { get; }

        /// <summary>
        /// The property data type.
        /// </summary>
        PropertyTypes Type { get; }

        /// <summary>
        /// The property value.
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// Choices for a property that is one of the multiple choice providing types (radio, combo, list, autocomplete).
        /// </summary>
        ICustomValueCollection Choices { get; }

        /// <summary>
        /// The property manager should allow custom choices for this property, if it is capable.
        /// </summary>
        bool ShouldAllowCustomChoices { get; }

        /// <summary>
        /// Custom choices for a property that is one of the multiple choice providing types (radio, combo, list, autocomplete).
        /// </summary>
        ICustomValueCollection CustomChoices { get; set; }

        /// <summary>
        /// Has a key for a common pool of custom values that can be made available to other properties who also use the same key.
        /// </summary>
        bool HasChoiceKey { get; }

        /// <summary>
        /// The key for the common pool of values.
        /// If this value is null, the property's name is used.
        /// </summary>
        string ChoiceKey { get; }

        /// <summary>
        /// Clear custom choices. 
        /// </summary>
        /// <remarks>
        /// Only local custom choices are cleared.  Choice pools must be cleared with a call to IChoicePool.ClearPool()
        /// </remarks>
        void ClearCustomChoices();

        /// <summary>
        /// Validate the property contents.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the property contains a valid value.</returns>
        bool Validate(object value);

    }



    /// <summary>
    /// PlugIn properties interface
    /// </summary>
    public interface ICustomProperty<T> : ICustomProperty
    {

        /// <summary>
        /// The property value.
        /// </summary>
        new T Value { get; set; }

        /// <summary>
        /// Choices for a property that is one of the multiple choice providing types (radio, combo, list, autocomplete).
        /// </summary>
        new ICustomValueCollection<T> Choices { get; }

        /// <summary>
        /// Custom choices for a property that is one of the multiple choice providing types (radio, combo, list, autocomplete).
        /// </summary>
        new ICustomValueCollection<T> CustomChoices { get; set; }

        /// <summary>
        /// Validate the property contents.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <returns>True if the property contains a valid value.</returns>
        bool Validate(T value);

    }

    public enum AutoCompleteMode
    {
        BeginsWith,
        Contains,
        EndsWith,
        DoesNotContain
    }

    public interface IAutoCompleteProperty<T> : ICustomProperty<T>
    {
        AutoCompleteMode Mode { get; set; }

        /// <summary>
        /// Gets auto-complete suggestions for the specified input.
        /// </summary>
        /// <param name="input">The input to analyze.</param>
        /// <returns></returns>
        /// <remarks>
        /// Custom properties will look up by choice keys, and get pool data in addition to local data.
        /// </remarks>
        Task<IList<T>> GetAutoCompleteSuggestions(T input);

    }
}

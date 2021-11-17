using DataTools.Observable;


using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DataTools.PluginFramework.Framework
{
    public abstract class PlugIn : ObservableBase, IPlugIn
    {
        protected IInitializationBundle _Bundle = new InitializationBundle();

        protected string _Description = "";
        protected VersionInfo _VersionInfo = new VersionInfo();

        protected ObservableCollection<ICustomPropertyGroup> propertyBag = new ObservableCollection<ICustomPropertyGroup>();
        protected bool disposedValue;

        public PlugIn()
        {

        }

        public PlugIn(SerializationInfo info, StreamingContext context)
        {
            var json = (string)info.GetValue($"{GetType().FullName}.Info", typeof(string));
            JsonConvert.PopulateObject(json, this);
        }

        public virtual IInitializationBundle GetInitializationBundle()
        {
            return _Bundle;
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue($"{GetType().FullName}.Info", JsonConvert.SerializeObject(this));
        }

        public virtual void Initialize(IInitializationBundle bundle)
        {
            _Bundle = bundle;
        }

        /// <summary>
        /// Arbitrary data associated with the overlay.
        /// </summary>
        public object Tag { get; set; }


        public abstract string Company { get; }

        public abstract bool HasConfigPanel { get; }

        public abstract bool IsSingleton { get; }

        public abstract string Name { get; }

        public abstract bool RequiresLicense { get; }

        public abstract string Copyright { get; }

        public abstract bool HasDependencies { get; }

        public abstract bool CanHaveDependencies { get; }

        public abstract bool DataProviderInitialized { get; protected set; }

        public abstract void OpenConfigPanel(object site);

        public abstract void AddDependency(IPlugIn overlay);

        public abstract void RemoveDependency(IPlugIn overlay);

        public abstract ICollection<IPlugIn> GetDependencies();

        public abstract ICollection<Type> GetValidDependencyTypes();

        public virtual IList<ICustomPropertyGroup> PropertyGroups => propertyBag;

        public virtual IVersionInfo VersionInfo => _VersionInfo;

        public virtual string Description
        {
            get => _Description;
            set
            {
                SetProperty(ref _Description, value);
            }
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ManagedPlugIn()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}

using DataTools.PluginFramework.Framework;

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

namespace DataTools.PluginFramework
{
    /// <summary>
    /// Represents a pool of choices keyed off of their respective value lists that can be used by property bag system properties where the type of property is multiple choice and one or more properties share the same choices.
    /// </summary>
    public interface IChoicePool : ICollection<ICustomValueCollection>, ISerializable
    {
        protected static readonly Dictionary<Guid, IChoicePool> poolRegistry = new Dictionary<Guid, IChoicePool>();

        static IChoicePool defaultChoicePool;

        public static event EventHandler DefaultChoicePoolChanged;
        
        /// <summary>
        /// Returns the choice pool registry for the current application domain.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyDictionary<Guid, IChoicePool> GetChoicePoolRegistry()
        {
            return new ReadOnlyDictionary<Guid, IChoicePool>(poolRegistry);
        }

        static IChoicePool()
        {
            // create the default choice pool, which is a standard implementation of choice pool.
            var cp = new ChoicePool();
            defaultChoicePool = cp;

            poolRegistry.Add(cp.Id, cp);
        }

        /// <summary>
        /// Gets the default choice pool of type <see cref="ChoicePool"/>.
        /// </summary>
        public static IChoicePool DefaultChoicePool => defaultChoicePool;

        /// <summary>
        /// Gets a choice pool by its lookup id.
        /// </summary>
        /// <param name="id">Lookup id.</param>
        /// <returns>An instance of <see cref="IChoicePool"/> or null if none was found.</returns>
        public static IChoicePool GetChoicePoolById(Guid id)
        {
            if (poolRegistry.ContainsKey(id))
            {
                return poolRegistry[id];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Add a choice pool to the global choice pool registry.
        /// </summary>
        /// <param name="obj">The <see cref="IChoicePool"/> implementation to register.</param>
        /// <param name="setAsDefault">True to set the specified choice pool as the default choice pool for the current application domain.</param>
        /// <remarks>
        /// If a choice pool of any type is already registered with the same <see cref="IChoicePool.Id"/> as <paramref name="obj"/>, then it will be replaced.<br />
        /// <br />
        /// The Id can be empty, but only one empty key may exist in the entire application domain, at any one time.  <br />  
        /// <br />  
        /// Repeat registrations of an empty key will replace the existing registration.  <br />
        /// </remarks>
        public static void RegisterChoicePool(IChoicePool obj, bool setAsDefault = false)
        {
            if (poolRegistry.ContainsKey(obj.Id))
            {
                poolRegistry[obj.Id] = obj;
            }
            else
            {
                poolRegistry.Add(obj.Id, obj);
            }

            if (setAsDefault)
            {
                defaultChoicePool = obj;
                DefaultChoicePoolChanged?.Invoke(obj, new EventArgs());
            }
        }

        /// <summary>
        /// Create a new instance of <typeparamref name="T"/> and add it to the global choice pool registry.
        /// </summary>
        /// <typeparam name="T">A createable reference object that implements the <see cref="IChoicePool"/> interface.</typeparam>
        /// <param name="setAsDefault">True to set the new choice pool as the default choice pool for the current application domain.</param>
        /// <returns></returns>
        /// <remarks>
        /// Implementing members are responsible for generating their own <see cref="Guid"/> Id.
        /// <br />
        /// Implementers are also encouraged to use <see cref="IChoicePool.GenerateChoicePoolId"/> to create a new Id.<br />
        /// <br />
        /// If a choice pool of any type is already registered with the same <see cref="IChoicePool.Id"/> as <paramref name="obj"/>, then it will be replaced.<br />
        /// <br />
        /// The Id can be empty, but only one empty key may exist in the entire application domain, at any one time.  <br />  
        /// <br />  
        /// Repeat registrations of an empty key will replace the existing registration.  <br />
        /// </remarks>
        public static IChoicePool RegisterChoicePoolType<T>(bool setAsDefault = false) where T : class, IChoicePool, new()
        {
            T obj = null;

            IChoicePool cp;

            cp = poolRegistry.Values.Where((cp) => cp.GetType() == typeof(T)).FirstOrDefault();
            if (cp != null) return cp;

            obj = new T();
            poolRegistry.Add(obj.Id, obj);

            if (setAsDefault)
            {
                defaultChoicePool = obj;
            }

            return obj;
        }

        /// <summary>
        /// Create a new instance of <paramref name="type"/> and add it to the global choice pool registry.
        /// </summary>
        /// <param name="type">The type of the instance to register.</param>
        /// <param name="setAsDefault">True to set the new choice pool as the default choice pool for the current application domain.</param>
        /// <returns></returns>
        /// <remarks>
        /// Implementing members are responsible for generating their own <see cref="Guid"/> Id.
        /// <br />
        /// Implementers are also encouraged to use <see cref="IChoicePool.GenerateChoicePoolId"/> to create a new Id.<br />
        /// <br />
        /// If a choice pool of any type is already registered with the same <see cref="IChoicePool.Id"/> as <paramref name="obj"/>, then it will be replaced.<br />
        /// <br />
        /// The Id can be empty, but only one empty key may exist in the entire application domain, at any one time.  <br />  
        /// <br />  
        /// Repeat registrations of an empty key will replace the existing registration.  <br />
        /// </remarks>
        public static IChoicePool RegisterChoicePoolType(Type type, bool setAsDefault = false)
        {
            IChoicePool obj = null;

            if (typeof(IChoicePool).IsAssignableFrom(type))
            {
                var cons = type.GetConstructors();

                foreach (var con in cons)
                {
                    var p = con.GetParameters();

                    if (p.Length == 1 && p[0].ParameterType == typeof(Guid))
                    {
                        var callparams = new object[1] { GenerateChoicePoolId() };

                        obj = (IChoicePool)con.Invoke(callparams);
                        break;
                    }
                    else if (p.Length == 0)
                    {
                        obj = (IChoicePool)con.Invoke(new object[0]);
                        break;
                    }
                }

                if (obj != null)
                {
                    poolRegistry.Add(obj.Id, obj);

                    if (setAsDefault)
                    {
                        defaultChoicePool = obj;
                    }

                    return obj;
                }
                else
                {
                    throw new MissingMethodException($"Cannot create a new instance of {type.FullName}.");
                }

            }
            else
            {
                throw new InvalidCastException("Cannot cast to IChoicePool.");
            }
        }

        /// <summary>
        /// Remove a choice pool from the global registry.
        /// </summary>
        /// <param name="obj">The choice pool to remove.</param>
        public static void UnregisterChoicePool(IChoicePool obj)
        {
            if (obj == null) return;
            poolRegistry.Remove(obj.Id);
        }
        
        /// <summary>
        /// Remove a choice pool from the global registry by id.
        /// </summary>
        /// <param name="id">The id of the choice pool to remove.</param>
        public static void UnregisterChoicePoolById(Guid id)
        {
            if (poolRegistry.ContainsKey(id)) poolRegistry.Remove(id);

        }

        /// <summary>
        /// Generates a new choice pool <see cref="Guid"/>.
        /// </summary>
        /// <returns></returns>
        public static Guid GenerateChoicePoolId()
        {
            return Guid.NewGuid();
        }

        /// <summary>
        /// The unique Id for this choice pool, irrespective of type.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Gets or sets a custom value collection for this choice pool, by its key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        ICustomValueCollection this[string key] { get; set; }

    }

}

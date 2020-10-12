using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataTools.Interop.Native;
using CoreCT.SystemInformation;
using System.ComponentModel;

namespace DataTools.Interop.Processor
{
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class CacheInfo
    {
        private CACHE_DESCRIPTOR source;

        internal CACHE_DESCRIPTOR Source
        {
            get => source;
            set => source = value;
        }

        internal CacheInfo(CACHE_DESCRIPTOR source)
        {
            this.source = source;
            _Level = source.Level;
            _Associativity = source.Associativity;
            _LineSize = source.LineSize;
            _Size = source.Size;
            _Type = source.Type;
        }

        /// <summary>
        /// Level
        /// </summary>
        private byte _Level;

        public byte Level
        {
            get => _Level;
        }


        /// <summary>
        /// Associativity
        /// </summary>
        private byte _Associativity;

        public bool IsFullyAssociative
        {
            get => _Associativity == 0xff;
        }

        private short _LineSize;

        /// <summary>
        /// Line size
        /// </summary>
        public short LineSize
        {
            get => _LineSize;
        }

        private int _Size;

        /// <summary>
        /// Size
        /// </summary>
        public int Size
        {
            get => _Size;
        }

        private ProcessorCacheType _Type;

        /// <summary>
        /// Type
        /// </summary>
        public ProcessorCacheType Type
        {
            get => _Type;
        }

        public override string ToString()
        {
            try
            {
                string s = source.ToString();
                return s;
            }
            catch
            {

            }
            
            return base.ToString();

        }

    }
}

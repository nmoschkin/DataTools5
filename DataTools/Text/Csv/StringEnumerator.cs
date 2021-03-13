using System;
using System.Collections.Generic;
using System.Collections;

namespace DataTools.Text.Csv
{


    public class StringEnumerator : IEnumerator<string>
    {
        private List<string> subj;
        private int pos = -1;

        internal StringEnumerator(List<string> subject)
        {
            subj = subject;
        }

        public string Current
        {
            get => subj[pos];
        }

        object IEnumerator.Current
        {
            get => subj[pos];
        }

        public bool MoveNext() => (++pos < subj.Count);

        public void Reset()
        {
            pos = -1;
        }


        private bool disposedValue; // To detect redundant calls

        // IDisposable
        protected virtual void Dispose(bool disposing)
        {
            disposedValue = true;
        }

        public void Dispose()
        {
            if (disposedValue) return;

            Dispose(true);
            GC.SuppressFinalize(this);
        }


    }
}

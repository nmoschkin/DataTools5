using System;
using System.Collections;
using System.Collections.Generic;


namespace DataTools.Text.Csv
{
    public class CsvRowEnumerator : IEnumerator<CsvRow>
    {
        private CsvWrapper subj;
        private int pos = -1;

        internal CsvRowEnumerator(CsvWrapper subject)
        {
            subj = subject;
        }

        public CsvRow Current
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

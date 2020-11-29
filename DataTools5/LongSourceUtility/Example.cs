// This is some comments


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utility.Examples
{
    public class Example
    {

        bool test1 = true;
        int nest = 0;


        string s1 = "This is somet text";

        string s2 = @"This is literal text";

        /**** THIS 
         * Is a multiline
         * comment
         */

        // This is a line comment
        // This is a line comment
        // This is a line comment


        string Prams = $"{DateTime.Now.ToString()}";

    }
    // This is a line comment
    // This is a line comment

    public struct ex2
    {
        public int a;
        public int b;
    }


    public enum ex3
    {
        val1,
        val2
    }

    public interface IFace
    {
        void Factor();

        string Text { get; set; }
    }


}

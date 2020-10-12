using System;
using DataTools.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Windows;

namespace Utility
{
    public partial class Form1 : Form
    {

        private string[] currentLines;

        private List<Marker> currentMarkers;


        public Form1()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            var dlg = new OpenFileDialog()
            {
                Filter = "C# Files (*.cs)|*.cs|Any File|*.*",
                InitialDirectory = Directory.GetCurrentDirectory(),
                Title = "Pick a C# Code File",
                CheckFileExists = true
            };

            //Console.WriteLine("Press Any Key To Continue.");

            //Console.ReadKey();



            var res = dlg.ShowDialog();


            if (res == DialogResult.Cancel) return;

            textBox1.Text = dlg.FileName;





        }

        public class WordObject
        {
            public string Word { get; set; }

            public int Line { get; set; }

            public static List<WordObject>LinesToWords(string[] lines)
            {
                WordObject o;
                List<WordObject> ret = new List<WordObject>();
                int c = 0;

                foreach (var s in lines)
                {
                    var words = TextTools.Words(s, SkipQuotes:true);

                    foreach (var w in words)
                    {
                        o = new WordObject()
                        {
                            Word = w,
                            Line = c
                        };

                        ret.Add(o);
                        o = null;
                    }

                    c++;
                }

                return ret;
            }

            public override string ToString()
            {
                return Word;
            }

        }

        public class Marker
        {
            public int StartLine { get; set; }

            public int EndLine { get; set; }

            public string Name { get; set; }

            public string Kind { get; set; }

            public List<WordObject> Content { get; } = new List<WordObject>();

            public override string ToString()
            {
                return $"{Kind} {Name}, Line: {StartLine} to {EndLine}";
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {

            var input = File.ReadAllLines(textBox1.Text);

            currentLines = input;

            var markers = new List<Marker>();
            currentMarkers = markers;

            var words = WordObject.LinesToWords(input);

            int i = 0, c = words.Count;

            int currlevel = 0;

            int thingLevel = 0;

            Marker curr;

            do
            {
                if (words[i].Word.StartsWith("//"))
                {
                    int cl = words[i].Line;

                    while (words[i].Line == cl && i < c) i++;

                    if (i >= c) throw new ArgumentException();
                }

                else if (words[i].Word.StartsWith("/*"))
                {
                    i += 1;
                    while (!words[i].Word.EndsWith("*/") && i < c) i++;

                    if (i >= c) throw new ArgumentException();
                }


                switch (words[i].Word)
                {

                    case "class":
                    case "enum":
                    case "struct":
                    case "interface":

                        if (i == c) throw new SyntaxErrorException();

                        // initialize a marker.
                        curr = new Marker();

                        curr.Kind = words[i].Word;
                        curr.Name = words[i + 1].Word;

                        int j = i - 1;
                        
                        while (j > 0)
                        {


                            if (words[j].Word.StartsWith("//"))
                            {
                                int cl = words[j].Line;

                                while (words[j].Line == cl && j >= 0) j--;

                                if (j < 0) throw new ArgumentException();
                                continue;
                            }

                            else if (words[j].Word.EndsWith("*/"))
                            {
                                j -= 1;
                                while (!words[j].Word.StartsWith("/*") && j >= 0) j--;

                                if (j < 0) throw new ArgumentException();
                                continue;
                            }

                            if (words[j].Word.StartsWith("\""))
                            {
                                j--;
                                continue;
                            }

                            if (words[j].Word.Contains("}"))
                            {
                                curr.StartLine = words[j + 1].Line;
                                break;
                            }

                            else if (words[j].Word.Contains("{"))
                            {
                                curr.StartLine = words[j + 1].Line;
                                break;
                            }


                            j--;
                        }

                        if (j < 0) throw new SyntaxErrorException();

                        j = i + 1;
                        thingLevel = currlevel;

                        while (j < c)
                        {

                            if (words[j].Word.StartsWith("//"))
                            {
                                int cl = words[j].Line;

                                while (words[j].Line == cl && j < c) j++;

                                if (j >= c) throw new ArgumentException();
                            }

                            else if (words[j].Word.StartsWith("/*"))
                            {
                                j += 1;
                                while (!words[j].Word.EndsWith("*/") && j < c) j++;

                                if (j >= c) throw new ArgumentException();
                            }

                            if (words[j].Word.StartsWith("\""))
                            {
                                j++;
                                continue;
                            }

                            if (words[j].Word.Contains("{"))
                            {
                                currlevel++;
                            }
                            else if (words[j].Word.Contains("}"))
                            {
                                currlevel--;
                                if (currlevel == thingLevel)
                                {
                                    curr.EndLine = words[j].Line;
                                    markers.Add(curr);

                                    curr = null;
                                    break;
                                }
                            }

                            j++;
                        }

                        if (j >= c) throw new SyntaxErrorException();
                        i = j + 1;

                        // go back to find the last } brace.

                        break;

                    default:
                        i++;
                        break;

                }

            } while (i < c);


            listBox1.Items.AddRange(currentMarkers.ToArray());
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex < 0) return;

            var marker = currentMarkers[listBox1.SelectedIndex];

            string t = "";

            for (var i = marker.StartLine; i <= marker.EndLine; i++)
            {
                if (t != "") t += "\r\n";
                t += currentLines[i];
            }

            textBox2.Text = t;





        }
    }
}

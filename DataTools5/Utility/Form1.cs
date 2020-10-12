using System;
using DataTools.Text;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Windows;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Runtime.ConstrainedExecution;

namespace Utility
{
    public partial class Form1 : Form
    {

        private string[] currentLines;

        private List<Marker> currentMarkers;

        private int preambleTo;

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

            string currns = null;

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

                    case "namespace":

                        if (i >= c + 1) throw new SyntaxErrorException();

                        currns = words[i + 1].Word;
                        preambleTo = words[i].Line - 1;
                        
                        i++;

                        break;

                    case "class":
                    case "enum":
                    case "struct":
                    case "interface":

                        if (i == c) throw new SyntaxErrorException();

                        // initialize a marker.
                        curr = new Marker
                        {
                            Namespace = currns,
                            Kind = words[i].Word,
                            Name = words[i + 1].Word
                        };

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

                                if (words[j].Word.EndsWith("*/"))
                                {
                                    j += 1;
                                    continue;
                                }

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

            textBox2.Text = OutputFile.FormatOutputText(marker, currentLines, preambleTo);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog()
            {
                SelectedPath = Directory.GetCurrentDirectory(),
                UseDescriptionForTitle = true,
                Description = "Select An Output Folder"
            };

            if (dlg.ShowDialog() != DialogResult.OK) return;

            var p = dlg.SelectedPath;

            foreach (var marker in currentMarkers)
            {
                var file = OutputFile.NewFile(p, marker, currentLines, preambleTo);
                file.Write();
            }

            System.Diagnostics.Process.Start("explorer.exe", p);
            

        }
    }


    public class WordObject
    {
        public string Word { get; set; }

        public int Line { get; set; }

        public static List<WordObject> LinesToWords(string[] lines)
        {
            WordObject o;
            List<WordObject> ret = new List<WordObject>();
            int c = 0;

            foreach (var s in lines)
            {
                var words = TextTools.Words(s, SkipQuotes: true);

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

        public string Namespace { get; set; }

        public List<WordObject> Content { get; } = new List<WordObject>();

        public override string ToString()
        {
            return $"{Kind} {Name}, Line: {StartLine} to {EndLine}";
        }

    }

    public class OutputFile
    {
        public string Text { get; set; }

        public string Filename { get; set; }

        public bool Write()
        {
            try
            {
                var p = Path.GetDirectoryName(Filename);
                Directory.CreateDirectory(p);

                File.WriteAllText(Filename, Text);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static OutputFile NewFile(string path, Marker marker, string[] lines, int preambleTo = -1)
        {
            return NewFile(path, marker.Kind, marker.Name, FormatOutputText(marker, lines, preambleTo));
        }

        public static OutputFile NewFile(string path, string type, string name, string text)
        {
            path = path.Trim().Trim('\\');

            switch (type)
            {
                case "class":
                    type = "Classes";
                    break;

                case "interface":
                    type = "Interfaces";
                    break;

                case "struct":
                    type = "Structs";
                    break;

                case "enum":
                    type = "Enums";
                    break;

            }
            return new OutputFile
            {
                Text = text,
                Filename = $"{path}\\{type}\\{name}.cs"
            };
        }


        public static OutputFile NewFile(string filename, string text)
        {
            return new OutputFile
            {
                Text = text,
                Filename = filename
            };
        }

        public static string FormatOutputText(Marker marker, string[] lines, int preambleTo = -1)
        {

            string t = "";

            for (var i = marker.StartLine; i <= marker.EndLine; i++)
            {
                if (t != "") t += "\r\n";
                t += lines[i];
            }

            var textOut = "";

            var pre = GetPreamble(lines, preambleTo);
            var ns = "";

            if (pre != null) textOut += pre + "\r\n";

            if (marker.Namespace != null)
            {
                textOut += $"namespace {marker.Namespace}\r\n{{\r\n";
            }

            textOut += t;

            if (marker.Namespace != null)
            {
                textOut += "\r\n}\r\n";
            }

            return textOut;
        }

        public static string GetPreamble(string[] lines, int preambleTo)
        {
            string s = "";

            for (var i = 0; i <= preambleTo; i++)
            {
                if (s != "") s += "\r\n";
                s += lines[i];
            }

            return s;
        }



    }




}

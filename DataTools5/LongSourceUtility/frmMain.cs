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
using System.Linq;

namespace Utility
{
    public partial class frmMain : Form
    {

        private string[] currentLines;

        private List<Marker> currentMarkers;

        private int preambleTo;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
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

            txtFilename.Text = dlg.FileName;





        }
        private void btnStart_Click(object sender, EventArgs e)
        {

            var markers = new List<Marker>();

            markers = Parse2(File.ReadAllText(txtFilename.Text));
            currentMarkers = markers;


            lstElements.Items.AddRange(currentMarkers.ToArray());
            btnFinish.Enabled = true;

            var inLines = File.ReadAllLines(txtFilename.Text);

            currentLines = inLines;

            return;

            //var words = WordObject.LinesToWords(inLines);

            //int i = 0, c = words.Count;

            //int currlevel = 0;

            //int thingLevel = 0;

            //string currns = null;

            //Marker curr;

            //do
            //{
            //    if (words[i].Word.StartsWith("//"))
            //    {
            //        int cl = words[i].Line;

            //        while (words[i].Line == cl && i < c) i++;

            //        if (i >= c) throw new ArgumentException();
            //    }

            //    else if (words[i].Word.StartsWith("/*"))
            //    {
            //        i += 1;
            //        while (!words[i].Word.EndsWith("*/") && i < c) i++;

            //        if (i >= c) throw new ArgumentException();
            //    }

            //    if (words[i].Word == "#region" || words[i].Word == "#endregion")
            //    {
            //        int cl = words[i].Line;
            //        while (words[i].Line == cl && i < c) i++;

            //        if (i >= c) throw new ArgumentException();
            //    }

            //    switch (words[i].Word)
            //    {

            //        case "namespace":

            //            if (i >= c + 1) throw new SyntaxErrorException();

            //            currns = words[i + 1].Word;
            //            preambleTo = words[i].Line - 1;
                        
            //            i++;

            //            break;
                                          
            //        case "class":
            //        case "enum":
            //        case "struct":
            //        case "interface":

            //            if (i == c) throw new SyntaxErrorException();

            //            // initialize a marker.
            //            curr = new Marker
            //            {
            //                Namespace = currns,
            //                Kind = words[i].Word,
            //                Name = words[i + 1].Word
            //            };

            //            int j = i - 1;
                        
            //            while (j > 0)
            //            {


            //                if (words[j].Word.StartsWith("//"))
            //                {
            //                    int cl = words[j].Line;

            //                    while (words[j].Line == cl && j >= 0) j--;

            //                    if (j < 0) throw new ArgumentException();
            //                    continue;
            //                }

            //                else if (words[j].Word.EndsWith("*/"))
            //                {
            //                    j -= 1;
            //                    while (!words[j].Word.StartsWith("/*") && j >= 0) j--;

            //                    if (j < 0) throw new ArgumentException();
            //                    continue;
            //                }

            //                if (words[j].Word.StartsWith("\""))
            //                {
            //                    j--;
            //                    continue;
            //                }

            //                if (words[j].Word.Contains("}"))
            //                {
            //                    curr.StartLine = words[j + 1].Line;
            //                    break;
            //                }

            //                else if (words[j].Word.Contains("{"))
            //                {
            //                    curr.StartLine = words[j + 1].Line;
            //                    break;
            //                }


            //                j--;
            //            }

            //            if (j < 0) throw new SyntaxErrorException();

            //            j = i + 1;
            //            thingLevel = currlevel;

            //            while (j < c)
            //            {

            //                if (words[j].Word.StartsWith("//"))
            //                {
            //                    int cl = words[j].Line;

            //                    while (words[j].Line == cl && j < c) j++;

            //                    if (j >= c) throw new ArgumentException();
            //                }

            //                else if (words[j].Word.StartsWith("/*"))
            //                {

            //                    if (words[j].Word.EndsWith("*/"))
            //                    {
            //                        j += 1;
            //                        continue;
            //                    }

            //                    j += 1;
            //                    while (!words[j].Word.EndsWith("*/") && j < c) j++;

            //                    if (j >= c) throw new ArgumentException();
            //                }

            //                if (words[j].Word.StartsWith("\""))
            //                {
            //                    j++;
            //                    continue;
            //                }

            //                if (words[j].Word.Contains("{"))
            //                {
            //                    currlevel++;
            //                }
            //                else if (words[j].Word.Contains("}"))
            //                {
            //                    currlevel--;
            //                    if (currlevel == thingLevel)
            //                    {
            //                        curr.EndLine = words[j].Line;
            //                        markers.Add(curr);

            //                        curr = null;
            //                        break;
            //                    }
            //                }

            //                j++;
            //            }

            //            if (j >= c) throw new SyntaxErrorException();
            //            i = j + 1;

            //            // go back to find the last } brace.

            //            break;

            //        default:
            //            i++;
            //            break;

            //    }

            //} while (i < c);


            //lstElements.Items.AddRange(currentMarkers.ToArray());
            //btnFinish.Enabled = true;
        }

        private List<Marker> Parse2(string text)
        {

            bool inQuot = false;
            bool inthing = false;

            int level = 0;
            int startL = 0;

            string currns;
            char[] allowed = (TextTools.AlphaNumericChars + "_-").ToCharArray();
            char[] nsallowed = (TextTools.AlphaNumericChars + "_-.").ToCharArray();

            var scans = new List<char[]>();

            scans.Add("class".ToCharArray());
            scans.Add("interface".ToCharArray());
            scans.Add("enum".ToCharArray());
            scans.Add("struct".ToCharArray());
            scans.Add("namespace".ToCharArray());
                    
            string kind = null, name = null;
            string namesp = null;

            int linestart = 0;
            int i, c;
            int line = 0;
            int j, d;
            int startLine = 0;
            int endLine = 0;
            int startPos, endPos;

            bool firstNs = true;
            int lreal = 0;
            int pre = 0;

            List<Marker> markers = new List<Marker>();
            Marker mark;

            char[] input = text.ToCharArray();

            c = input.Length;


            for (i = 0; i < c; i++)
            {
                if (input[i] == '\r' || input[i] == '\t') continue;

                if (input[i] == '\n')
                {
                    line++;
                    linestart = i + 1;
                    continue;
                }

                // Comments

                if (input[i] == '/')
                {
                    if (i >= c - 1) break;

                    if (input[i + 1] == '/')
                    {
                        i += 2;
                        while (input[i] != '\n' && i < c - 1)
                        {
                            i++;
                        }

                        linestart = i;
                        line++;
                        continue;
                    }
                    else if (input[i + 1] == '*')
                    {
                        i += 2;
                        while (i < c - 2)
                        {
                            if (input[i] == '\n')
                            {
                                line++;
                                i++;
                                linestart = i;
                                continue;
                            }

                            if (input[i] == '*' && input[i + 1] == '/')
                            {
                                i++;
                                break;
                            }

                            i++;
                        }

                        continue;
                    }
                }

                // Literals 
                if (input[i] == '@')
                {
                    if (i > c - 1) throw new SyntaxErrorException();

                    if (input[i + 1] == '\"')
                    {
                        for (j = i + 2; j < c; j++)
                        {
                            if (input[j] == '\n')
                            {
                                line++;
                                j++;
                                linestart = j;
                                continue;
                            }

                            if (input[j] == '\"')
                            {
                                if (j >= c - 1 || input[j + 1] != '\"')
                                {
                                    // end of the string
                                    i = j + 1;
                                    break;
                                }
                            }
                        }
                    }
                }


                // Quotes 
                if (input[i] == '\"')
                {
                    if (i > c - 1) throw new SyntaxErrorException();

                    for (j = i + 1; j < c; j++)
                    {
                        if (input[j] == '\n')
                        {
                            line++;
                            j++;
                            linestart = j;
                            continue;
                        }
                        if (input[j] == '\"')
                        {
                            if (input[j - 1] != '\\')
                            {
                                // end of the string
                                i = j;
                                break;
                            }
                        }
                    }

                }

                if (inthing)
                {

                    if (input[i] == '{')
                    {
                        level++;
                    }
                    else if (input[i] == '}')
                    {
                        level--;
                        if (level == startL)
                        {
                            endPos = i;
                            endLine = line;

                            mark = new Marker()
                            {
                                StartLine = lreal,
                                EndLine = endLine,
                                Kind = kind,
                                Name = name,
                                Namespace = namesp
                            };

                            markers.Add(mark);
                            inthing = false;
                           
                            lreal = line + 1;
                            name = kind = null;
                        }
                    }

                    continue;
                }


                if (char.IsWhiteSpace(input[i])) continue;

                if (char.IsLetter(input[i]))
                {

                    foreach (var scan in scans)
                    {
                        var sc = new string(scan);

                        if (scan[0] == input[i])
                        {
                            int ss = line;

                            for (j = 0; j < scan.Length; j++)
                            {
                                if (scan[j] != input[i]) break;
                                i++;
                            }

                            if (sc == "namespace")
                            {

                                if (j == scan.Length && i < c - 1 && !nsallowed.Contains(input[i]))
                                {
                                    while (!nsallowed.Contains(input[i]) && i < c - 1) i++;

                                    int sn = i;

                                    while (nsallowed.Contains(input[i]) && i < c - 1) i++;
                                    int en = i;

                                    namesp = text.Substring(sn, en - sn);
                                    if (firstNs)
                                    {
                                        pre = ss - 1;
                                        lreal = pre + 3;
                                    }
                                    firstNs = false;

                                    break;
                                }

                            }
                            else
                            {

                                if (j == scan.Length && i < c - 1 && !allowed.Contains(input[i]))
                                {
                                    while (!allowed.Contains(input[i]) && i < c - 1) i++;

                                    int sn = i;

                                    while (allowed.Contains(input[i]) && i < c - 1) i++;
                                    int en = i;

                                    name = text.Substring(sn, en - sn);

                                    kind = new string(scan);
                                    startLine = line;
                                    startPos = linestart;
                                    inthing = true;
                                    startL = level;

                                    break;
                                }

                            }

                        }
                    }

                    continue;
                }

            }

            preambleTo = pre;
            return markers;

        }

        private void lstElements_SelectionChanged(object sender, EventArgs e)
        {
            if (lstElements.SelectedIndex < 0) return;

            var marker = currentMarkers[lstElements.SelectedIndex];

            txtCode.Text = OutputFile.FormatOutputText(marker, currentLines, preambleTo);

        }

        private void btnFinish_Click(object sender, EventArgs e)
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFilename.Text = null;
            txtCode.Text = null;
            lstElements.Items.Clear();
            currentLines = null;
            currentMarkers = null;
            btnFinish.Enabled = false;

        }

        private void txtFilename_TextChanged(object sender, EventArgs e)
        {
            btnStart.Enabled = File.Exists(txtFilename.Text);
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

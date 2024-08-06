using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project_Fichier_DXF
{
    public partial class Form1 : Form
    {
        private List<Line> lines = new List<Line>();
        private bool drawing = false;
        private Point startPoint;
        private string fileName = @"C:\Test.dxf";
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = fileName;
            pic.MouseDown += new MouseEventHandler(pic_MouseDown_1);
            pic.MouseMove += new MouseEventHandler(pic_MouseMove_1);
            pic.MouseUp += new MouseEventHandler(pic_MouseUp);
            pic.Paint += new PaintEventHandler(pic_Paint_1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "DXF files (*.dxf)|*.dxf",
                Title = "Save DXF File",
                FileName = textBox1.Text
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                ExportToDXF(fileName);
                MessageBox.Show("Export completed.");
            }
        }
            private void ExportToDXF(string fileName)
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    writer.WriteLine("0");
                    writer.WriteLine("SECTION");
                    writer.WriteLine("2");
                    writer.WriteLine("HEADER");
                    writer.WriteLine("0");
                    writer.WriteLine("ENDSEC");
                    writer.WriteLine("0");
                    writer.WriteLine("SECTION");
                    writer.WriteLine("2");
                    writer.WriteLine("TABLES");
                    writer.WriteLine("0");
                    writer.WriteLine("TABLE");
                    writer.WriteLine("2");
                    writer.WriteLine("LAYER");
                    writer.WriteLine("70");
                    writer.WriteLine("1");
                    writer.WriteLine("0");
                    writer.WriteLine("LAYER");
                    writer.WriteLine("2");
                    writer.WriteLine("Layer1");
                    writer.WriteLine("70");
                    writer.WriteLine("0");
                    writer.WriteLine("62");
                    writer.WriteLine("7");
                    writer.WriteLine("6");
                    writer.WriteLine("CONTINUOUS");
                    writer.WriteLine("0");

                    writer.WriteLine("ENDTAB");
                    writer.WriteLine("0");
                    writer.WriteLine("ENDSEC");
                    writer.WriteLine("0");
                    writer.WriteLine("SECTION");
                    writer.WriteLine("2");
                    writer.WriteLine("ENTITIES");

                    foreach (var line in lines)
                    {
                        writer.WriteLine("0");
                        writer.WriteLine("LINE");
                        writer.WriteLine("8");
                        writer.WriteLine("Layer1");
                        writer.WriteLine("10");
                        writer.WriteLine(line.Start.X);
                        writer.WriteLine("20");
                        writer.WriteLine(pic.Height - line.Start.Y);
                        writer.WriteLine("30");
                        writer.WriteLine("0");
                        writer.WriteLine("11");
                        writer.WriteLine(line.End.X);
                        writer.WriteLine("21");
                        writer.WriteLine(pic.Height - line.End.Y);
                        writer.WriteLine("31");
                        writer.WriteLine("0");
                    }

                    writer.WriteLine("0");
                    writer.WriteLine("ENDSEC");
                    writer.WriteLine("0");
                    writer.WriteLine("EOF");
                }
            }

        private void button1_Click(object sender, EventArgs e)
        {

            lines.Clear();
            pic.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit  ?", "Exit confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void pic_MouseDown_1(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                startPoint = e.Location;
                drawing = true;
            }
            else if (e.Button == MouseButtons.Right && lines.Count > 0)
            {
                lines.RemoveAt(lines.Count - 1);
                pic.Invalidate();
            }
        }

        private void pic_MouseMove_1(object sender, MouseEventArgs e)
        {


        }

        private void pic_MouseUp(object sender, MouseEventArgs e)
        { 
                if (drawing)
                {
                    drawing = false;
                    lines.Add(new Line(startPoint, e.Location));
                    pic.Invalidate();
                };
        }

        private void pic_Paint_1(object sender, PaintEventArgs e)
        { 
                foreach (var line in lines)
                {
                    e.Graphics.DrawLine(Pens.Black, line.Start, line.End);
                }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class Line
        {
            public Point Start { get; set; }
            public Point End { get; set; }

            public Line(Point start, Point end)
            {
                Start = start;
                End = end;
            }
        }
}

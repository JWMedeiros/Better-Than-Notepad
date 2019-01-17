//Project 1 due November 29,2018 Created by John Medeiros
//This project is a text editor that is better than Notepad, but not better than Notepad++
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace Project_1
{
    public partial class Form1 : Form
    {
        PrintDocument doc = new PrintDocument();
        PrintDialog dialog = new PrintDialog();
        string textToPrint = "";
        System.Drawing.Font fontToPrint;

        public Form1()
        {
            InitializeComponent();
            doc.PrintPage += new PrintPageEventHandler(document_PrintPage);
            fileToolStripMenuItem.ToolTipText = "Shortcut: Alt+F, Displays the file controls in the text editor.";
            windowToolStripMenuItem.ToolTipText = "Shortcut: Alt+W, Displays the current files in the text editor when clicked.";
            exitToolStripMenuItem.ToolTipText = "Shortcut: Alt+End, Exits the text editor.";
            newToolStripMenuItem.ToolTipText = "Creates a new file in the text editor";
            openToolStripMenuItem.ToolTipText = "Opens a file in the text editor";
            saveToolStripMenuItem.ToolTipText = "Saves the current file in the text editor";
            printToolStripMenuItem.ToolTipText = "Prints the current file in the text editor";
            closeToolStripMenuItem.ToolTipText = "Closes the current file in the text editor";
        }

        //Generates a new Form with rich textbox and character counter
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 f = new Form2();
            f.MdiParent = this;
            f.Text = "Untitled";
            f.Show();
        }

        //Saves the current child form
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            RichTextBox thebox = (RichTextBox)activeChild.ActiveControl;
            saveFileDialog1.DefaultExt = "txt";
            saveFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            if (activeChild.Text=="Untitled")
            {
                if (saveFileDialog1.ShowDialog()==DialogResult.OK)
                {
                    if (saveFileDialog1.FileName!="")
                    {
                        System.IO.StreamWriter sw;
                        sw = System.IO.File.CreateText(saveFileDialog1.FileName);
                        sw.Write(thebox.Text);
                        activeChild.Text = saveFileDialog1.FileName;
                        sw.Close();
                    }
                }
            }
            else
            {
                System.IO.File.WriteAllText(activeChild.Text,thebox.Text);
            }
        }

        //Allows for printing of the current child form
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            if (activeChild != null)
            {
                try
                {
                    RichTextBox thebox = (RichTextBox)activeChild.ActiveControl;
                    textToPrint = thebox.Text;
                    fontToPrint = thebox.Font;
                    dialog.AllowSomePages = true;
                    dialog.ShowHelp = true;
                    dialog.Document = doc;
                    DialogResult result = dialog.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        doc.Print();
                    }
                }
                catch
                {
                    MessageBox.Show("You need to select an active document to print");
                }
            }
            
        }

        //Print functionality
        private void document_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int numChar = 0;
            int numLines = 0;
            e.Graphics.MeasureString(textToPrint, fontToPrint, e.MarginBounds.Size, StringFormat.GenericTypographic, out numChar, out numLines);
            e.Graphics.DrawString(textToPrint, fontToPrint, System.Drawing.Brushes.Black, e.MarginBounds, StringFormat.GenericTypographic);
            textToPrint = textToPrint.Substring(numChar);
            e.HasMorePages = (textToPrint.Length > 0);
        }

        //Opens file
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.DefaultExt = "txt";
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog1.Filter = "Text Files (*.txt)|*.txt|Html Files (*.htm)|*.htm|Rich Text Files (*.rtf)|*.rtf|All Files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.FileName = string.Empty;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr;
                sr = System.IO.File.OpenText(openFileDialog1.FileName);
                Form2 f = new Form2();
                f.MdiParent = this;
                f.Text = openFileDialog1.FileName;
                Form activeChild = f;
                f.Show();
                RichTextBox thebox = (RichTextBox)f.ActiveControl;
                while (!sr.EndOfStream)
                {
                    thebox.Text += sr.ReadToEnd();
                }
                sr.Close();
            }
        }

        //Runs the close tool strip for every child form in the text editor, if any are cancelled will not exit the program, otherwise it will exit
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            if (activeChild!=null)
            {
                foreach (Form f in this.MdiChildren)
                {
                    closeToolStripMenuItem_Click(sender, e);
                    activeChild = this.ActiveMdiChild;
                }
                if (activeChild==null)
                {
                    this.Dispose();
                    this.Close();
                }
                
            }
            else
            {
                this.Dispose();
                this.Close();
            }
            
        }

        //Closes the active child form
        public void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            DialogResult result;
            RichTextBox thebox = (RichTextBox)activeChild.ActiveControl;
            if (activeChild!=null)
            {
                if (activeChild.Text == "Untitled")
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    result = MessageBox.Show("This document is unsaved, would you like to save it?", "Unsaved File", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        saveToolStripMenuItem_Click(sender, e);
                        activeChild.Dispose();
                        activeChild.Close();
                    }
                    else if (result == DialogResult.No)
                    {
                        activeChild.Dispose();
                        activeChild.Close();
                    }
                }
                else
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    result = MessageBox.Show("This document might have unsaved changes, would you like to save?", "Save File?", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        System.IO.File.WriteAllText(activeChild.Text, thebox.Text);
                        activeChild.Dispose();
                        activeChild.Close();
                    }
                    else 
                    {
                        activeChild.Dispose();
                        activeChild.Close();
                    }
                }
            }
            else
            {
                System.Media.SystemSounds.Exclamation.Play();
                MessageBox.Show("To close a document, you must first have one open.","No Document to Close");
            }
        }

        //close event for the form (runs the exit function)
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            exitToolStripMenuItem_Click(sender,e);
        }

        //Unused
        private void fileToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
        }

        //The function of this is to capitalize the first letter of every sentence
        private void capitalizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form activeChild = this.ActiveMdiChild;
            RichTextBox thebox = (RichTextBox)activeChild.ActiveControl;
            bool isNewSentense = true;
            var result = new StringBuilder(thebox.Text.Length);
            for (int i = 0; i < thebox.Text.Length; i++)
            {
                if (isNewSentense == true && char.IsLetter(thebox.Text[i]))
                {
                    result.Append(char.ToUpper(thebox.Text[i]));
                    isNewSentense = false;
                }
                else
                {
                    result.Append(thebox.Text[i]);
                }
                if (thebox.Text[i] == '!' || thebox.Text[i] == '?' || thebox.Text[i] == '.')
                {
                    isNewSentense = true;
                }

            }
            thebox.Text = result.ToString();
        }
    }
}

using System;
namespace DeveMazeGeneratorGui
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button11 = new System.Windows.Forms.Button();
            this.button12 = new System.Windows.Forms.Button();
            this.button13 = new System.Windows.Forms.Button();
            this.button14 = new System.Windows.Forms.Button();
            this.button15 = new System.Windows.Forms.Button();
            this.button16 = new System.Windows.Forms.Button();
            this.button17 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button18 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button19 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(158, 211);
            this.button1.Margin = new System.Windows.Forms.Padding(2);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(141, 74);
            this.button1.TabIndex = 0;
            this.button1.Text = "Just a speed test";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(651, 11);
            this.button2.Margin = new System.Windows.Forms.Padding(2);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(146, 58);
            this.button2.TabIndex = 1;
            this.button2.Text = "Some testing with inlining";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(398, 11);
            this.button3.Margin = new System.Windows.Forms.Padding(2);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(214, 128);
            this.button3.TabIndex = 2;
            this.button3.Text = "Generate 1k big mazes multithreaded";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(441, 143);
            this.button4.Margin = new System.Windows.Forms.Padding(2);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(116, 103);
            this.button4.TabIndex = 3;
            this.button4.Text = "Generate 1k mazes";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(608, 201);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(186, 117);
            this.button5.TabIndex = 4;
            this.button5.Text = "Big maze";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(12, 11);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(116, 80);
            this.button6.TabIndex = 5;
            this.button6.Text = "AggressiveInlining vs Non Aggressive inlining";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(13, 98);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(115, 78);
            this.button7.TabIndex = 6;
            this.button7.Text = "Bovenstaande single threaded";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(651, 73);
            this.button8.Margin = new System.Windows.Forms.Padding(2);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(146, 46);
            this.button8.TabIndex = 7;
            this.button8.Text = "Test with callback";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(333, 282);
            this.button9.Margin = new System.Windows.Forms.Padding(2);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(151, 48);
            this.button9.TabIndex = 8;
            this.button9.Text = "Generate 2x the same";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(158, 296);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(141, 87);
            this.button10.TabIndex = 9;
            this.button10.Text = "Just another speed test thingy";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(315, 430);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(482, 173);
            this.listBox1.TabIndex = 10;
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(159, 11);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(150, 80);
            this.button11.TabIndex = 11;
            this.button11.Text = "Generate a simple maze";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(12, 205);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(115, 87);
            this.button12.TabIndex = 12;
            this.button12.Text = "InnerMapType comparisson";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // button13
            // 
            this.button13.Location = new System.Drawing.Point(12, 487);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(150, 37);
            this.button13.TabIndex = 13;
            this.button13.Text = "Find long path maze";
            this.button13.UseVisualStyleBackColor = true;
            this.button13.Click += new System.EventHandler(this.button13_Click);
            // 
            // button14
            // 
            this.button14.Location = new System.Drawing.Point(168, 487);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(141, 37);
            this.button14.TabIndex = 14;
            this.button14.Text = "Find shortest path maze";
            this.button14.UseVisualStyleBackColor = true;
            this.button14.Click += new System.EventHandler(this.button14_Click);
            // 
            // button15
            // 
            this.button15.Location = new System.Drawing.Point(224, 580);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(75, 23);
            this.button15.TabIndex = 15;
            this.button15.Text = "GC.Collect();";
            this.button15.UseVisualStyleBackColor = true;
            this.button15.Click += new System.EventHandler(this.button15_Click);
            // 
            // button16
            // 
            this.button16.Location = new System.Drawing.Point(12, 549);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(75, 54);
            this.button16.TabIndex = 16;
            this.button16.Text = "Stop met long path maze";
            this.button16.UseVisualStyleBackColor = true;
            this.button16.Click += new System.EventHandler(this.button16_Click);
            // 
            // button17
            // 
            this.button17.Location = new System.Drawing.Point(159, 97);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(150, 79);
            this.button17.TabIndex = 17;
            this.button17.Text = "The Test: HDBitArray, BitArreintjeFast, Walls, Loading, Pathfinding, Saving path " +
    "etc";
            this.button17.UseVisualStyleBackColor = true;
            this.button17.Click += new System.EventHandler(this.button17_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(503, 324);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(291, 100);
            this.textBox1.TabIndex = 18;
            this.textBox1.Text = "Info about mazes:\r\n0 = False = Wall = Black\r\n1 = True = Empty = White";
            // 
            // button18
            // 
            this.button18.Location = new System.Drawing.Point(333, 335);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(151, 38);
            this.button18.TabIndex = 19;
            this.button18.Text = "Size comparison, walls vs normal";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "Debug information:";
            // 
            // button19
            // 
            this.button19.Location = new System.Drawing.Point(12, 295);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(116, 78);
            this.button19.TabIndex = 21;
            this.button19.Text = "InnerMapType comparisson without HardDiskArray";
            this.button19.UseVisualStyleBackColor = true;
            this.button19.Click += new System.EventHandler(this.button19_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 615);
            this.Controls.Add(this.button19);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button17);
            this.Controls.Add(this.button16);
            this.Controls.Add(this.button15);
            this.Controls.Add(this.button14);
            this.Controls.Add(this.button13);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "DeveMazeGeneratorTestGUIAppThing";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button19;
    }
}


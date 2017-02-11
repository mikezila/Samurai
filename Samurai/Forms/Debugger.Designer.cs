namespace Samurai
{
    partial class Debugger
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
            this.stepButton = new System.Windows.Forms.Button();
            this.memoryDumpListbox = new System.Windows.Forms.ListBox();
            this.closeDebuggerButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cpuStateLabel = new System.Windows.Forms.Label();
            this.stackListBox = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.registersListBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.opcodeListBox = new System.Windows.Forms.ListBox();
            this.dumpMemoryButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // stepButton
            // 
            this.stepButton.Enabled = false;
            this.stepButton.Font = new System.Drawing.Font("Courier New", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stepButton.Location = new System.Drawing.Point(12, 41);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(156, 43);
            this.stepButton.TabIndex = 1;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // memoryDumpListbox
            // 
            this.memoryDumpListbox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.memoryDumpListbox.FormattingEnabled = true;
            this.memoryDumpListbox.ItemHeight = 16;
            this.memoryDumpListbox.Location = new System.Drawing.Point(6, 6);
            this.memoryDumpListbox.Name = "memoryDumpListbox";
            this.memoryDumpListbox.Size = new System.Drawing.Size(506, 308);
            this.memoryDumpListbox.TabIndex = 3;
            // 
            // closeDebuggerButton
            // 
            this.closeDebuggerButton.Location = new System.Drawing.Point(12, 12);
            this.closeDebuggerButton.Name = "closeDebuggerButton";
            this.closeDebuggerButton.Size = new System.Drawing.Size(156, 23);
            this.closeDebuggerButton.TabIndex = 4;
            this.closeDebuggerButton.Text = "Close Debugger";
            this.closeDebuggerButton.UseVisualStyleBackColor = true;
            this.closeDebuggerButton.Click += new System.EventHandler(this.closeDebuggerButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cpuStateLabel);
            this.groupBox1.Location = new System.Drawing.Point(174, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(184, 105);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CPU State";
            // 
            // cpuStateLabel
            // 
            this.cpuStateLabel.AutoSize = true;
            this.cpuStateLabel.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cpuStateLabel.Location = new System.Drawing.Point(6, 16);
            this.cpuStateLabel.Name = "cpuStateLabel";
            this.cpuStateLabel.Size = new System.Drawing.Size(0, 16);
            this.cpuStateLabel.TabIndex = 0;
            // 
            // stackListBox
            // 
            this.stackListBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stackListBox.FormattingEnabled = true;
            this.stackListBox.ItemHeight = 16;
            this.stackListBox.Location = new System.Drawing.Point(458, 33);
            this.stackListBox.Name = "stackListBox";
            this.stackListBox.Size = new System.Drawing.Size(88, 84);
            this.stackListBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(458, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Stack";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(364, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Registers";
            // 
            // registersListBox
            // 
            this.registersListBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registersListBox.FormattingEnabled = true;
            this.registersListBox.ItemHeight = 16;
            this.registersListBox.Location = new System.Drawing.Point(364, 33);
            this.registersListBox.Name = "registersListBox";
            this.registersListBox.Size = new System.Drawing.Size(88, 84);
            this.registersListBox.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 123);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(526, 348);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.memoryDumpListbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(518, 322);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "As Bytes";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.opcodeListBox);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(518, 322);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "As Opcodes";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // opcodeListBox
            // 
            this.opcodeListBox.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.opcodeListBox.FormattingEnabled = true;
            this.opcodeListBox.ItemHeight = 16;
            this.opcodeListBox.Location = new System.Drawing.Point(6, 6);
            this.opcodeListBox.Name = "opcodeListBox";
            this.opcodeListBox.Size = new System.Drawing.Size(506, 308);
            this.opcodeListBox.TabIndex = 0;
            // 
            // dumpMemoryButton
            // 
            this.dumpMemoryButton.Enabled = false;
            this.dumpMemoryButton.Location = new System.Drawing.Point(12, 90);
            this.dumpMemoryButton.Name = "dumpMemoryButton";
            this.dumpMemoryButton.Size = new System.Drawing.Size(156, 27);
            this.dumpMemoryButton.TabIndex = 1;
            this.dumpMemoryButton.Text = "Dump Memory";
            this.dumpMemoryButton.UseVisualStyleBackColor = true;
            this.dumpMemoryButton.Click += new System.EventHandler(this.dumpMemoryButton_Click);
            // 
            // Debugger
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 463);
            this.ControlBox = false;
            this.Controls.Add(this.dumpMemoryButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.registersListBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.stackListBox);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.closeDebuggerButton);
            this.Controls.Add(this.stepButton);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Debugger";
            this.ShowIcon = false;
            this.Text = "Debugger";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Debugger_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.ListBox memoryDumpListbox;
        private System.Windows.Forms.Button closeDebuggerButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label cpuStateLabel;
        private System.Windows.Forms.ListBox stackListBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox registersListBox;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox opcodeListBox;
        private System.Windows.Forms.Button dumpMemoryButton;
    }
}
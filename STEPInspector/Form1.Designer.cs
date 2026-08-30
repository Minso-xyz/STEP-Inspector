namespace STEPInspector
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            button_OpenSTEPFile = new Button();
            treeView_STEP = new TreeView();
            richTextBox_Entity = new RichTextBox();
            label_Status = new Label();
            SuspendLayout();
            // 
            // button_OpenSTEPFile
            // 
            button_OpenSTEPFile.Location = new Point(22, 22);
            button_OpenSTEPFile.Name = "button_OpenSTEPFile";
            button_OpenSTEPFile.Size = new Size(163, 36);
            button_OpenSTEPFile.TabIndex = 0;
            button_OpenSTEPFile.Text = "Select STEP file";
            button_OpenSTEPFile.UseVisualStyleBackColor = true;
            button_OpenSTEPFile.Click += button_OpenSTEPFile_Click;
            // 
            // treeView_STEP
            // 
            treeView_STEP.Location = new Point(22, 107);
            treeView_STEP.Name = "treeView_STEP";
            treeView_STEP.ShowNodeToolTips = true;
            treeView_STEP.Size = new Size(381, 242);
            treeView_STEP.TabIndex = 9;
            treeView_STEP.AfterSelect += treeView_STEP_AfterSelect;
            // 
            // richTextBox_Entity
            // 
            richTextBox_Entity.Location = new Point(409, 107);
            richTextBox_Entity.Name = "richTextBox_Entity";
            richTextBox_Entity.Size = new Size(265, 242);
            richTextBox_Entity.TabIndex = 10;
            richTextBox_Entity.Text = "";
            // 
            // label_Status
            // 
            label_Status.AutoSize = true;
            label_Status.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label_Status.ForeColor = SystemColors.ActiveCaptionText;
            label_Status.Location = new Point(22, 78);
            label_Status.Name = "label_Status";
            label_Status.Size = new Size(222, 15);
            label_Status.TabIndex = 11;
            label_Status.Text = "Select the STEP standard and STEP file.";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(693, 361);
            Controls.Add(label_Status);
            Controls.Add(richTextBox_Entity);
            Controls.Add(treeView_STEP);
            Controls.Add(button_OpenSTEPFile);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form1";
            Text = "STEP Inspector";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button_OpenSTEPFile;
        private TreeView treeView_STEP;
        private RichTextBox richTextBox_Entity;
        private Label label_Status;
    }
}


namespace ProjectTreeRenamer
{
    partial class RenameForm
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.textBox_Find = new System.Windows.Forms.TextBox();
            this.textBox_Replace = new System.Windows.Forms.TextBox();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.TimerShowForm = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Find:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Replace With:";
            // 
            // buttonReplace
            // 
            this.buttonReplace.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonReplace.Location = new System.Drawing.Point(12, 63);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(75, 23);
            this.buttonReplace.TabIndex = 2;
            this.buttonReplace.Text = "Replace";
            this.buttonReplace.UseVisualStyleBackColor = true;
            // 
            // textBox_Find
            // 
            this.textBox_Find.Location = new System.Drawing.Point(98, 10);
            this.textBox_Find.Name = "textBox_Find";
            this.textBox_Find.Size = new System.Drawing.Size(246, 20);
            this.textBox_Find.TabIndex = 3;
            // 
            // textBox_Replace
            // 
            this.textBox_Replace.Location = new System.Drawing.Point(98, 35);
            this.textBox_Replace.Name = "textBox_Replace";
            this.textBox_Replace.Size = new System.Drawing.Size(246, 20);
            this.textBox_Replace.TabIndex = 4;
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.Location = new System.Drawing.Point(98, 63);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 5;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // TimerShowForm
            // 
            this.TimerShowForm.Interval = 50;
            this.TimerShowForm.Tick += new System.EventHandler(this.TimerShowForm_Tick);
            // 
            // RenameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 107);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.textBox_Replace);
            this.Controls.Add(this.textBox_Find);
            this.Controls.Add(this.buttonReplace);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenameForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RenameForm";
            this.TopMost = true;
            this.Shown += new System.EventHandler(this.RenameForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonReplace;
        public System.Windows.Forms.TextBox textBox_Find;
        public System.Windows.Forms.TextBox textBox_Replace;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Timer TimerShowForm;
    }
}
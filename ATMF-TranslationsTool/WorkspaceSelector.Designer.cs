
namespace ATMF_TranslationsTool
{
    partial class WorkspaceSelector
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
            this.btnCreateWorkspace = new System.Windows.Forms.Button();
            this.btnOpenWorkspace = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnCreateWorkspace
            // 
            this.btnCreateWorkspace.Location = new System.Drawing.Point(73, 99);
            this.btnCreateWorkspace.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnCreateWorkspace.Name = "btnCreateWorkspace";
            this.btnCreateWorkspace.Size = new System.Drawing.Size(145, 43);
            this.btnCreateWorkspace.TabIndex = 3;
            this.btnCreateWorkspace.Text = "Create Workspace";
            this.btnCreateWorkspace.UseVisualStyleBackColor = true;
            this.btnCreateWorkspace.Click += new System.EventHandler(this.btnCreateWorkspace_Click);
            // 
            // btnOpenWorkspace
            // 
            this.btnOpenWorkspace.Location = new System.Drawing.Point(73, 41);
            this.btnOpenWorkspace.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.btnOpenWorkspace.Name = "btnOpenWorkspace";
            this.btnOpenWorkspace.Size = new System.Drawing.Size(145, 43);
            this.btnOpenWorkspace.TabIndex = 2;
            this.btnOpenWorkspace.Text = "Open Workspace";
            this.btnOpenWorkspace.UseVisualStyleBackColor = true;
            this.btnOpenWorkspace.Click += new System.EventHandler(this.btnOpenWorkspace_Click);
            // 
            // WorkspaceSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 179);
            this.Controls.Add(this.btnCreateWorkspace);
            this.Controls.Add(this.btnOpenWorkspace);
            this.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.MaximumSize = new System.Drawing.Size(322, 218);
            this.MinimumSize = new System.Drawing.Size(322, 218);
            this.Name = "WorkspaceSelector";
            this.Text = "Choose a workspace";
            this.Load += new System.EventHandler(this.WorkspaceSelector_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateWorkspace;
        private System.Windows.Forms.Button btnOpenWorkspace;
    }
}
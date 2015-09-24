namespace TouchOfGoodNews
{
    partial class frmMain
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
            this.lvTitles = new System.Windows.Forms.ListView();
            this.cmdDB = new System.Windows.Forms.Button();
            this.cmdGo = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTitles
            // 
            this.lvTitles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvTitles.GridLines = true;
            this.lvTitles.Location = new System.Drawing.Point(0, 0);
            this.lvTitles.Name = "lvTitles";
            this.lvTitles.Size = new System.Drawing.Size(448, 261);
            this.lvTitles.TabIndex = 1;
            this.lvTitles.UseCompatibleStateImageBehavior = false;
            this.lvTitles.View = System.Windows.Forms.View.List;
            // 
            // cmdDB
            // 
            this.cmdDB.Location = new System.Drawing.Point(230, 12);
            this.cmdDB.Name = "cmdDB";
            this.cmdDB.Size = new System.Drawing.Size(206, 52);
            this.cmdDB.TabIndex = 4;
            this.cmdDB.Text = "Test";
            this.cmdDB.UseVisualStyleBackColor = true;
            this.cmdDB.Click += new System.EventHandler(this.cmdDB_Click);
            // 
            // cmdGo
            // 
            this.cmdGo.Location = new System.Drawing.Point(12, 12);
            this.cmdGo.Name = "cmdGo";
            this.cmdGo.Size = new System.Drawing.Size(212, 52);
            this.cmdGo.TabIndex = 3;
            this.cmdGo.Text = "Go";
            this.cmdGo.UseVisualStyleBackColor = true;
            this.cmdGo.Click += new System.EventHandler(this.cmdGo_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.cmdDB);
            this.splitContainer1.Panel1.Controls.Add(this.cmdGo);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvTitles);
            this.splitContainer1.Size = new System.Drawing.Size(448, 336);
            this.splitContainer1.SplitterDistance = 71;
            this.splitContainer1.TabIndex = 4;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(448, 336);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMain";
            this.Text = "Articles";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvTitles;
        private System.Windows.Forms.Button cmdDB;
        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}


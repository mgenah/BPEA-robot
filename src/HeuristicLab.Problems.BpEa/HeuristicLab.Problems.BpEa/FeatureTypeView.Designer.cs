using HeuristicLab.Core.Views;
using HeuristicLab.Data;

namespace HeuristicLab.Problems.BpEa
{
    partial class FeatureTypeView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NameView = new System.Windows.Forms.TextBox();
            this.MinTextBox = new System.Windows.Forms.TextBox();
            this.MaxTextBox = new System.Windows.Forms.TextBox();
            this.NameLabel = new System.Windows.Forms.Label();
            this.MinLabel = new System.Windows.Forms.Label();
            this.MaxLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NameView
            // 
            this.NameView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            this.NameView.Text = null;
            this.NameView.Location = new System.Drawing.Point(58, 0);
            this.NameView.Name = "NameView";
            this.NameView.ReadOnly = true;
            this.NameView.Size = new System.Drawing.Size(268, 20);
            this.NameView.TabIndex = 0;

            // 
            // MinTextBox
            // 
            this.MinTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            this.MinTextBox.Text = null;
            this.MinTextBox.Location = new System.Drawing.Point(58, 26);
            this.MinTextBox.Name = "MinTextBox";
            this.MinTextBox.ReadOnly = false;
            this.MinTextBox.Size = new System.Drawing.Size(268, 20);
            this.MinTextBox.TabIndex = 1;

            //this.NameTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nameTextBox_KeyDown);
            //this.NameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.nameTextBox_Validating);
            //this.NameTextBox.Validated += new System.EventHandler(this.nameTextBox_Validated);
            // 
            // MaxTextBox
            // 
            this.MaxTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
            this.MaxTextBox.Text = null;
            this.MaxTextBox.Location = new System.Drawing.Point(58, 52);
            this.MaxTextBox.Name = "MaxTextBox";
            this.MaxTextBox.ReadOnly = false;
            this.MaxTextBox.Size = new System.Drawing.Size(268, 20);
            this.MaxTextBox.TabIndex = 2;
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(3, 3);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(36, 13);
            this.NameLabel.TabIndex = 3;
            this.NameLabel.Text = "Name:";
            // 
            // MinLabel
            // 
            this.MinLabel.AutoSize = true;
            this.MinLabel.Location = new System.Drawing.Point(3, 29);
            this.MinLabel.Name = "MinLabel";
            this.MinLabel.Size = new System.Drawing.Size(36, 13);
            this.MinLabel.TabIndex = 4;
            this.MinLabel.Text = "Min:";
            // 
            // MaxLabel
            // 
            this.MaxLabel.AutoSize = true;
            this.MaxLabel.Location = new System.Drawing.Point(3, 55);
            this.MaxLabel.Name = "MaxLabel";
            this.MaxLabel.Size = new System.Drawing.Size(36, 13);
            this.MaxLabel.TabIndex = 5;
            this.MaxLabel.Text = "Max:";
            // 
            // StringConvertibleValueTupleView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.MinLabel);
            this.Controls.Add(this.MaxLabel);
            this.Controls.Add(this.NameView);
            this.Controls.Add(this.MinTextBox);
            this.Controls.Add(this.MaxTextBox);
            this.Name = "FeatureTypeView";
            this.Size = new System.Drawing.Size(351, 80);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        /*
        protected virtual void nameTextBox_Validating(object sender, CancelEventArgs e)
        {
            if ((Content != null) && (Content.CanChangeName))
            {
                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    e.Cancel = true;
                    errorProvider.SetError(nameTextBox, "Name cannot be empty");
                    nameTextBox.SelectAll();
                    return;
                }
                Content.Name = nameTextBox.Text;
                // check if variable name was set successfully
                if (!Content.Name.Equals(nameTextBox.Text))
                {
                    e.Cancel = true;
                    errorProvider.SetError(nameTextBox, "Invalid Name");
                    nameTextBox.SelectAll();
                }
            }
        }
        protected virtual void nameTextBox_Validated(object sender, EventArgs e)
        {
            errorProvider.SetError(nameTextBox, string.Empty);
        }
        protected virtual void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) || (e.KeyCode == Keys.Return))
                nameLabel.Focus();  // set focus on label to validate data
            if (e.KeyCode == Keys.Escape)
            {
                nameTextBox.Text = Content.Name;
                nameLabel.Focus();  // set focus on label to validate data
            }
        }*/

        #endregion

        protected System.Windows.Forms.TextBox NameView;
        protected System.Windows.Forms.TextBox MinTextBox;
        protected System.Windows.Forms.TextBox MaxTextBox;
        protected System.Windows.Forms.Label NameLabel;
        protected System.Windows.Forms.Label MinLabel;
        protected System.Windows.Forms.Label MaxLabel;

    }
}
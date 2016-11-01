namespace ChoHanClient
{
    partial class PlayerForm
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

        #region Windows LogInForm Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.OddButton = new System.Windows.Forms.Button();
            this.WrongPlayerOneLabel = new System.Windows.Forms.Label();
            this.RightPlayerOneLabel = new System.Windows.Forms.Label();
            this.ScorePlayerOneLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.EvenButton = new System.Windows.Forms.Button();
            this.YourChoiceLabel = new System.Windows.Forms.Label();
            this.SessionListBox = new System.Windows.Forms.ListBox();
            this.PlayerListBox = new System.Windows.Forms.ListBox();
            this.LeaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(124, 136);
            this.ConfirmButton.Margin = new System.Windows.Forms.Padding(4);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(99, 24);
            this.ConfirmButton.TabIndex = 24;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // OddButton
            // 
            this.OddButton.Location = new System.Drawing.Point(124, 100);
            this.OddButton.Margin = new System.Windows.Forms.Padding(4);
            this.OddButton.Name = "OddButton";
            this.OddButton.Size = new System.Drawing.Size(100, 28);
            this.OddButton.TabIndex = 23;
            this.OddButton.Text = "Odd";
            this.OddButton.UseVisualStyleBackColor = true;
            this.OddButton.Click += new System.EventHandler(this.OddButton_Click);
            // 
            // WrongPlayerOneLabel
            // 
            this.WrongPlayerOneLabel.ForeColor = System.Drawing.Color.Red;
            this.WrongPlayerOneLabel.Location = new System.Drawing.Point(16, 55);
            this.WrongPlayerOneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.WrongPlayerOneLabel.Name = "WrongPlayerOneLabel";
            this.WrongPlayerOneLabel.Size = new System.Drawing.Size(57, 21);
            this.WrongPlayerOneLabel.TabIndex = 22;
            this.WrongPlayerOneLabel.Text = "Wrong";
            this.WrongPlayerOneLabel.Visible = false;
            // 
            // RightPlayerOneLabel
            // 
            this.RightPlayerOneLabel.ForeColor = System.Drawing.Color.Green;
            this.RightPlayerOneLabel.Location = new System.Drawing.Point(16, 55);
            this.RightPlayerOneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.RightPlayerOneLabel.Name = "RightPlayerOneLabel";
            this.RightPlayerOneLabel.Size = new System.Drawing.Size(57, 21);
            this.RightPlayerOneLabel.TabIndex = 19;
            this.RightPlayerOneLabel.Text = "Right";
            this.RightPlayerOneLabel.Visible = false;
            // 
            // ScorePlayerOneLabel
            // 
            this.ScorePlayerOneLabel.Location = new System.Drawing.Point(92, 36);
            this.ScorePlayerOneLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ScorePlayerOneLabel.Name = "ScorePlayerOneLabel";
            this.ScorePlayerOneLabel.Size = new System.Drawing.Size(57, 21);
            this.ScorePlayerOneLabel.TabIndex = 17;
            this.ScorePlayerOneLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(16, 36);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 20);
            this.label2.TabIndex = 15;
            this.label2.Text = "Your score:";
            // 
            // CommentLabel
            // 
            this.CommentLabel.Location = new System.Drawing.Point(16, 11);
            this.CommentLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(543, 25);
            this.CommentLabel.TabIndex = 14;
            this.CommentLabel.Text = "Welcome to Cho Han";
            this.CommentLabel.Click += new System.EventHandler(this.CommentLabel_Click_1);
            // 
            // EvenButton
            // 
            this.EvenButton.Location = new System.Drawing.Point(16, 100);
            this.EvenButton.Margin = new System.Windows.Forms.Padding(4);
            this.EvenButton.Name = "EvenButton";
            this.EvenButton.Size = new System.Drawing.Size(100, 28);
            this.EvenButton.TabIndex = 13;
            this.EvenButton.Text = "Even";
            this.EvenButton.UseVisualStyleBackColor = true;
            this.EvenButton.Click += new System.EventHandler(this.EvenButton_Click);
            // 
            // YourChoiceLabel
            // 
            this.YourChoiceLabel.Location = new System.Drawing.Point(16, 76);
            this.YourChoiceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.YourChoiceLabel.Name = "YourChoiceLabel";
            this.YourChoiceLabel.Size = new System.Drawing.Size(187, 20);
            this.YourChoiceLabel.TabIndex = 26;
            this.YourChoiceLabel.Text = "You choose:";
            // 
            // SessionListBox
            // 
            this.SessionListBox.FormattingEnabled = true;
            this.SessionListBox.ItemHeight = 16;
            this.SessionListBox.Items.AddRange(new object[] {
            "Sessions"});
            this.SessionListBox.Location = new System.Drawing.Point(229, 11);
            this.SessionListBox.Margin = new System.Windows.Forms.Padding(4);
            this.SessionListBox.Name = "SessionListBox";
            this.SessionListBox.Size = new System.Drawing.Size(108, 148);
            this.SessionListBox.TabIndex = 27;
            this.SessionListBox.SelectedIndexChanged += new System.EventHandler(this.SessionListBox_SelectedIndexChanged);
            // 
            // PlayerListBox
            // 
            this.PlayerListBox.FormattingEnabled = true;
            this.PlayerListBox.ItemHeight = 16;
            this.PlayerListBox.Items.AddRange(new object[] {
            "Players"});
            this.PlayerListBox.Location = new System.Drawing.Point(229, 11);
            this.PlayerListBox.Margin = new System.Windows.Forms.Padding(4);
            this.PlayerListBox.Name = "PlayerListBox";
            this.PlayerListBox.Size = new System.Drawing.Size(108, 148);
            this.PlayerListBox.TabIndex = 28;
            this.PlayerListBox.Visible = false;
            // 
            // LeaveButton
            // 
            this.LeaveButton.Location = new System.Drawing.Point(16, 135);
            this.LeaveButton.Name = "LeaveButton";
            this.LeaveButton.Size = new System.Drawing.Size(100, 25);
            this.LeaveButton.TabIndex = 29;
            this.LeaveButton.Text = "Leave";
            this.LeaveButton.UseVisualStyleBackColor = true;
            this.LeaveButton.Click += new System.EventHandler(this.LeaveButton_Click);
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(353, 176);
            this.Controls.Add(this.LeaveButton);
            this.Controls.Add(this.PlayerListBox);
            this.Controls.Add(this.SessionListBox);
            this.Controls.Add(this.YourChoiceLabel);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.OddButton);
            this.Controls.Add(this.WrongPlayerOneLabel);
            this.Controls.Add(this.RightPlayerOneLabel);
            this.Controls.Add(this.ScorePlayerOneLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.EvenButton);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlayerForm";
            this.Text = "PlayerForm";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button OddButton;
        private System.Windows.Forms.Label WrongPlayerOneLabel;
        private System.Windows.Forms.Label RightPlayerOneLabel;
        private System.Windows.Forms.Label ScorePlayerOneLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Button EvenButton;
        private System.Windows.Forms.Label YourChoiceLabel;
        private System.Windows.Forms.ListBox SessionListBox;
        private System.Windows.Forms.ListBox PlayerListBox;
        private System.Windows.Forms.Button LeaveButton;
    }
}
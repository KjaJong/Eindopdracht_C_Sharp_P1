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
            this.ConfirmButton.Location = new System.Drawing.Point(57, 110);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(75, 23);
            this.ConfirmButton.TabIndex = 24;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // OddButton
            // 
            this.OddButton.Location = new System.Drawing.Point(93, 81);
            this.OddButton.Name = "OddButton";
            this.OddButton.Size = new System.Drawing.Size(75, 23);
            this.OddButton.TabIndex = 23;
            this.OddButton.Text = "Odd";
            this.OddButton.UseVisualStyleBackColor = true;
            this.OddButton.Click += new System.EventHandler(this.OddButton_Click);
            // 
            // WrongPlayerOneLabel
            // 
            this.WrongPlayerOneLabel.ForeColor = System.Drawing.Color.Red;
            this.WrongPlayerOneLabel.Location = new System.Drawing.Point(12, 45);
            this.WrongPlayerOneLabel.Name = "WrongPlayerOneLabel";
            this.WrongPlayerOneLabel.Size = new System.Drawing.Size(43, 17);
            this.WrongPlayerOneLabel.TabIndex = 22;
            this.WrongPlayerOneLabel.Text = "Wrong";
            this.WrongPlayerOneLabel.Visible = false;
            // 
            // RightPlayerOneLabel
            // 
            this.RightPlayerOneLabel.ForeColor = System.Drawing.Color.Green;
            this.RightPlayerOneLabel.Location = new System.Drawing.Point(12, 45);
            this.RightPlayerOneLabel.Name = "RightPlayerOneLabel";
            this.RightPlayerOneLabel.Size = new System.Drawing.Size(43, 17);
            this.RightPlayerOneLabel.TabIndex = 19;
            this.RightPlayerOneLabel.Text = "Right";
            this.RightPlayerOneLabel.Visible = false;
            // 
            // ScorePlayerOneLabel
            // 
            this.ScorePlayerOneLabel.Location = new System.Drawing.Point(69, 29);
            this.ScorePlayerOneLabel.Name = "ScorePlayerOneLabel";
            this.ScorePlayerOneLabel.Size = new System.Drawing.Size(43, 17);
            this.ScorePlayerOneLabel.TabIndex = 17;
            this.ScorePlayerOneLabel.Text = "0";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 16);
            this.label2.TabIndex = 15;
            this.label2.Text = "Your score:";
            // 
            // CommentLabel
            // 
            this.CommentLabel.Location = new System.Drawing.Point(12, 9);
            this.CommentLabel.Name = "CommentLabel";
            this.CommentLabel.Size = new System.Drawing.Size(407, 20);
            this.CommentLabel.TabIndex = 14;
            this.CommentLabel.Text = "Welcome to Cho Han";
            // 
            // EvenButton
            // 
            this.EvenButton.Location = new System.Drawing.Point(12, 81);
            this.EvenButton.Name = "EvenButton";
            this.EvenButton.Size = new System.Drawing.Size(75, 23);
            this.EvenButton.TabIndex = 13;
            this.EvenButton.Text = "Even";
            this.EvenButton.UseVisualStyleBackColor = true;
            this.EvenButton.Click += new System.EventHandler(this.EvenButton_Click);
            // 
            // YourChoiceLabel
            // 
            this.YourChoiceLabel.Location = new System.Drawing.Point(12, 62);
            this.YourChoiceLabel.Name = "YourChoiceLabel";
            this.YourChoiceLabel.Size = new System.Drawing.Size(140, 16);
            this.YourChoiceLabel.TabIndex = 26;
            this.YourChoiceLabel.Text = "You choose:";
            // 
            // SessionListBox
            // 
            this.SessionListBox.FormattingEnabled = true;
            this.SessionListBox.Items.AddRange(new object[] {
            "Sessions"});
            this.SessionListBox.Location = new System.Drawing.Point(172, 9);
            this.SessionListBox.Name = "SessionListBox";
            this.SessionListBox.Size = new System.Drawing.Size(82, 160);
            this.SessionListBox.TabIndex = 27;
            this.SessionListBox.SelectedIndexChanged += new System.EventHandler(this.SessionListBox_SelectedIndexChanged);
            // 
            // PlayerListBox
            // 
            this.PlayerListBox.FormattingEnabled = true;
            this.PlayerListBox.Items.AddRange(new object[] {
            "Players"});
            this.PlayerListBox.Location = new System.Drawing.Point(172, 9);
            this.PlayerListBox.Name = "PlayerListBox";
            this.PlayerListBox.Size = new System.Drawing.Size(82, 160);
            this.PlayerListBox.TabIndex = 28;
            this.PlayerListBox.Visible = false;
            // 
            // LeaveButton
            // 
            this.LeaveButton.Location = new System.Drawing.Point(57, 139);
            this.LeaveButton.Name = "LeaveButton";
            this.LeaveButton.Size = new System.Drawing.Size(75, 23);
            this.LeaveButton.TabIndex = 29;
            this.LeaveButton.Text = "Leave";
            this.LeaveButton.UseVisualStyleBackColor = true;
            this.LeaveButton.Click += new System.EventHandler(this.LeaveButton_Click);
            // 
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(266, 186);
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
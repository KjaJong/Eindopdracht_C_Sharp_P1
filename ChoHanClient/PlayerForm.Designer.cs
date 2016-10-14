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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Leavebutton = new System.Windows.Forms.Button();
            this.ConfirmButton = new System.Windows.Forms.Button();
            this.OddButton = new System.Windows.Forms.Button();
            this.WrongPlayerOneLabel = new System.Windows.Forms.Label();
            this.RightPlayerOneLabel = new System.Windows.Forms.Label();
            this.ScorePlayerOneLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CommentLabel = new System.Windows.Forms.Label();
            this.EvenButton = new System.Windows.Forms.Button();
            this.YourChoiceLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Leavebutton
            // 
            this.Leavebutton.Location = new System.Drawing.Point(76, 171);
            this.Leavebutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Leavebutton.Name = "Leavebutton";
            this.Leavebutton.Size = new System.Drawing.Size(100, 28);
            this.Leavebutton.TabIndex = 25;
            this.Leavebutton.Text = "Leave";
            this.Leavebutton.UseVisualStyleBackColor = true;
            this.Leavebutton.Click += new System.EventHandler(this.Leavebutton_Click);
            // 
            // ConfirmButton
            // 
            this.ConfirmButton.Location = new System.Drawing.Point(76, 135);
            this.ConfirmButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.ConfirmButton.Name = "ConfirmButton";
            this.ConfirmButton.Size = new System.Drawing.Size(100, 28);
            this.ConfirmButton.TabIndex = 24;
            this.ConfirmButton.Text = "Confirm";
            this.ConfirmButton.UseVisualStyleBackColor = true;
            this.ConfirmButton.Click += new System.EventHandler(this.ConfirmButton_Click);
            // 
            // OddButton
            // 
            this.OddButton.Location = new System.Drawing.Point(124, 100);
            this.OddButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            this.WrongPlayerOneLabel.Click += new System.EventHandler(this.WrongPlayerOneLabel_Click);
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
            this.ScorePlayerOneLabel.Location = new System.Drawing.Point(105, 34);
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
            // 
            // EvenButton
            // 
            this.EvenButton.Location = new System.Drawing.Point(16, 100);
            this.EvenButton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
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
            // PlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 229);
            this.Controls.Add(this.YourChoiceLabel);
            this.Controls.Add(this.Leavebutton);
            this.Controls.Add(this.ConfirmButton);
            this.Controls.Add(this.OddButton);
            this.Controls.Add(this.WrongPlayerOneLabel);
            this.Controls.Add(this.RightPlayerOneLabel);
            this.Controls.Add(this.ScorePlayerOneLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.CommentLabel);
            this.Controls.Add(this.EvenButton);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "PlayerForm";
            this.Text = "PlayerForm";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Leavebutton;
        private System.Windows.Forms.Button ConfirmButton;
        private System.Windows.Forms.Button OddButton;
        private System.Windows.Forms.Label WrongPlayerOneLabel;
        private System.Windows.Forms.Label RightPlayerOneLabel;
        private System.Windows.Forms.Label ScorePlayerOneLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label CommentLabel;
        private System.Windows.Forms.Button EvenButton;
        private System.Windows.Forms.Label YourChoiceLabel;
    }
}
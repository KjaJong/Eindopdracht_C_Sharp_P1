﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ChoHanClient.ClientForms;

namespace ChoHanClient
{
    public partial class PlayerForm : Form
    {
        public bool? Answer { get; set; }
        public bool ConfirmAnswer { get; set; }
        public string SessionName { get; set; }

        delegate void SetTextCallback(bool text, int score);

        delegate void SetCommentCallBack(string text);

        delegate void SetListCallBack(List<String> sessions);

        delegate void SetSwitchBoxCallBack();


        public PlayerForm()
        {
            Answer = null;
            InitializeComponent();
            Visible = true;
            FormClosing += PlayerForm_FromClosing;
        }

        private void PlayerForm_FromClosing(object sender, FormClosingEventArgs e)
        {
            LogInForm.Client.Disconnect();
        }

        private void EvenButton_Click(object sender, EventArgs e)
        {
            if (!ConfirmAnswer)
            {
                Answer = true;
                YourChoiceLabel.Text = "You choose: even";
                Console.WriteLine(Answer.ToString());
            }
        }

        private void OddButton_Click(object sender, EventArgs e)
        {
            if (!ConfirmAnswer)
            {
                Answer = false;
                YourChoiceLabel.Text = "You choose: odd";
                Console.WriteLine(Answer.ToString());
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (Answer == null)
            {
                UpdateMessageLabel("Please choose a answer first.");
                return;
            }
            ConfirmAnswer = true;
        }

        public void Update(bool rightAnswerPlayerOne, int score)
        {
            if (this.ScorePlayerOneLabel.InvokeRequired)
            {
                var d = new SetTextCallback(Update);
                this.Invoke(d, rightAnswerPlayerOne, score);
            }
            else
            {

                ScorePlayerOneLabel.Text = score.ToString();

                Answer = null;
                ConfirmAnswer = false;

                if (rightAnswerPlayerOne)
                {
                    RightPlayerOneLabel.Visible = true;
                    WrongPlayerOneLabel.Visible = false;
                }
                else
                {
                    WrongPlayerOneLabel.Visible = true;
                    RightPlayerOneLabel.Visible = false;
                }
                ResetChoiceLabel();
            }
        }


        public void UpdateMessageLabel(string text)
        {
            if (this.CommentLabel.InvokeRequired)
            {
                SetCommentCallBack d = new SetCommentCallBack(UpdateMessageLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                CommentLabel.Text = text;
            }
        }

        private void ResetChoiceLabel()
        {
            YourChoiceLabel.Text = "You choose: ";
        }

        private void SessionListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SessionListBox.SelectedIndex == -1) return;
            if (SessionListBox.SelectedItem.ToString().Equals("Sessions")) return;
            if (LogInForm.Client.JoinSession(SessionListBox.SelectedItem.ToString()))
            {
                SessionName = SessionListBox.SelectedItem.ToString();
                SwitchBox();
                ResetPanel();
            }
            else
            {
                UpdateMessageLabel("Sorry, session intertupted");
            }
        }

        public void FillPlayerBox(List<string> sessions)
        {
            if (this.SessionListBox.InvokeRequired)
            {
                var d = new SetListCallBack(FillPlayerBox);
                this.Invoke(d, sessions);
            }
            else
            {
                ResetPlayerListBox();
                foreach (var s in sessions)
                {
                    PlayerListBox.Items.Add(s);
                }
            }
        }

        public void FillSessionBox(List<string> sessions)
        {
            if (this.SessionListBox.InvokeRequired)
            {
                var d = new SetListCallBack(FillSessionBox);
                this.Invoke(d, sessions);
            }
            else
            {
                SessionListBox.Items.Clear();
                SessionListBox.Items.Add("Sessions");
                foreach (var s in sessions)
                {
                    SessionListBox.Items.Add(s);
                }
            }
        }

        public void ResetPlayerListBox()
        {
            PlayerListBox.Items.Clear();
            PlayerListBox.Items.Add("Player");
        }

        public void SwitchBox()
        {
            if (SessionListBox.InvokeRequired)
            {
                SetSwitchBoxCallBack d = new SetSwitchBoxCallBack(SwitchBox);
                this.Invoke(d, new object[] { });
            }
            else
            {
                SessionListBox.Visible = !SessionListBox.Visible;
                PlayerListBox.Visible = !PlayerListBox.Visible;
                RefreshButton.Visible = !RefreshButton.Visible;
                ResetPlayerListBox();
            }
        }

        private void CommentLabel_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("odd (Cho) or even (Han). " +
                "After all players have made their choice, the die are thrown and the result is announced. " +
                "The awnser must be given within 15 seconds.",
                "A short explanation.");
        }

        public void ResetPanel()
        {
            WrongPlayerOneLabel.Visible = false;
            RightPlayerOneLabel.Visible = false;
            ScorePlayerOneLabel.Text = "0";
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LogInForm.Client.RefreshSessions();
        }
    }
}

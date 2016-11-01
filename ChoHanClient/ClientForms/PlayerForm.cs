using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChoHanClient.ClientForms;

namespace ChoHanClient
{
    public partial class PlayerForm : Form
    {
        //answer can be null, the problem is. You cans stall the game
        //TODO: make the game unstallable
        public bool ?Answer { get; set; }
        public bool ConfirmAnswer { get; set; }

        delegate void SetTextCallback(bool text, int score);

        delegate void SetCommentCallBack(string text);


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
            Environment.Exit(1);
        }

        private void EvenButton_Click(object sender, EventArgs e)
        {
            if(!ConfirmAnswer)
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
       
        public void Update(bool rightAnswerPlayerOne, int score )
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
                this.Invoke(d, new object[] {text});
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
            LogInForm.Client.JoinSession(SessionListBox.SelectedItem.ToString());
        }

        public void FillSessionBox(List<string> sessions)
        {
            SessionListBox.Items.Clear();
            SessionListBox.Items.Add("Sessions");
            foreach (var s in sessions)
            {
                SessionListBox.Items.Add(s);
            }
        }

        public void switchBox()
        {
            SessionListBox.Visible = !SessionListBox.Visible;
            PlayerListBox.Visible = !PlayerListBox.Visible;
        }

        private void CommentLabel_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "In ChoHan a player chooses wheter a roll of a pair of die is odd (Cho) or even (Han). " +
                "After all players have made their choice, the die are thrown and the result is announced. " +
                "The awnser must be given within 15 seconds.",
                "A short explanation.");
        }
    }
}

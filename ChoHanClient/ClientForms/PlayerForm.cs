using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChoHanClient
{
    public partial class PlayerForm : Form
    {
        //answer can be null, the problem is. You cans stall the game
        //TODO: make the game unstallable
        public bool ?Answer { get; set; }
        public bool ConfirmAnswer { get; set; }

        delegate void SetTextCallback(string text, string score);

        delegate void SetCommentCallBack(string text);


        public PlayerForm()
        {
            Answer = null;
            InitializeComponent();
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

        private void Leavebutton_Click(object sender, EventArgs e)
        {
            Environment.Exit(1);
        }

        public void Update(string rightAnswerPlayerOne, string score )
        {
            if (this.ScorePlayerOneLabel.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(Update);
                this.Invoke(d, rightAnswerPlayerOne, score);
            }
            else
            {

                ScorePlayerOneLabel.Text = score;

                //TODO This doesn't seem to function
                Answer = null;
                ConfirmAnswer = false;

                switch (rightAnswerPlayerOne)
                {
                    case "True":
                        RightPlayerOneLabel.Visible = true;
                        WrongPlayerOneLabel.Visible = false;
                        break;
                        
                    case "False":
                        WrongPlayerOneLabel.Visible = true;
                        RightPlayerOneLabel.Visible = false;
                        break;
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
    }
}

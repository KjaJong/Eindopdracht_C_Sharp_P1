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
        public List<int> ScoresOtherPLayers;

        public PlayerForm()
        {
            Answer = null;
            ScoresOtherPLayers = new List<int>();
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

        public void Update(string RightAnswerPlayerOne, string score )
        {
        //    ScorePlayerOneLabel.Text = scores.ElementAt(0).ToString();
        //    ScoresOtherPLayers = scores;
        //TODO This doesn't seem to function
            Answer = null;
            ConfirmAnswer = false;

            YourChoiceLabel.Text = "You choose:";

            switch (RightAnswerPlayerOne)
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
            
        }

        public void UpdateMessageLabel(string text)
        {
            CommentLabel.Text = text;
        }

        public void ResetChoiceLabel()
        {
            YourChoiceLabel.Text = "You choose: ";
        }
    }
}

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
        public List<int> scoresOtherPLayers;

        public PlayerForm()
        {
            Answer = null;
            scoresOtherPLayers = new List<int>();
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

        public void Update(bool RightAnswerPlayerOne, List<int> scores )
        {
            ScorePlayerOneLabel.Text = scores.ElementAt(0).ToString();
            scoresOtherPLayers = scores;
            Answer = null;
            ConfirmAnswer = false;

            switch (RightAnswerPlayerOne)
            {
                case true:
                    RightPlayerOneLabel.Visible = true;
                    WrongPlayerOneLabel.Visible = false;
                    break;

                case false:
                    WrongPlayerOneLabel.Visible = true;
                    RightPlayerOneLabel.Visible = false;
                    break;
            }
            
        }

        public void UpdateMessageLabel(string text)
        {
            CommentLabel.Text = text;
        }
    }
}

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
        public bool ?Answer { get; set; }
        public bool ConfirmAnswer { get; set; }
        public Client Client { get; set; }

        public PlayerForm(Client client)
        {
            Client = client;
            Answer = null;
            InitializeComponent();
        }

        private void EvenButton_Click(object sender, EventArgs e)
        {
            if(!ConfirmAnswer)
            {
                Answer = true;
                YourChoiceLabel.Text = "You choose: even";
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
            Client.sendMessage(ConfirmAnswer);
        }

        private void Leavebutton_Click(object sender, EventArgs e)
        {
            Client.TCPClient.Close();
            Environment.Exit(1);
        }

        public void Update(bool answerPlayerTwo, bool RightAnswerPlayerOne, bool RightAnswerPlayerTwo, int scorePlayerOne, int scorePlayerTwo)
        {
            ScorePlayerOneLabel.Text = scorePlayerOne.ToString();
            ScorePlayerTwoLabel.Text = scorePlayerTwo.ToString();
            Answer = null;
            ConfirmAnswer = false;

            switch (answerPlayerTwo)
            {
                case true:
                    PlayerTwoChoiceLabel.Text = "Player two choose: even";
                    break;

                case false:
                    PlayerTwoChoiceLabel.Text = "Player two choose: odd";
                    break;
            }

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


            switch (RightAnswerPlayerTwo)
            {
                case true:
                    RightPlayerTwoLabel.Visible = true;
                    WrongPlayerTwoLabel.Visible = false;
                    break;

                case false:
                    WrongPlayerTwoLabel.Visible = true;
                    RightPlayerTwoLabel.Visible = false;
                    break;
            }
        }

        public void UpdateMessageLabel(string text)
        {
            CommentLabel.Text = text;
        }
    }
}

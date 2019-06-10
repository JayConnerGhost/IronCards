using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Card:UserControl
    {
        public int ParentLaneId { get; }
        public string CardName { get; }
        public string CardDescription { get; }
        public int CardPoints { get; }

        public Card(int parentLaneId, string cardName, string cardDescription, int cardPoints)
        {
            ParentLaneId = parentLaneId;
            CardName = cardName;
            CardDescription = cardDescription;
            CardPoints = cardPoints;
            BuildCard();

        }

        private void BuildCard()
        {
            var cardBodyLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
            };
            this.BorderStyle = BorderStyle.FixedSingle;


            this.BackColor=Color.AliceBlue;
           
            this.Margin = new Padding(10,20,10,10);
            this.Width = 240;
            this.Height = 100;
            //Name row 
            var nameLabel = new Label() { Text = CardName,Width = 200};
            nameLabel.BackColor=Color.AliceBlue;
            nameLabel.BorderStyle = BorderStyle.None;
            cardBodyLayout.Controls.Add(nameLabel);

            var pointsLayout = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, Size=new Size(200,30)};
            var pointsLabel=new Label(){Text = "Points: ",Width = 40};
            pointsLayout.Controls.Add(pointsLabel);
            var pointsValue=new Label(){Text=CardPoints.ToString(), Width = 25};
            pointsLayout.Controls.Add(pointsValue);
            cardBodyLayout.Controls.Add(pointsLayout);
            this.Controls.Add(cardBodyLayout);
        }
    }
}
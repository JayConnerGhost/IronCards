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
            var cardBodyLayout = new FlowLayoutPanel() {FlowDirection = FlowDirection.TopDown,Dock = DockStyle.Fill};
            cardBodyLayout.Padding = Padding.Add(new Padding(5),new Padding( 5));
            this.BorderStyle = BorderStyle.Fixed3D;


            this.BackColor=Color.AliceBlue;
            this.Margin = new Padding(10,20,10,10);
            this.Width = 240;
            this.Height = 200;
            //Name row 
            var nameLabel = new MetroTextBox() {ReadOnly = true, Text = CardName,Width = 200};
            cardBodyLayout.Controls.Add(nameLabel);
            var pointsLabel=new MetroLabel(){Text = "Points: ",Width = 50};
            cardBodyLayout.Controls.Add(pointsLabel);
            var pointsValue=new MetroTextBox(){Text=CardPoints.ToString(), Width = 20};
            cardBodyLayout.Controls.Add(pointsValue);
            this.Controls.Add(cardBodyLayout);
        }
    }
}
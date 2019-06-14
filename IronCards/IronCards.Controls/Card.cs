using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Card:UserControl
    {
        public int ParentLaneId { get; set; }
        public string CardName { get; }
        public string CardDescription { get; }
        public int CardPoints { get; }
        public int CardId { get; }

        public Card(int parentLaneId, string cardName, string cardDescription, int points, int cardId)
        {
            ParentLaneId = parentLaneId;
            CardName = cardName;
            CardDescription = cardDescription;
            CardPoints = points;
            CardId = cardId;
            BuildCard();
        }


        private void BuildCard()
        {
            var cardBodyLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock = DockStyle.Fill,
            };
            cardBodyLayout.MouseDown += CardBodyLayout_MouseDown;
            this.BorderStyle = BorderStyle.FixedSingle;


            this.BackColor=Color.AliceBlue;
           
            this.Margin = new Padding(10,20,10,10);
            this.Width = 240;
            this.Height = 100;
            //Id
            var IdLabel = new Label()
            {
                Text = CardId.ToString(),
                Width = 25,
                BackColor=Color.AliceBlue,
                BorderStyle = BorderStyle.None
            };
            cardBodyLayout.Controls.Add(IdLabel);
            //Name row 
            var nameLabel = new Label
            {
                Text = CardName, Width = 200, BackColor = Color.AliceBlue, BorderStyle = BorderStyle.None
            };
            cardBodyLayout.Controls.Add(nameLabel);

            var pointsLayout = new FlowLayoutPanel() { FlowDirection = FlowDirection.LeftToRight, Size=new Size(200,30)};
            var pointsLabel=new Label(){Text = "Points: ",Width = 40};
            pointsLayout.Controls.Add(pointsLabel);
            var pointsValue=new Label(){Text=CardPoints.ToString(), Width = 25};
            pointsLayout.Controls.Add(pointsValue);
            cardBodyLayout.Controls.Add(pointsLayout);
            this.Controls.Add(cardBodyLayout);
        }

        private void CardBodyLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }
    }
}
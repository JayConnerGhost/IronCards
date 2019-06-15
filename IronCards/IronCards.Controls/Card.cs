using System;
using System.Drawing;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Card:UserControl
    {
        public int ParentLaneId { get; set; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public int CardPoints { get; set; }
        public int CardId { get; set; }

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
            this.Height = 120;
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
            var controlsLayout = new FlowLayoutPanel
            {
                Size = new Size(235, 30), FlowDirection = FlowDirection.RightToLeft
            };
            var editButton=new Button(){Text = "Edit"};
            editButton.Click += EditButton_Click;
            var viewButton=new Button(){Text = "View"};
            viewButton.Click += ViewButton_Click;
            controlsLayout.Controls.Add(editButton);
            controlsLayout.Controls.Add(viewButton);
            cardBodyLayout.Controls.Add(pointsLayout);
            cardBodyLayout.Controls.Add(controlsLayout);
            this.Controls.Add(cardBodyLayout);
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            EventHandler<CardViewArgs> handler = CardRequestingView;
            handler?.Invoke(this, new CardViewArgs() { CardId = CardId,CardDescription = CardDescription,CardName = CardName, CardPoints = CardPoints});
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            EventHandler<CardEditArgs> handler = CardRequestingEdit;
            handler?.Invoke(this, new CardEditArgs() { Card = this, CardId = CardId });
        }
       
        private void CardBodyLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }

        public event EventHandler<CardViewArgs> CardRequestingView;
        public event EventHandler<CardEditArgs> CardRequestingEdit;
    }

    public class CardViewArgs : EventArgs
    {
        public string CardDescription{get; set; }
        public string CardName{get; set; }
        public int CardPoints{get; set; }
        public int CardId { get; set; }
    }

    public class CardEditArgs : EventArgs
    {
        public Card Card { get; set; }
        public int CardId { get; set; }
    }
}
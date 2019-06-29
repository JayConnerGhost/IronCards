using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IronCards.Dialogs;
using IronCards.Services;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Card : UserControl
    {
        private readonly ToolTip _globalToolTip;
        public int ParentLaneId { get; set; }
        public string CardName { get; set; }
        public string CardDescription { get; set; }
        public int CardPoints { get; set; }
        public int CardId { get; set; }
        public ICardDatabaseService DatabaseService { get; set; }

        private ContextMenuStrip contextMenu;
        public Card(int parentLaneId, string cardName, string cardDescription, int points, int cardId,ToolTip globalToolTip)
        {
            _globalToolTip = globalToolTip;
            ParentLaneId = parentLaneId;
            CardName = cardName;
            CardDescription = cardDescription;
            CardPoints = points;
            CardId = cardId;
            BuildCard();
        }

        private void BuildCard()
        {
            BuildMenu();
            var cardBodyLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown, Dock = DockStyle.Fill, ContextMenuStrip = contextMenu,
            };
            cardBodyLayout.MouseDown += CardBodyLayout_MouseDown;
            
            this.BorderStyle = BorderStyle.FixedSingle;


            this.BackColor = Color.AliceBlue;

            this.Margin = new Padding(10, 20, 10, 10);
            this.Width = 240;
            this.Height = 120;
            //Id
            var IdLabel = new Label()
            {
                Text = CardId.ToString(),
                Width = 25,
                BackColor = Color.AliceBlue,
                BorderStyle = BorderStyle.None
            };
            cardBodyLayout.Controls.Add(IdLabel);
            //Name row 
            var nameLabel = new Label
            {
                Text = CardName, Width = 200, BackColor = Color.AliceBlue, BorderStyle = BorderStyle.None,
                Name = "nameLabel",
                
            };
            _globalToolTip.SetToolTip(nameLabel,CardName);
            cardBodyLayout.Controls.Add(nameLabel);

            var pointsLayout = new FlowLayoutPanel()
                {FlowDirection = FlowDirection.LeftToRight, Size = new Size(200, 30)};
            var pointsLabel = new Label() {Text = "Points: ", Width = 40};
            pointsLayout.Controls.Add(pointsLabel);
            var pointsValue = new Label() {Text = CardPoints.ToString(), Width = 25, Name = "pointsLabel"};
            pointsLayout.Controls.Add(pointsValue);
            var controlsLayout = new FlowLayoutPanel
            {
                Size = new Size(235, 30), FlowDirection = FlowDirection.RightToLeft
            };
            var editButton = new Button() {Text = "Edit"};
            editButton.Click += EditButton_Click;
            var viewButton = new Button() {Text = "View"};
            viewButton.Click += ViewButton_Click;
            controlsLayout.Controls.Add(editButton);
            controlsLayout.Controls.Add(viewButton);
            cardBodyLayout.Controls.Add(pointsLayout);
            cardBodyLayout.Controls.Add(controlsLayout);
            this.Controls.Add(cardBodyLayout);

        }

        private void BuildMenu()
        {
          
            contextMenu = new ContextMenuStrip();
            var deleteCard = new ToolStripButton("Delete", null, DeleteCardOnClick);
            contextMenu.Items.Add(deleteCard);
            this.ContextMenuStrip = contextMenu;
        }

        public void DeleteCardOnClick(object sender, EventArgs e)
        {
            DeleteCard();
        }

        public void DeleteCard()
        {

            DatabaseService.Delete(this.CardId);
            //TODO code in here to delete card from ui.
            this.Dispose();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            new ViewCardDialog().ShowDialog(this.CardName, this.CardDescription, this.CardPoints, this.CardId);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var result =
                new EditCardDialog().ShowDialog(this.CardId, this.CardName, this.CardDescription, this.CardPoints);
            //TODO update card values 
            UpdateValues(result.Item1, result.Item2, CardId, result.Item4);

            //ToDO update database
            var cardDocument = new CardDocument
            {
                Id = CardId,
                CardDescription = CardDescription,
                CardName = CardName,
                CardPoints = CardPoints,
                ParentLaneId = ParentLaneId
            };
            DatabaseService.Update(cardDocument);
        } 

        private void CardBodyLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }


        public void UpdateValues(string cardName, string cardDescription, int Id, int cardPoints)
        {
            this.CardId = Id;
            this.CardName = cardName;
            this.CardDescription = cardDescription;
            this.CardPoints = cardPoints;
            UpdateUi();
        }

        private void UpdateUi()
        {
            var nameLabel = (Label) this.Controls.Find("nameLabel", true).First();
            nameLabel.Text = CardName;
            _globalToolTip.SetToolTip(nameLabel,CardName);
            var pointsLabel = (Label) this.Controls.Find("pointsLabel", true).First();
            pointsLabel.Text = CardPoints.ToString();

        }
    }

   
}
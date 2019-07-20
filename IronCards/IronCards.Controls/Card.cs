using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using IronCards.Dialogs;
using IronCards.Objects;
using IronCards.Services;
using MetroFramework.Controls;

namespace IronCards.Controls
{
    public class Card : UserControl
    {
        class Data
        {
          
            public int ParentLaneId { get; set; }
            public string CardName { get; set; }
            public string CardDescription { get; set; }
            public int CardPoints { get; set; }
            public int CardId { get; set; }

            public CardTypes CardType { get; set; }
        }

        public ICardDatabaseService DatabaseService { get; set; }
        private readonly ToolTip _globalToolTip;
        private ContextMenuStrip contextMenu;
        private Data cardData = new Data(); 
        public Card(int parentLaneId, string cardName, string cardDescription, int points, int cardId,ToolTip globalToolTip, CardTypes cardType)
        {
            _globalToolTip = globalToolTip;
            cardData.ParentLaneId = parentLaneId;
            cardData.CardName = cardName;
            cardData.CardDescription = cardDescription;
            cardData.CardPoints = points;
            cardData.CardId = cardId;
            cardData.CardType = cardType;
            BuildCard(cardData.CardType);
        }

        private void BuildCard(CardTypes cardType)
        {
            BuildMenu();
            var cardBodyLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown, Dock = DockStyle.Fill, ContextMenuStrip = contextMenu,
            };
            cardBodyLayout.MouseDown += CardBodyLayout_MouseDown;
            
            this.BorderStyle = BorderStyle.FixedSingle;


            SetBackColor(this, cardType);


            this.Margin = new Padding(10, 20, 10, 10);
            this.Width = 240;
            this.Height = 120;
            //Id
            var IdLabel = new Label()
            {
                Text = cardData.CardId.ToString(),
                Width = 25,
           
                BorderStyle = BorderStyle.None
            };
            cardBodyLayout.Controls.Add(IdLabel);
            //Name row 
            var nameLabel = new Label
            {
                Text = cardData.CardName, Width = 200,  BorderStyle = BorderStyle.None,
                Name = "nameLabel",
                
            };
            _globalToolTip.SetToolTip(nameLabel, cardData.CardName);
            cardBodyLayout.Controls.Add(nameLabel);

            var propertiesLayout = new FlowLayoutPanel()
                {FlowDirection = FlowDirection.LeftToRight, Size = new Size(200, 30)};
            var pointsLabel = new Label() {Text = "Points: ", Width = 40};
            propertiesLayout.Controls.Add(pointsLabel);
            var pointsValue = new Label() {Text = cardData.CardPoints.ToString(), Width = 25, Name = "pointsLabel"};
            propertiesLayout.Controls.Add(pointsValue);

       
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
            cardBodyLayout.Controls.Add(propertiesLayout);
            cardBodyLayout.Controls.Add(controlsLayout);
            this.Controls.Add(cardBodyLayout);

        }

        private void SetBackColor(Card card, CardTypes cardType)
        {
            card.BackColor=CardTypesUtilities.GetColor(cardType);
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

            DatabaseService.Delete(this.cardData.CardId);
            //TODO code in here to delete card from ui.
            this.Dispose();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            //TODO : - > add in card type 
            new ViewCardDialog().ShowDialog(this.cardData.CardName, this.cardData.CardDescription, this.cardData.CardPoints, this.cardData.CardId, this.cardData.CardType);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var result =
                new EditCardDialog().ShowDialog(this.cardData.CardId, this.cardData.CardName, this.cardData.CardDescription, this.cardData.CardPoints, this.cardData.CardType);
            //TODO update card values 
            UpdateValues(result.Item1, result.Item2, cardData.CardId, result.Item4,result.Item6);

            //ToDO update database
            var cardDocument = new CardDocument
            {
                Id = cardData.CardId,
                CardDescription = cardData.CardDescription,
                CardName = cardData.CardName,
                CardPoints = cardData.CardPoints,
                ParentLaneId = cardData.ParentLaneId,
                CardType= cardData.CardType.ToString()
            };
            DatabaseService.Update(cardDocument);
        } 

        private void CardBodyLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }


        public void UpdateValues(string cardName, string cardDescription, int Id, int cardPoints, string cardType)
        {
            this.cardData.CardId = Id;
            this.cardData.CardName = cardName;
            this.cardData.CardDescription = cardDescription;
            this.cardData.CardPoints = cardPoints;
            switch (cardType)
            {
                case "Idea":
                    this.cardData.CardType = CardTypes.Idea;
                    break;
                case "Requirement":
                    this.cardData.CardType = CardTypes.Requirement;
                    break;
                case "ExternalRequirement":
                    this.cardData.CardType = CardTypes.ExternalRequirement;
                    break;
                case "Bug":
                    this.cardData.CardType = CardTypes.Bug;
                    break;
            }
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            var nameLabel = (Label) this.Controls.Find("nameLabel", true).First();
            nameLabel.Text = cardData.CardName;
            _globalToolTip.SetToolTip(nameLabel, cardData.CardName);
            var pointsLabel = (Label) this.Controls.Find("pointsLabel", true).First();
            pointsLabel.Text = cardData.CardPoints.ToString();

            this.BackColor=CardTypesUtilities.GetColor(cardData.CardType);
            
        }
    }
}
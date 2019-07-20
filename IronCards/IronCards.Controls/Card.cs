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
            public int FeatureId { get; set; }
            public string FeatureName { get; set; }
            public int ParentLaneId { get; set; }
            public string CardName { get; set; }
            public string CardDescription { get; set; }
            public int CardPoints { get; set; }
            public int CardId { get; set; }
            public CardTypes CardType { get; set; }
        }
        public int FeatureId
        {
            get => _cardData.FeatureId;
            set => _cardData.FeatureId = value;
        }
        public string FeatureName
        {
            get => _cardData.FeatureName;
            set => _cardData.FeatureName = value;
        }
        public int CardPoints
        {
            get => _cardData.CardPoints;
            set => _cardData.CardPoints = value;
        }

        public int ParentLaneId
        {
            get => _cardData.ParentLaneId;
            set => _cardData.ParentLaneId = value;
        }
        public int Id
        {
            get => _cardData.CardId;
            set => _cardData.CardId = value;
        }
        public string CardName
        {
            get => _cardData.CardName;
            set => _cardData.CardName = value;
        }

        public string CardDescription
        {
            get => _cardData.CardDescription;
            set => _cardData.CardDescription = value;
        }

        public CardTypes CardType
        {
            get => _cardData.CardType;
            set => _cardData.CardType = value;
        }

        public ICardDatabaseService DatabaseService { get; set; }
        private readonly ToolTip _globalToolTip;
        private ContextMenuStrip contextMenu;
        private readonly Data _cardData = new Data(); 
        public Card(int parentLaneId, string cardName, string cardDescription, int points, int cardId,ToolTip globalToolTip, CardTypes cardType, int featureId, string featureName)
        {
            _globalToolTip = globalToolTip;
            _cardData.FeatureId = featureId;
            _cardData.FeatureName = featureName;
            _cardData.ParentLaneId = parentLaneId;
            _cardData.CardName = cardName;
            _cardData.CardDescription = cardDescription;
            _cardData.CardPoints = points;
            _cardData.CardId = cardId;
            _cardData.CardType = cardType;
            BuildCard(_cardData);
        }

    

        private void BuildCard(Data data)
        {
            BuildMenu();
            var cardBodyLayout = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown, Dock = DockStyle.Fill, ContextMenuStrip = contextMenu,
            };
            cardBodyLayout.MouseDown += CardBodyLayout_MouseDown;
            
            this.BorderStyle = BorderStyle.FixedSingle;


            SetBackColor(this, data.CardType);


            this.Margin = new Padding(10, 20, 10, 10);
            this.Width = 240;
            this.Height = 120;
            //Id
            var IdLabel = new Label()
            {
                Text = data.CardId.ToString(),
                Width = 25,
           
                BorderStyle = BorderStyle.None
            };
            cardBodyLayout.Controls.Add(IdLabel);
            //Name row 
            var nameLabel = new Label
            {
                Text = data.CardName, Width = 200,  BorderStyle = BorderStyle.None,
                Name = "nameLabel",
                
            };
            _globalToolTip.SetToolTip(nameLabel, data.CardName);
            cardBodyLayout.Controls.Add(nameLabel);

            var propertiesLayout = new FlowLayoutPanel()
                {FlowDirection = FlowDirection.LeftToRight, Size = new Size(200, 30)};
            var pointsLabel = new Label() {Text = "Points: ", Width = 40};
            propertiesLayout.Controls.Add(pointsLabel);
            var pointsValue = new Label() {Text = data.CardPoints.ToString(), Width = 25, Name = "pointsLabel"};
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

            DatabaseService.Delete(this._cardData.CardId);
            //TODO code in here to delete card from ui.
            this.Dispose();
        }

        private void ViewButton_Click(object sender, EventArgs e)
        {
            //TODO : - > add in card type 
            new ViewCardDialog().ShowDialog(this._cardData.CardName, this._cardData.CardDescription, this._cardData.CardPoints, this._cardData.CardId, this._cardData.CardType);
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var result =
                new EditCardDialog().ShowDialog(this._cardData.CardId, this._cardData.CardName, this._cardData.CardDescription, this._cardData.CardPoints, this._cardData.CardType);
            //TODO update card values 
            UpdateValues(result.Item1, result.Item2, _cardData.CardId, result.Item4,result.Item6);

            //ToDO update database
            var cardDocument = new CardDocument
            {
                Id = _cardData.CardId,
                CardDescription = _cardData.CardDescription,
                CardName = _cardData.CardName,
                CardPoints = _cardData.CardPoints,
                ParentLaneId = _cardData.ParentLaneId,
                CardType= _cardData.CardType.ToString()
            };
            DatabaseService.Update(cardDocument);
        } 

        private void CardBodyLayout_MouseDown(object sender, MouseEventArgs e)
        {
            this.DoDragDrop(this, DragDropEffects.Move);
        }


        public void UpdateValues(string cardName, string cardDescription, int Id, int cardPoints, string cardType)
        {
            this._cardData.CardId = Id;
            this._cardData.CardName = cardName;
            this._cardData.CardDescription = cardDescription;
            this._cardData.CardPoints = cardPoints;
            switch (cardType)
            {
                case "Idea":
                    this._cardData.CardType = CardTypes.Idea;
                    break;
                case "Requirement":
                    this._cardData.CardType = CardTypes.Requirement;
                    break;
                case "ExternalRequirement":
                    this._cardData.CardType = CardTypes.ExternalRequirement;
                    break;
                case "Bug":
                    this._cardData.CardType = CardTypes.Bug;
                    break;
            }
            
            UpdateUi();
        }

        private void UpdateUi()
        {
            var nameLabel = (Label) this.Controls.Find("nameLabel", true).First();
            nameLabel.Text = _cardData.CardName;
            _globalToolTip.SetToolTip(nameLabel, _cardData.CardName);
            var pointsLabel = (Label) this.Controls.Find("pointsLabel", true).First();
            pointsLabel.Text = _cardData.CardPoints.ToString();

            this.BackColor=CardTypesUtilities.GetColor(_cardData.CardType);
            
        }
    }
}
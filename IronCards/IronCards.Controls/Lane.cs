using System;
using System.Configuration;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Security.Permissions;
using System.Windows.Forms;
using IronCards.Dialogs;
using IronCards.Services;
using MetroFramework.Components;
using MetroFramework.Controls;
using MetroFramework.Drawing;

namespace IronCards.Controls
{
    public class Lane:BaseControl
    {
        private readonly ICardDatabaseService _cardDatabaseService;
        private readonly ToolTip _globalToolTip;
        public int ProjectId { get; set; }
        private FlowLayoutPanel _cardContainer;

        enum TextChangedValue
        {
            Changed,
            Unchanged
        }
        public int Id { get; set; }

       
            

        public Lane(string laneLabel, ICardDatabaseService cardDatabaseService, ToolTip globalToolTip, int projectId)
        {
            _cardDatabaseService = cardDatabaseService;
            _globalToolTip = globalToolTip;
            this.ProjectId = projectId;
            BuildLane(laneLabel);
        
        }

        

        private void BuildLane(string laneLabel)
        {
            BorderStyle = BorderStyle.FixedSingle;

            Width = 290;

            Controls.Add(BuildLabel(laneLabel));
            var cardContainer = BuildCardContainer();
            Controls.Add(cardContainer);
            BuildsContextMenu(this);
        }

        private Control BuildCardContainer()
        {
           
            _cardContainer = new FlowLayoutPanel
            {
                FlowDirection = FlowDirection.TopDown,
                Dock=DockStyle.Fill,
                Padding = new Padding(10,10,10,10),
                WrapContents = false,
                AutoScroll = true
            };
            BorderStyle = BorderStyle.FixedSingle;
            _cardContainer.AllowDrop = true;
            _cardContainer.DragEnter += _cardContainer_DragEnter;
            _cardContainer.DragLeave += _cardContainer_DragLeave;
            _cardContainer.DragDrop += _cardContainer_DragDrop;
            return _cardContainer;
        }

        private void _cardContainer_DragDrop(object sender, DragEventArgs e)
        {
            var target = (Card)e.Data.GetData(typeof(Card));
        }

        private void _cardContainer_DragLeave(object sender, EventArgs e)
        {
           //issue to be solved 
        }

        private void _cardContainer_DragEnter(object sender, DragEventArgs e)
        {
            var target = (Card) e.Data.GetData(typeof(Card));
           
            this.AddCard(target);
      
            //LaneRequestingEditCardLane
            EventHandler<EditCardLaneArgs> handler = LaneRequestingEditCardLane;
            handler?.Invoke(this, new EditCardLaneArgs() { NewLaneId = this.Id, target=target});

        }

        private void BuildsContextMenu(UserControl lane)
        {
            var laneContextMenu=new ContextMenuStrip();
            var deleteButton = new ToolStripButton("Delete Lane",null,OnDeleteClick);
            laneContextMenu.Items.Add(deleteButton);
            var addLaneButton =new ToolStripButton("Insert Lane",null,OnAddLaneClick);
            laneContextMenu.Items.Add(addLaneButton);
            var addCardButton = new ToolStripButton("Insert Card", null, OnAddCardClick);
            laneContextMenu.Items.Add(addCardButton);
            lane.ContextMenuStrip=laneContextMenu;
         

        }

      

        private void OnAddCardClick(object sender, EventArgs e)
        {
            EventHandler<AddCardArgs> handler = LaneRequestingAddCard;
            handler?.Invoke(this, new AddCardArgs(){LaneId = Id,Target = this});
        }

        private void OnAddLaneClick(object sender, EventArgs e)
        {
            EventHandler<LaneAddArgs> handler = LaneRequestingAddLane;
            handler?.Invoke(this, new LaneAddArgs(){LaneName = "newLane",ProjectId = ProjectId,ProjectName = ProductName});
        }

        private void OnDeleteClick(object sender, EventArgs e)
        {

            EventHandler<LaneDeleteArgs> handler = LaneRequestingDelete;
            handler?.Invoke(this, new LaneDeleteArgs() { LaneId = Id,Target =this});
        }

        private Control BuildLabel(string laneLabel)
        {
            var label = new MetroTextBox() {Text = laneLabel,Width = 200,ReadOnly = true,Dock = DockStyle.Top};
            label.Click += Label_Click;
            label.Leave += Label_Leave;
            label.TextChanged += Label_TextChanged;
            return label;
        }

        private void Label_TextChanged(object sender, System.EventArgs e)
        {
            ((MetroTextBox) (sender)).Tag = TextChangedValue.Changed;
        }

        private void Label_Leave(object sender, System.EventArgs e)
        {
            try
            {
                var textBox = ((MetroTextBox) sender);
            if ((TextChangedValue)textBox.Tag == TextChangedValue.Changed)
            {
               
                    EventHandler<LaneTitleEditedArgs> handler = LaneRequestingTitleChanged;
                    handler?.Invoke(this, new LaneTitleEditedArgs() { LaneId = Id, NewTitle = textBox.Text.Trim(),ProjectId = ProjectId});
                    //raise event passing out new Args 
               
            }
            textBox.Tag = TextChangedValue.Unchanged;

            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                //Ignore exception 
            }

        }

        private void Label_Click(object sender, System.EventArgs e)
        {
            ((MetroTextBox) (sender)).ReadOnly = false;
        }

        public void DeleteAllCardsInLane()
        {
            var containerControls = _cardContainer.Controls;


            foreach (Card card in containerControls)
            {
                card.DeleteCard();
                _cardDatabaseService.Delete(card.Id);
            }
        }

        public void AddCard(Card card)
        {
            //If null add event handlers
            card.DatabaseService = _cardDatabaseService;
            card.Name = card.Id.ToString();
            _cardContainer.Controls.Add(card);
        }

       
        public event EventHandler<LaneTitleEditedArgs> LaneRequestingTitleChanged;
        public event EventHandler<LaneDeleteArgs> LaneRequestingDelete;
        public event EventHandler<LaneAddArgs> LaneRequestingAddLane;
        public event EventHandler<AddCardArgs> LaneRequestingAddCard;
        public event EventHandler<EditCardLaneArgs> LaneRequestingEditCardLane;
 

    }

    

    public class EditCardLaneArgs:EventArgs
    {
        public int NewLaneId { get; set; }
        public Card target { get; set; }
    }

    public class LaneTitleEditedArgs : EventArgs
    {
        public int LaneId { get; set; }
        public string NewTitle { get; set; }
        public int ProjectId { get; set; }
    }

    public class LaneDeleteArgs : EventArgs
    {
        public Lane Target { get; set; }
        public int LaneId { get; set; }
    }

    public class LaneAddArgs : EventArgs
    {
        public string ProjectName { get; set; }
        public int ProjectId { get; set; }
        public string LaneName { get; set; }
    }

    public class AddCardArgs : EventArgs
    {
        public Lane Target { get; set; }
        public int LaneId { get; set; }
    }

 
}


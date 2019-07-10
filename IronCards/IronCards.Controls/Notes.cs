using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OutlookStyleControls;

namespace IronCards.Controls
{
    public partial class Notes : UserControl
    {
        private SplitterPanel _editor;
        private SplitterPanel _grid;

        public Notes()
        {
            InitializeComponent();
            BuildOuterContainer();
            BuildGrid(_grid);
            BuildNoteEditor(_editor);
        }

        private void BuildNoteEditor(SplitterPanel editor)
        {
            editor.AutoScroll = true;
           var editorTableLayout=new TableLayoutPanel();
           editorTableLayout.AutoScroll = true;
           editorTableLayout.Dock = DockStyle.Fill;
           editorTableLayout.RowCount = 3;
           editorTableLayout.ColumnCount = 2;
           editorTableLayout.BorderStyle = BorderStyle.Fixed3D;

           var titleLabel=new Label(){Text="Note Title"};
           var titleTextbox=new TextBox(){Width=200};

           //TODO - flow layout panel for the next two controls
            var titleLayout=new FlowLayoutPanel(){Width = 500,Height=50};
        
            titleLayout.Controls.Add(titleLabel);
            titleLayout.Controls.Add(titleTextbox);
           editorTableLayout.Controls.Add(titleLayout,0,0);
           var descriptionLabel = new Label() {Text = "Note Description"};
           var descriptionTextBox=new TextBox(){Multiline = true, Width = 500,Height=300,ScrollBars = ScrollBars.Vertical};
           editorTableLayout.Controls.Add(descriptionLabel,0,1);
           editorTableLayout.Controls.Add(descriptionTextBox,0,2);
           var saveButton=new Button(){Text = "Save Note", Anchor =( AnchorStyles.Right | AnchorStyles.Top), Height = 30};
            saveButton.Click += delegate(object o, EventArgs args)
                {
                    SaveNewEntry(titleTextbox.Text, descriptionTextBox.Text);
                };
           var cancelButton=new Button(){Text = "Cancel Edit", Anchor =( AnchorStyles.Right | AnchorStyles.Top), Height = 30};
            cancelButton.Click += CancelButton_Click;

           var buttonsLayout=new FlowLayoutPanel(){FlowDirection = FlowDirection.RightToLeft,Width=500};
           buttonsLayout.Controls.Add(saveButton);
           buttonsLayout.Controls.Add(cancelButton);
            editorTableLayout.Controls.Add(buttonsLayout, 0,3);
           
           editor.Controls.Add(editorTableLayout);
        }

        private void SaveNewEntry(string title, string description)
        {
            //TODO: Implementation to save to db and add to grid 
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void BuildGrid(SplitterPanel grid)
        {
            //ConfigureGrid here
             OutlookGrid notesGrid=new OutlookGrid();
            // notesGrid.
             notesGrid.Dock = DockStyle.Fill;
             grid.Controls.Add(notesGrid);
        }
       
        private void BuildOuterContainer()
        {
            var splitContainer = new SplitContainer {Orientation = Orientation.Horizontal,Dock=DockStyle.Fill};
            splitContainer.Panel1.Name = "NotesEditor";
            splitContainer.Panel2.Name = "Grid";
            splitContainer.VerticalScroll.Enabled=true;
            _editor = splitContainer.Panel1;
            _grid = splitContainer.Panel2;
            this.Controls.Add(splitContainer);
        }
    }
}

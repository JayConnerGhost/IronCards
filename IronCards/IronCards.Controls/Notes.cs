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
using IronCards.Services;
using OutlookStyleControls;

namespace IronCards.Controls
{
    public partial class Notes : BaseControl
    {
        private readonly INotesDatabaseService _notesDatabaseService;
        private readonly int _projectId;
        private SplitterPanel _editor;
        private SplitterPanel _grid;

        public Notes(INotesDatabaseService notesDatabaseService,int projectId)
        {
            _notesDatabaseService = notesDatabaseService;
            _projectId = projectId;
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
           var titleTextBox=new TextBox(){Width=200};

           //TODO - flow layout panel for the next two controls
            var titleLayout=new FlowLayoutPanel(){Width = 500,Height=50};
        
            titleLayout.Controls.Add(titleLabel);
            titleLayout.Controls.Add(titleTextBox);
           editorTableLayout.Controls.Add(titleLayout,0,0);
           var descriptionLabel = new Label() {Text = "Note Description"};
           var descriptionTextBox=new TextBox(){Multiline = true, Width = 500,Height=300,ScrollBars = ScrollBars.Vertical};
           editorTableLayout.Controls.Add(descriptionLabel,0,1);
           editorTableLayout.Controls.Add(descriptionTextBox,0,2);
           var saveButton=new Button(){Text = "Save Note", Anchor =( AnchorStyles.Right | AnchorStyles.Top), Height = 30};
            saveButton.Click += delegate(object o, EventArgs args)
                {
                    SaveNewEntry(titleTextBox.Text, descriptionTextBox.Text);
                    ResetControls(titleTextBox, descriptionTextBox);
                };
           var cancelButton=new Button(){Text = "Cancel Edit", Anchor =( AnchorStyles.Right | AnchorStyles.Top), Height = 30};
            cancelButton.Click += CancelButton_Click;

           var buttonsLayout=new FlowLayoutPanel(){FlowDirection = FlowDirection.RightToLeft,Width=500};
           buttonsLayout.Controls.Add(saveButton);
           buttonsLayout.Controls.Add(cancelButton);
            editorTableLayout.Controls.Add(buttonsLayout, 0,3);
            editor.Controls.Add(editorTableLayout);
        }

        private void ResetControls(TextBox titleTextBox, TextBox descriptionTextBox)
        {
            titleTextBox.Text = string.Empty;
            descriptionTextBox.Text = string.Empty;
        }

        private void SaveNewEntry(string title, string description)
        {
            int Id = _notesDatabaseService.Insert(title, description, _projectId);
            NotesGridUpdate(Id,title,description);
            
        }

        private void NotesGridUpdate(object id, string title, string description)
        {
            //throw new NotImplementedException();
            //TODO: once grid in place insert new record at the bottom of exsisting records 
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
        var data = _notesDatabaseService.GetAllForProject(_projectId);
        notesGrid.BindData(data,"Note");
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

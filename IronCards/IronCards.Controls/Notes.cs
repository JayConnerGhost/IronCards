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
        private ErrorProvider _newErrorProvider;
        private ListView _listView;
        public Notes(INotesDatabaseService notesDatabaseService,int projectId)
        {
    
            _newErrorProvider = new ErrorProvider();
            _notesDatabaseService = notesDatabaseService;
            _projectId = projectId;
            InitializeComponent();
            BuildOuterContainer();
            BuildList(_grid);
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
        //TODO add empty validatorrs fortextbox controls
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
                bool executeOperation = true;
                    if (descriptionTextBox.Text.Length < 1)
                    {
                        _newErrorProvider.SetError(descriptionTextBox,"Please enter a full description for the note");
                        executeOperation = false;
            
                    }
                    else
                    {
                        _newErrorProvider.SetError(descriptionTextBox,string.Empty);
                    }

                    if (titleTextBox.Text.Length < 1)
                    {
                        _newErrorProvider.SetError(titleTextBox, "Please enter a full title for the note");
                        executeOperation = false;
             

                    }
                    else
                    {
                        _newErrorProvider.SetError(titleTextBox, string.Empty);
                    }

                    if (executeOperation)
                    {
                        SaveNewEntry(titleTextBox.Text, descriptionTextBox.Text);
                        ResetControls(titleTextBox, descriptionTextBox);
                     
                    }
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
            NotesListUpdate(Id.ToString(),title,description);
            
        }

        private void NotesListUpdate(string id, string title, string description)
        {
            _listView.Items.Add(id, title, null);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }


        private void BuildList(SplitterPanel list)
        {
            var horzontalListSplitter = new SplitContainer
            {
                Orientation = Orientation.Vertical, SplitterDistance = 5, Dock = DockStyle.Fill
            };
             _listView = new ListView {Dock = DockStyle.Fill, View = View.List};
           var data = _notesDatabaseService.GetAllForProject(_projectId);
           foreach (var noteDocument in data)
           {
               _listView.Items.Add(noteDocument.Id.ToString(), noteDocument.Title, null);
           }

           var bodyTextBox=new TextBox(){Multiline = true,Dock = DockStyle.Fill, ReadOnly = true,ScrollBars = ScrollBars.Vertical};


           _listView.ItemSelectionChanged += delegate(object o, ListViewItemSelectionChangedEventArgs args)
            {
                if (!args.IsSelected)
                {
                    return;
                }
                var noteId=args.Item.Name;
                var noteText = _notesDatabaseService.FindNoteTextByNoteId(noteId);
                bodyTextBox.Text = noteText;
            };
           horzontalListSplitter.Panel1.Controls.Add(_listView);
           horzontalListSplitter.Panel2.Controls.Add(bodyTextBox);
            list.Controls.Add(horzontalListSplitter);

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

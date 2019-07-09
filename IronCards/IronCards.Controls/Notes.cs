﻿using System;
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
           var editorTableLayout=new TableLayoutPanel();
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
           var descriptionTextBox=new TextBox(){Multiline = true, Width = 500,Height=300};
           editorTableLayout.Controls.Add(descriptionLabel,0,1);
           editorTableLayout.Controls.Add(descriptionTextBox,0,2);

           editor.Controls.Add(editorTableLayout);
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
            _editor = splitContainer.Panel1;
            _grid = splitContainer.Panel2;
            this.Controls.Add(splitContainer);
        }
    }
}

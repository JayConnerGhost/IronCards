using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace IronCards.Controls
{
    public partial class Attachments : BaseControl
    {
        private readonly int _projectId;
        readonly ListView _fileList = new ListView() { Width = 450, Height = 450 };
        string[] _fileNames;
        string attachmentPath;
        public Attachments(int projectId) : base()
        {
            _projectId = projectId;
            InitializeComponent();
            GetAttachmentsDirectory();
            CreateUploadTrigger();
         
        }

        private void CreateUploadTrigger()
        {
            var layout = new FlowLayoutPanel();
            layout.FlowDirection = FlowDirection.TopDown;
            layout.Controls.Add(BuildFileUploadControl());
            layout.Controls.Add(BuildFileListControl());
            layout.Dock = DockStyle.Fill;
            this.Controls.Add(layout);
        }

        private void LoadAttachments()
        {
            _fileList.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(attachmentPath + @"\" + _projectId);
            FileInfo[] files = d.GetFiles("*.*");
            foreach (var file in files)
            {
                ListViewItem fileItem = new ListViewItem(file.Name, 0) { Tag = file.FullName };
                _fileList.Items.Add(fileItem);
            }
        }

        private FlowLayoutPanel BuildFileListControl()
        {
            var fileListViewLayout = new FlowLayoutPanel();
            fileListViewLayout.FlowDirection = FlowDirection.TopDown;
        
            _fileList.FullRowSelect = true;
            _fileList.MultiSelect = false;
            _fileList.View = View.List;
            

            _fileList.ItemSelectionChanged += _fileList_ItemSelectionChanged;

            LoadAttachments();
            
            _fileList.Anchor = AnchorStyles.Top;
            fileListViewLayout.Controls.Add(_fileList);
            fileListViewLayout.Dock = DockStyle.Top;
            fileListViewLayout.Height = 500;
            fileListViewLayout.Width = 600;
            fileListViewLayout.Anchor = AnchorStyles.Top;
            fileListViewLayout.AutoScroll = true;
            return fileListViewLayout;
        }

        private void _fileList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                Process.Start((string) e.Item.Tag);
            }
        }

        private FlowLayoutPanel BuildFileUploadControl()
        {

            var fileUploadLayout = new FlowLayoutPanel();
            fileUploadLayout.FlowDirection = FlowDirection.LeftToRight;
            fileUploadLayout.BorderStyle = BorderStyle.FixedSingle;
            fileUploadLayout.Width = 200;
            fileUploadLayout.Height = 50;
            fileUploadLayout.WrapContents = false;
            var fileUploadLabel = new Label() { Text = "Upload File", Height = 25, Width = 80 };
            fileUploadLayout.Controls.Add(fileUploadLabel);
            var fileupLoadButton = new Button() { Text = "Choose File", Height = 25, Width = 100 };
            fileupLoadButton.Click += FileUpLoadButton_Click;
            fileUploadLayout.Controls.Add(fileupLoadButton);
            return fileUploadLayout;
        }

        private void FileUpLoadButton_Click(object sender, EventArgs e)
        {
            var fileOpenDialog = new OpenFileDialog();
            fileOpenDialog.Multiselect = true;
            var fileResult = fileOpenDialog.ShowDialog();
 
            if (fileResult == DialogResult.OK)
            {
                foreach (var file in fileOpenDialog.FileNames)
                {
                    var filePathName = new DirectoryInfo(file.ToString()).Name;
                    File.Copy(file.ToString(), attachmentPath + "/"+_projectId.ToString()+"/" + filePathName);
                }
            }

            LoadAttachments();
        }

        private void GetAttachmentsDirectory()
        {
            var configSettingsReader = new AppSettingsReader();
            attachmentPath = (string)configSettingsReader.GetValue("AttachmentPath", typeof(string));
            if (!Directory.Exists(attachmentPath+"/"+_projectId))
            {
                Directory.CreateDirectory(attachmentPath + "/" + _projectId);
            }

        }
    }
}

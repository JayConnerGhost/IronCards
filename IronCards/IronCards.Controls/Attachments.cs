using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Drawing;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IronCards.Controls
{
    public partial class Attachments : BaseControl
    {
        string[] _fileNames;
        string attachmentPath;
        public Attachments():base()
        {
            InitializeComponent();
            GetAttachmentsDirectory();
            CreateUploadTrigger();
           // LoadAttachments();
        }

        private void CreateUploadTrigger()
        {
            var layout =new FlowLayoutPanel();
            layout.FlowDirection = FlowDirection.TopDown;
            var fileUploadLayout = new FlowLayoutPanel();
            fileUploadLayout.FlowDirection = FlowDirection.LeftToRight;
            fileUploadLayout.Width = 200;
            fileUploadLayout.WrapContents = false;
            var fileUploadLabel = new Label() {Text = "Upload File",Height=25, Width=80};
            fileUploadLayout.Controls.Add(fileUploadLabel);
            var fileupLoadButton=new Button(){Text="Upload", Height=25 , Width=100};
            fileupLoadButton.Click += FileUpLoadButton_Click;
            fileUploadLayout.Controls.Add(fileupLoadButton);
            layout.Controls.Add(fileUploadLayout);
            layout.Dock = DockStyle.Fill;
            this.Controls.Add(layout);


        }

        private void FileUpLoadButton_Click(object sender, EventArgs e)
        {
            var fileOpenDialog= new OpenFileDialog();
            fileOpenDialog.Multiselect = true;
            var fileResult=fileOpenDialog.ShowDialog();
            //Handle file upload
            if (fileResult == DialogResult.OK)
            {
                foreach (var file in fileOpenDialog.FileNames)
                {
                    string filepathName=new DirectoryInfo(file.ToString()).Name;
                    File.Copy(file.ToString(),attachmentPath+"/"+ filepathName);
                }
            }
            //TODO: Reindex list of files 
        }

        private void GetAttachmentsDirectory()
        {
            var configSettingsReader= new AppSettingsReader();
            attachmentPath = (string)configSettingsReader.GetValue("AttachmentPath",typeof(string));
            if (!Directory.Exists(attachmentPath))
            {
                Directory.CreateDirectory(attachmentPath);
            }
     
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ArkaliaCore.Tool
{
    public partial class EditorForm : Form
    {
        public Kernel.KernelFile CurrentEditedKernel { get; set; }

        public EditorForm()
        {
            InitializeComponent();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (CurrentEditedKernel != null)
            {
                var editor = new MapFlagEditorForm();
                editor.MdiParent = this;
                editor.Show();
            }
        }

        private void chargerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dialog = new OpenFileDialog();
            dialog.Title = "Selectionner le kernel";
            dialog.Filter = "Arkalia Kernel (.ark)|*.ark|All Files (*.*)|*.*";
            dialog.ShowDialog();
            var path = dialog.FileName;
            if (path != "")
            {

            }
        }

        private void nouveauToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CurrentEditedKernel = new Kernel.KernelFile();
        }

        private void sauvegarderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.CurrentEditedKernel != null)
            {
                var dialog = new SaveFileDialog();
                dialog.Title = "Selectionner l'emplacement de sauvegarde";
                dialog.Filter = "Arkalia Kernel (.ark)|*.ark|All Files (*.*)|*.*";
                dialog.ShowDialog();
                var path = dialog.FileName;
                if (path != "")
                {
                    this.CurrentEditedKernel.Path = path;
                    this.CurrentEditedKernel.Save();
                }
            }
        }
    }
}

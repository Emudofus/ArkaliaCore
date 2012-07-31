using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ArkaliaCore.Tool
{
    public static class Program
    {

        public static EditorForm Editor { get; set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Editor = new EditorForm();
            Application.Run(Editor);
        }
    }
}

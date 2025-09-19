using System.ComponentModel;
using System.Windows.Forms;

namespace Core.UI
{
    public partial class WarningDialog : Form
    {
        public WarningDialog()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        [Description("Content"), Category("Custom")]
        public string Content
        {
            get => ctrLabelContent.Text;
            set => ctrLabelContent.Text = value;
        }
    }
}

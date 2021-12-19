using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Slagalica
{
    /// <summary>
    /// Interaction logic for NameInputDialog.xaml
    /// </summary>
    public partial class NameInputDialog : Window
    {
        private const string ERROR_MSG = "Name already exists!";
        private const string LBL_MSG = "You won! If you want to be ranked enter a name:";
        private const string NAME_MISS = "Please provide name";

        public NameInputDialog()
        {
            InitializeComponent();
            lblMsg.Content = LBL_MSG;
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            if ("".Equals(NameInput))
            {
                lblError.Content = NAME_MISS;
                return;
            }

            //check if name exists
            if (!Player.CheckIfNameExists(NameInput))
            {
                this.DialogResult = true;
            }
            else
            {
                lblError.Content = ERROR_MSG;
            }
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            inputName.SelectAll();
            inputName.Focus();
        }

        public string NameInput
        {
            get { return inputName.Text; }
        }
    }
}

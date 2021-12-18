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
        private string ERROR_MSG = "Name already exists!";

        public NameInputDialog()
        {
            InitializeComponent();
        }
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
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

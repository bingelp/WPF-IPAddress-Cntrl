using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IPAddressCntrl
{
/// <summary>
    /// Interaction logic for IPAddressControl.xaml
    /// </summary>
    public partial class IPAddressControl : UserControl
    {

        #region Constructor
        /// <summary>
        ///  Constructor for the control.
        /// </summary>
        public IPAddressControl()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(IPAddressControl_Loaded);
            txtboxFirstOctet.GotKeyboardFocus += txtboxFirstOctet_GotKeyboardFocus;
        }
        #endregion

        #region Private Variables
        private const string ErrorGeneralMessage = "An Error occured in the control.";
        private const string ErrorInputNotIPTypeMessage = "Input text is not of IP address type.";
        #endregion

        #region Private Methods

        private static void TextboxTextCheck(object sender)
        {
            TextBox txtbox = (TextBox)sender;
            txtbox.Text = GetNumberFromString(txtbox.Text);
            if (string.Empty != txtbox.Text)
            {
                if (Convert.ToInt32(txtbox.Text) > 255)
                {
                    txtbox.Text = "255";
                }
                //GetNumberFromString prevents negative value
                //else if (Convert.ToInt32(txtbox.Text) < 0)
                //{
                //    txtbox.Text = "0";
                //}
            }
            //fix caret index
            txtbox.CaretIndex = txtbox.Text.Length;
        }

        /// <summary>
        /// Strips non-numerical characters from input string
        /// </summary>
        /// <param name="str">input string</param>
        /// <returns>numerical string or empty string</returns>
        private static string GetNumberFromString(string str)
        {
            StringBuilder numberBuilder = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsNumber(c))
                {
                    numberBuilder.Append(c);
                }
            }
            return numberBuilder.ToString();
        }

        /// <summary>
        /// Moves focus from the current text box to the next text box
        /// </summary>
        /// <param name="currentTextBox">the current text box</param>
        /// <returns>True if focus was moved</returns>
        private bool FocusNext(TextBox currentTextBox)
        {
            if (currentTextBox == txtboxFirstOctet)
            {
                txtboxSecondOctet.Focus();
                txtboxSecondOctet.SelectAll();
            }
            else if (currentTextBox == txtboxSecondOctet)
            {
                txtboxThirdOctet.Focus();
                txtboxThirdOctet.SelectAll();
            }
            else if (currentTextBox == txtboxThirdOctet)
            {
                txtboxFourthOctet.Focus();
                txtboxFourthOctet.SelectAll();
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// Moves foucs from current text box to previous text box
        /// </summary>
        /// <param name="currentTextBox">the current text box</param>
        /// <returns>True if focuse was moved</returns>
        private bool FocusPrevious(TextBox currentTextBox)
        {
            if (currentTextBox == txtboxSecondOctet)
            {
                txtboxFirstOctet.Focus();
                txtboxFirstOctet.SelectAll();
            }
            else if (currentTextBox == txtboxThirdOctet)
            {
                txtboxSecondOctet.Focus();
                txtboxSecondOctet.SelectAll();
            }
            else if (currentTextBox == txtboxFourthOctet)
            {
                txtboxThirdOctet.Focus();
                txtboxThirdOctet.SelectAll();
            }
            else
            {
                return false;
            }
            return true;
        }

        #endregion

        #region Private Events

        void IPAddressControl_Loaded(object sender, RoutedEventArgs e)
        {
            txtboxFirstOctet.Focus();
        }

        void txtboxFirstOctet_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            txtboxFirstOctet.SelectAll();
        }

        private void txtbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                TextboxTextCheck(sender);
                IPAddress = Text;
            }
            catch (Exception ex)
            {
                throw new Exception(ErrorGeneralMessage, ex);
            }
        }

        private void txtbox_PreviewKeyDown(object sender, KeyEventArgs e)
    {
            try
            {
                var txtbox = (TextBox)sender;
                if ((e.Key == Key.OemPeriod || e.Key == Key.Decimal || e.Key == Key.Tab) && txtbox.Text.Length >= 1)
                {
                    if (e.Key == Key.Tab && (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)))
                    {
                        e.Handled = FocusPrevious(txtbox);
                        return;
                    }
                    e.Handled = FocusNext(txtbox);
                }
                else if (e.Key == Key.Right && txtbox.CaretIndex == txtbox.Text.Length && txtbox.Text.Length >= 1)
                {
                    e.Handled = FocusNext(txtbox);
                }
                else if (e.Key == Key.Left && txtbox.CaretIndex == 0)
                {
                    e.Handled = FocusPrevious(txtbox);
                }
                else if (e.Key == Key.Back && txtbox.Text.Length == 0)
                {
                    e.Handled = FocusPrevious(txtbox);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void txtbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Handled)
                return;
            var txtbox = (TextBox)sender;
            if (txtbox.SelectedText.Length == 0 && txtbox.Text.Length == 3)
            {
                FocusNext(txtbox);
            }

        }
        #endregion

        #region Public Properties
        
        /// <summary>
        /// Gets or Sets the text of the control.
        /// If input text is not of IP type type then throws and argument exception.
        /// </summary>
        public string Text
        {
            get
            {
                return String.Format("{0}.{1}.{2}.{3}", txtboxFirstOctet.Text, txtboxSecondOctet.Text, txtboxThirdOctet.Text, txtboxFourthOctet.Text);
            }
            set
            {
                try
                {
                    if(string.IsNullOrEmpty(value))
                    {
                        txtboxFirstOctet.Text = string.Empty;
                        txtboxSecondOctet.Text = string.Empty;
                        txtboxThirdOctet.Text = string.Empty;
                        txtboxFourthOctet.Text = string.Empty;
                    }
                    else
                    {
                        string[] splitValues = value.Split('.');
                        txtboxFirstOctet.Text = splitValues[0];
                        txtboxSecondOctet.Text = splitValues[1];
                        txtboxThirdOctet.Text = splitValues[2];
                        txtboxFourthOctet.Text = splitValues[3];
                    }
                }
                catch (Exception ex)
                {
                   throw new ArgumentException(ErrorInputNotIPTypeMessage, ex);
                }
            }
        }

        #endregion

        public string IPAddress
        {
            get { return (string)GetValue(IPAddressProperty); }
            set { SetValue(IPAddressProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IPAddress.  This enables animation, styling, binding, etc...
        public static DependencyProperty IPAddressProperty =
            DependencyProperty.Register("IPAddress", typeof(string), typeof(IPAddressControl), new PropertyMetadata("",OnIPAddressChanged));

        private static void OnIPAddressChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var cntrl = (IPAddressControl)o;
            cntrl.Text = (string)e.NewValue;
        }
   
        
    }

}


using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Multitool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Assign the logical direction forward/backward to a single variable
        LogicalDirection fwd = LogicalDirection.Forward;
        //LogicalDirection bckwd = LogicalDirection.Backward;

        // Set the temp text to be displayed
        string placeHolderText = "Generic input box";

        public MainWindow()
        {
            InitializeComponent();
            SetTempText();
        }

        private void btn_CompareStrings_Click(object sender, RoutedEventArgs e)
        {
            // Group the two text boxes together
            (RichTextBox A, RichTextBox B) txtBox = (rtxtBx_A, rtxtBx_B);

            // Group the two text boxes' documents together
            (FlowDocument A, FlowDocument B) txtBox_Doc = (txtBox.A.Document, txtBox.B.Document);

            // Get the start/end point of the content for box A
            (TextPointer start, TextPointer end) txtBoxLimit_A = (txtBox_Doc.A.ContentStart, txtBox_Doc.A.ContentEnd);

            // Get the start/end point of the content for box B
            (TextPointer start, TextPointer end) txtBoxLimit_B = (txtBox_Doc.B.ContentStart, txtBox_Doc.B.ContentEnd);

            // Set text bacgkround to white
            txtBox.A.SelectAll();
            txtBox.B.SelectAll();
            txtBox.A.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
            txtBox.B.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

            // Get the text boxes' content
            (TextRange A, TextRange B) txtBox_Range =
                (new TextRange(txtBoxLimit_A.start, txtBoxLimit_A.end),
                new TextRange(txtBoxLimit_B.start, txtBoxLimit_B.end));

            // Get the text boxes' actual text
            (string A, string B) txtBox_Text = (txtBox_Range.A.Text, txtBox_Range.B.Text);

            // Get the TextPointer to the start of the text content for text box A and B
            var insStartA = txtBoxLimit_A.start.GetInsertionPosition(fwd);
            var insStartB = txtBoxLimit_B.start.GetInsertionPosition(fwd);

            // Set a TextPointer for the current position
            var navigatorA = insStartA.GetNextInsertionPosition(fwd);
            var navigatorB = insStartB.GetNextInsertionPosition(fwd);

            // Set caret postion at the start of the content for both text boxes
            rtxtBx_A.CaretPosition = insStartA;
            rtxtBx_B.CaretPosition = insStartB;

            // If the inputs are the same...
            if (txtBox_Text.A.Equals(txtBox_Text.B))
            {
                // Set the text to "Match!" and background to light green
                txtBlk_Result.Text = "Match!";
                txtBlk_Result.Background = Brushes.LightGreen;
            }
            //Otherwise...
            else
            {
                // Loop through the characters in the text
                while (navigatorA != null && navigatorB != null)
                {
                    // Select the next character 
                    // ***(Calling get next insertion position on caret actually moves it)
                    rtxtBx_A.Selection.Select(rtxtBx_A.CaretPosition, rtxtBx_A.CaretPosition.GetNextInsertionPosition(fwd));
                    rtxtBx_B.Selection.Select(rtxtBx_B.CaretPosition, rtxtBx_B.CaretPosition.GetNextInsertionPosition(fwd));

                    // Assign the two selections to variables
                    var selectionA = rtxtBx_A.Selection;
                    var selectionB = rtxtBx_B.Selection;

                    // wherever the selected text is different...
                    // ***Using != instead of a bang for easier readability
                    if (selectionA.Text.Equals(selectionB.Text) != true)
                    {
                        // Highlight the different characters
                        selectionA.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                        selectionB.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
                    }

                    // Set the navigators to the next TextPointer position
                    navigatorA = rtxtBx_A.CaretPosition.GetNextInsertionPosition(fwd);
                    navigatorB = rtxtBx_B.CaretPosition.GetNextInsertionPosition(fwd);

                }
                // If the two strings are not the same length...
                if (txtBox_Text.A.Length != txtBox_Text.B.Length)
                {
                    // Inform the user their lengths are different
                    WrongResult($"Different length! " +
                        $"String {(txtBox_Text.A.Length > txtBox_Text.B.Length ? "A" : "B")} is longer.");
                }
                // Otherwise...
                else
                {
                    // Inform the user the inputs don't match
                    WrongResult("No match!");
                }
            }

        }

        // Clear the input
        private void btn_Clear_Click(object sender, RoutedEventArgs e)
        {
            // Clear both boxes of content
            ClearBothBoxes();

            // Resets the text of the result box
            ResetResult();

            // Set the placeholder text back
            SetTempText();
        }


        private void btn_Region_Click(object sender, RoutedEventArgs e)
        {
            // Reset result to start off clean
            ResetResult();

            // Get the trimmed content for text box A and B
            var inputA = GetTextInA(true);

            // If there are at least two characters present...
            if (inputA.Length >= 2)
            {
                // Parse the numbers
                if (int.TryParse(inputA.Substring(0, 2), out int numbers))
                {
                    // Create local dictionary to store the values
                    RegionDictionary _localRegionList = RegionList.ReturnRegionList();

                    // If the numbers provided are 
                    if (_localRegionList.ContainsKey(numbers))
                    {
                        txtBlk_Result.Text =
                        $"Region name: {_localRegionList[numbers].RegionName} " +
                        $"BRP code: {_localRegionList[numbers].BrpCode:00}";
                    }
                }
                else
                {
                    WrongResult("Invalid input!");
                }
            }
        }

        // Clears the temporary text on focus
        private void rtxtBx_A_GotFocus(object sender, RoutedEventArgs e)
        {
            // Get the current contents of text box A
            var txt = GetTextInA(true);

            // Change the text only if the text is the temp text
            if (txt.Equals(placeHolderText))
            {
                rtxtBx_A.Document.Blocks.Clear();
            }
        }

        // Adds the temp text back when focus is lost
        private void rtxtBx_A_LostFocus(object sender, RoutedEventArgs e)
        {
            SetTempText();
        }

        // Output the text with proper case
        private void btn_ProperCase_Click(object sender, RoutedEventArgs e)
        {
            // Get the text from the box
            var tempText = GetTextInA(true).ToLower();

            // Set-up a formating option
            TextInfo txtInf = new CultureInfo("en-US", false).TextInfo;

            // Get the proper case into a string
            tempText = txtInf.ToTitleCase(tempText);

            txtBlk_Result.Text = tempText;

            //rtxtBx_A.Document.Blocks.Add(new Paragraph(new Run(tempText)));
        }
        private void rtxtBx_B_GotFocus(object sender, RoutedEventArgs e)
        {
            SetTempText();
        }

        #region Helper Methods
        /// <summary>
        /// Provides basic clean-up for the result
        /// </summary>
        private void ResetResult()
        {
            // If text is not already "Result"...
            if (txtBlk_Result.Text != "Result")
            {
                // Sets the text to result
                txtBlk_Result.Text = "Result";
                // Sets the background to the default white
                txtBlk_Result.Background = Brushes.White;
            }
        }

        /// <summary>
        /// The default method to call when there is an issue
        /// </summary>
        /// <param name="errorMessage"></param>
        private void WrongResult(string errorMessage)
        {
            txtBlk_Result.Text = errorMessage;
            txtBlk_Result.Background = Brushes.LightSalmon;
        }

        /// <summary>
        /// Sets the default temp text to be displayed
        /// </summary>
        private void SetTempText()
        {
            string txt = GetTextInA(true);

            // Change the text only if there is no text
            if (string.IsNullOrEmpty(txt))
            {
                // Sets a content container for the actual text
                FlowDocument doc = new FlowDocument();

                // Writes the text to a paragraph
                Paragraph p = new Paragraph(new Run(placeHolderText));

                // Sets the styling
                p.FontStyle = FontStyles.Italic;
                p.Foreground = Brushes.Gray;

                // Adds the paragraph to the content container
                doc.Blocks.Add(p);

                // Displays the text
                rtxtBx_A.Document = doc;
            }
        }

        /// <summary>
        /// Returns the text string of box A
        /// </summary>
        /// <param name="isTrimmed"></param>
        /// <returns></returns>
        private string GetTextInA(bool isTrimmed)
        {
            // Returns the current contents of the text box
            return
                (isTrimmed
                ? new TextRange(rtxtBx_A.Document.ContentStart, rtxtBx_A.Document.ContentEnd).Text.Trim()
                : new TextRange(rtxtBx_A.Document.ContentStart, rtxtBx_A.Document.ContentEnd).Text);

        }

        /// <summary>
        /// Clears both text boxes
        /// </summary>
        private void ClearBothBoxes()
        {
            rtxtBx_A.Document.Blocks.Clear();
            rtxtBx_B.Document.Blocks.Clear();
        }

        #endregion

    }
}

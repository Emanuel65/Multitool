﻿using System.Collections.Generic;
using System.Globalization;
using System.Windows;
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
        LogicalDirection bckwd = LogicalDirection.Backward;

        // Set the temp text to be displayed
        string placeHolderText = "Generic input box";

        public MainWindow()
        {
            InitializeComponent();
            SetTempText();
        }

        private void btn_CompareStrings_Click(object sender, RoutedEventArgs e)
        {
            // Get the start/end point of the content for box A
            var inputStartA = rtxtBx_A.Document.ContentStart;
            var inputEndA = rtxtBx_A.Document.ContentEnd;

            // Get the start/end point of the content for box B
            var inputStartB = rtxtBx_B.Document.ContentStart;
            var inputEndB = rtxtBx_B.Document.ContentEnd;

            // Set text bacgkround to white
            rtxtBx_A.SelectAll();
            rtxtBx_B.SelectAll();
            rtxtBx_A.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
            rtxtBx_B.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);

            // Get the content for text box A and B
            var inputA = new TextRange(inputStartA, inputEndA).Text;
            var inputB = new TextRange(inputStartB, inputEndB).Text;


            // Get the TextPointer to the start of the text content for text box A and B
            var insStartA = inputStartA.GetInsertionPosition(fwd);
            var insStartB = inputStartB.GetInsertionPosition(fwd);

            // Set a TextPointer for the current position
            var navigatorA = insStartA.GetNextInsertionPosition(fwd);
            var navigatorB = insStartB.GetNextInsertionPosition(fwd);

            // Set caret postion at the start of the content for both text boxes
            rtxtBx_A.CaretPosition = insStartA;
            rtxtBx_B.CaretPosition = insStartB;

            // If the inputs are the same...
            if (inputA.Equals(inputB))
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
                if (inputA.Length != inputB.Length)
                {
                    // Inform the user their lengths are different
                    WrongResult("Different length!");
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

            // Get the start/end point of the content for box A
            var inputStartA = rtxtBx_A.Document.ContentStart;
            var inputEndA = rtxtBx_A.Document.ContentEnd;

            // Get the trimmed content for text box A and B
            var inputA = GetTextInA(true);

            // If there are at least two characters present...
            if (inputA.Length >= 2)
            {
                // Parse the numbers
                if (int.TryParse(inputA.Substring(0, 2), out int numbers))
                {
                    // Create local dictionary to store the values
                    Dictionary<int, RegionData> _localDict = RegionData.regionDictionary;

                    // If the numbers provided are 
                    if (RegionData.regionDictionary.ContainsKey(numbers))
                    {
                        txtBlk_Result.Text =
                        $"Region name: {_localDict[numbers].regionName} " +
                        $"BRP code: {_localDict[numbers].brpCode}";
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

            // Get the current contents of the text box
            var txt = new TextRange(rtxtBx_A.Document.ContentStart, rtxtBx_A.Document.ContentEnd).Text.Trim();

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
            string txt = GetTextInA(false);

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

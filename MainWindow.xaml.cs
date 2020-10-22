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
		// Assign the logical direction forward/backward to a constant
		private const LogicalDirection forward = LogicalDirection.Forward;
		//private const LogicalDirection backward = LogicalDirection.Backward;

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
			(RichTextBox txtBx_A, RichTextBox txtBx_B) = (RichTextBox_A, RichTextBox_B);

			// Group the two text boxes' documents together
			(FlowDocument txtBxDoc_A, FlowDocument txtBxDoc_B) = (txtBx_A.Document, txtBx_B.Document);

			// Get the start/end point of the content for box A
			(TextPointer txtBxContStart_A, TextPointer txtBxContEnd_A) = (txtBxDoc_A.ContentStart, txtBxDoc_A.ContentEnd);

			// Get the start/end point of the content for box B
			(TextPointer txtBxContStart_B, TextPointer txtBxContEnd_B) = (txtBxDoc_B.ContentStart, txtBxDoc_B.ContentEnd);

			// Get the text boxes' content
			(TextRange txtBxRange_A, TextRange txtBxRange_B) =
				(new TextRange(txtBxContStart_A, txtBxContEnd_A),
				new TextRange(txtBxContStart_B, txtBxContEnd_B));

			// Get the text boxes' actual text
			(string txtBoxText_A, string txtBoxText_B) = (txtBxRange_A.Text, txtBxRange_B.Text);

			// Check for user input before proceeding
			if (GetTextInA(true) != placeHolderText)
			{
				// Get the TextPointer to the start of the text content for text box A and B
				var insStartA = txtBxContStart_A.GetInsertionPosition(forward);
				var insStartB = txtBxContStart_B.GetInsertionPosition(forward);

				SetAllToWhite(txtBx_A, txtBx_B);

				// Set a TextPointer for the current position
				var navigatorA = insStartA.GetNextInsertionPosition(forward);
				var navigatorB = insStartB.GetNextInsertionPosition(forward);

				// Set caret postion at the start of the content for both text boxes
				RichTextBox_A.CaretPosition = insStartA;
				RichTextBox_B.CaretPosition = insStartB;

				// If the inputs are the same...
				if (txtBoxText_A.Equals(txtBoxText_B))
				{
					// Set the text to "Match!" and background to light green
					txtBlk_Result.Text = "Match!";
					txtBlk_Result.Background = Brushes.LightGreen;
				}
				//Otherwise...
				else
				{
					// Loop through the characters in the text
					while ((navigatorA != null && navigatorB != null) && (string.IsNullOrWhiteSpace(txtBoxText_B) != true))
					{
						// Select the next character 
						// ***(Calling get next insertion position on caret actually moves it)
						RichTextBox_A.Selection.Select(RichTextBox_A.CaretPosition, RichTextBox_A.CaretPosition.GetNextInsertionPosition(forward));
						RichTextBox_B.Selection.Select(RichTextBox_B.CaretPosition, RichTextBox_B.CaretPosition.GetNextInsertionPosition(forward));

						// Assign the two selections to variables
						(TextSelection txtBxTxtSel_A, TextSelection txtBxTxtSel_B) = (RichTextBox_A.Selection, RichTextBox_B.Selection);

						// Wherever the selected text is different...
						// ***Using != instead of a bang for easier readability
						if (txtBxTxtSel_A.Text.Equals(txtBxTxtSel_B.Text) != true)
						{
							// Highlight the different characters
							txtBxTxtSel_A.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
							txtBxTxtSel_B.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.Yellow);
						}

						// Set the navigators to the next TextPointer position
						navigatorA = RichTextBox_A.CaretPosition.GetNextInsertionPosition(forward);
						navigatorB = RichTextBox_B.CaretPosition.GetNextInsertionPosition(forward);

					}
					// If the two strings are not the same length...
					if (txtBoxText_A.Length != txtBoxText_B.Length)
					{
						// Inform the user their lengths are different
						WrongResult($"Different length! " +
							$"String {(txtBoxText_A.Length > txtBoxText_B.Length ? "A" : "B")} is longer.");
					}
					// Otherwise...
					else
					{
						// Inform the user the inputs don't match
						WrongResult("No match!");
					}
				}
			}

		}

		// Output the text with proper case
		private void btn_ProperCase_Click(object sender, RoutedEventArgs e)
		{
			if (GetTextInA(true) != placeHolderText)
			{
				// Get the text from the box
				var tempText = GetTextInA(true).ToLower();

				// Set-up a formating option
				TextInfo txtInf = new CultureInfo("en-US", false).TextInfo;

				// Get the proper case into a string
				tempText = txtInf.ToTitleCase(tempText);

				// Display the result
				txtBlk_Result.Text = tempText;
				txtBlk_Result.Background = Brushes.White;
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
			if (inputA.Length >= 2 && inputA != placeHolderText)
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
				RichTextBox_A.Document.Blocks.Clear();
			}
		}

		// Adds the temp text back when focus is lost
		private void rtxtBx_A_LostFocus(object sender, RoutedEventArgs e)
		{
			SetTempText();
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


			SetAllToWhite(RichTextBox_A, RichTextBox_B);
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
				Paragraph p = new Paragraph(new Run(placeHolderText))
				{
					// Sets the styling
					FontStyle = FontStyles.Italic,
					Foreground = Brushes.Gray
				};

				// Adds the paragraph to the content container
				doc.Blocks.Add(p);

				// Displays the text
				RichTextBox_A.Document = doc;
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
				? new TextRange(RichTextBox_A.Document.ContentStart, RichTextBox_A.Document.ContentEnd).Text.Trim()
				: new TextRange(RichTextBox_A.Document.ContentStart, RichTextBox_A.Document.ContentEnd).Text);

		}

		/// <summary>
		/// Clears both text boxes
		/// </summary>
		private void ClearBothBoxes()
		{
			RichTextBox_A.Document.Blocks.Clear();
			RichTextBox_B.Document.Blocks.Clear();
		}

		private static void SetAllToWhite(RichTextBox txtBx_A, RichTextBox txtBx_B)
		{
			// Set text bacgkround to white
			txtBx_A.SelectAll();
			txtBx_B.SelectAll();
			txtBx_A.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
			txtBx_B.Selection.ApplyPropertyValue(TextElement.BackgroundProperty, Brushes.White);
		}


		#endregion

	}
}

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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_CompareStrings_Click(object sender, RoutedEventArgs e)
        {
            // Get the start/end point of the content for box A
            var inputStartA = rtxtBx_A.Document.ContentStart;
            var inputEndA = rtxtBx_A.Document.ContentEnd;

            // Get the start/end point of the content for box B
            var inputStartB = rtxtBx_B.Document.ContentStart;
            var inputEndB = rtxtBx_B.Document.ContentEnd;

            // Get the content for text box A and B
            var inputA = new TextRange(inputStartA, inputEndA).Text;
            var inputB = new TextRange(inputStartB, inputEndB).Text;

            // Assign the logical direction forward to a single variable
            var fwd = LogicalDirection.Forward;

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
                    // Inform the user and return
                    txtBlk_Result.Text = "Different length!";
                    txtBlk_Result.Background = Brushes.LightSalmon;
                }
            }

        }

    }
}

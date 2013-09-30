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
using System.Windows.Forms;
using System.IO;

///
/// Author name: Ganesh Chandrasekaran
/// E-mail: gxc4795@rit.edu
/// 
/// Program: To simulate phone keypad using T9 dictionary using predictive and
/// non-predictive mode
///
namespace Lab_2___T9_Dictionary
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool isPredictive;
        bool isSingleClick;
        System.Windows.Forms.Timer timer;
        static int clickCount;
        String appendChar;

        Dictionary<String, List<String>> validWords = new Dictionary<String, List<String>>();

        static StringBuilder inputKey = new StringBuilder("");
        static List<String> displayToUser;
        // static int counterForWords = 0;


        /// <summary>
        /// Initializes all the data members and sets interval for the timer.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            isSingleClick = true;
            isPredictive = true;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(TimerEventProcessor);
            readFromWordsText();

        }

        /// <summary>
        /// This method is called as soon as the set interval time passes.
        /// It stops the timer and appends the text to the text display
        /// and sets the isSingleClick value to true.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Base class for classes containing event data</param>
        void TimerEventProcessor(object sender, EventArgs e)
        {
            timer.Stop();

            textBox1.AppendText(appendChar);

            isSingleClick = true;
        }

        /// <summary>
        /// This method reads from the text file containing list of all valid words and stores 
        /// it in a linked hash table.
        /// </summary>
        public void readFromWordsText()
        {
            try
            {
                StreamReader sr = new StreamReader("C:\\Users\\GANESH\\Documents\\Visual Studio 2010\\Projects\\Lab 2 - T9 Dictionary\\Lab 2 - T9 Dictionary\\words.txt");

                while (true)
                {
                    String line = sr.ReadLine();

                    if (line == null)
                    {
                        break;
                    }
                    String key = generateKey(line.ToLower());
                    List<String> tempList;
                    validWords.TryGetValue(key, out tempList);
                    if (tempList == null)
                    {
                        tempList = new List<String>();
                        tempList.Add(line);
                        validWords.Add(key, tempList);
                        //    Console.WriteLine("Key: " + key + "   Value: " + line);
                    }
                    else
                    {
                        tempList.Add(line);
                        //  Console.WriteLine("Key: " + key + "   Value: " + line);
                    }
                    //validWords.Add(key,line);

                    // Console.WriteLine(line);
                    //  i++;
                }
                //Console.WriteLine(i);
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }

        String generateKey(String line)
        {
            char[] charArray = line.ToCharArray();
            StringBuilder stringValue = new StringBuilder("");
            foreach (char ch in charArray)
            {
                if (ch == 'a' || ch == 'b' || ch == 'c')
                    stringValue.Append(2);

                if (ch == 'd' || ch == 'e' || ch == 'f')
                    stringValue.Append(3);

                if (ch == 'g' || ch == 'h' || ch == 'i')
                    stringValue.Append(4);

                if (ch == 'j' || ch == 'k' || ch == 'l')
                    stringValue.Append(5);

                if (ch == 'm' || ch == 'n' || ch == 'o')
                    stringValue.Append(6);

                if (ch == 'p' || ch == 'q' || ch == 'r' || ch == 's')
                    stringValue.Append(7);

                if (ch == 't' || ch == 'u' || ch == 'v')
                    stringValue.Append(8);

                if (ch == 'w' || ch == 'x' || ch == 'y' || ch == 'z')
                    stringValue.Append(9);
            }

            return stringValue.ToString();
        }

        public void displayToUserList()
        {
            String newText = textBox1.Text;
            if (!newText.Contains(" "))
            {
                String forKey = inputKey.ToString();

                if (!validWords.ContainsKey(forKey))
                {
                    for (int i = 0; i < forKey.Length; i++)
                    {
                        newText += "-";
                    }
                    textBox1.Text = newText;
                }

                if (validWords.ContainsKey(forKey))
                {
                    validWords.TryGetValue(forKey, out displayToUser);

                    foreach (String word in displayToUser)
                    {
                        textBox1.Text = word;
                        displayToUser.Remove(word);
                        break;
                    }
                }
            }
            else if (newText.Contains(" "))
            {
                String[] words = newText.Split(' ');
                StringBuilder prefixWords = new StringBuilder("");
                for (int i = 0; i < words.Length - 1; i++)
                {
                    prefixWords.Append(words[i] + " ");
                }
                String newPrefix = prefixWords.ToString();

                String forKey = inputKey.ToString();

                if (!validWords.ContainsKey(forKey))
                {
                    for (int i = 0; i < forKey.Length; i++)
                    {
                        newText += "-";
                    }
                    textBox1.Text = newPrefix + newText;
                }

                if (validWords.ContainsKey(forKey))
                {
                    validWords.TryGetValue(forKey, out displayToUser);

                    foreach (String word in displayToUser)
                    {
                        textBox1.Text = newPrefix + word;
                        displayToUser.Remove(word);
                        break;
                    }
                }
            }
        }


        /// <summary>
        /// Handles single click event for Keypad button 2.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "a";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "c";
                }
            }
            else if (isPredictive)
            {
                inputKey.Append("2");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 2.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button2_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "b";
            }
        }

        /// <summary>
        /// Handles single click event for Keypad button 3.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "d";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "f";
                }
            }

            else if (isPredictive)
            {
                inputKey.Append("3");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 3.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button3_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "e";
            }
        }

        /// <summary>
        /// Handles single click event for Keypad button 4.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button4_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "g";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "i";
                }
            }
            else if (isPredictive)
            {
                inputKey.Append("4");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 4.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button4_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "h";
            }
        }

        /// <summary>
        /// Handles single click event for Keypad button 5.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "j";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "l";
                }
            }
            else if (isPredictive)
            {
                inputKey.Append("5");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 5.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button5_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "k";
            }
        }

        /// <summary>
        /// Handles single click event for Keypad button 6.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button6_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "m";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "o";
                }
            }
            else if (isPredictive)
            {
                inputKey.Append("6");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 6.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button6_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "n";
            }
        }

        /// <summary>
        /// Handles single click event for Keypad button 7.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button7_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "p";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "r";
                }
                else if (clickCount == 4)
                {
                    appendChar = "s";
                }
            }

            else if (isPredictive)
            {
                inputKey.Append("7");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 7.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button7_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "q";
            }
        }


        /// <summary>
        /// Handles single click event for Keypad button 8.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button8_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "t";
                    isSingleClick = false;
                    clickCount = 0;

                }
                else if (clickCount == 3)
                {
                    appendChar = "v";
                }
            }

            else if (isPredictive)
            {
                inputKey.Append("8");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 8.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button8_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "u";
            }
        }


        /// <summary>
        /// Handles single click event for Keypad button 9.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button9_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = "w";
                    isSingleClick = false;
                    clickCount = 0;
                }
                else if (clickCount == 3)
                {
                    appendChar = "y";
                }
                else if (clickCount == 4)
                {
                    appendChar = "z";
                }
            }

            else if (isPredictive)
            {
                inputKey.Append("9");
                displayToUserList();
            }
        }

        /// <summary>
        /// Handles double click event for Keypad button 9.
        /// 
        /// In non-predictive mode it will append single character to text display
        /// after the passing of set time interval depending on number of clicks.
        /// 
        /// In predictive mode it will add the characters of keypad one by one to a list
        /// and generate all permuatations of string in the list and suggest valid 
        /// words to the user in the text display.
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Provides data for mouse button related event</param>
        private void button9_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isPredictive)
            {
                clickCount++;
                appendChar = "x";
            }
        }

        /// <summary>
        /// Erases the last character in Non-Predictive Mode
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button10_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                String text = textBox1.Text;
                String trimmedText;
                if (text.Length > 0)
                {
                    trimmedText = text.Substring(0, text.Length - 1);
                }
                else
                {
                    trimmedText = "";
                }
                textBox1.Text = trimmedText;
            }
        }

        /// <summary>
        /// This method toggles between predictive and non-predictive mode
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button13_Click(object sender, RoutedEventArgs e)
        {
            String text = button13.Content.ToString();

            if (text == "Predictive Mode: ON")
            {
                if (isPredictive)
                {
                    isPredictive = false;
                }
                button13.Content = "Predictive Mode: OFF";
            }
            else
            {
                if (!isPredictive)
                {
                    isPredictive = true;
                }
                button13.Content = "Predictive Mode: ON";
            }
        }

        /// <summary>
        /// This method adds a space into the user text display
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button12_Click(object sender, RoutedEventArgs e)
        {
            if (!isPredictive)
            {
                if (isSingleClick)
                {
                    timer.Start();
                    appendChar = " ";
                    isSingleClick = false;
                }
            }

            else if (isPredictive)
            {
                String text = textBox1.Text;

                //String forKey = inputKey.ToString();

                String newText = "";
                //if (!validWords.ContainsKey(forKey))
                //{
                //    for (int i = 0; i < forKey.Length; i++)
                //    {
                //        newText += "-";
                //    }
                //    textBox1.Text = newText;
                //    return;
                //}

                newText = text + " ";

                textBox1.Text = newText;
                inputKey = new StringBuilder("");
                displayToUser = null;
            }
        }

        /// <summary>
        /// This method fetches next valid word for the user text display
        /// </summary>
        /// <param name="sender">Root of type hierarchy</param>
        /// <param name="e">Contains state information and event data
        ///  associated with a routed event</param>
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            String text = textBox1.Text;

            if (text.Contains(" "))
            {
                String[] words = text.Split(' ');
                StringBuilder prefixWords = new StringBuilder("");
                for (int i = 0; i < words.Length - 1; i++)
                {
                    prefixWords.Append(words[i] + " ");
                }
                String newPrefix = prefixWords.ToString();
                Console.WriteLine(newPrefix);

                if (displayToUser != null)
                {
                    foreach (String word in displayToUser)
                    {
                        textBox1.Text = newPrefix + word;
                        displayToUser.Remove(word);
                        break;
                    }
                }
            }
            else if (!text.Contains(" "))
            {
                foreach (String word in displayToUser)
                {
                    textBox1.Text = word;
                    displayToUser.Remove(word);
                    break;
                }
            }
        }
    }
}

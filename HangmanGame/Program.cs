using System;
using System.Text;
using System.Linq;

namespace HangmanGame
{
    class Program
    {
        private static bool endProgram = false;
        private static int guess = 1;
        private static readonly int startIdx = 3;
        private static string userInput = "";
        private static string hiddenWord = "";
        private static int wordIndex;
        private static readonly string[] wordList =
        {
            "HangMan",
            "regeringskris",
            "blomma",
            "hybridbil",
            "segelbåt",
            "andningspaus",
            "bilpool",
            "honung",
            "brasa",
            "tändare",
            "kaffekopp",
            "motorsåg",
            "president",
            "klocka",
            "giraFF",
            "Gitarr"
        };
        private static readonly string[] screenBuffer =
        {
            "\t\t\t--==HangMan==--",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            ""
        };
        private static int screenBufferIndex = screenBuffer.Length - 1;

        private static readonly string[] hangPole =
        {
            "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2557",
            "\u2551",
            "\u2551",
            "\u2551",
            "\u2551",
            "\u2551",
            "\u2551",
            "\u2551",
            "\u2554\u2550\u2550\u2563",
            "\u2552\u2550\u2550\u2569\u2550\u2550\u2563",
            "\u2551\u0020\u0020\u0020\u0020\u0020\u2551",
            "\u2550\u2550\u2550\u2550\u2550\u2550\u2550\u2569\u2550\u2550\u2550\u2550\u2550\u255d"
        };

        private static int hangPoleIndex = hangPole.Length - 1;

        private static readonly string[] hangMan =
        {
            "\u2552\u2550",
            "\u2502\u0020",
            "\u250c\u2534\u2510",
            "\u2514\u252c\u2518",
            "\u2215\u2502\u005c",
            "\u2502\u0020",
            "\u2215\u0020\u005c",
            "Oh No, he died!"
        };

        private static readonly string[] guesses = new string[10];

        private static int hangManIndex = 0;

        private static readonly StringBuilder sb = new StringBuilder();

        static void Main(string[] args)
        {
            Random rand = new Random();
            wordIndex = rand.Next(0, wordList.Length);
            hiddenWord = string.Concat(Enumerable.Repeat('_', wordList[wordIndex].Length));
            while (guess <= 10 && endProgram == false)
            {
                screenBuffer[2] = "\tGissa detta ord : " + FormatWord(hiddenWord);
                DrawScreen();
                if (!hiddenWord.ToLower().Equals(wordList[wordIndex].ToLower()))
                {
                    Console.Write("\t" + (11 - guess) + " försök kvar. Skriv bokstav, del av, eller hela ordet : ");
                    userInput = Console.ReadLine().Trim();

                    if (userInput.Length > 0)
                    {
                        if (wordList[wordIndex].ToLower().Contains(userInput.ToLower()))
                        {
                            UpdateHiddenWord(userInput);
                            screenBuffer[startIdx + guess] = ("Ja, " + FormatWord(CorrectPartsInHW(userInput))).PadRight(60, ' ')
                                + screenBuffer[startIdx + guess].PadLeft(30, ' ')[^10..].PadLeft(35, ' ');
                        }
                        else
                        {
                            screenBuffer[startIdx + guess] = ("Nej, " + userInput + " ingår inte!").PadRight(60, ' ')
                                + screenBuffer[startIdx + guess].PadLeft(30, ' ')[^10..].PadLeft(35, ' ');
                        }
                        if (!guesses.Contains(userInput.ToLower()))
                        {
                            if (guess <= 6)
                            {
                                screenBuffer[screenBufferIndex] = screenBuffer[screenBufferIndex].PadRight(60, ' ')[0..60] + hangPole[hangPoleIndex].PadLeft(35, ' ');
                                screenBufferIndex--;
                                hangPoleIndex--;
                                screenBuffer[screenBufferIndex] = screenBuffer[screenBufferIndex].PadRight(60, ' ')[0..60] + hangPole[hangPoleIndex].PadLeft(35, ' ');
                                screenBufferIndex--;
                                hangPoleIndex--;
                            }
                            if (guess > 6)
                            {
                                screenBuffer[screenBufferIndex] = screenBuffer[screenBufferIndex].PadRight(60, ' ')[0..60] + hangMan[hangManIndex].PadLeft(25, ' ') + hangPole[hangManIndex].PadLeft(10, ' ');
                                screenBufferIndex++;
                                hangManIndex++;
                                screenBuffer[screenBufferIndex] = screenBuffer[screenBufferIndex].PadRight(60, ' ')[0..60] + hangMan[hangManIndex].PadLeft(25, ' ') + hangPole[hangManIndex].PadLeft(10, ' ');
                                screenBufferIndex++;
                                hangManIndex++;
                            }
                            guesses[guess - 1] = userInput.ToLower();
                            guess++;
                        }
                    }
                }
                else
                {
                    UpdateHiddenWord(userInput);
                    DrawScreen();
                    Console.WriteLine($"{wordList[wordIndex]} är rätt! Hangman klarade sig denna gången!");
                    endProgram = true;
                    Console.ReadKey();
                }
            }
        }

        private static void DrawScreen()
        {
            Console.Clear();
            for (int i = 0; i < screenBuffer.Length; i++)
            {
                Console.WriteLine(screenBuffer[i]);
            }
        }

        private static string FormatWord(string inputString)
        {
            //string outputString = "";
            char[] tmpArray = inputString.ToCharArray();
            sb.Clear();
            for (int i = 0; i < inputString.Length; i++)
            {
                //outputString += $" {inputString.ToCharArray()[i]} ";
                sb.Append(" ").Append(tmpArray[i]).Append(" ");
            }
            //return outputString;
            return sb.ToString();
        }

        private static string CorrectPartsInHW(string inputString)
        {
            string tmpString = "";
            for (int i = 0; i < wordList[wordIndex].Length; i++)
            {
                if (userInput.ToLower().Contains(wordList[wordIndex].ToLower()[i..(i + 1)]))
                {
                    tmpString += wordList[wordIndex].ToCharArray()[i];
                }
                else
                {
                    tmpString += '_';
                }
            }
            return tmpString;
        }

        private static void UpdateHiddenWord(string strInput)
        {
            string tmpString = "";
            for (int i = 0; i < hiddenWord.Length; i++)
            {
                if (strInput.ToLower().Contains(wordList[wordIndex].ToLower()[i..(i + 1)]))
                {
                    tmpString += wordList[wordIndex][i..(i + 1)];
                }
                else
                {
                    tmpString += hiddenWord[i] != '_' ? hiddenWord[i] : '_';
                }
            }
            hiddenWord = tmpString;
        }
    }
}

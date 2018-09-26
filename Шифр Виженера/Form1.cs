using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Шифр_Виженера
{

    public partial class Form1 : Form
    {
        public static char[] ENGletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        public static char[] RUSletters = "АБВГДЕЁЖЗИКЛМНОПРСТУФХЦЧШЩЪЫЬЭЮЯ".ToCharArray();


        public static bool belongs(char x, char[] line)
        {
            bool belongs = false;
            for (int counter = 0; counter<line.Length; counter++)
            {
                if (x == line[counter])
                {
                    belongs = true;
                    break;
                }
            }
            return belongs;
        }

        public static char[] ToBigLet(string text, char[] alphabet)
        {
            text = text.ToUpper(); 
            char[] NotCorrected = text.ToCharArray();
            int spacecounter = 0;
            for (int govnocounter = 0; govnocounter < NotCorrected.Length; govnocounter++)
            {
                if (!belongs(NotCorrected[govnocounter], alphabet))
                {
                    spacecounter++;
                }
            }
            char[] Corrected = new char[NotCorrected.Length-spacecounter];
            int counter2, corcounter = 0;
            bool OK;
            for (int counter = 0; counter < NotCorrected.Length; counter++)
            {
                OK = false;
                for (counter2 = 0; counter2 < alphabet.Length; counter2++)
                {
                    if (NotCorrected[counter] == alphabet[counter2])
                    {
                        OK = true;
                    }
                }
                if (OK == true)
                {
                    Corrected[corcounter] = NotCorrected[counter];
                    corcounter++;
                }
            }
            return Corrected;
        }



        public static char[] KeyGeneration(string text, char[] alphabet, int KeyLenght)
        {
            char[] KeySrc = ToBigLet(text, alphabet);
            char[] Key = new char[KeyLenght];
            int Counter;
            int Counter2;
            if (KeySrc.Length != Key.Length)
            {
                if (KeySrc.Length > Key.Length)
                {
                    for (Counter = 0; Counter <= Key.Length; Counter++)
                    {
                        if (Counter < Key.Length)
                        {
                            Key[Counter] = KeySrc[Counter];
                        }
                    }
                }
                else
                {
                    try
                    {

                        for (Counter = 0; Counter <= ((Key.Length - (Key.Length % KeySrc.Length)) / KeySrc.Length) + 1; Counter++)
                        {
                            for (Counter2 = 0; Counter2 < KeySrc.Length; Counter2++)
                            {
                                if (Counter * KeySrc.Length + Counter2 < Key.Length)
                                {
                                    Key[Counter * KeySrc.Length + Counter2] = KeySrc[Counter2];
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
            return Key; 
        }


        public static int GetLetNum(char letter, char[] alphabet)
        {
            int number = 404;
            for (int counter = 0; counter<alphabet.Length; counter++)
            {
                if (letter == alphabet[counter])
                {
                    number = counter;
                    break;
                }
            }
            return number;
        }



        public static char[] Encryption(char[] PlainText, char[] Key, char[] alphabet)
        {
            int PlainTNum, KeyNum, EncNum;
            char[] cipher = new char[PlainText.Length];
            for (int counter = 0; counter < PlainText.Length; counter++)
            {
                PlainTNum = GetLetNum(PlainText[counter], alphabet);
                KeyNum = GetLetNum(Key[counter], alphabet);
                EncNum = (PlainTNum + KeyNum) % alphabet.Length;
                if (EncNum == 404 || KeyNum == 404)
                {
                    return "ERROR".ToCharArray();
                }
                if (!String.IsNullOrWhiteSpace(Convert.ToString(PlainText[counter])) || PlainText[counter] == ' ')
                {
                    cipher[counter] = alphabet[EncNum];
                }
            }
            return cipher;
        }

        public static char[] Decryption(char[] Cipher, char[] Key, char[] alphabet)
        {
            int PlainTNum, KeyNum, EncNum;
            char[] PlainText = new char[Cipher.Length];
            for (int counter = 0; counter < PlainText.Length; counter++)
            {
                EncNum = GetLetNum(Cipher[counter], alphabet);
                KeyNum = GetLetNum(Key[counter], alphabet);
                if (EncNum == 404 || KeyNum == 404)
                {
                    return "ERROR".ToCharArray();
                }
                PlainTNum = (EncNum - KeyNum + alphabet.Length) % alphabet.Length;
                if (!String.IsNullOrWhiteSpace(Convert.ToString(Cipher[counter])) || PlainText[counter] == ' ')
                {
                    PlainText[counter] = alphabet[PlainTNum];
                }
            }
            return PlainText;
        }


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            richTextBox2.Text = "";
            char[] PlainText = ToBigLet(richTextBox1.Text, ENGletters);
            if (String.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                label1.Text = "ОШИБКА: неверно введен ключ";
                return;
            }
            if (ToBigLet(richTextBox3.Text, ENGletters).Length >= PlainText.Length)
            {
                label6.Text = "running";
                label6.ForeColor = Color.Green;
            }
            else
            {
                label6.Text = "regular";
                label6.ForeColor = Color.Red;
            }
            char[] Key = KeyGeneration(richTextBox3.Text, ENGletters, PlainText.Length);
            if (String.IsNullOrWhiteSpace(Key.ToString()))
            {
                label1.Text = "ОШИБКА: неверно введен ключ";
                return;
            }
            char[] Cipher = Encryption(PlainText, Key, ENGletters);
            int space = 0;
            for (int ct = 0; ct<Cipher.Length; ct++)
            {
                richTextBox2.Text += Cipher[ct].ToString();
                space++;
                if (space == 5)
                {
                    space = 0;
                    richTextBox2.Text += " ";
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            label1.Text = "";
            richTextBox1.Text = "";
            char[] Cipher = ToBigLet(richTextBox2.Text, ENGletters);
            if (String.IsNullOrWhiteSpace(richTextBox3.Text))
            {
                label1.Text = "ОШИБКА: неверно введен ключ";
                return;
            }
            if (ToBigLet(richTextBox3.Text, ENGletters).Length >= Cipher.Length)
            {
                label6.Text = "running";
                label6.ForeColor = Color.Green;
            }
            else
            {
                label6.Text = "regular";
                label6.ForeColor = Color.Red;
            }
            char[] Key = KeyGeneration(richTextBox3.Text, ENGletters, Cipher.Length);
            if (String.IsNullOrWhiteSpace(Key.ToString()))
            {
                label1.Text = "ОШИБКА: неверно введен ключ";
                return;
            }
            char[] PlainText = Decryption(Cipher, Key, ENGletters);
            for (int ct = 0; ct < PlainText.Length; ct++)
            {
                richTextBox1.Text += PlainText[ct].ToString();
            }
        }
    }
}

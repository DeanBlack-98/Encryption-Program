using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace WindowsFormsApp15
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        string path, extension;
        Byte[] vsv, alfa, delta, beta;

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                openFileDialog1.CheckFileExists = true;
                openFileDialog1.CheckPathExists = true;
                path = openFileDialog1.FileName;
                label3.Text = path;
                extension = Path.GetExtension(path);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string zulu;
            Byte[] zebra = File.ReadAllBytes("C:/Users/User/Documents/League of Legends/Replays/data.txt");

            zulu = DecryptStringFromBytes_DES(zebra, alfa, delta);
            //string allie = Encoding.ASCII.GetString(zulu);

            //abel4.Text = "" + allie;
            File.WriteAllBytes("C:/Users/User/Documents/League of Legends/Replays/dataD.txt", Encoding.ASCII.GetBytes(zulu)); 
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Create new instance of DES class
            DES Alfa = DES.Create();
            //Assign A key from input after casting it to a byte
            alfa = Encoding.ASCII.GetBytes(textBox1.Text);
            try
            {
                Alfa.Key = alfa;
                Alfa.BlockSize = 128;
                //Reads file to encrypt and converts file into bytes
                beta = File.ReadAllBytes(label3.Text);
                //cast bytes to string for encryption
                string ekko = Encoding.ASCII.GetString(beta);
                //Generates An IV a hidden key for encrypting
                Alfa.GenerateIV();
                delta = Alfa.IV;
                vsv = EncryptStringToBytesDES(ekko, alfa, delta);
                File.WriteAllBytes("C:/Users/User/Documents/League of Legends/Replays/data.txt", vsv);
                File.WriteAllBytes("C:/Users/User/Documents/League of Legends/Replays/data1.txt", beta);
                string charlie = Encoding.ASCII.GetString(vsv);
                label3.Text = "" + charlie;
            }
            catch (System.ArgumentException)
            {
                MessageBox.Show("Please enter a password with 8 Charakters");
            }
        }
        static byte[] EncryptStringToBytesDES(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;

            // Create an Aes object
            // with the specified key and IV.
            using (DES Alfa = DES.Create())
            {
                Alfa.Key = Key;
                Alfa.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = Alfa.CreateEncryptor(Alfa.Key, Alfa.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        static string DecryptStringFromBytes_DES(byte[] cipherText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an DES object
            // with the specified key and IV.
            using (DES Alfa = DES.Create() )
            {
                Alfa.Key = Key;
                Alfa.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = Alfa.CreateDecryptor(Alfa.Key, Alfa.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
}

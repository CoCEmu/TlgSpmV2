using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeleSharp.TL;
using TeleSharp.TL.Messages;
using TLSharp.Core;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;


namespace TlgSpm
{
    public enum sendchat
    {
        All = 0,
        User = 1,
        Group = 2,
        Channel = 0
    }
    class Messaage
    {
        public string message { get; internal set; }
        public Int64 PauseTime { get;  internal set; }
        public DateTime TimeToSend { get; set; }
        public sendchat ChatType { get; internal set; }
    }
    class Worker
    {
        public string number;
        public List<Messaage> Messages;

        TelegramClient client;
        TLDialogs dialogs;

        string decrpted;
        private void Connect()
        {
            FileSessionStore store = new FileSessionStore();
            while (true)
            {
                try
                {

                    ServiceReference1.Service1Client tgsrv = new ServiceReference1.Service1Client();
                    if (decrpted == "" || decrpted == null)
                    {
                        StringCipher sdec = new StringCipher();
                        var res = tgsrv.GetCleintData(null, null);
                        decrpted = sdec.Decrypt(tgsrv.GetCleintData(null, null), "Amoo namoosan to hacker?");
                    }
                    client = new TelegramClient(Convert.ToInt32(decrpted.Split(':')[0]), decrpted.Split(':')[1], store, "session");
                    break;
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error detect,maybe network not connected and you're session is expired.first check youre inter net connection is problem exist,delete session.dat file and re open program again...");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
            }

            while (true)
            {
                try
                {

                    client.ConnectAsync().Wait();
                    if (client.IsUserAuthorized() == false)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Try to connect...");
                        Console.ForegroundColor = ConsoleColor.White;
                        if (number == "" || number == null)
                        {
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine("Re open Program Again...");
                            Thread.Sleep(2000);
                            Environment.Exit(0);
                        }
                        var hash = client.SendCodeRequestAsync(number).Result;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Code has ben sent.");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Please Enter the Code");
                        Console.ForegroundColor = ConsoleColor.White;
                        var code = Console.ReadLine(); // you can change code in debugger
                        var user = client.MakeAuthAsync(number, hash, code).Result;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Logged in Complate!");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Logged in Complate!");
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    }
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.InnerException.ToString());
                    Console.ForegroundColor = ConsoleColor.White;
                    Thread.Sleep(3000);
                }
            }
        }
        private void Disconnect()
        {
            client.Dispose();
        }
        public void Start()
        {

            LoadConfig();
            Connect();
            ProccessSend();
           
        }

        private void ProccessSend()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Send Messages Started");
            Console.ForegroundColor = ConsoleColor.White;
            UpdateDialogs();
            while (true)
            {
                foreach (Messaage msg in Messages)
                {
                    if (msg.TimeToSend <= DateTime.Now)
                    {
                        SendMessages(msg.message, msg.ChatType);
                        msg.TimeToSend.AddSeconds(msg.PauseTime);
                    }
                }
                Disconnect();
                Random rnd = new Random();
                Thread.Sleep(rnd.Next(2000, 10000));
                Connect();
            }
        }

        private void UpdateDialogs()
        {
            dialogs = new TLDialogs();
            while (true)
            {
                if (client == null)
                    return;
                try
                {
                    dialogs = (TLDialogs)client.GetUserDialogsAsync().Result;
                    break;
                }
                catch(Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Thread.Sleep(2000);
                    Console.WriteLine("Delete Some Dialogs First...");
                    Thread.Sleep(2000);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Press Enter to continue.. after delete you're Dialog...");
                    Console.ReadKey();
                    Console.ForegroundColor = ConsoleColor.White;
                }
            }
        }
        private void SendMessages(string message, sendchat SendChatType = 0)
        {
            try
            {
                if (dialogs.dialogs.lists.Count == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("No Dialog found.");
                    Console.ForegroundColor = ConsoleColor.White;
                    return;
                }
                int i = 0;
                // while (true)
                {
                    if (SendChatType == sendchat.All || SendChatType == sendchat.User)
                    {
                        foreach (TLObject obj in dialogs.users.lists)
                        {
                            TLObject obj1 = obj;
                            TLObject obj2 = obj;
                            TLObject obj3 = obj;
                            TLUser obuser = null;
                            TLChat obchat = null;
                            TLChannel obchannel = null;
                            bool chat = false;
                            bool user = false;
                            bool channel = false;
                            i++;

                            {
                                try
                                {

                                    obj1 = obj;
                                    obuser = obj1 as TLUser;
                                    if (obuser != null)
                                        user = true;

                                    if (user == false)
                                    {
                                        obj2 = obj;
                                        obchat = obj2 as TLChat;
                                        if (obchat != null)
                                            chat = true;
                                    }
                                    if (chat == false)
                                    {
                                        obj3 = obj;
                                        obchannel = obj3 as TLChannel;
                                        if (obchannel != null)
                                            channel = true;
                                    }
                                }
                                catch
                                {

                                }

                            }
                            if (user)
                            {
                                try
                                {
                                    client.SendTypingAsync(new TLInputPeerUser() { user_id = obuser.id, access_hash = obuser.access_hash.Value });
                                    Thread.Sleep(1500);
                                    client.SendMessageAsync(new TLInputPeerUser { user_id = obuser.id, access_hash = obuser.access_hash.Value }, message).Wait();
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                    if (SendChatType != sendchat.User)
                        foreach (TLObject obj in dialogs.chats.lists)
                        {
                            TLObject obj1 = obj;
                            TLObject obj2 = obj;
                            TLObject obj3 = obj;
                            TLUser obuser = null;
                            TLChat obchat = null;
                            TLChannel obchannel = null;
                            bool chat = false;
                            bool user = false;
                            bool channel = false;
                            i++;
                            try
                            {

                                obj1 = obj;
                                obuser = obj1 as TLUser;
                                if (obuser != null)
                                    user = true;

                                if (user == false)
                                {
                                    obj3 = obj;
                                    obchannel = obj3 as TLChannel;
                                    if (obchannel != null)
                                        channel = true;

                                }
                                if (channel == false)
                                {
                                    obj2 = obj;
                                    obchat = obj2 as TLChat;
                                    if (obchat != null)
                                        chat = true;
                                }
                            }
                            catch
                            {

                            }
                            if (SendChatType == sendchat.Group || SendChatType == sendchat.All)
                            {

                                if (chat)
                                {
                                    try
                                    {
                                        client.SendTypingAsync(new TLInputPeerChat() { chat_id = obchat.id });
                                        Thread.Sleep(1500);
                                        client.SendMessageAsync(new TLInputPeerChat { chat_id = obchat.id }, message).Wait();
                                        // Console.WriteLine(i.ToString() + "   -  message send to " + obchat.id);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                        
                            if (SendChatType == sendchat.Channel || SendChatType == sendchat.All)
                            {
                                if (channel)
                                {
                                    try
                                    {
                                        client.SendMessageAsync(new TLInputPeerChannel { channel_id = obchannel.id, access_hash = obchannel.access_hash.Value }, message).Wait();
                                        //     Console.WriteLine(i.ToString() + "   -  message send to " + obchannel.id);
                                    }
                                    catch
                                    {

                                    }

                                }
                            }

                        }


                }


                /*  var contact = client.GetContactsAsync().Result;
                  foreach(TeleSharp.TL.TLUser us in contact.users.lists)
                  {
                      Console.WriteLine(us.first_name);
                  }
                */
            }
            catch(Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error Detected...");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Continue...");
                Console.ForegroundColor = ConsoleColor.White;
            }

        }
        public void LoadConfig()
        {
            Messages = new List<Messaage>();

            try
            {
                File.OpenWrite(@"config.txt").Close();
                Console.ForegroundColor = ConsoleColor.White;
                //Console.WriteLine("after change config.txt , press any key...");
                // Read the file and display it line by line. 
                System.IO.StreamReader file = new System.IO.StreamReader(@"config.txt");
                string texts = file.ReadToEnd();
                if (texts != null && texts != "")
                {

                    {
                        try
                        {

                            var msg = texts.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);
                            Console.ForegroundColor = ConsoleColor.Green;
                            for (int i = 0; i < msg.Length; i++)
                            {
                                if (i == 0)
                                    number = msg[0];
                                else
                                {
                                    Messaage ms = new Messaage { message = msg[i].Split(new string[] { "::" }, 2, StringSplitOptions.RemoveEmptyEntries)[0], PauseTime = (Convert.ToInt64( msg[i].Split(new string[] { "::" }, 2, StringSplitOptions.RemoveEmptyEntries)[1]))};
                                    ms.TimeToSend = DateTime.Now.AddSeconds(ms.PauseTime);
                                    Messages.Add(ms);
                                }
                               
                            }
                                Console.WriteLine(Messages.Count.ToString() + " Messages Loaded");
                                Console.ForegroundColor = ConsoleColor.White;
                                file.Close();
                                return;
                        }
                        catch (Exception ex)
                        {

                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Config File NOT Loaded!");
                            Console.ForegroundColor = ConsoleColor.White;
                            file.Close();
                            Console.WriteLine("after change config.txt , Press Enter to continue..");
                            Console.ReadLine();

                        }
                    }
                }
                else
                {
                    file.Close();
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Enter Youre phone number,example:989123456789");
                    Console.ForegroundColor= ConsoleColor.White;
                    string num = Console.ReadLine();
                    number = num;
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("2 sample message added...");
                    string message = num+ "\r\nبنام خداوند بخشنده ی بخشایشگر::5\r\nHttp://TLG.qq-wow.com/Update::15";
                    Messaage ms = new Messaage { message = "www.iapi.ir", PauseTime = 5 };
                    Messaage ms2 = new Messaage { message = "https://github.com/CoCEmu/iapi", PauseTime = 15 };
                    ms.TimeToSend = DateTime.Now.AddSeconds(ms.PauseTime);
                    Messages.Add(ms);
                    Messages.Add(ms2);
                    StreamWriter fw = new StreamWriter(@"config.txt");
                    fw.WriteLine(message);
                    fw.Close();
                }
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Config.txt File not exist.");
            }
        }
    }
    public  class StringCipher
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        private const int Keysize = 256;

        // This constant determines the number of iterations for the password bytes generation function.
        private const int DerivationIterations = 1000;

        public  string Decrypt(string cipherText, string passPhrase)
        {
            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [32 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Get the saltbytes by extracting the first 32 bytes from the supplied cipherText bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 32 bytes from the supplied cipherText bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipherText string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        private  byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }
    }
}
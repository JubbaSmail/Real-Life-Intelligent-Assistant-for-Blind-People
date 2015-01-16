using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Media;

namespace SpeechUI
{
    class Program
    {
        static SoundPlayer spError = new SoundPlayer("error.wav");
        static SoundPlayer spOk = new SoundPlayer("ok.wav");
        static SoundPlayer spWaiting = new SoundPlayer("waiting.wav");
        static SoundPlayer spRegister = new SoundPlayer("register.wav");
        static SoundPlayer spLogin = new SoundPlayer("login.wav");
        static SpeechSynthesizer synth = new SpeechSynthesizer();
        static int recv;
        static byte[] data;
        static IPEndPoint ipep;
        static Socket newsock;
        static IPEndPoint sender;
        static EndPoint Remote;
        static bool recognitionAllowed = true;
        static bool askAgain = false;
        static System.Collections.ObjectModel.ReadOnlyCollection<RecognizedWordUnit> myWords;
        static SpeechRecognitionEngine recognitionEngine;
        static string recognizedSpeech;
        static string previousSpeech;
        static bool isWaiting = false;
        static bool isRegistering = false;
        // static bool waitingYesOrNo = false;
        // static DateTime dt;

        static private void Communicate(SpeechRecognizedEventArgs args, string answer)
        {
            data = new byte[1024];
            byte[] size = new byte[1];
            Console.WriteLine("Processing ...");

            if (answer == "")
            {
                myWords = args.Result.Words;
                recognizedSpeech = "";
                foreach (RecognizedWordUnit word in myWords)
                {
                    if (word.Confidence < 0.9f)
                        askAgain = true;
                    recognizedSpeech += word.Text + " ";
                }
                data = Encoding.ASCII.GetBytes(recognizedSpeech);
                if (askAgain)
                {
                    Console.WriteLine("Have you said " + recognizedSpeech + " ?"); //tts
                    synth.Speak("Have you said " + recognizedSpeech);
                    previousSpeech = recognizedSpeech;
                    foreach (RecognizedWordUnit word in myWords)
                    {
                        Console.WriteLine(word.Text + " " + word.Confidence + " ");
                    }
                    //  dt = DateTime.Now;
                    // waitingYesOrNo = true;
                    return; //.................................................
                }
            }
            else if (answer == "Yes")
            {
                data = Encoding.ASCII.GetBytes(previousSpeech);
            }

            foreach (RecognizedWordUnit word in args.Result.Words)
            {
                // You can change the minimun confidence level here
                //if (word.Confidence > 0.8f)
                Console.WriteLine(word.Text + " ( " + word.Confidence.ToString() + " ) " + " ");
            }
            Console.WriteLine(Environment.NewLine);

            size[0] = (byte)data.Length;
            newsock.SendTo(size, 1, SocketFlags.None, Remote);// sending size..................
            newsock.SendTo(data, data.Length, SocketFlags.None, Remote);// sending data..................
            byte[] recvByte = new byte[1];
            bool isRecvd = false; // has client recieved message ?
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    recv = newsock.ReceiveFrom(recvByte, ref Remote);//receive byte-----------------------
                    if (recvByte[0] == 0) // if error
                    {
                        spError.PlaySync();
                        Console.WriteLine("not executed");
                        break;
                    }
                    else if (recvByte[0] == 1) // if succeeded
                    {
                        isRecvd = true;
                        isWaiting = false;
                        spWaiting.Stop(); // stops waiting sound.
                        Console.WriteLine("executed successfully");
                        spOk.PlaySync(); // message sent successfully
                        break;
                    }
                    else if (recvByte[0] == 3) // 3 for registering ...
                    {
                        spRegister.PlayLooping();
                        isRegistering = true;
                        isRecvd = true;
                        Console.WriteLine("registering...Say stop when you finish");
                        break;
                    }
                }
                catch (Exception) // if timeout
                {
                    newsock.SendTo(size, 1, SocketFlags.None, Remote);// sending size..................
                    newsock.SendTo(data, data.Length, SocketFlags.None, Remote);// sending data..................
                }
            } //end for loop


        }

        static private void Initialize()
        {
            synth.Volume = 100;

            recognitionEngine = new SpeechRecognitionEngine();
            recognitionEngine.SetInputToDefaultAudioDevice();
            recognitionEngine.SpeechRecognized += (s, args) => //........................................
            {
                if (args.Result.Words[0].Text == "ALO")
                {
                    if (isWaiting)
                    {
                        synth.Speak("Processing your command. Please wait.");
                    }
                    else if (isRegistering)
                    {
                        Console.WriteLine("Help");
                        synth.Speak("Registering. Say stop, to save path.");
                    }
                }
                else if (isRegistering)
                {
                    if (args.Result.Words[0].Text == "Stop")
                    {
                        isRegistering = false;
                        Communicate(args, "");
                    }
                    return;
                }
                else if (askAgain)
                {
                    askAgain = false;
                    //  waitingYesOrNo = true;
                    if (args.Result.Words[0].Text == "Yes")
                    {
                        Communicate(args, "Yes");
                    }
                    else
                    {
                        if (isRegistering)
                            spRegister.PlayLooping();
                        Console.WriteLine("Repeat Again Please...");
                    }
                }
                else
                {
                    if (args.Result.Words[0].Text != "Yes" && args.Result.Words[0].Text != "No")
                    {
                        Communicate(args, "");
                    }
                    else
                    {
                        Console.WriteLine("Ignored");
                    }
                }
            };
        }

        static private Grammar CreateSampleGrammar()
        {
            GrammarBuilder gbr_help = new GrammarBuilder("ALO");
            GrammarBuilder gbr_takeME = new GrammarBuilder("Take me to");
            GrammarBuilder gbr_emergency = new GrammarBuilder("Emergency");
            GrammarBuilder gbr_stop = new GrammarBuilder("Stop");
            GrammarBuilder gbr_time = new GrammarBuilder("Time");
            GrammarBuilder gbr_mail = new GrammarBuilder("Mail");
            GrammarBuilder gbr_register = new GrammarBuilder("Register");
            GrammarBuilder gbr_callFriend = new GrammarBuilder("Call friend");
            GrammarBuilder gbr_shutDown = new GrammarBuilder("Shutdown");
            GrammarBuilder gbr_whereAmI = new GrammarBuilder("Where am I");
            GrammarBuilder gbr_yes = new GrammarBuilder("Yes");
            GrammarBuilder gbr_no = new GrammarBuilder("No");

            Choices ch_takeMe = new Choices(File.ReadLines("Places.txt").ToArray());
            gbr_takeME.Append(ch_takeMe);

            GrammarBuilder gb_sendTo = new GrammarBuilder("Send to");
            Choices ch_sendTo = new Choices(File.ReadLines("Friends.txt").ToArray());
            gb_sendTo.Append(ch_sendTo);
            Choices ch_mail = new Choices("Incoming", "Outgoing", gb_sendTo);
            gbr_mail.Append(ch_mail);

            GrammarBuilder gb_location = new GrammarBuilder("Path");
            Choices ch_location = new Choices(File.ReadLines("Places.txt").ToArray());
            gb_location.Append(ch_location);
            Choices ch_register = new Choices("Location ", gb_location);
            gbr_register.Append(ch_register);

            Choices ch_callFriend = new Choices(File.ReadLines("Friends.txt").ToArray());
            gbr_callFriend.Append(ch_callFriend);

            Choices root = new Choices(gbr_stop, gbr_help, gbr_callFriend, gbr_emergency, gbr_mail, gbr_no, gbr_register,
                gbr_shutDown, gbr_time, gbr_takeME, gbr_whereAmI, gbr_yes);

            GrammarBuilder grammarBuilder = new GrammarBuilder(root);

            Grammar g = new Grammar(grammarBuilder);
            g.Name = "Available programs";
            return g;
        }

        static void Main(string[] args)
        {
            data = new byte[1024];
            ipep = new IPEndPoint(IPAddress.Any, 9050); // my port
            newsock = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            newsock.Bind(ipep);
            Console.WriteLine("Waiting for a client...");
            sender = new IPEndPoint(IPAddress.Any, 0); // my client (address + port)
            Remote = (EndPoint)(sender);
            newsock.ReceiveTimeout = 0; //milliseconds
            ////run client
            //Process.Start("Main_Client.exe");
            //////
            recv = newsock.ReceiveFrom(data, ref Remote); // receive "OK" message .-------------------------
            string received = Encoding.ASCII.GetString(data, 0, recv);
            Console.WriteLine(received);
            Console.WriteLine("Message received from {0}:", Remote.ToString());
            newsock.ReceiveTimeout = 0; //milliseconds
            Console.WriteLine("Speak please...");
            spLogin.Play();
            Initialize();
            try
            {
                recognitionEngine.UnloadAllGrammars();
                Grammar cg = CreateSampleGrammar();
                recognitionEngine.LoadGrammar(cg);
                recognitionEngine.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            string command = Console.ReadLine();

        }
    }
}


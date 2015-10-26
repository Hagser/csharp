using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using SSS=System.Speech.Synthesis;
using SSR=System.Speech.Recognition;
using System.Configuration;

namespace Text2Speech
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SSS.SpeechSynthesizer ss = new System.Speech.Synthesis.SpeechSynthesizer();
        SSR.SpeechRecognitionEngine sre = new System.Speech.Recognition.SpeechRecognitionEngine();

        bool bListen = false;
        bool bSpell = false;
        bool bAddWordQ = false;
        bool bAddWordA = false;
        bool bSpeaking = false;

        string strNewword = "";
        string strSaidLast = "";

        Hashtable htWords = new Hashtable();
        AppSettingsReader asr = new AppSettingsReader();
        string strOutputFile = Application.StartupPath + "\\"+ DateTime.Today.ToShortDateString().Replace("-","") +".wav";
        bool bSaveFile = false;
        
        string strGrammarFile = "";
        
        private void delOutFile()
        {
            try
            {
                delOutFile(strOutputFile);
            }
            catch(Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void delOutFile(string in_filename)
        {
            try
            {
                if (File.Exists(in_filename))
                {
                    File.Delete(in_filename);
                }
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void loadGrammar()
        {
            sre.UnloadAllGrammars();
            htWords.Clear();
            StreamReader sr = File.OpenText(strGrammarFile);
            int icnt = 0;
            while (!sr.EndOfStream && icnt <10000)
            {
                string strLine = sr.ReadLine();
                if (strLine != "")
                {
                    SSR.GrammarBuilder gb = new System.Speech.Recognition.GrammarBuilder();
                    gb.Append(strLine);
                    SSR.Grammar gram = new System.Speech.Recognition.Grammar(gb);
                    sre.LoadGrammar(gram);

                    htWords.Add(htWords.Count, strLine.ToLower());
                }
                icnt++;
            }
        }
        private void speak(bool in_async, string in_text)
        {
            try
            {

                SSS.PromptBuilder pb = new System.Speech.Synthesis.PromptBuilder();
                pb.AppendText(in_text);
                strSaidLast = in_text;
                ListViewItem lvi = lvSpoken.Items.Add(in_text);
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                speak(in_async,pb);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(string in_text)
        {
            try
            {

                SSS.PromptBuilder pb = new System.Speech.Synthesis.PromptBuilder();
                pb.AppendText(in_text);
                strSaidLast = in_text;
                ListViewItem lvi = lvSpoken.Items.Add(in_text);
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                speak(pb);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }

        private void speak(bool in_async, string in_text, bool in_save_to_file)
        {
            try
            {
                SSS.PromptBuilder pb = new System.Speech.Synthesis.PromptBuilder();
                pb.AppendText(in_text);
                strSaidLast = in_text;
                ListViewItem lvi = lvSpoken.Items.Add(in_text);
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                speak(in_async, pb, in_save_to_file);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(bool in_async, string in_text, bool in_save_to_file,string in_filename)
        {
            try
            {
                SSS.PromptBuilder pb = new System.Speech.Synthesis.PromptBuilder();
                pb.AppendText(in_text);
                strSaidLast = in_text;
                ListViewItem lvi = lvSpoken.Items.Add(in_text);
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                speak(in_async, pb, in_save_to_file, in_filename);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(string in_text, bool in_save_to_file)
        {
            try
            {
                SSS.PromptBuilder pb = new System.Speech.Synthesis.PromptBuilder();
                pb.AppendText(in_text);
                strSaidLast = in_text;
                ListViewItem lvi = lvSpoken.Items.Add(in_text);
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                speak(pb, in_save_to_file);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(bool bAsync,System.Speech.Synthesis.PromptBuilder pb)
        {
            try
            {
                if (bSaveFile)
                {
                    ss.SetOutputToNull();
                    delOutFile();
                    ss.SetOutputToWaveFile(strOutputFile);
                }
                if (bAsync)
                {
                    ss.SpeakAsync(pb);
                }
                else
                {
                    ss.Speak(pb);
                }
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(System.Speech.Synthesis.PromptBuilder pb)
        {
            try
            {
                if (bSaveFile)
                {
                    ss.SetOutputToNull();
                    delOutFile();
                    ss.SetOutputToWaveFile(strOutputFile);
                }

                ss.SpeakAsync(pb);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(bool bAsync, System.Speech.Synthesis.PromptBuilder pb, bool in_save_to_file)
        {
            try
            {
                if (in_save_to_file)
                {
                    ss.SetOutputToNull();
                    delOutFile();
                    ss.SetOutputToWaveFile(strOutputFile);
                }
                if (bAsync)
                {
                    ss.SpeakAsync(pb);
                }
                else
                {
                    ss.Speak(pb);
                }
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void speak(bool bAsync, System.Speech.Synthesis.PromptBuilder pb, bool in_save_to_file, string in_filename)
        {
            try
            {
                if (in_save_to_file)
                {
                    SSS.PromptStyle ps = new System.Speech.Synthesis.PromptStyle(SSS.PromptRate.ExtraSlow);
                    ps.Emphasis = SSS.PromptEmphasis.Moderate ;
                    ps.Volume = SSS.PromptVolume.Soft;
                    pb.AppendBreak(new TimeSpan(0,0,1));
                    pb.StartStyle(ps);
                    ss.SetOutputToNull();
                    delOutFile(Application.StartupPath + "\\" + in_filename);
                    ss.SetOutputToWaveFile(Application.StartupPath + "\\" + in_filename);
                    pb.EndStyle();
                }
                if (bAsync)
                {
                    ss.SpeakAsync(pb);
                }
                else
                {
                    ss.Speak(pb);
                }
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        
        private void speak(System.Speech.Synthesis.PromptBuilder pb,bool in_save_to_file)
        {
            try
            {
                if (in_save_to_file)
                {
                    ss.SetOutputToNull();
                    delOutFile();
                    ss.SetOutputToWaveFile(strOutputFile);
                }
                ss.SpeakAsync(pb);
            }
            catch (Exception ex)
            {
                speak("An error occured:" + ex.Message, false);
            }
        }
        private void addWord(string in_word)
        {
            using (StreamWriter sw = File.AppendText(strGrammarFile))
            {
                if (!htWords.ContainsValue(in_word.ToLower()))
                {
                    sw.WriteLine(in_word);
                    htWords.Add(htWords.Count, in_word.ToLower());
                }
                sw.Close();
            }
            loadGrammar();
            
        }

        private void tsi_Click(object sender, EventArgs e)
        {
            ToolStripItem tsi = (ToolStripItem)sender;
            ss.SelectVoice(tsi.Tag.ToString());
            speak("You have selected voice: " + tsi.Tag.ToString(),false);
        }
        private void btnSpeak_Click(object sender, EventArgs e)
        {            
            if (txtText.Text.Length > 0)
            {
                speak(txtText.Text);
            }
            else
            {
                speak("Enter a text for me to speak!",false);
            }
            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            
            strGrammarFile = asr.GetValue("strGrammarFile","".GetType()).ToString();

            foreach (System.Speech.Synthesis.InstalledVoice iv in ss.GetInstalledVoices())
            {
                SSS.VoiceInfo vi = iv.VoiceInfo;
                string strName = vi.Name + "/" + vi.Age.ToString() + "/" + vi.Gender.ToString() + "/" + vi.Culture.DisplayName;
                ToolStripItem tsi = setVoiceToolStripMenuItem.DropDownItems.Add(strName);
                tsi.Tag = vi.Name;
                tsi.Click += new EventHandler(tsi_Click);
            }
            sre.SpeechDetected += new EventHandler<System.Speech.Recognition.SpeechDetectedEventArgs>(sre_SpeechDetected);
            sre.SpeechRecognized += new EventHandler<System.Speech.Recognition.SpeechRecognizedEventArgs>(sre_SpeechRecognized);

            ss.SpeakStarted += new EventHandler<System.Speech.Synthesis.SpeakStartedEventArgs>(ss_SpeakStarted);
            ss.SpeakCompleted += new EventHandler<System.Speech.Synthesis.SpeakCompletedEventArgs>(ss_SpeakCompleted);

            loadGrammar();
            sre.SetInputToDefaultAudioDevice();
            sre.RecognizeAsync(SSR.RecognizeMode.Multiple);
        }
        private void ss_SpeakCompleted(object sender, System.Speech.Synthesis.SpeakCompletedEventArgs e)
        {
            bSpeaking = false;
            tSSLblDetect.Text = "SpeakCompleted:" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString();
        }
        private void ss_SpeakStarted(object sender, System.Speech.Synthesis.SpeakStartedEventArgs e)
        {
            bSpeaking = true;
            tSSLblDetect.Text = "SpeakStarted:" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString();
        }
        private void tim_time_Tick(object sender, EventArgs e)
        {
            speak(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), false);
        }
        private void sre_SpeechRecognized(object sender, System.Speech.Recognition.SpeechRecognizedEventArgs e)
        {
            if (!bSpeaking)
            {
                ListViewItem lvi= lvReq.Items.Add(e.Result.Text);
                lvi.SubItems.Add((Math.Round(e.Result.Confidence,2)*100).ToString() + "%");
                lvi.SubItems.Add(DateTime.Now.ToLongTimeString());
                lvi.EnsureVisible();
                //foreach (SSR.RecognizedPhrase rp in e.Result.Alternates){
                    string rpt = e.Result.Text;
                    if (rpt.Equals("listen"))
                    {
                        bListen = true;
                        speak("OK, talk to me.");
                    }
                    if (rpt.Equals("stop") && bListen)
                    {
                        bListen = false;
                        bSpell = false;
                        bAddWordA = false;
                        bAddWordQ = false;
                        strNewword = "";
                        speak("My ears are shut.");
                    }
                    else if (bListen)
                    {
                        if (rpt.Equals("add word") || rpt.Equals("add new word") || rpt.Equals("new word"))
                        {
                            bSpell = true;
                            bAddWordQ = false;
                            strNewword = "";
                            speak("Spell the word and say done, when done.");
                        }
                        else if (bSpell && rpt.Length == 1)
                        {
                            strNewword += rpt;
                            speak(strNewword);
                        }
                        else if (bSpell && !bAddWordQ && rpt.Equals("no"))
                        {
                            strNewword = strNewword.Length > 1 ? strNewword.Substring(0, strNewword.Length - 1) : "";
                            speak(strNewword);
                        }
                        else if (bSpell && rpt.Equals("done"))
                        {
                            bAddWordQ = true;
                            speak("Add word '" + strNewword + "'?");
                        }
                        else if (bSpell && bAddWordQ && rpt.Equals("yes"))
                        {
                            bSpell = false;
                            bAddWordQ = false;
                            bAddWordA = true;
                            if(htWords.ContainsValue(strNewword.ToLower()))
                            {
                                speak("Word '" + strNewword + "' already exists.");
                            }
                            else
                            {
                                addWord(strNewword);
                                speak("Word '" + strNewword + "' added.");
                            }
                            strNewword = "";
                        }
                        else if (bSpell && bAddWordQ && rpt.Equals("no"))
                        {
                            bAddWordQ = false;
                            bAddWordA = true;
                            speak(strNewword);
                        }
                        /*else if (bSpell && rpt.Length != 1)
                        {
                            speak("Please repeat!");
                        }*/
                        else if (bSpell && rpt.Length > 1)
                        {
                            strNewword += strNewword.Length>0?" " + rpt:rpt;
                            speak(strNewword);
                        }
                        else if (rpt.Equals("hello"))
                        {
                            speak("hello");
                        }
                        else if (rpt.Equals("thanks"))
                        {
                            speak("Don't mention it.");
                        }
                        else if (rpt.Equals("repeat"))
                        {
                            if (strSaidLast != "")
                            {
                                speak(strSaidLast);
                            }
                        }
                        else if (rpt.Equals("tell me the time"))
                        {
                            speak(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(), false);
                        }
                        else if (rpt.Equals("exit"))
                        {
                            Application.Exit();
                        }
                    }
                //}
            }
        }
        private void sre_SpeechDetected(object sender, System.Speech.Recognition.SpeechDetectedEventArgs e)
        {
            if (!bSpeaking)
            {
                tSSLblDetect.Text = "SD:" + DateTime.Now.ToLongTimeString() + "." + DateTime.Now.Millisecond.ToString();
            }
        }
        private void txtText_KeyUp(object sender, KeyEventArgs e)
        {
            if (sayWhatIWriteToolStripMenuItem.Checked)
            {
                this.Text = e.KeyValue.ToString();
                if (afterSentenceToolStripMenuItem.Checked)
                {
                    if ((e.Shift && (e.KeyValue == 49 || e.KeyValue == 187)) || e.KeyValue == 190)
                    {
                        string strSay = txtText.Text;
                        int idot = strSay.LastIndexOf(".",strSay.Length-2);
                        int iexcl = strSay.LastIndexOf("!", strSay.Length-2);
                        int iques = strSay.LastIndexOf("?", strSay.Length-2);
                        int istart=Math.Max(Math.Max(idot,iexcl),iques);

                        strSay = strSay.Length - 1 == istart || istart==-1 ? strSay : strSay.Substring(istart+1);
                        speak(strSay);
                    }                
                }
                else if (afterSpaceToolStripMenuItem.Checked)
                {
                    if (e.KeyCode == Keys.Space || (e.Shift && (e.KeyValue == 49 || e.KeyValue == 187)) || e.KeyValue == 190)
                    {
                        string strSay = txtText.Text;
                        string[] strSayArr = strSay.Split(System.Text.Encoding.Default.GetChars(System.Text.Encoding.Default.GetBytes(" ")));
                        if (strSayArr.Length > 0)
                        {
                            if ((e.Shift && (e.KeyValue == 49 || e.KeyValue == 187)) || e.KeyValue == 190)
                            {
                                strSay = strSayArr[strSayArr.Length - 1];
                            }
                            else
                            {
                                strSay = strSayArr[strSayArr.Length - 2];
                            }
                            speak(strSay);
                        }
                    }
                }
            }
        }
        private void sayWhatIWriteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!sayWhatIWriteToolStripMenuItem.Checked)
            {
                afterSpaceToolStripMenuItem.Checked = sayWhatIWriteToolStripMenuItem.Checked;
                afterSentenceToolStripMenuItem.Checked = sayWhatIWriteToolStripMenuItem.Checked;
            }
        }
        private void afterSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            afterSentenceToolStripMenuItem.Checked = !afterSpaceToolStripMenuItem.Checked;
            sayWhatIWriteToolStripMenuItem.Checked = afterSpaceToolStripMenuItem.Checked;
        }
        private void afterSentenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            afterSpaceToolStripMenuItem.Checked = !afterSentenceToolStripMenuItem.Checked;
            sayWhatIWriteToolStripMenuItem.Checked = afterSentenceToolStripMenuItem.Checked;
        }
        private void tellMeTheTimeToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            tim_time.Enabled = tellMeTheTimeToolStripMenuItem.Checked;
            if (tellMeTheTimeToolStripMenuItem.Checked)
            {
                speak(DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(),false);
            }
        }
        private void saveToFileToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            bSaveFile = saveToFileToolStripMenuItem.Checked;
            if (bSaveFile)
            {
                speak("What I say will be saved to a file.",false);
            }
            else
            {
                speak("What I say will NOT be saved to a file.", false);
            }
        }

        private void saveToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void readFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text file|*.txt";
            ofd.Multiselect = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string[] strFilenames = ofd.FileNames;
                foreach (string strFilename in strFilenames)
                {
                    FileInfo fi = new FileInfo(strFilename);
                    using (StreamReader sr = File.OpenText(strFilename))
                    {
                        string strText = sr.ReadToEnd();
                        sr.Close();
                        strText = strText.Replace("*", "").Replace("[", "").Replace("]", "").Replace("\"", "").Replace("'", "").Replace(":", ".").Replace("\r\n", " ");
                        speak(false, strText, true,Path.GetFileNameWithoutExtension(fi.Name) + ".wav");
                    }
                }
                MessageBox.Show("All saved!");
            }
            ofd.Dispose();

        }

    }
}

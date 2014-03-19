#region DISCLAIMER
/* Copyright (c) 2013 EVO Payments International - All Rights Reserved.
 *
 * This software and documentation is subject to and made
 * available only pursuant to the terms of an executed license
 * agreement, and may be used only in accordance with the terms
 * of said agreement. This software may not, in whole or in part,
 * be copied, photocopied, reproduced, translated, or reduced to
 * any electronic medium or machine-readable form without
 * prior consent, in writing, from EVO Payments International, INC.
 *
 * Use, duplication or disclosure by the U.S. Government is subject
 * to restrictions set forth in an executed license agreement
 * and in subparagraph (c)(1) of the Commercial Computer
 * Software-Restricted Rights Clause at FAR 52.227-19; subparagraph
 * (c)(1)(ii) of the Rights in Technical Data and Computer Software
 * clause at DFARS 252.227-7013, subparagraph (d) of the Commercial
 * Computer Software--Licensing clause at NASA FAR supplement
 * 16-52.227-86; or their equivalent.
 *
 * Information in this software is subject to change without notice
 * and does not represent a commitment on the part of EVO Payments International.
 * 
 * Sample Code is for reference Only and is intended to be used for educational purposes. It's the responsibility of 
 * the software company to properly integrate into thier solution code that best meets thier production needs. 
*/
#endregion DISCLAIMER

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using SampleCode.CwsTransactionProcessing;

namespace SampleCode
{
    public partial class Magensa : Form
    {
        public BankcardTransaction _bct;
        public TestTriggers TT;
        public bool ProcessTransaction;
        public string MaskedPAN
        {
            get
            {
                return TxtMaskedPAN.Text;
            }
            //set
            //{
            //    TxtMaskedPAN.Text = value;
            //}
        }
        public TypeCardType CardType
        {
            get
            {
                return (TypeCardType)CboMagensaCardType.SelectedItem;
            }
            //set
            //{
            //    (TypeCardType)CboMagensaCardType.SelectedItem = value;
            //}
        }

        public Magensa()
        {
            InitializeComponent();

            CboTriggerTests.Items.Add(new TestTriggers("10.00", "E860", "Magensa returned OK."));
            CboTriggerTests.Items.Add(new TestTriggers("10.05", "E860", "Magensa error number: Y001. No PAN Found in Track2 Data."));
            CboTriggerTests.Items.Add(new TestTriggers("10.10", "E860", "Magensa error number: Y091. Invalid KSID."));
            CboTriggerTests.Items.Add(new TestTriggers("10.15", "E860", "Magensa error number: Y093. Invalid MagnePrint."));
            CboTriggerTests.Items.Add(new TestTriggers("10.20", "E860", "Magensa error number: Y094. Invalid MagnePrint. Invalid transaction CRC/PAN."));
            CboTriggerTests.Items.Add(new TestTriggers("10.25", "E860", "Magensa error number: Y095. Error Scoring Card."));
            CboTriggerTests.Items.Add(new TestTriggers("10.30", "E860", "Magensa error number: Y096. Inactive MagnePrint reference."));
            CboTriggerTests.Items.Add(new TestTriggers("10.35", "E860", "Magensa error number: Y097. DUKPT KSN and counter is replayed."));
            CboTriggerTests.Items.Add(new TestTriggers("10.40", "E860", "Magensa error number: Y098. Problem Decrypting Data."));
            CboTriggerTests.Items.Add(new TestTriggers("10.45", "E860", "Magensa error number: Y099. Error Validating Credentials."));
            CboTriggerTests.Items.Add(new TestTriggers("10.50", "E898", "Magensa returned an invalid data error: <Insert Magensa StatusCode>. <Insert Magensa input validation error "));
            CboTriggerTests.Items.Add(new TestTriggers("10.55", "E899", "Magensa returned an unknown error.  <Insert Magensa StatusCode>. <StatusMsg>."));

            CboMagensaCardType.Sorted = true;
            CboMagensaCardType.DataSource = Enum.GetValues(typeof(TypeCardType));
            CboMagensaCardType.SelectedIndex = -1;
        }

        public void CallingForm(ref BankcardTransaction bct)
        {
            _bct = bct;
            TxtAmount.Text = _bct.TransactionData.Amount.ToString();
        }

        private void CmdGenerateFields_Click(object sender, EventArgs e)
        {
           
            // The below code is specific to the newer version of the MagTek Dynamag USB MSR
            // It takes the raw swipe generated from the Dynamag to populate the correct lines below.
            try
            {
                if (ChkPipeDelimited.Checked)
                {
                    string[] lines = TxtTrackInformation.Text.Split('|');

                    if (lines.Length < 13)
                        MessageBox.Show("No Magensa Swipes to use");
                    else
                    {
                        string encryptedTrack2 = lines[3];
                        string encryptedMagnePrintData = lines[6];
                        string DUKPTserialNo = lines[9];
                        string magenPrintStatus = lines[5];
                        string deviceSerialNum = lines[7];
                        string txtTrack2Masked = lines[0].Substring(lines[0].IndexOf(";") + 1).Replace("?", "").Trim();
                        //string pan = txtTrack2Masked.Substring(0, 16);
                        string[] track2 = txtTrack2Masked.Split('=');
                        string panTrack2 = track2[0];
                        string discTrack2 = track2[1];

                        //Get Masked Pan and CardType


                        HelperMethods h = new HelperMethods();


                        int masklength = panTrack2.Length - 10;
                        int count = 0;
                        string maskingCount = panTrack2.Substring(0, 6);
                        do
                        {
                            maskingCount += "X";
                            count++;
                        } while (count < masklength);

                        TxtMaskedPAN.Text = maskingCount + panTrack2.Substring(panTrack2.Length - 4, 4);

                        masklength = discTrack2.Length - 4;
                        maskingCount = discTrack2.Substring(0, 4);
                        count = 0;
                        do
                        {
                            maskingCount += "X";
                            count++;
                        } while (count < masklength);

                        TxtTrack2Masked.Text = TxtMaskedPAN.Text + "=" + maskingCount;

                        try { CboMagensaCardType.SelectedItem = h.CardTypeLookup(panTrack2); }
                        catch { }

                        TxtMagnePrintData.Text = encryptedMagnePrintData;
                        TxtDukptKeySerialNumber.Text = DUKPTserialNo;
                        TxtTrack2EncryptedData.Text = encryptedTrack2;
                        TxtMagnePrintStatus.Text = magenPrintStatus;
                        TxtDeviceSerialNumber.Text = deviceSerialNum;
                    }
                }
                else
                {
                    //StringReader strReader = new StringReader(TxtTrackData.Text);
                    string[] lines = TxtTrackInformation.Text.Replace("\r", "").Split('\n');

                    if (lines.Length < 21)
                        MessageBox.Show("No Magensa Swipes to use");
                    else
                    {

                        string encryptedTrack2 = lines[6].Substring(lines[6].IndexOf("=") + 1).Replace(" ", "");
                        string encryptedMagnePrintData = lines[12].Substring(lines[12].IndexOf("=") + 1).Replace(" ", "");
                        string DUKPTserialNo = lines[2].Substring(lines[2].IndexOf("=") + 1).Replace(" ", "");
                        string magenPrintStatus = lines[10].Substring(lines[10].IndexOf("=") + 1).Replace(" ", "");
                        string deviceSerialNum = lines[14].Substring(lines[14].IndexOf("=") + 2);

                        string txtTrack2Masked = lines[18].Substring(lines[18].IndexOf(";") + 1).Replace("?", "").Trim();
                        string[] track2 = txtTrack2Masked.Split('=');
                        string panTrack2 = track2[0];
                        string discTrack2 = track2[1];

                        //Get Masked Pan and CardType
                        TxtTrack2Masked.Text = lines[18].Substring(lines[18].IndexOf("=") + 1).Replace(" ", "").Trim();
                        HelperMethods h = new HelperMethods();

                        int masklength = panTrack2.Length - 8;
                        int count = 0;
                        string maskingCount = panTrack2.Substring(0, 4);
                        do
                        {
                            maskingCount += "X";
                            count++;
                        } while (count < masklength);

                        TxtMaskedPAN.Text = maskingCount + panTrack2.Substring(panTrack2.Length - 4, 4);

                        masklength = discTrack2.Length - 4;
                        maskingCount = discTrack2.Substring(0, 4);
                        count = 0;
                        do
                        {
                            maskingCount += "X";
                            count++;
                        } while (count < masklength);

                        TxtTrack2Masked.Text = TxtMaskedPAN.Text + "=" + maskingCount;

                        try { CboMagensaCardType.SelectedItem = h.CardTypeLookup(panTrack2); }
                        catch { }

                        TxtMagnePrintData.Text = encryptedMagnePrintData;
                        TxtDukptKeySerialNumber.Text = DUKPTserialNo;
                        TxtTrack2EncryptedData.Text = encryptedTrack2;
                        TxtMagnePrintStatus.Text = magenPrintStatus;
                        TxtDeviceSerialNumber.Text = deviceSerialNum;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    

        private void CmdUseValues_Click(object sender, EventArgs e)
        {
            if (TxtMagnePrintData.Text.Length < 1 | TxtDukptKeySerialNumber.Text.Length < 1 | TxtTrack2EncryptedData.Text.Length < 1 | TxtMagnePrintStatus.Text.Length <1)
            {
                MessageBox.Show("Required information is missing");
                return;
            }

            _bct.TenderData.CardData = null;
            if (_bct.TenderData.CardSecurityData == null) { _bct.TenderData.CardSecurityData = new CardSecurityData(); }
            
            _bct.TenderData.CardSecurityData.CVData = null;
            _bct.TenderData.CardSecurityData.CVDataProvided = CVDataProvided.NotSet;
            _bct.TenderData.CardSecurityData.IdentificationInformation = TxtMagnePrintData.Text;
            _bct.TenderData.SecurePaymentAccountData = TxtTrack2EncryptedData.Text;
            _bct.TenderData.EncryptionKeyId = TxtDukptKeySerialNumber.Text;
            _bct.TenderData.SwipeStatus = TxtMagnePrintStatus.Text;
            _bct.TenderData.DeviceSerialNumber = TxtDeviceSerialNumber.Text;
            _bct.TransactionData.EntryMode = EntryMode.Track2DataFromMSR;
            // Note Reference should be a unique value for each transaction being sent.
            _bct.TransactionData.Reference = "11";

            _bct.TransactionData.Amount = Convert.ToDecimal(TxtAmount.Text);
            if (TxtScoreThreshold.Text.Length > 0)
                _bct.TransactionData.ScoreThreshold = TxtScoreThreshold.Text;

            ProcessTransaction = true;
            this.Close();
        }

        private void CboTriggerTests_SelectedIndexChanged(object sender, EventArgs e)
        {
            TT = (TestTriggers) CboTriggerTests.SelectedItem;
            TxtAmount.Text = TT.Trigger;
        }

        private void CmdPopulateWithTestValues_Click(object sender, EventArgs e)
        {
            if (ChkPipeDelimited.Checked)
            {
                // The below information is from the latest version of the MagTek Dynamag USB MSR
                TxtTrackInformation.Text =
                    "%B5499990001006781^TESTCARD/MC/NDCECOMMERCE^15120000000000000000?;5499990001006781=15120000000000000000?|0600|AE9F6AE3EBD5A16B0369884F57EAC1C5DA7C4BD34004BA81421E9BF7CA3C0F0494264E258128C078DF377F6E453A70E31B29DD624A02D41D9B04A5D575F2BE9FC927F06D85FADAE8|C126841EDE220A5FA3A1E147A6A83F813D53644E6082195DBDD29203AAD8B38447180B9E4902CE1B||61403000|914CEA90C623ED14B9DE35CDB3F05F74AC5ECD8F90A8C1A8AA6945C0895E05A718437CEBDCD8C1AA020FD4F1E4EE99E3D74A5850AB9034B7|B1CBB65090413AA|D956BFF98750C117|9011880B1CBB6500006B|1422||1000";
            }
            else
                TxtTrackInformation.Text =
                    "Card Encode Type = ISO\r\n\r\nDUKPT Key Serial Number = 9010010B0C247200001F\r\n\r\nTrack 1 Encrypted = CF BD 82 96 5A 66 FF C8 8D 51 2C 8D 1C 4B 1D B6 7C 45 5B 6C 1E 36 17 6A 20 26 BF 27 94 2F C5 71 7D 89 B1 BE A7 9B 88 14 26 47 9C 51 FA 30 1C 2D 99 F6 83 78 A0 66 4C 0D 79 6A 08 36 95 F6 1F 30 \r\n\r\nTrack 2 Encrypted = EA D1 C2 F9 6D DE 13 1B 35 6B 2C 6C 98 25 D3 68 C9 C6 F7 26 84 75 72 FC BC 78 81 18 C4 9F 46 F7 87 6E A8 7A 71 75 E4 7A \r\n\r\nTrack 3 Encrypted = \r\n\r\nMagnePrint Status (hex) = 00304061\r\n\r\nMagnePrint Data (hex) = A3 63 34 A6 4D C8 A5 35 67 EE 0F 28 AD B6 5E EB F1 5D 2B 22 AB 39 2F C2 DC 79 E3 4A A4 90 C3 6E 6E C1 90 A1 F3 E5 19 63 8F CC AA F0 37 23 CA CA 05 84 43 1C AE 3E 1B B1 \r\n\r\nDevice Serial Number = B0C2472071812AA\r\n\r\nTrack 1 Masked = %B4111000010001111^IPCOMMERCE/TESTCARD^13120000000000000000000?\r\n\r\nTrack 2 Masked = ;4111000010001111=13120000000000000000?\r\n\r\nEncrypted Session ID (Hex) = 14 1B 21 95 AB 89 D8 C9\r\n";

        }

        private void CmdClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lnkManageAccountById_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.magtek.com/support/software/demo_programs/encoder_reader.asp");
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.magtek.com/V2/products/magnesafe/index.asp");
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }

    public class TestTriggers
    {
        public string Trigger;
        public string StatusCode;
        public string StatusMessage;
        public TestTriggers(string trigger, string statusCode, string statusMessage)
        {
            Trigger = trigger;
            StatusCode = statusCode;
            StatusMessage = statusMessage;
        }
        public override string ToString()
        {// Generates the text shown in the combo box
            return Trigger + " - " + StatusCode + " - " + StatusMessage;
        }
    }
}
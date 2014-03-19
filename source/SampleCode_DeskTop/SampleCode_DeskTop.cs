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
using System.Configuration;
using System.IO;
using System.ServiceModel;
using System.Windows.Forms;
using SampleCode.CWSServiceInformation;
using SampleCode.CwsTransactionProcessing;
using Capture = SampleCode.CwsTransactionProcessing.Capture;
using BankcardTransaction = SampleCode.CwsTransactionProcessing.BankcardTransaction;
using BankcardTransactionPro = SampleCode.CwsTransactionProcessing.BankcardTransactionPro;
using BankcardTransactionResponse = SampleCode.CwsTransactionProcessing.BankcardTransactionResponse;
using BankcardTransactionResponsePro = SampleCode.CwsTransactionProcessing.BankcardTransactionResponsePro;
using CardData = SampleCode.CwsTransactionProcessing.CardData;
using CommercialCardResponse = SampleCode.CwsTransactionProcessing.CommercialCardResponse;
using IndustryType = SampleCode.CWSServiceInformation.IndustryType;
using LineItemDetail = SampleCode.CwsTransactionProcessing.LineItemDetail;

#region GENERATING the Proxy with svcUtil.exe
/* Generating the proxy using svcutil.exe
     * Location in Vista : C:\Program Files\Microsoft SDKs\Windows\v6.0A\bin
     * Note:  the use of lists,  and merge switch below. 
     * Note: To contain all of the wsdl and xsd files I created a child folder "CWSSOAP".
     * svcutil.exe CWSSOAP/CwsServiceInformation.wsdl /namespace:*,schemas.evosnap.com.Ipc.General.WCF.Contracts.Common.External.SvcInfo /ct:System.Collections.Generic.List`1 /config:app.config /mergeConfig
     * svcutil.exe https://api.cipcert.goevo.com/2.0.19/Txn?wsdl /namespace:*,schemas.evosnap.com.Ipc.General.WCF.Contracts.Common.External.Txn  /ct:System.Collections.Generic.List`1 /config:app.config /mergeConfig
     * svcutil.exe https://api.cipcert.goevo.com/2.0.19/DataServices?wsdl /namespace:*,schemas.evosnap.com.Ipc.General.WCF.Contracts.Common.External.DataServices  /ct:System.Collections.Generic.List`1 /config:app.config /mergeConfig
     * 
     * Note : for VB.NET customers you should add the switch /language:VB to command lines above
     * 
     * Note: Need to add the following references
     * System.Runtime.Serialization
     * System.ServiceModel
     * 
     * Note : If using SVCUtil.exe to create your config file and you application implements CaptureAll or Capture Selective you 
     * should consider increasing your "closeTimeout" and "sendTimeout" from the default of 1.00 to 5.00 
    */
#endregion END GENERATING the Proxy with svcUtil.exe

namespace SampleCode
{
    public partial class SampleCode_DeskTop : Form
    {
    #region Variable Declarations
        public HelperMethods Helper = new HelperMethods();//The following class performs many of the different operations needs for service information and trasaction processing
        private string _authorizeTxn = "";//Used for a Capture() transaction type.
        private ElectronicCheckingTransactionResponse _queryAccountTxn;//Used for a check Authorize() transaction.
        private StoredValueTransactionResponse _queryAccountSVATxn;//Used for a SVA Authorize() transaction.
        // Unique identifier used to obtain a applicationProfileId and is specific to the certified application
        // A new PTLS SocketId will be provided once your solution is approved for production.
        public string PtlsSocketId = "MIIFCzCCA/OgAwIBAgICAoAwDQYJKoZIhvcNAQEFBQAwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb20wHhcNMTMwODIzMTg1NjA5WhcNMjMwODIxMTg1NjA5WjCBjDELMAkGA1UEBhMCVVMxETAPBgNVBAgTCENvbG9yYWRvMQ8wDQYDVQQHEwZEZW52ZXIxGjAYBgNVBAoTEUlQIENvbW1lcmNlLCBJbmMuMT0wOwYDVQQDEzRxYmtXM25TZ0FJQUFBUDhBSCtDY0FBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx68dD32BLjiDC9RdkIFY2P8N/bzvV75qWWemh0hO3zswggMY6BtKM7xVAoeVbEUP/HxOSlBasKE4tY/Y9hfDoqaszojt5BfqGYqAnUZ/7yjlfROdDu5q1p7AJ8DsEg9o5rpp0/88tj1+XK43JpE0PHtRecCdpsiKGclAdvaGRiXVMR0U6/nNjoNdptSo3Kd8DXSU4xWfiwrVWYUMu9otetiwutJNB3jUfsW5incr1OZ7vkFa58Eltb57UygQ5i31FSrVuBfS4UMQKVBP1V7wsVQlcC+QBNjlsGiATzdqtJBgcaI+BkPEJkF7kpDae3fNbQ77AhVFsoGV30bZCSoSNwIDAQABo4IBTjCCAUowCQYDVR0TBAIwADAdBgNVHQ4EFgQU2t+wf1VVGvks5M1zZlNa92YYUAEwgeYGA1UdIwSB3jCB24AU3+ASnJQimuunAZqQDgNcnO2HuHShgbekgbQwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb22CCQD/yDY5hYVsVzA1BgNVHR8ELjAsMCqgKKAmhiRodHRwOi8vY3JsLmlwY29tbWVyY2UuY29tL2NhLWNybC5jcmwwDQYJKoZIhvcNAQEFBQADggEBAIGOvmbUPdUs3FMbQ95rpT7hShEkAbRnQjp8yY1ql48obQM0mTjQ4CfAXPELZ1xe8KyC4jaurW9KMuCkApwC8b8cgdKWg1ujtKkrNGhhDQRLcclNB6q5JTXrP0gQgrr43yHxh4vaAA8GTvkg7j2hrTUkksmc7JNIto0XsHlfvrUv8XCQIeQsFyy/nLHpQIkXwvAS6fcml6KMRTgQJm2yLZCfYVs6n18VDd9LCYWO9Y6majWoqgyHZ5Gy2qT7V+YxgDMUrZa7Fd66xHTWskO8wc7kuW5ZKaB29ewbAXIY31AHi4dAuGS6znPxnRg1kE01aDQ1FFCcajKtovg3di8PICU=";//Used when saving Applicationdata. This field should be compiled into your code. You will receive one PTLSSocketId per application you certify.
        
        //Service Information variables
        public Operations SupportedTxnTypes = new Operations();
        public ServiceInformation _si;
        public List<string> MerchantProfileIds = new List<string>();
        private MerchantProfile _merP = new MerchantProfile();
        public BankcardService _bcs = new BankcardService();
        public ElectronicCheckingService _ecks = new ElectronicCheckingService();
        public StoredValueService _svas = new StoredValueService();

        private static DataGenerator dg = new DataGenerator();

        private FaultHandler.FaultHandler _FaultHandler = new FaultHandler.FaultHandler();
        public bool _blnEncryptedIdentityToken;//Flag used to determine if the identity token is encrypted or not. Recommendation is to alway encrypt.
        private bool _blnPersistedConfigExists;//Flag used to determine if a previous config exists.
        private ToolTip comboIndToolTip = new ToolTip();
        private ToolTip helpLinkToolTip = new ToolTip();

    #endregion Variable Declarations

        public SampleCode_DeskTop()
        {
            InitializeComponent();
            menuStrip1.ForeColor = System.Drawing.Color.DarkMagenta;
            
            DefaultSetupValues();
            #region Setup The Application
            txtDisclaimer.Text = "Copyright (c) 2013 EVO Payments International - All Rights Reserved.\r\n\r\n"
                                 + " This software and documentation is subject to and made"
                                 + " available only pursuant to the terms of an executed license"
                                 + " agreement, and may be used only in accordance with the terms"
                                 + " of said agreement. This software may not, in whole or in part,"
                                 + " be copied, photocopied, reproduced, translated, or reduced to"
                                 + " any electronic medium or machine-readable form without"
                                 + " prior consent, in writing, from EVO Payments International, INC.\r\n\r\n"
                                 + " Use, duplication or disclosure by the U.S. Government is subject"
                                 + " to restrictions set forth in an executed license agreement"
                                 + " and in subparagraph (c)(1) of the Commercial Computer"
                                 + " Software-Restricted Rights Clause at FAR 52.227-19; subparagraph"
                                 + " (c)(1)(ii) of the Rights in Technical Data and Computer Software"
                                 + " clause at DFARS 252.227-7013, subparagraph (d) of the Commercial"
                                 + " Computer Software--Licensing clause at NASA FAR supplement"
                                 + " 16-52.227-86; or their equivalent.\r\n\r\n"
                                 + " Information in this software is subject to change without notice"
                                 + " and does not represent a commitment on the part of EVO Payments International.\r\n\r\n"
                                 + " Sample Code is for reference Only and is intended to be used for educational purposes. It's the responsibility of "
                                 + " the software company to properly integrate into thier solution code that best meets thier production needs. "
                                ;

            //Setup Process Image from a base64 string
            //SvcInfo
            pictureBox1.Image = Helper.ImageFromBase64String(@"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");
            //Txn
            pictureBox2.Image = Helper.ImageFromBase64String(@"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");
            //Arrow
            picArrow.Image = Helper.ImageFromBase64String(@"iVBORw0KGgoAAAANSUhEUgAAAJAAAAAcCAYAAACONDYbAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAgY0hSTQAAeiYAAICEAAD6AAAAgOgAAHUwAADqYAAAOpgAABdwnLpRPAAAAAlwSFlzAAAOwQAADsEBuJFr7QAABK1JREFUeF7tmmtMm1UYx2t0LrLExQTXQCZjyjK5KAxGGdCWFkahwAqlpescDAYWWgQ21DkQWjouG8vGrWokyMJgeNmAMtwQjYqEDedliSFzGo2ZYx/m4od90sxP/j1v+wYobWkjMW18T5NfnvfpeU7fc/7nn9PTCw/0QRVYgwK8NfSlXakCoAaiJnBSoLGtx2tVqIG8loo7hZJ8PRpbO72aMDWQVzJxqyi3tB7rQ5IwO/e1x4lTA3mUiHsFqvIGbIzcgy0x6fjrwYNVBaAG4p4/PM5YpXsdQfH7EPi8AlkaHTWQR8VogYMCqhfrEZywH5EZBjwamgJL74BbhWw70PS1eSirTkNvPkPhuAaHTgwjq9SMp0UliFXVIkxSAn6UDJ/PXHVpIpuB3h2bQmBCKZLL3kDSwW4KxzVIPGjBczlHoG0txC6tAUGxWkSK8vHbvXtOJrIbyPoxArZlQFhmgcTwDlIqeh0pJzmDnsIFDcQVfWQTeRuGvhIYJ2QISajFpug8iHOKXBvoPesneCREDmHJqxi5fxhDd+oxeLuOcJSNTL4GbpG+vy5xlr0eIpHifxoMkvUavtOIcwt6dHy1GxqTFtvT9NgQJkP1kSYHE9l2ILuBspF4oBojv6vwLVpxHW22SHGlQQurC6PR/5fv0I4PFkphnJQis6YYoUkVeHybFO9fGF800ZKBtmSTHegwen9OxfT9OiygF7/gTQqHNbiLM5i4W4UTc+moG8mBQGPAZkERtsZlYf7GTZuJnAw0cDsDb83vRceshlCAzisat3SwbV0kdl0pIDBx7+rMsu1XSaT4tQbdc1qcmlHj6PBuFJqTEJEiRUBwPOJlWvzxp/0LRgcDictq0fejDKYRBYwE06gdI6FpOaStaYxhDxtzSXTExOZmEs2jBCZal6MgeZ4TTcxzY3k4xjDuilzyPINyETNzbVWimcTmi+7II20M+TaOjdtjy4Q3KO11F1VonlCh9UNvyUerrV6NlktqtP0bSF+mf9tlFY5fVhMK/lsukddnmGTQ4ORnKuhOJUKgECPwGREe2xyHL6/fcH4LWxeaA0n5y6gb2oXy0ynQd0pc0yFBBWkzdC0nhc2lJK6gk827pdATKh2QsHkqicvoItcMPUsY2OuXSPSMlK1JI3EF3WxuIXEFlWxeRSLXMXSkouFCJkwT6UgrFiA4UgLexnBY+s+7PkRvCFcgKrUIESI1IsUaCoc1CE8uwLPC/dBZtHjBlAz+9lSs40ejsLLB9cf4c6NT4McqIVYfgkhVQ+G4BkJlDaIyDVC8osUOeTLW83dCmF20eO5Z7iLbGWj2m+9RauxHfY+VMOZTmno/woHa44gTydF29gufjsXXWvji/u0DnyJGbkB4hg7RcjUCw3YiOEqKmz/dcv9ThttfynzUMDMzAx6P/lHAR/JDpWtAkECLp2IywQvYiqnpa26HQlfJV6vkx/dVVxixKSYPDz8Zger69lVHSg3kxwvpq6ExBnqIvwMC2T787WEQ1EC+WiU/vm9O8Wt4IkyIH9yce5wO0X48Fzo0HyggUerRP2z16s50B/JKJm4VDZ6f9HrC1EBeS0ULXSlADUR9sSYF/gESXt5n2ozMfwAAAABJRU5ErkJggg==");
            txtIntroduction.Text = "The following code sample provides an overview of the components and concepts related to the development of client"
                                + " applications that communicate with Commerce Web Services, as well as a step-by-step guide for"
                                + " developing applications that implement the Commerce Web Services API. The code is meant to be used as"
                                + " a compliment to the online documentation. The developer should be familiar with both, so they can"
                                + " make the best design decisions to meet their technical and business needs."
                                + " \r\n\r\nThe first step in setting up your application is 'Preparing the Application to Transact'. This step"
                                + " is necessary during the initial setup of the application. There are three key values obtained that you'll"
                                + " want to persist and cache for the second step which is 'Transaction Processing'. The cacheable values include The"
                                + " 'applicationProfielId', 'serviceId' and the 'merchantProfileId'. Once these values are obtained, your application"
                                + " should always go directly to 'Transaction Processing'."
                                ;



    #endregion Setup The Application

            try{CboindustryType.SelectedItem = ConfigurationSettings.AppSettings["IndustryType"];}
            catch{}

            //Setup Card Types CboCardTypes
            CboCardTypes.Sorted = true;
            CboCardTypes.DataSource = Enum.GetValues(typeof(TypeCardType));
            try { CboCardTypes.SelectedItem = TypeCardType.MasterCard; }
            catch { }

            //Set form values to data generator
            BindControlsToDataGenerator();
        }

        #region Preparing to Transact

        private void cmdSignOnWithToken_Click(object sender, EventArgs e)
        {   //The SignOn() operation facilitates the authentication of identity tokens and the exchange of session tokens during application 
            //sign on. The SignOn() operation must be called before all other operations to retrieve a session token.
            try
            {
                Cursor = Cursors.WaitCursor;

                if (txtIdentityToken.Text.Length < 1)
                {//In this case we have a solution solution selected which does not have a identity token
                    MessageBox.Show("Please enter a valid identity token");
                    tabControl1.SelectedTab = tbIntroduction;
                    return;
                }

                //Set the endpoint URI's to the values in the text boxes
                Helper.BaseSvcEndpointPrimary = txtSvcPrimary.Text;
                Helper.BaseSvcEndpointSecondary = txtSvcSecondary.Text;
                Helper.BaseTxnEndpointPrimary = txtTxnPrimary.Text;
                Helper.BaseTxnEndpointSecondary = txtTxnSecondary.Text;
                Helper.BaseTMSEndpointPrimary = txtTMSPrimary.Text;
                Helper.BaseTMSEndpointSecondary = txtTMSSecondary.Text;

                if (!Helper.SetSvcEndpoint()) MessageBox.Show("Unable to setup the service endpoint\r\n" + Helper._message);
                if (!Helper.SetTxnEndpoint()) MessageBox.Show("Unable to setup the Transaction endpoint\r\n" + Helper._message);
                if (!Helper.SetTMSEndpoint()) MessageBox.Show("Unable to setup the TMS endpoint\r\n" + Helper._message);

                Helper.DelegatedServiceKey = "";//Reset just in case a delegate key was used before.

                if (ckBoxDelegatedSignOn.Checked)
                {//Use Delegation. Note for card processing this is not the normal authentication approach.
                    Helper.DelegatedServiceKey = txtDelegatedServiceKey.Text;
                    Helper.CheckTokenExpire();
                }
                else
                {//Use SignOnWithToken
                    Helper.CheckTokenExpire();
                }
                 
                cmdManageApplicationData.Enabled = true;
                cmdRetrieveServiceInformation.Enabled = true;
                CmdManageMerchantData.Enabled = true;
                chkStep1.Checked = true;
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage))
                { MessageBox.Show(strErrorId + " : " + strErrorMessage); }
                else { MessageBox.Show(ex.Message); }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cmdRetrieveServiceInformation_Click(object sender, EventArgs e)
        {
            try
            {
                Helper.CheckTokenExpire();//Make sure the current token is valid
                GetServiceInformation();
                chkStep3.Checked = true;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    if (!Helper.SetSvcEndpoint()) MessageBox.Show("Unable to setup the service endpoint\r\n" + Helper._message);
                    GetServiceInformation();
                    chkStep3.Checked = true;
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show("Neither the primary or secondary endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to RetrieveServiceInformation\r\nError Message : " + ex.Message, "RetrieveServiceInformation Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage))
                { MessageBox.Show(strErrorId + " : " + strErrorMessage); }
                else { MessageBox.Show(ex.Message); }
            }
        }

        private void cmdSaveApplicationConfiguration_Click(object sender, EventArgs e)
        {
            /*The second step in preparing the application to transact is to retrieve, update (if necessary), and save the appropriate 
              * characteristics and configuration information associated with your payment-enabled application. This is a one-time event 
              * that should be performed upon initial installation or launch of the application.
              
              * SECURITY CONSIDERATIONS
              * Stored on file system with read/write permission for only the application/service and IT Administration
              * Stored in DB with read/write permission for only the application/service and IT Administration
            */

            try
            {
                Cursor = Cursors.WaitCursor;

                ManageApplicationData MAD = new ManageApplicationData();
                MAD.CallingForm(Helper, dg);
                MAD.ShowDialog(this);
                if (MAD.SaveSuccess)
                {
                    chkStep2.Checked = true; //Reload dropdowns if Update or Add performed}

                    //We now have something to persiste
                    cmdPersistConfig.Enabled = true;
                    cmdDeletePersistCached.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CmdManageMerchantData_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                ManageMerchantProfile mPav = new ManageMerchantProfile();
                mPav.CallingForm(_merP, true, _bcs, _ecks, _svas, Helper.ServiceID, dg);
                mPav.ShowDialog(this);
                if (mPav._Dirty) GetServiceInformation(); //Reload dropdowns if Update or Add performed.
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
        
        #endregion END Preparing to Transact

        #region Transaction Processing

        private void cmdAuthorizeAndCapture_Click(object sender, EventArgs e)
        {
            //The AuthorizeAndCapture() operation is used to authorize and capture a transaction in a single invocation.

            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.AuthAndCapture) { MessageBox.Show("AuthAndCapture Not Supported"); return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null)
            {
                if (!chkProcessAsPINDebitTxn.Checked &&
                    _bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.Both)
                {
                    if (_bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeOnly)
                    {
                        MessageBox.Show("This service only support AuthorizeOnly for Credit transactions");
                        Cursor = Cursors.Default;
                        return;
                    }
                    if (_bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeAndCaptureOnly)
                    {
                        MessageBox.Show("This service only support AuthorizeAndCaptureOnly for Credit transactions");
                        Cursor = Cursors.Default;
                        return;
                    }
                }

                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    processResponse(Helper.ProcessBCPTransaction(TransactionType.AuthorizeAndCapture, BCtransaction, null, null, null,
                                                 null, null, null, null, ChkAcknowledge.Checked,
                                                 ChkForceCloseBatch.Checked));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
            else if (_svas != null) //Process as a Check transaction
            {
                try
                {
                    StoredValueTransaction SVATransaction = dg.SetStoredValueTxnData();
                    //Let's Query a transaction
                    processResponse(Helper.ProcessSVATransaction(TransactionType.Authorize, SVATransaction, null, null, null, null, ChkAcknowledge.Checked));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default;
                }
            }
        }

        private void cmdAuthorize_Click(object sender, EventArgs e)
        {//The Authorize() operation is used to authorize transactions prior to capture.
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Authorize) { MessageBox.Show("Authorize Not Supported"); return; }
            //Check to see if the transaction is a PINDebit and that the AuthorizeAndCapture is used.
            if (chkProcessAsPINDebitTxn.Checked) { MessageBox.Show("A PINDebit transaction can only be processed as an AuthorizeAndCapture Transaction"); return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();// DataGenerator.SetBankCardTxnData();
                    processResponse(Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_ecks != null) //Process as a Check transaction
            {
                //First verify if all transactions selected are "Authorize" transactions
                List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
                foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
                {
                    if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.QueryAccount.ToString() | ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.QueryAccount.ToString())
                    {
                        MessageBox.Show("All selected messages must be of type QueryAccount or Authorize");
                        Cursor = Cursors.Default;
                        return;
                    }
                    txnsToProcess.Add(((ResponseDetails)(itemChecked)));
                }
                try
                {
                    if (txnsToProcess.Count == 0)
                    {
                        MessageBox.Show("You must select a QueryAccount transaction to use for the Authorize");
                        return;
                    }
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        ElectronicCheckingTransaction ECKTransaction = dg.SetElectronicCheckTxnData();

                        _queryAccountTxn = (ElectronicCheckingTransactionResponse)_RD.Response;
                        
                        if (_queryAccountTxn == null)
                        {
                            MessageBox.Show("You must first perform a QueryAccount() to verify that the account accepts ACH transactions");
                            return;
                        }
                        if (!_queryAccountTxn.ACHCapable)
                        {
                            MessageBox.Show("Your previous QueryAccount() returned a response where the transaction is not ACHCapable");
                            return;
                        }

                        //Let's Authorize the transaction
                        //If a modified account or routing number is returned, those numbers should be used for the check transaction.  
                        //Good for verifying the info before running the charge – like when you initially set-up a recurring payment.
                        if (ECKTransaction.TenderData.CheckData.AccountNumber != _queryAccountTxn.ModifiedAccountNumber)
                            ECKTransaction.TenderData.CheckData.AccountNumber = _queryAccountTxn.ModifiedAccountNumber;
                        if (ECKTransaction.TenderData.CheckData.RoutingNumber != _queryAccountTxn.ModifiedRoutingNumber)
                            ECKTransaction.TenderData.CheckData.RoutingNumber = _queryAccountTxn.ModifiedRoutingNumber;

                        _queryAccountTxn = null; //Clear out the value as we're going to process a Check
                        //Let's Query a transaction
                        processResponse(Helper.ProcessECKTransaction(TransactionType.Authorize, ECKTransaction, null,
                                                                     null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));

                    }

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_svas != null) //Process as a Check transaction
            {
                try
                {
                    StoredValueTransaction SVATransaction = dg.SetStoredValueTxnData();
                    //Let's Query a transaction
                    processResponse(Helper.ProcessSVATransaction(TransactionType.Authorize, SVATransaction, null, null, null, null, ChkAcknowledge.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdCapture_Click(object sender, EventArgs e)
        {//The Capture() operation is used to capture a single transaction after it has been successfully authorized.
            if (ChkLstTransactionsProcessed.CheckedItems.Count == 0) { MessageBox.Show("Please Select (Check) transactions for Capture"); return; }
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Capture) { MessageBox.Show("Capture Not Supported"); return; }

            Cursor = Cursors.WaitCursor;
            
            //First verify if all transactions selected are "Authorize" transactions
            List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
            foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
            {
                if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString())
                {
                    MessageBox.Show("All selected messages must be of type Authorize");
                    Cursor = Cursors.Default; 
                    return;
                }
                txnsToProcess.Add(((ResponseDetails)(itemChecked)));
            }

            //Now process each Authorize message selected
            foreach (ResponseDetails _RD in txnsToProcess)
            {
                if (_bcs != null) //Process a BankCard Transaction
                {
                    try
                    {
                        BankcardCapturePro BCDifferenceData = new BankcardCapturePro();
                        BCDifferenceData.TransactionId = _RD.Response.TransactionId;

                        if (ChkMultiplePartialCapture.Checked)
                        {
                            //Let's demonstrate capturing two multipartialCaptures. Note that multipartialCapture requries the BankcarCapturePro() object
                            BCDifferenceData.Amount = Convert.ToDecimal(TxtAmount.Text);
                            if (TxtTip.Text.Length > 0)
                                if (Convert.ToDecimal(TxtTip.Text) > 0)
                                    BCDifferenceData.TipAmount = Convert.ToDecimal(TxtTip.Text);
                            BCDifferenceData.MultiplePartialCapture = true;

                            processResponse(Helper.ProcessBCPTransaction(TransactionType.Capture, null, BCDifferenceData, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.Capture, null, BCDifferenceData, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        }
                        else
                        {
                            //For demonstrations let's show adding a two dollar tip
                            BCDifferenceData.Amount = Convert.ToDecimal(TxtAmount.Text);
                            if (TxtTip.Text.Length > 0)
                                if (Convert.ToDecimal(TxtTip.Text) > 0)
                                    BCDifferenceData.TipAmount = Convert.ToDecimal(TxtTip.Text);

                            BCDifferenceData.MultiplePartialCapture = false;
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.Capture, null, BCDifferenceData, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
                else if (_svas != null) //Process a BankCard Transaction
                {
                    try
                    {
                        StoredValueCapture SVDifferenceData = new StoredValueCapture();
                        string strAuthTxn = _authorizeTxn;
                        _authorizeTxn = ""; //Reset so that a new Authorize will be required            
                        SVDifferenceData.TransactionId = strAuthTxn;


                        SVDifferenceData.Amount = Convert.ToDecimal(TxtAmount.Text);
                        //SVDifferenceData.TipAmount = 2.00M;

                        processResponse(Helper.ProcessSVATransaction(TransactionType.Capture, null, null, SVDifferenceData, null, null, ChkAcknowledge.Checked));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
                else if (_ecks != null) //Process as a Check transaction
                {
                    try
                    {
                        MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        Cursor = Cursors.Default;
                    }
                }
            }

        }

        private void cmdCaptureAll_Click(object sender, EventArgs e)
        {//The CaptureAll() operation is used to capture all transactions that have been successfully authorized.
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.CaptureAll) { MessageBox.Show("CaptureAll Not Supported"); Cursor = Cursors.Default; return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    //First verify if all transactions selected are "Authorize" transactions
                    List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
                    foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
                    {
                        if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.ReturnById.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Return.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Adjust.ToString())
                        {
                            MessageBox.Show("All selected messages must be of type Authorize, ReturnById, Return or Adjust");
                            Cursor = Cursors.Default;
                            return;
                        }
                        txnsToProcess.Add(((ResponseDetails)(itemChecked)));
                    }

                    List<Capture> captures = new List<Capture>();
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        //The following would only be set if a different amount than what was originally authorized needs to be captured. So for example a tip.
                        if (ChkCapSelectiveDiffData.Checked)
                        {
                            BankcardCapture bc = new BankcardCapture();
                            bc.Amount = Convert.ToDecimal(TxtAmount.Text);
                                //Amount represents the dollar amount charged to the card holder.
                            if (TxtTip.Text.Length > 0)
                                if (Convert.ToDecimal(TxtTip.Text) > 0)
                                    bc.TipAmount = Convert.ToDecimal(TxtTip.Text);
                                        //Tip is generally only a reporting field.
                            bc.TransactionId = _RD.Response.TransactionId;
                            captures.Add(bc);
                        }
                    }
                    if (_ecks != null) //Process as a Check transaction
                    {
                        if (chkCaptureAllAndSelectiveAsync.Checked)
                            processResponse(Helper.ProcessECKCaptureAll(TransactionType.CaptureAllAsync, captures, ChkAcknowledge.Checked, false));
                        else
                            processResponse(Helper.ProcessECKCaptureAll(TransactionType.CaptureAll, captures, ChkAcknowledge.Checked, false));
                    }
                    else
                    {
                        if (chkCaptureAllAndSelectiveAsync.Checked)
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.CaptureAllAsync, null, null, null, null, null, null, null, captures, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        else
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.CaptureAll, null, null, null, null, null, null, null, captures, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    if (chkCaptureAllAndSelectiveAsync.Checked)
                        processResponse(Helper.ProcessECKTransaction(TransactionType.CaptureAllAsync, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    else
                        processResponse(Helper.ProcessECKTransaction(TransactionType.CaptureAll, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdCaptureSelective_Click(object sender, EventArgs e)
        {//The CaptureSelective() operation is used to capture a specific list of transactions that have been successfully authorized.
            if (ChkLstTransactionsProcessed.CheckedItems.Count == 0) { MessageBox.Show("Please Select (Check) transactions for CaptureSelective"); return; }
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.CaptureSelective) { MessageBox.Show("CaptureSelective Not Supported"); return; }

            Cursor = Cursors.WaitCursor;
            
            List<ResponseDetails> response = new List<ResponseDetails>();
            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    List<string> transactionIDs = new List<string>();
                    List<Capture> captures = new List<Capture>();

                    //First verify if all transactions selected are "Authorize" transactions
                    List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
                    foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
                    {
                        if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.ReturnById.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Return.ToString()
                            && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Adjust.ToString())
                        {
                            MessageBox.Show(@"All selected messages must be of type Authorize, ReturnById, Return or Adjust");
                            Cursor = Cursors.Default;
                            return;
                        }
                        txnsToProcess.Add(((ResponseDetails)(itemChecked)));
                    }

                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        //First Set the list of transactionID's to pass
                        transactionIDs.Add(_RD.Response.TransactionId);

                        //The following would only be set if a different amount than what was originally authorized needs to be captured. So for example a tip.
                        if (ChkCapSelectiveDiffData.Checked)
                        {
                            BankcardCapture bc = new BankcardCapture();
                            bc.Amount = Convert.ToDecimal(TxtAmount.Text); //Amount represents the dollar amount charged to the card holder.
                            if (TxtTip.Text.Length > 0)
                                if (Convert.ToDecimal(TxtTip.Text) > 0)
                                    bc.TipAmount = Convert.ToDecimal(TxtTip.Text);//Tip is generally only a reporting field.
                            bc.TransactionId = _RD.Response.TransactionId;
                            captures.Add(bc);
                        }
                    }

                    if (chkCaptureAllAndSelectiveAsync.Checked)
                        processResponse(Helper.ProcessBCPTransaction(TransactionType.CaptureSelectiveAsync, null, null, null, null, null, null, transactionIDs, captures, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    else
                        processResponse(Helper.ProcessBCPTransaction(TransactionType.CaptureSelective, null, null, null, null, null, null, transactionIDs, captures, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));

                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdReturnById_Click(object sender, EventArgs e)
        {//The ReturnById() operation is used to return a transaction that has been previously captured.
            if (ChkLstTransactionsProcessed.CheckedItems.Count == 0) { MessageBox.Show("Please Select (Check) transactions for ReturnById"); return; }
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.ReturnById) { MessageBox.Show("ReturnById Not Supported");return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardReturn BCRDifferenceData = new BankcardReturn();
                    List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();

                    if (_bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeOnly)
                    {//NOTE : Please note that in the case of some Service Providers AuthorizeAndCapture is not supported for ReturnById. Use Authorize Instead
                        //In this case it's a terminal capture service that supports PINDebit
                        //First verify if all transactions selected are "Authorize" transactions
                        foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
                        {
                            if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString())
                            {
                                MessageBox.Show("All selected messages must be of type Authorize");
                                Cursor = Cursors.Default;
                                return;
                            }
                            txnsToProcess.Add(((ResponseDetails)(itemChecked)));
                        }

                        DialogResult Result;
                        Result = MessageBox.Show("Would you like to return the amount originally captured?", "Amount to Return", MessageBoxButtons.YesNo);
                        //Now process each message selected
                        foreach (ResponseDetails _RD in txnsToProcess)
                        {
                            BCRDifferenceData.TransactionId = _RD.Response.TransactionId;
                            BCRDifferenceData.TransactionCode = TransactionCode.NotSet;
                            BCRDifferenceData.Amount = (Result == DialogResult.Yes ? 0.00M : Convert.ToDecimal(TxtAmount.Text) );
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.ReturnById, null, null, null, BCRDifferenceData, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        }
                    }
                    else
                    {
                        //In this case it's a host capture solution. So only AuthorizeAndCapture and Capture transactions are permitted.
                        //First verify if all transactions selected are "AuthorizeAndCapture" or "Capture" transactions
                        foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
                        {
                            if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.AuthorizeAndCapture.ToString()
                                && ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Capture.ToString())
                            {
                                MessageBox.Show("All selected messages must be of type AuthorizeAndCapture or Capture");
                                Cursor = Cursors.Default;
                                return;
                            }
                            txnsToProcess.Add(((ResponseDetails)(itemChecked)));
                        }

                        DialogResult Result;
                        Result = MessageBox.Show("Would you like to return the amount originally captured?", "Amount to Return", MessageBoxButtons.YesNo);
                        //Now process each  message selected
                        foreach (ResponseDetails _RD in txnsToProcess)
                        {
                            //Let's return the transaction
                            BCRDifferenceData.TransactionId = _RD.Response.TransactionId;
                            BCRDifferenceData.Amount = (Result == DialogResult.Yes ? 0.00M : Convert.ToDecimal(TxtAmount.Text));
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.ReturnById, null, null, null, BCRDifferenceData, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_svas != null) //Process a Stored Value Transaction
            {
                //Check to see if this transaction type is supported
                if (!SupportedTxnTypes.ReturnById) { MessageBox.Show("ReturnById Not Supported"); Cursor = Cursors.Default; return; }

                try
                {
                    StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();
                    List<ResponseDetails> response = new List<ResponseDetails>();
                    StoredValueReturn SVRDifferenceData = new StoredValueReturn();
                    string strTransactionId = "";
                    SVtransaction.TransactionData.Amount = Convert.ToDecimal(TxtAmount.Text);

                    if (_svas.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeAndCaptureOnly)
                    {
                        response = Helper.ProcessSVATransaction(TransactionType.AuthorizeAndCapture, SVtransaction, null, null, null, null, ChkAcknowledge.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]); 
                        StoredValueTransactionResponse SVR = (StoredValueTransactionResponse)response[0].Response;
                        strTransactionId = SVR.TransactionId;
                        //Now in ordert to process a ReturnById we must first Capture the transaction. This can be done with CaptureAll or CaptureSelective.
                        response = Helper.ProcessBCPTransaction(TransactionType.CaptureAll, null, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);

                        //Now Let's return the transaction
                        SVRDifferenceData.TransactionId = strTransactionId;
                        SVRDifferenceData.Amount = Convert.ToDecimal(TxtAmount.Text);
                        response = Helper.ProcessSVATransaction(TransactionType.ReturnById, null, null, null, SVRDifferenceData, null, ChkAcknowledge.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdReturn_Click(object sender, EventArgs e)
        {//The Return() operation is used to return funds to a payment account without associating the return with a specific transaction that has been previously captured.
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.ReturnUnlinked) { MessageBox.Show("ReturnUnlinked Not Supported"); return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    processResponse(Helper.ProcessBCPTransaction(TransactionType.Return, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            if (_svas != null) //Process a BankCard Transaction
            {
                try
                {
                    StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();
                    processResponse(Helper.ProcessSVATransaction(TransactionType.Return, SVtransaction, null, null, null, null, ChkAcknowledge.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdAdjust_Click(object sender, EventArgs e)
        {
            if (ChkLstTransactionsProcessed.CheckedItems.Count == 0) { MessageBox.Show("Please Select (Check) Authorize transactions for Adjust"); return; }
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Adjust) { MessageBox.Show("Adjust Not Supported"); return; }

            Cursor = Cursors.WaitCursor;

            //First verify if all transactions selected are "Authorize" transactions
            List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
            foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
            {
                if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString() & ((ResponseDetails)(itemChecked)).TransactionType != TransactionType.AuthorizeAndCapture.ToString())
                {
                    MessageBox.Show("All selected messages must be of type Authorize or AuthorizeAndCapture");
                    Cursor = Cursors.Default;
                    return;
                }
                txnsToProcess.Add(((ResponseDetails)(itemChecked)));
            }

            List<ResponseDetails> response = new List<ResponseDetails>();
            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        Adjust aTransaction = new Adjust();
                        //Let's Undo or Void the transaction
                        aTransaction.TransactionId = _RD.Response.TransactionId;
                        aTransaction.Amount = Convert.ToDecimal(TxtAmount.Text);
                        if (TxtTip.Text.Length > 0)
                            if (Convert.ToDecimal(TxtTip.Text) > 0)
                                aTransaction.TipAmount = Convert.ToDecimal(TxtTip.Text);

                        processResponse(Helper.ProcessBCPTransaction(TransactionType.Adjust, null, null, null, null, aTransaction, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdUndo_Click(object sender, EventArgs e)
        {//The Undo() operation is used to void (Credit Card) or reverse (PIN Debit) a transaction that has been previously authorized.
            if (ChkLstTransactionsProcessed.CheckedItems.Count == 0) { MessageBox.Show("Please Select (Check) transactions for Undo"); return; }
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Undo) { MessageBox.Show("Undo Not Supported"); Cursor = Cursors.Default; return; }

            Cursor = Cursors.WaitCursor;

            //First verify if all transactions selected are "Authorize" transactions
            List<ResponseDetails> txnsToProcess = new List<ResponseDetails>();
            foreach (object itemChecked in ChkLstTransactionsProcessed.CheckedItems)
            {
                if (((ResponseDetails)(itemChecked)).TransactionType != TransactionType.Authorize.ToString())
                {
                    MessageBox.Show("All selected messages must be of type Authorize");
                    Cursor = Cursors.Default;
                    return;
                }
                txnsToProcess.Add(((ResponseDetails)(itemChecked)));
            }

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        BankcardUndo uTransaction = new BankcardUndo();
                        //Let's Undo or Void the transaction
                        uTransaction.TransactionId = _RD.Response.TransactionId;
                        
                        if (chkProcessAsPINDebitTxn.Checked)
                            uTransaction.PINDebitReason = PINDebitUndoReason.ResponseTimeout;

                        if (ChkForceVoid.Checked)
                            uTransaction.ForceVoid = true;

                        processResponse(Helper.ProcessBCPTransaction(TransactionType.Undo, null, null, null, null, null, uTransaction, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            if (_svas != null) //Process a Stored Value Transaction
            {
                try
                {
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        Undo uTransaction = new Undo();
                        //Now Let's Undo or Void the transaction
                        uTransaction.TransactionId = _RD.Response.TransactionId;
                        processResponse(Helper.ProcessSVATransaction(TransactionType.Undo, null, null, null, null, uTransaction, ChkAcknowledge.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    //Now process each message selected
                    foreach (ResponseDetails _RD in txnsToProcess)
                    {
                        Undo uECKCheck = new Undo();
                        uECKCheck.TransactionId = _RD.Response.TransactionId;
                        processResponse(Helper.ProcessECKTransaction(TransactionType.Undo, null, null, null, uECKCheck, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdQueryAccount_Click(object sender, EventArgs e)
        {//The QueryAccount() operation is used to perform a balance inquiry on a cardholder's account to determine the current account balance.
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.QueryAccount) { MessageBox.Show("QueryAccount Not Supported"); return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try { }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_svas != null) //Process a Stored Value Transaction
            {
                try
                {
                    StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();
                   //Let's authorize a transaction
                    processResponse(Helper.ProcessSVATransaction(TransactionType.QueryAccount, SVtransaction, null, null, null, null, ChkAcknowledge.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
            try
            {
                ElectronicCheckingTransaction ECKTransaction = dg.SetElectronicCheckTxnData();
                //Let's Query a transaction
                processResponse(Helper.ProcessECKTransaction(TransactionType.QueryAccount, ECKTransaction, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdVerify_Click(object sender, EventArgs e)
        {
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Verify) { MessageBox.Show("Verify Not Supported"); Cursor = Cursors.Default; return; }

            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    BCtransaction.TenderData.CardSecurityData.AVSData.Street = "123 Happy";
                    processResponse(Helper.ProcessBCPTransaction(TransactionType.Verify, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void CmdRequestTransaction_Click(object sender, EventArgs e)
        {//The RequestTransaction() operation allows for the retrieval of any transaction matching the supplied tender data. This is useful in situations 
         // when the application does not receive a response from CWS indicating the TransactionState.
         // The following will use an Authorize follwed by the RequestTransaction to simulate a dropped response.
            
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Authorize) { MessageBox.Show(@"Authorize Not Supported. Unable to demonstrate RequestTransaction() simulation"); return; }

            Cursor = Cursors.WaitCursor;
           
            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    BCtransaction.TransactionData.Amount = 10.94M;//The trigger value of *.94 will simulate a timeout scenario.
                    Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally { Cursor = Cursors.Default; }

            }
            else if (_svas != null) //Process a Stored Value Transaction
            {
                StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();
                try
                {

                    List<ResponseDetails> response = new List<ResponseDetails>();
                    //Let's authorize a transaction
                    SVtransaction.TransactionData.Amount = 10.94M;//The trigger value of *.94 will simulate a timeout scenario.
                    response = Helper.ProcessSVATransaction(TransactionType.Authorize, SVtransaction, null, null, null, null, ChkAcknowledge.Checked);
                    if (response.Count > 0)
                    {
                        _queryAccountSVATxn = (StoredValueTransactionResponse)response[0].Response;
                    }
                }
                catch (FaultException<CWSTransactionServiceUnavailableFault> ex)
                {
                    Helper.ProcessSVATransaction(TransactionType.RequestTransaction, SVtransaction, null, null, null, null, ChkAcknowledge.Checked);
                   // if (responses != null && responses.Length >= 1)
                       // bcpClient.Acknowledge(serviceKey, responses[0].TransactionId, applicationId, merchantProfile, serviceId, addendumData);
                }

                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
            }

        }

        #region Additional Operations
        
        private void cmdCardTokenization_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //NOTE: The first step with tokenization is to obtain a token. This can be done with either a AuthorizeAndCapture or an Authorize.
            //NOTE: In this example, we'll use an "Authorize" or pre-capture

            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Authorize && !SupportedTxnTypes.AuthAndCapture) { MessageBox.Show("Tokenization requires either Authorize or AuthorizeAndCapture supported."); Cursor = Cursors.Default; return; }

            List<ResponseDetails> response = new List<ResponseDetails>();
            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    //First Let's get a token
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    response = Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                    if (response.Count < 1) { return; }
                    ChkLstTransactionsProcessed.Items.Add(response[0]);
                    BankcardTransactionResponse BCR = (BankcardTransactionResponse)response[0].Response; 

                    //Now Let's use the returned token to AuthorizeAndCapture a transaction
                    BCtransaction = dg.SetBankCardTxnData();//Reset the back card information
                    //Since we used a generic method to set the bankcard data let's clear out the card data and use the token, card type and masked pan returned from
                    //the Authorize above.
                    //NOTE : For tokenization you'll need to store at a minimum the token, cardtype and MaskedPAN. It's also recommended to store the expiration date
                    BCtransaction.TenderData.CardData = new CardData();
                    BCtransaction.TenderData.PaymentAccountDataToken = BCR.PaymentAccountDataToken;
                    BCtransaction.TenderData.CardData.CardType = BCR.CardType;
                    BCtransaction.TenderData.CardData.Expire = "1213"; // Note : that in a swipe track data the format is "YYMM" however here it's "MMYY"
                    BCtransaction.TenderData.CardData.PAN = BCR.MaskedPAN; //You'll need to set the masked PAN that was returned with the original token

                    //Now Let's Authorize a new transaction using the PaymentAccountDataToken
                    response = Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                    if (response.Count < 1) { return; }
                    ChkLstTransactionsProcessed.Items.Add(response[0]);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdManageAccount_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.ManageAccount) { MessageBox.Show("Manage Account Not Supported"); Cursor = Cursors.Default; return; }

            if (_svas != null) //Process a Stored Value Transaction
            {
                try
                {
                    StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();
                    
                    if (rdoActivate.Checked)
                        SVtransaction.TransactionData.OperationType = OperationType.Activate;
                    if (rdoDeactivate.Checked)
                        SVtransaction.TransactionData.OperationType = OperationType.Deactivate;
                    if (rdoReload.Checked)
                        SVtransaction.TransactionData.OperationType = OperationType.Reload;

                    processResponse(Helper.ProcessSVATransaction(TransactionType.ManageAccount, SVtransaction, null, null, null, null, ChkAcknowledge.Checked));
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            
        }

        private void cmdManageAccountById_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.ManageAccountById) { MessageBox.Show("Manage Account Not Supported"); Cursor = Cursors.Default; return; }

            List<ResponseDetails> response = new List<ResponseDetails>();
            if (_svas != null) //Process a Stored Value Transaction
            {
                try
                {
                    StoredValueTransaction SVtransaction = dg.SetStoredValueTxnData();

                    SVtransaction.TransactionData.OperationType = OperationType.Activate;

                    response = Helper.ProcessSVATransaction(TransactionType.ManageAccount, SVtransaction, null, null, null, null,
                                                 ChkAcknowledge.Checked);
                    if (response.Count < 1) { return; }
                    ChkLstTransactionsProcessed.Items.Add(response[0]);

                    StoredValueTransactionResponse SVR = (StoredValueTransactionResponse)response[0].Response;
                    string strTransactionId = SVR.TransactionId;

                    StoredValueManage SVManage = new StoredValueManage();
                    SVManage.TransactionId = strTransactionId;
                    SVManage.Amount = Convert.ToDecimal(TxtAmount.Text);

                    if (rdoActivate.Checked)
                        SVManage.OperationType = OperationType.Activate;
                    if (rdoDeactivate.Checked)
                        SVManage.OperationType = OperationType.Deactivate;
                    if (rdoReload.Checked)
                        SVManage.OperationType = OperationType.Reload;

                    response = Helper.ProcessSVATransaction(TransactionType.ManageAccountById, null, SVManage, null, null, null, ChkAcknowledge.Checked);
                    if (response.Count < 1) { return; }
                    ChkLstTransactionsProcessed.Items.Add(response[0]);
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }

        }

        private void cmdPurchaseCardL2_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (_bcs != null) //Process a BankCard Transaction
            {
                //NOTE: The first step with purchase card level 2 is to submit a Authorize to verify that the card is a valid purchase card
                //Check to see if this the service is a host capture or terminal capture solution. This will drive logice in terms of useing an AuthorizeAndCapture Versus Authorize to Capture
                if (_bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeOnly)//In this case it's a Host capture so either an Authorize followed by capture or AuthorizeAndCapture should be used.
                    if (!SupportedTxnTypes.Authorize && !SupportedTxnTypes.AuthAndCapture) { MessageBox.Show("Purchase card requires support for Authorize or AuthorizeAndCapture"); Cursor = Cursors.Default; return; }
                    else//In this case its a terminal capture service which requires an Authorize followed by a captureall or capture selective
                        if (!SupportedTxnTypes.Authorize | !SupportedTxnTypes.CaptureAll) { MessageBox.Show("Purchase card requires support for Authorize as well as CaptureAll or CaptureSelective"); Cursor = Cursors.Default; return; }

                List<ResponseDetails> response = new List<ResponseDetails>();
                try
                {
                    BankcardTransactionPro BCtransaction = new BankcardTransactionPro();
                    //First Check to see if this work flow is an "AuthorizeAndCapture" or an "Authorize" followed by a "Capture".
                    if (chkL2AuthAndCapture.Checked && _bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeOnly)
                    {//In this case the work flow is a "AuthorizeAndCapture"
                        BankCardProProcessingOptions BCPO = new BankCardProProcessingOptions(PurchaseCardLevel.Level2, false, true);
                        BCtransaction = dg.SetBankCardTxnData(BCPO);
                        response = Helper.ProcessBCPTransaction(TransactionType.AuthorizeAndCapture, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);
                    }
                    else if (!chkL2AuthAndCapture.Checked)
                    {//In this case the work flow is an "Authorize" followed by a "Capture"
                        BankCardProProcessingOptions BCPO = new BankCardProProcessingOptions(PurchaseCardLevel.Level2, false, false);
                        BCtransaction = dg.SetBankCardTxnData(BCPO);

                        response = Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);
                        BankcardTransactionResponsePro BCR = (BankcardTransactionResponsePro)response[0].Response;

                        //Note : Now let's send the capture for the level 2 transaction. 
                        //First verify that the card is a valid Purchase Card. 
                        if (BCR.CommercialCardResponse == CommercialCardResponse.BusinessCard | BCR.CommercialCardResponse == CommercialCardResponse.CorporateCard | BCR.CommercialCardResponse == CommercialCardResponse.PurchaseCard)
                        {
                            BankcardCapturePro BCP = new BankcardCapturePro();
                            BCP.Level2Data = dg.SetLevel2Data();
                            BCP.TransactionId = BCR.TransactionId;//Set the transactionId to the original Authorize

                            response = Helper.ProcessBCPTransaction(TransactionType.Capture, null, BCP, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                            if (response.Count < 1) { return; }
                            ChkLstTransactionsProcessed.Items.Add(response[0]);
                        }
                        else
                        {
                            MessageBox.Show("Capture failed as Card PAN was not a valid Level 2 or Level 3 purchase Card");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Unable to process purchase Card as the transaction types requested are not supported. Specifically AuthorizeAndCapture");
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdPurchaseCardL3_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            //NOTE: The first step with purchase card level 2 is to submit a Authorize to verify that the card is a valid purchase card
            //Check to see if this transaction type is supported
            if (!SupportedTxnTypes.Authorize | !SupportedTxnTypes.Capture) { MessageBox.Show("Purchase card requires support for Authorize as well as Capture"); Cursor = Cursors.Default; return; }

            List<ResponseDetails> response = new List<ResponseDetails>();
            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransactionPro BCtransaction = new BankcardTransactionPro();
                    
                    //First Check to see if this work flow is an "AuthorizeAndCapture" or an "Authorize" followed by a "Capture".
                    if (chkL3AuthAndCapture.Checked)
                    {//In this case the work flow is a "AuthorizeAndCapture"
                        BankCardProProcessingOptions BCPO = new BankCardProProcessingOptions(PurchaseCardLevel.Level3, false, true);
                        BCtransaction = dg.SetBankCardTxnData(BCPO);
                        response = Helper.ProcessBCPTransaction(TransactionType.AuthorizeAndCapture, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);
                    }
                    else
                    {//In this case the work flow is an "Authorize" followed by a "Capture"
                        BankCardProProcessingOptions BCPO = new BankCardProProcessingOptions(PurchaseCardLevel.Level3, false, false);
                        BCtransaction = dg.SetBankCardTxnData(BCPO);

                        response = Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                        if (response.Count < 1) { return; }
                        ChkLstTransactionsProcessed.Items.Add(response[0]);
                        BankcardTransactionResponsePro BCR = (BankcardTransactionResponsePro)response[0].Response;

                        //Note : Now let's send the capture for the level 3 transaction. 
                        //First verify that the card is a valid Purchase Card. 
                        if (BCR.CommercialCardResponse == CommercialCardResponse.BusinessCard | BCR.CommercialCardResponse == CommercialCardResponse.CorporateCard | BCR.CommercialCardResponse == CommercialCardResponse.PurchaseCard)
                        {
                            BankcardCapturePro BCP = new BankcardCapturePro();
                            BCP.Level2Data = dg.SetLevel2Data(); //Level 3 purchase card is always inclusive of Level 1 and Level 2 data.

                            List<LineItemDetail> LIDS = new List<LineItemDetail>();
                            //For each line on the receipt the following needs to be called. 
                            for (int i = 0; i < 3; i++)//Add three line items
                            {
                                LIDS.Add(dg.SetLevel3Data());
                            }
                            BCP.LineItemDetails = LIDS;
                            BCP.TransactionId = BCR.TransactionId;//Set the transactionId to the original Authorize
                            response = Helper.ProcessBCPTransaction(TransactionType.Capture, null, BCP, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked);
                            if (response.Count < 1) { return; }
                            ChkLstTransactionsProcessed.Items.Add(response[0]);
                        }
                        else
                        {
                            MessageBox.Show("Capture failed as Card PAN was not a valid Level 2 or Level 3 purchase Card");
                        }
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void cmdForcePost_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;

            if (txtApprovalCode.Text.Length < 1) { MessageBox.Show("Approval Code Required for a Force Post Transaction"); Cursor = Cursors.Default; return; }

            if (_bcs != null) //Process a BankCard Transaction
            {
                try
                {
                    BankcardTransaction BCtransaction = dg.SetBankCardTxnData();
                    BCtransaction.TransactionData.ApprovalCode = txtApprovalCode.Text; //The ForcePost requires an approval code to be present

                    if (_bcs.Tenders.CreditAuthorizeSupport == CreditAuthorizeSupportType.AuthorizeOnly)//Verify if this is a Terminal Capture solution that supports PINDebit.
                    {//This is a terminal capture service that supports PINDebit so use the Authorize transaction type
                        if (SupportedTxnTypes.Authorize)
                            processResponse(Helper.ProcessBCPTransaction(TransactionType.Authorize, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                        else
                            MessageBox.Show("Force Post is not possible");
                    }
                    else
                    {//This is a host capture service so use the AuthorizeAndCapture transaction
                        processResponse(Helper.ProcessBCPTransaction(TransactionType.AuthorizeAndCapture, BCtransaction, null, null, null, null, null, null, null, ChkAcknowledge.Checked, ChkForceCloseBatch.Checked));
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }

            }
            else if (_ecks != null) //Process as a Check transaction
            {
                try
                {
                    MessageBox.Show(@"Placeholder for ECK code. Please ask your solution consultant for an example");
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
                finally { Cursor = Cursors.Default; }
            }
        }

        private void viewTransactionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Helper.SessionToken.Length < 1)
            {
                MessageBox.Show("Please obtain a valid SessionToken before using Transaction Manangement Services (TMS)");
                tabControl1.SelectedTab = tbIntroduction;
                return;
            }
            TransactionManagementServices TMS = new TransactionManagementServices();
            TMS.ShowDialog(this);
        }

        #endregion Additional Operations

        #region Process Response

        private void processResponse (List<ResponseDetails> _RD)
        {
            if (_RD != null && _RD.Count > 0)
            {
                foreach (ResponseDetails rd in _RD)
                {
                    ChkLstTransactionsProcessed.Items.Add(rd);
                    
                    try
                    {
                        if (rd.Response.Status == Status.Successful && (rd.TransactionType == "Authorize" | rd.TransactionType == "AuthorizeAndCapture"))
                        {
                            BankcardTransactionResponse BTR = (BankcardTransactionResponse)rd.Response;
                            if (BTR.PaymentAccountDataToken.Length > 0)
                            {
                                if (BTR.MaskedPAN.Length < 1)
                                    BTR.MaskedPAN = rd.MaskedPan;
                                if (BTR.CardType == TypeCardType.NotSet)
                                    BTR.CardType = rd.CardType;

                                //Your logic may be different
                                bool match = false;
                                foreach (TokenizedTransaction t in CboTokenizedCard.Items)
                                {
                                    if (t.MaskedPAN == BTR.MaskedPAN && t.CardType == BTR.CardType)
                                        match = true;
                                }
                                //only add if this is a new card
                                if (!match)
                                    CboTokenizedCard.Items.Add(new TokenizedTransaction(BTR.PaymentAccountDataToken, TxtExpirationDate.Text, BTR.MaskedPAN, BTR.CardType));
                            }
                        }
                    }
                    catch { }
                }
                //Uncheck all boxes
                for (int i = 0; i < ChkLstTransactionsProcessed.Items.Count; ++i)
                    ChkLstTransactionsProcessed.SetItemChecked(i, false);
            }
            MessageBox.Show(Helper._message);
        }

        #endregion Process Response
        #endregion Transaction Processing

        #region Helper Methods

        #region Service Information

        private bool AvailableTxnTypes(Operations supportedTxnTypes)
        {
            //Disable all buttons
            cmdAuthorizeAndCapture.Enabled = false;
            cmdAuthorize.Enabled = false;
            cmdCapture.Enabled = false;
            cmdCaptureAll.Enabled = false;
            cmdCaptureSelective.Enabled = false;
            cmdQueryAccount.Enabled = false;
            cmdReturnById.Enabled = false;
            cmdReturnUnlinked.Enabled = false;
            cmdAdjust.Enabled = false;
            cmdUndo.Enabled = false;
            cmdVerify.Enabled = false;
            cmdCardTokenization.Enabled = false;
            cmdPurchaseCardL2.Enabled = false;
            cmdPurchaseCardL3.Enabled = false;
            cmdManageAccount.Enabled = false;
            cmdManageAccountById.Enabled = false;
            cmdForcePost.Enabled = false;            
           
            chkL2AuthAndCapture.Enabled = false;
            chkL3AuthAndCapture.Enabled = false;
            chkCaptureAllAndSelectiveAsync.Enabled = false;
            chkProcessAsPINDebitTxn.Enabled = false;
            ChkProcessAsPINLessDebit.Enabled = false;           
            ChkAllowPartialApprovals.Enabled = false;
            ChkForceVoid.Enabled = false;

            rdoActivate.Enabled = false;
            rdoDeactivate.Enabled = false;
            rdoReload.Enabled = false;

            //Check to see if the transaction type is supported
            if (supportedTxnTypes == null) { return false; }

            //Enable buttons that are not availabe based on the selected service

            
            
            if (supportedTxnTypes.AuthAndCapture){cmdAuthorizeAndCapture.Enabled = true;cmdForcePost.Enabled = true;}
            if (supportedTxnTypes.Authorize)cmdAuthorize.Enabled = true;
            if (supportedTxnTypes.Capture) cmdCapture.Enabled = true;
            if (supportedTxnTypes.CaptureAll)
            {
                cmdCaptureAll.Enabled = true;
                chkCaptureAllAndSelectiveAsync.Enabled = true;
            }
            if (supportedTxnTypes.CaptureSelective)
            {
                cmdCaptureSelective.Enabled = true;
                chkCaptureAllAndSelectiveAsync.Enabled = true;
            }
            if (supportedTxnTypes.QueryAccount) cmdQueryAccount.Enabled = true;
            if (supportedTxnTypes.ReturnById) cmdReturnById.Enabled = true;
            if (supportedTxnTypes.ReturnUnlinked) cmdReturnUnlinked.Enabled = true;
            if (supportedTxnTypes.Adjust) cmdAdjust.Enabled = true;
            if (supportedTxnTypes.Undo) cmdUndo.Enabled = true;
            if (supportedTxnTypes.Verify) cmdVerify.Enabled = true;
            if (supportedTxnTypes.ManageAccount) cmdManageAccount.Enabled = true;
            if (supportedTxnTypes.ManageAccountById) cmdManageAccountById.Enabled = true;

            //Sample for tokenizatino requires an Authorize as well as AuthorizeAndCapture
            if (supportedTxnTypes.AuthAndCapture & supportedTxnTypes.Authorize) cmdCardTokenization.Enabled = true; //Turned off as this is not longer available
            //Turn on the SVA radio buttons 
            if (_svas != null)
            {
                rdoActivate.Enabled = true;
                rdoDeactivate.Enabled = true;
                rdoReload.Enabled = true;
            }

            return true;
        }

        private void GetServiceInformation()
        {
            //Clean up any previous retrievals
            cboAvailableServices.Items.Clear();//Reset The Services Dropdown
            cboAvailableServices.Text = "";

            CboWorkFlowIdsByServiceId.Items.Clear();//Reset The Workflow Dropdown
            CboWorkFlowIdsByServiceId.Text = "";

            if(!_blnPersistedConfigExists)
                Helper.ServiceID = "";
            cboAvailableProfiles.Items.Clear();//Reset The Profiles Dropdown
            cboAvailableProfiles.Text = "";
            if (!_blnPersistedConfigExists) 
                Helper.MerchantProfileId = "";
            txtAboutTheService.Text = "";

            //Reset previously selected services
            _bcs = null;
            _ecks = null;
            _svas = null;

            //The GetServiceInformation() operation provides information about the services that are available to a specific Service Key. 
            //This operation should be automatically invoked during initial application configuration, and manually by an application 
            //administrator if/when available services are updated.

            _si = Helper.Cwssic.GetServiceInformation(Helper.SessionToken);
            
            if (_si.BankcardServices != null)
            {
                foreach (BankcardService BCS in _si.BankcardServices)
                {
                    cboAvailableServices.Items.Add(new Item(BCS.ServiceId + "-BCS-" + BCS.ServiceName, BCS.ServiceId, ""));
                }
            }
            if (_si.ElectronicCheckingServices != null)
            {
                foreach (ElectronicCheckingService ECKS in _si.ElectronicCheckingServices)
                {
                    cboAvailableServices.Items.Add(new Item(ECKS.ServiceId + "-ECK-" + Helper.TranslateServiceIdToFriendlyName(ECKS), ECKS.ServiceId, ""));
                }
            }
            if (_si.StoredValueServices != null)
            {
                foreach (StoredValueService SVAS in _si.StoredValueServices)
                {
                    cboAvailableServices.Items.Add(new Item(SVAS.ServiceId + "-SVAS-" + Helper.TranslateServiceIdToFriendlyName(SVAS), SVAS.ServiceId, ""));
                }
            }
            Helper.ServiceInformation = _si;
            txtPersistedAndCached.Text = "ApplicationProfileId : " + Helper.ApplicationProfileId + "\r\nServiceId : " + Helper.ServiceID + "\r\nWorkflowId : " + Helper.WorkflowID + "\r\nMerchantProfileId : " + Helper.MerchantProfileId;
        }

        public void GetMerchantProfileIds()
        {
            cboAvailableProfiles.Items.Clear();//Reset The Dropdown
            cboAvailableProfiles.Text = "";
            if (!_blnPersistedConfigExists)
                Helper.MerchantProfileId = "";

            /* A Merchant Profile is associated with a specific service id and tender type. For example, if there is a Bankcard 
             * service available that supports both Credit and PIN Debit, a Merchant Profile is needed for both Credit and PIN Debit.
             * The GetMerchantProfiles() operation retrieves all Merchant Profiles for a specific service and tender type.
            */
            List<string> MerchantProfileIds = Helper.Cwssic.GetMerchantProfileIds(Helper.SessionToken, Helper.ServiceID, TenderType.Credit);
            
            if (MerchantProfileIds != null)
            {
                foreach (string MPID in MerchantProfileIds)
                {
                    if (MPID == "")
                    {
                        //Although empty MerchantProfileIds should not exist perform the check just in case. 
                        cboAvailableProfiles.Items.Add(new Item("<default>", "", ""));
                    }
                    else
                    {
                        cboAvailableProfiles.Items.Add(new Item(MPID, MPID, ""));
                    }
                }
            }
            txtPersistedAndCached.Text = "ApplicationProfileId : " + Helper.ApplicationProfileId + "\r\nServiceId : " + Helper.ServiceID + "\r\nWorkflowId : " + Helper.WorkflowID + "\r\nMerchantProfileId : " + Helper.MerchantProfileId;
        }

        #endregion Service Information

        #region FORM EVENTS

        private void DefaultSetupValues()
        {
            //Reset all items as a new Service Key was selected
            Helper.ApplicationProfileId = "";
            Helper.ServiceID = "";
            Helper.MerchantProfileId = "";
            txtPersistedAndCached.Text = "";
            cmdPersistConfig.Enabled = false;
            cmdDeletePersistCached.Enabled = false;
            cmdSignOnWithToken.Enabled = false;
            cmdManageApplicationData.Enabled = false;
            cmdRetrieveServiceInformation.Enabled = false;
            //Disable all Transaction buttons
            cmdAuthorizeAndCapture.Enabled = false;
            cmdAuthorize.Enabled = false;
            cmdCapture.Enabled = false;
            cmdCaptureAll.Enabled = false;
            cmdCaptureSelective.Enabled = false;
            cmdQueryAccount.Enabled = false;
            cmdReturnById.Enabled = false;
            cmdReturnUnlinked.Enabled = false;
            cmdAdjust.Enabled = false;
            cmdUndo.Enabled = false;
            cmdVerify.Enabled = false;
            ChkMultiplePartialCapture.Enabled = false;
            //CmdAcknowledge.Enabled = false;
            //CmdRequestTransaction.Enabled = false;
            cmdManageAccount.Enabled = false;
            cmdManageAccountById.Enabled = false;
            cmdForcePost.Enabled = false;            

            cmdForcePost.Enabled = false;

            if (ConfigurationSettings.AppSettings["DelegatedSignOnSupported"] == "true")
            {
                txtDelegatedServiceKey.Visible = true;
                lnkLblDelegatedSignOn.Visible = true;
                ckBoxDelegatedSignOn.Visible = true;
            }
            if (ConfigurationSettings.AppSettings["DelegatedSignOnSupported"] == "false")
            {
                txtDelegatedServiceKey.Visible = false;
                lnkLblDelegatedSignOn.Visible = false;
                ckBoxDelegatedSignOn.Visible = false;
            }
            //Reset Check boxes
            chkStep1.Checked = false;
            chkStep2.Checked = false;
            chkStep3.Checked = false;
            ckBoxDelegatedSignOn.Checked = false;

            txtAboutTheService.Text = "";

            cboAvailableServices.Items.Clear();//Reset The Dropdown
            cboAvailableServices.Text = "";
            cboAvailableProfiles.Items.Clear();//Reset The Dropdown
            cboAvailableProfiles.Text = "";
            CboWorkFlowIdsByServiceId.Items.Clear();//Reset The Dropdown
            CboWorkFlowIdsByServiceId.Text = "";

            ChkCardNotPresent.Checked = true;
        }

        private void BindControlsToDataGenerator()
        {
            TxtAmount.DataBindings.Add("Text", dg, "Amount");
            TxtTip.DataBindings.Add("Text", dg, "Tip");
            TxtKeySerialNumber.DataBindings.Add("Text", dg, "KeySerialNumber");
            TxtEncryptedPIN.DataBindings.Add("Text", dg, "EncryptedPIN");
            TxtCashBack.DataBindings.Add("Text", dg, "CashBack");
            ChkAllowPartialApprovals.DataBindings.Add("Checked", dg, "AllowPartialApprovals");
            
            //CboCardTypes.DataBindings.Add("SelectedItem", dg, "CardTypes");
            TxtExpirationDate.DataBindings.Add("Text", dg, "ExpirationDate");
            TxtPAN.DataBindings.Add("Text", dg, "PAN");
            //CboTokenizedCard.DataBindings.Add("SelectedItem", dg, "TokenizedTransaction");
            TxtTrackDataFromMSR.DataBindings.Add("Text", dg, "TrackDataFromMSRVal");
            DataGenerator.Helper = Helper;
            chkEncryptIdentityToken.DataBindings.Add("Checked", dg, "EncryptedIdentityToken");
            chkStep2.DataBindings.Add("Checked", dg, "Step2");
            txtPersistedAndCached.DataBindings.Add("Text", dg, "PersistedAndCached");
            DataGenerator.SI = _si;
            DataGenerator.MerchantProfileIds = MerchantProfileIds;
        }

        private void cmdGo_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                if (txtIdentityToken.Text.Length < 1) { MessageBox.Show("Please enter a valid Identity Token"); return; }

                Helper.ServiceKey = Helper.RetrieveServiceKeyFromIdentityToken(txtIdentityToken.Text.Trim());
                Helper.IdentityToken = txtIdentityToken.Text;

                DefaultSetupValues();

                //Set the endpoint URI's to the values in the text boxes
                Helper.BaseSvcEndpointPrimary = txtSvcPrimary.Text;
                Helper.BaseSvcEndpointSecondary = txtSvcSecondary.Text;
                Helper.BaseTxnEndpointPrimary = txtTxnPrimary.Text;
                Helper.BaseTxnEndpointSecondary = txtTxnSecondary.Text;
                Helper.BaseTMSEndpointPrimary = txtTMSPrimary.Text;
                Helper.BaseTMSEndpointSecondary = txtTMSSecondary.Text;
                
                if (Helper.ServiceKey.Length > 0 && dg.LoadPersistedConfig())
                {
                    _blnPersistedConfigExists = true;
                    if (Helper.ApplicationProfileId.Length > 0 && Helper.ServiceID.Length > 0 && Helper.IdentityToken.Length > 0)
                    {
                        DialogResult Result;
                        Result = MessageBox.Show("Since you've previously persisted ApplicationProfileId, MerchantProfileId and ServiceId would you like to go directly to 'Transaction Processing'?", "Continue to Transaction Processing", MessageBoxButtons.YesNo);
                        if (Result == DialogResult.Yes)
                        {
                            tabControl1.SelectedTab = tbTransactionProcessing;
                        }
                        else
                        {
                            tabControl1.SelectedTab = tbPreparingApplicationToTransact;
                        }
                    }
                    else
                    {
                        tabControl1.SelectedTab = tbPreparingApplicationToTransact;
                        cmdSignOnWithToken.Enabled = true;
                        return;
                    }

                    if (!Helper.SetSvcEndpoint()) MessageBox.Show("Unable to setup the service endpoint\r\n" + Helper._message);
                    if (!Helper.SetTxnEndpoint()) MessageBox.Show("Unable to setup the Transaction endpoint\r\n" + Helper._message);
                    if (!Helper.SetTMSEndpoint()) MessageBox.Show("Unable to setup the TMS endpoint\r\n" + Helper._message);

                    cmdSignOnWithToken.Enabled = true;
                    cmdManageApplicationData.Enabled = true;
                    cmdRetrieveServiceInformation.Enabled = true;
                    CmdManageMerchantData.Enabled = true;

                    //Perform Step 1 as a previous configuration was already saved
                    Helper.CheckTokenExpire();
                    chkStep1.Checked = true;

                    //Check to see if a previously saved ServiceID exists. 
                    if (Helper.ServiceID.Length > 1)
                    {
                        //Perform Step 3 as a previous configuration was already saved
                        GetServiceInformation();
                        try
                        {
                            foreach (Item i in cboAvailableServices.Items)
                            {
                                if (i.Value1 == Helper.ServiceID)
                                {
                                    cboAvailableServices.SelectedItem = i;
                                    break;
                                }
                            }
                            chkStep3.Checked = true;
                            cmdPersistConfig.Enabled = true;
                            cmdDeletePersistCached.Enabled = true;
                        }
                        catch { }

                    }
                    //Check to see if a previously saved WorkflowId exists. 
                    if (Helper.WorkflowID.Length > 1)
                    {
                        try
                        {
                            foreach (Item i in CboWorkFlowIdsByServiceId.Items)
                            {
                                if (i.Value1 == Helper.WorkflowID)
                                {
                                    CboWorkFlowIdsByServiceId.SelectedItem = i;
                                    break;
                                }
                            }
                        }
                        catch { }
                    }
                    if (Helper.MerchantProfileId.Length > 1 | Helper.MerchantProfileId == "")
                    {
                        //Perform Step 4 as a previous configuration was already saved
                        GetMerchantProfileIds();
                        try
                        {
                            //cboAvailableProfiles.SelectedItem = _ProfileId;
                            foreach (Item i in cboAvailableProfiles.Items)
                            {
                                if (i.Value1 == Helper.MerchantProfileId)
                                {
                                    cboAvailableProfiles.SelectedItem = i;
                                    break;
                                }
                            }
                            cmdPersistConfig.Enabled = true;
                            cmdDeletePersistCached.Enabled = true;
                        }
                        catch { }
                    }

                    string strEncryptedIdentityToken = "\r\nIdentity Token [NOT ENCRYPTED]";
                    if (_blnEncryptedIdentityToken)
                    {
                        strEncryptedIdentityToken = "\r\nIdentity Token [ENCRYPTED]";
                        _blnEncryptedIdentityToken = true;
                    }
                    txtPersistedAndCached.Text = "ApplicationProfileId : " + Helper.ApplicationProfileId + "\r\nServiceId : " + Helper.ServiceID + "\r\nWorkflowId : " + Helper.WorkflowID + "\r\nMerchantProfileId : " + Helper.MerchantProfileId + strEncryptedIdentityToken;
                    _blnPersistedConfigExists = false;// set back to false so the form behaves as normal. 
                }
                else
                {
                    tabControl1.SelectedTab = tbPreparingApplicationToTransact;
                    cmdSignOnWithToken.Enabled = true;
                }

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cmdPopulateWithMyTestValues_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Identity token and PTLS SocketId are values provided by your solution consultant for both Sandbox testing as well as Production deployment." +
                            " For convenience these values are filled out automaticaly in the Sample Code for Sandbox testing. In production you'll need to update each value." +
                            " Please also note that the Identity Token expires at 3 years or if a security breach is detected.");

            //NOTE : The following values changes from Sandbox to Production.  These values are provided by your Solution Consultant
            PtlsSocketId = "MIIFCzCCA/OgAwIBAgICAoAwDQYJKoZIhvcNAQEFBQAwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb20wHhcNMTMwODIzMTg1NjA5WhcNMjMwODIxMTg1NjA5WjCBjDELMAkGA1UEBhMCVVMxETAPBgNVBAgTCENvbG9yYWRvMQ8wDQYDVQQHEwZEZW52ZXIxGjAYBgNVBAoTEUlQIENvbW1lcmNlLCBJbmMuMT0wOwYDVQQDEzRxYmtXM25TZ0FJQUFBUDhBSCtDY0FBQUVBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUFBQUE9MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAx68dD32BLjiDC9RdkIFY2P8N/bzvV75qWWemh0hO3zswggMY6BtKM7xVAoeVbEUP/HxOSlBasKE4tY/Y9hfDoqaszojt5BfqGYqAnUZ/7yjlfROdDu5q1p7AJ8DsEg9o5rpp0/88tj1+XK43JpE0PHtRecCdpsiKGclAdvaGRiXVMR0U6/nNjoNdptSo3Kd8DXSU4xWfiwrVWYUMu9otetiwutJNB3jUfsW5incr1OZ7vkFa58Eltb57UygQ5i31FSrVuBfS4UMQKVBP1V7wsVQlcC+QBNjlsGiATzdqtJBgcaI+BkPEJkF7kpDae3fNbQ77AhVFsoGV30bZCSoSNwIDAQABo4IBTjCCAUowCQYDVR0TBAIwADAdBgNVHQ4EFgQU2t+wf1VVGvks5M1zZlNa92YYUAEwgeYGA1UdIwSB3jCB24AU3+ASnJQimuunAZqQDgNcnO2HuHShgbekgbQwgbExNDAyBgNVBAMTK0lQIFBheW1lbnRzIEZyYW1ld29yayBDZXJ0aWZpY2F0ZSBBdXRob3JpdHkxCzAJBgNVBAYTAlVTMREwDwYDVQQIEwhDb2xvcmFkbzEPMA0GA1UEBxMGRGVudmVyMRowGAYDVQQKExFJUCBDb21tZXJjZSwgSW5jLjEsMCoGCSqGSIb3DQEJARYdYWRtaW5AaXBwYXltZW50c2ZyYW1ld29yay5jb22CCQD/yDY5hYVsVzA1BgNVHR8ELjAsMCqgKKAmhiRodHRwOi8vY3JsLmlwY29tbWVyY2UuY29tL2NhLWNybC5jcmwwDQYJKoZIhvcNAQEFBQADggEBAIGOvmbUPdUs3FMbQ95rpT7hShEkAbRnQjp8yY1ql48obQM0mTjQ4CfAXPELZ1xe8KyC4jaurW9KMuCkApwC8b8cgdKWg1ujtKkrNGhhDQRLcclNB6q5JTXrP0gQgrr43yHxh4vaAA8GTvkg7j2hrTUkksmc7JNIto0XsHlfvrUv8XCQIeQsFyy/nLHpQIkXwvAS6fcml6KMRTgQJm2yLZCfYVs6n18VDd9LCYWO9Y6majWoqgyHZ5Gy2qT7V+YxgDMUrZa7Fd66xHTWskO8wc7kuW5ZKaB29ewbAXIY31AHi4dAuGS6znPxnRg1kE01aDQ1FFCcajKtovg3di8PICU=";
            //Value provided by solution consultant. You'll have one idenityToken for Sandbox and a different one for production. The value needs 
            // to be configurable as the token expires every 3 years or if a security breach is detected.
            txtIdentityToken.Text = ConfigurationSettings.AppSettings["IdentityToken"];

            //Set the Primary and Secondary Endpoints from the Config
            txtSvcPrimary.Text = ConfigurationSettings.AppSettings["BaseSvcEndpointPrimary"];
            txtSvcSecondary.Text = ConfigurationSettings.AppSettings["BaseSvcEndpointSecondary"];
            txtTxnPrimary.Text = ConfigurationSettings.AppSettings["BaseTxnEndpointPrimary"];
            txtTxnSecondary.Text = ConfigurationSettings.AppSettings["BaseTxnEndpointSecondary"];
            txtTMSPrimary.Text = ConfigurationSettings.AppSettings["BaseDataServicesEndpointPrimary"];
            txtTMSSecondary.Text = ConfigurationSettings.AppSettings["BaseDataServicesEndpointSecondary"];
        }

        private void cboAvailableServices_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool blnMatch = false;
            Item item = (Item)cboAvailableServices.SelectedItem;
            Helper.ServiceID = item.Value1;
            if (!_blnPersistedConfigExists)
                Helper.MerchantProfileId = "";

            CboWorkFlowIdsByServiceId.Items.Clear();
            CboWorkFlowIdsByServiceId.Text = "";

            txtAboutTheService.Text = "";

            //Note : Since the sample application only show one class or the other, reset to null and initialize below.
            _bcs = null;
            _ecks = null;
            _svas = null;
            
            #region BankcardService
            if (_si.BankcardServices != null)
            {
                foreach (BankcardService BCS in _si.BankcardServices)
                {
                    if (BCS.ServiceId == Helper.ServiceID)
                    {
                        _bcs = BCS; //Set the BankCard Service to be used

                        SupportedTxnTypes = BCS.Operations;
                        //Toggle the buttons to match supported transaction types.
                        if (!AvailableTxnTypes(SupportedTxnTypes))
                        {
                            MessageBox.Show("This Service is not supported with CWS");
                            return;
                        }

                        blnMatch = true;

                        // Determine if it's possible to send more than one capture             
                        if (BCS.MultiplePartialCapture)
                            ChkMultiplePartialCapture.Enabled = true;

                        txtAboutTheService.Text = "ServiceId : " + BCS.ServiceId
                                                  //Unique identifier for available services. These values change from sandbox to production.
                                                  + "\r\nService Name : " + BCS.ServiceName
                                                  + "\r\nAlternative Merchant Data : " + BCS.AlternativeMerchantData
                                                  //Indicates whether the service supports the display of AlternativeMerchantData on accountholder statements. Subject to issuer discretion.
                                                  + "\r\nAutoSettle Supported : " + BCS.AutoBatch
                                                  + "\r\nEncryptionKey : " + BCS.EncryptionKey
                                                  //Indicates whether AutoBatch is supported.
                                                  + "\r\n\r\nAVSData Fields Required: "
                                                  + "\r\n - CardHolderName: " + BCS.AVSData.CardholderName
                                                  + "\r\n - Street: " + BCS.AVSData.Street
                                                  + "\r\n - City: " + BCS.AVSData.City
                                                  + "\r\n - State: " + BCS.AVSData.StateProvince
                                                  + "\r\n - PostalCode: " + BCS.AVSData.PostalCode
                                                  + "\r\n - Phone: " + BCS.AVSData.Phone
                                                  + "\r\n - Country: " + BCS.AVSData.Country
                                                  + "\r\nCutoffTime : " + BCS.CutoffTime
                                                  //If the service supports AutoBatch, this is the cutoff time for new transactions to be included in the daily automated batch. 
                                                  //+ "\r\nEncryptionKey : " + BCS.EncryptionKey //Public key used for encrypting payment account data.
                                                  + "\r\nManagedBilling : " + BCS.ManagedBilling
                                                  //Indicates whether the service supports Service Provider managed billing.
                                                  + "\r\nMaximumBatchItems : " + BCS.MaximumBatchItems
                                                  //Maximum number of items included in a single batch.
                                                  + "\r\nMaximumLineItems : " + BCS.MaximumLineItems
                                                  //Maximum number of line items that may be provided when providing Level 3 line item data.
                                                  + "\r\nMultiplePartialCapture : " + BCS.MultiplePartialCapture
                                                  //Indicates whether the service supports multiple partial captures of a single authorization.
                                                  + "\r\n\r\nOperations Supported:"
                                                  //Specifies which operations are supported for the service.
                                                  + "\r\n - Adjust : " + BCS.Operations.Adjust
                                                  + "\r\n - AuthAndCapture : " + BCS.Operations.AuthAndCapture
                                                  + "\r\n - Authorize : " + BCS.Operations.Authorize
                                                  + "\r\n - Capture : " + BCS.Operations.Capture
                                                  + "\r\n - CaptureAll : " + BCS.Operations.CaptureAll
                                                  + "\r\n - CaptureSelective : " + BCS.Operations.CaptureSelective
                                                  + "\r\n - CloseBatch : " + BCS.Operations.CloseBatch
                                                  + "\r\n - Disburse : " + BCS.Operations.Disburse
                                                  + "\r\n - ManageAccount : " + BCS.Operations.ManageAccount
                                                  + "\r\n - ManageAccountById : " + BCS.Operations.ManageAccountById
                                                  + "\r\n - QueryAccount : " + BCS.Operations.QueryAccount
                                                  + "\r\n - QueryRejected : " + BCS.Operations.QueryRejected
                                                  + "\r\n - ReturnById : " + BCS.Operations.ReturnById
                                                  + "\r\n - ReturnUnlinked : " + BCS.Operations.ReturnUnlinked
                                                  + "\r\n - Undo : " + BCS.Operations.Undo
                                                  + "\r\n - Verify : " + BCS.Operations.Verify
                                                  + "\r\nPurchaseCardLevel : " + BCS.PurchaseCardLevel
                                                  //Specifies highest level of purchase card support. Level 3 support implies support for Level 2 as well.
                                                  + "\r\n\r\nTender Information:" //Contains specific information about the supported payment tenders.
                                                  + "\r\n - BatchAssignmentSupport : " + BCS.Tenders.BatchAssignmentSupport //Indicates level of BatchAssignment supported by the service. Required.
                                                  + "\r\n - CredentialsRequired : " + BCS.Tenders.CredentialsRequired
                                                  + "\r\n - Credit : " + BCS.Tenders.Credit//Indicates support for the Credit Card tender. Required.
                                                  + "\r\n - CreditAuthorizeSupport : " + BCS.Tenders.CreditAuthorizeSupport //Indicates the level of credit authorization support on a service. Required. Maps to the CreditAuthorizeSupportType enumeration.
                                                  + "\r\n - CreditReturnSupportType : " + BCS.Tenders.CreditReturnSupportType
                                                  + "\r\n - CreditReversalSupportType : " + BCS.Tenders.CreditReversalSupportType
                                                  + "\r\n - PINDebit : " + BCS.Tenders.PINDebit//Indicates support for the PIN Debit tender. Required.
                                                  + "\r\n - PINDebitReturnSupportType : " + BCS.Tenders.PINDebitReturnSupportType //Indicates if Return is supported by the service for PIN Debit transactions. Required. Maps to the PinDebitReturnSupportType enumeration. Defaults to 'Standalone'.
                                                  + "\r\n - PinDebitUndoSupport : " + BCS.Tenders.PinDebitUndoSupport //Indicates if Undo is supported by the service for PIN Debit transactions. Required. Maps to the PINDebitUndoSupportType enumeration.
                                                  + "\r\n - PINDebitUndoTenderDataRequired : " + BCS.Tenders.PINDebitUndoTenderDataRequired //Specifies whether a new card swipe is required when performing an Undo on a PIN Debit card. Required.
                                                  + "\r\n - PINlessDebit : " + BCS.Tenders.PINlessDebit //Indicates support for PINless Debit. Required.
                                                  + "\r\n - PartialApprovalSupportType : " + BCS.Tenders.PartialApprovalSupportType
                                                  + "\r\n - QueryRejectedSupport : " + BCS.Tenders.QueryRejectedSupport
                                                  + "\r\n - TrackDataSupport : " + BCS.Tenders.TrackDataSupport
                            //Indicates if date ranges are supported for QueryRejected on a service. Required. Maps to the QueryRejectedSupportType enumeration.
                            ;

                        //show the arrow to indicate service information
                        picArrow.Visible = true;
                        tmrServiceArrow.Interval = 2000;
                        tmrServiceArrow.Start();
                        tmrServiceArrow.Enabled = true;
                        tmrServiceArrow.Tick += new EventHandler(OnTimedEvent);

                        //Enable Purchase Card buttons depending on the Servie returned
                        if (BCS.PurchaseCardLevel == PurchaseCardLevel.Level2)
                        {
                            cmdPurchaseCardL2.Enabled = true;
                            chkL2AuthAndCapture.Enabled = true;
                            cmdPurchaseCardL3.Enabled = false;
                            chkL3AuthAndCapture.Enabled = false;
                        }
                        if (BCS.PurchaseCardLevel == PurchaseCardLevel.Level3)
                        {
                            cmdPurchaseCardL2.Enabled = true;
                            chkL2AuthAndCapture.Enabled = true;
                            cmdPurchaseCardL3.Enabled = true;
                            chkL3AuthAndCapture.Enabled = true;
                        }

                        //Check to see if PINDebit supported
                        chkProcessAsPINDebitTxn.Enabled = (BCS.Tenders.PINDebit ? true : false);

                        //Check to see if PINLess Debit supported
                        ChkProcessAsPINLessDebit.Enabled = (BCS.Tenders.PINlessDebit ? true : false);

                        //Some service providers support partial approvals as well as forced void. 
                        ChkAllowPartialApprovals.Enabled = true;
                        ChkForceVoid.Enabled = true;

                        //Some service providers require Username and Password.
                        if (Helper.CredentialRequired())
                        {
                            txtCredUserName.Enabled = true;
                            txtCredPassword.Enabled = true;
                        }
                        else
                        {
                            txtCredUserName.Enabled = false;
                            txtCredPassword.Enabled = false;
                        }

                        //We now have something to persiste
                        cmdPersistConfig.Enabled = true;
                        cmdDeletePersistCached.Enabled = true;

                        //List the current service selected
                        if (Helper.ServiceID.Length > 0)
                        {
                            lblCurrentService.Text = @"Processing as Service : " +
                                                     BCS.ServiceName + @" - " +
                                                     BCS.ServiceId;
                            lblCurrentServiceClass.Text = @"Service Class : BCP - Bank Card Processing";
                        }
                    }
                }
            }

            #endregion END BankcardService

            #region ElectronicCheckingService
            if (_si.ElectronicCheckingServices != null)
            {
                foreach (ElectronicCheckingService ECKS in _si.ElectronicCheckingServices)
                {
                    if (ECKS.ServiceId == Helper.ServiceID)
                    {
                        _ecks = ECKS; //Set the BankCard Service to be used

                        SupportedTxnTypes = ECKS.Operations;
                        //Toggle the buttons to match supported transaction types.
                        if (!AvailableTxnTypes(SupportedTxnTypes))
                        {
                            MessageBox.Show("This Service is not supported with CWS");
                            return;
                        }

                        blnMatch = true;

                        txtAboutTheService.Text = "ServiceId : " + ECKS.ServiceId
                                                  //Unique identifier for available services. These values change from sandbox to production.
                                                  + "\r\nService Name : " +
                                                  Helper.TranslateServiceIdToFriendlyName(ECKS)
                                                  //Operations Supported
                                                  + "\r\n\r\nOperations Supported:"
                                                  //Specifies which operations are supported for the service.
                                                  + "\r\n - Adjust : " + ECKS.Operations.Adjust
                                                  + "\r\n - AuthAndCapture : " + ECKS.Operations.AuthAndCapture
                                                  + "\r\n - Authorize : " + ECKS.Operations.Authorize
                                                  + "\r\n - Capture : " + ECKS.Operations.Capture
                                                  + "\r\n - CaptureAll : " + ECKS.Operations.CaptureAll
                                                  + "\r\n - CaptureSelective : " + ECKS.Operations.CaptureSelective
                                                  + "\r\n - CloseBatch : " + ECKS.Operations.CloseBatch
                                                  + "\r\n - QueryAccount : " + ECKS.Operations.QueryAccount
                                                  + "\r\n - ReturnById : " + ECKS.Operations.ReturnById
                                                  + "\r\n - ReturnUnlinked : " + ECKS.Operations.ReturnUnlinked
                                                  + "\r\n - Undo : " + ECKS.Operations.Undo
                                                  + "\r\n - Verify : " + ECKS.Operations.Verify
                                                  + "\r\n\r\nTender Information:"
                                                  //Contains specific information about the supported payment tenders.
                                                  + "\r\n - BatchAssignmentSupport : " +
                                                  ECKS.Tenders.BatchAssignmentSupport
                                                  //Indicates level of BatchAssignment supported by the service. Required.
                                                  + "\r\n - Credit : " + ECKS.Tenders.Credit
                                                  //Indicates support for the Credit Card tender. Required.
                                                  + "\r\n - CreditAuthorizeSupport : " +
                                                  ECKS.Tenders.CreditAuthorizeSupport
                                                  //Indicates the level of credit authorization support on a service. Required. Maps to the CreditAuthorizeSupportType enumeration.
                                                  + "\r\n - PINDebit : " + ECKS.Tenders.PINDebit
                                                  //Indicates support for the PIN Debit tender. Required.
                                                  + "\r\n - PINDebitReturnSupportType : " +
                                                  ECKS.Tenders.PINDebitReturnSupportType
                                                  //Indicates if Return is supported by the service for PIN Debit transactions. Required. Maps to the PinDebitReturnSupportType enumeration. Defaults to 'Standalone'.
                                                  + "\r\n - PinDebitUndoSupport : " + ECKS.Tenders.PinDebitUndoSupport
                                                  //Indicates if Undo is supported by the service for PIN Debit transactions. Required. Maps to the PINDebitUndoSupportType enumeration.
                                                  + "\r\n - PINDebitUndoTenderDataRequired : " +
                                                  ECKS.Tenders.PINDebitUndoTenderDataRequired
                                                  //Specifies whether a new card swipe is required when performing an Undo on a PIN Debit card. Required.
                                                  + "\r\n - PINlessDebit : " + ECKS.Tenders.PINlessDebit
                                                  //Indicates support for PINless Debit. Required.
                                                  + "\r\n - QueryRejectedSupport : " + ECKS.Tenders.QueryRejectedSupport
                            //Indicates if date ranges are supported for QueryRejected on a service. Required. Maps to the QueryRejectedSupportType enumeration.
                            ;

                        //show the arrow to indicate service information
                        picArrow.Visible = true;
                        tmrServiceArrow.Interval = 2000;
                        tmrServiceArrow.Start();
                        tmrServiceArrow.Enabled = true;
                        tmrServiceArrow.Tick += new EventHandler(OnTimedEvent);

                        //Some service providers require Username and Password.
                        if (Helper.CredentialRequired())
                        {
                            txtCredUserName.Enabled = true;
                            txtCredPassword.Enabled = true;
                        }
                        else
                        {
                            txtCredUserName.Enabled = false;
                            txtCredPassword.Enabled = false;
                        }

                        //We now have something to persiste
                        cmdPersistConfig.Enabled = true;
                        cmdDeletePersistCached.Enabled = true;

                        //List the current service selected
                        if (Helper.ServiceID.Length > 0)
                        {
                            lblCurrentService.Text = @"Processing as Service : " +
                                                     Helper.TranslateServiceIdToFriendlyName(ECKS) + @" - " +
                                                     ECKS.ServiceId;
                            lblCurrentServiceClass.Text = @"Service Class : ECK - Electronic Check Processing";
                        }
                    }
                }
            }

            #endregion END ElectronicCheckingService

            #region StoredValueService
            if (_si.StoredValueServices != null)
            {
                foreach (StoredValueService SVAS in _si.StoredValueServices)
                {
                    if (SVAS.ServiceId == Helper.ServiceID)
                    {
                        _svas = SVAS; //Set the Stored Value Service to be used

                        SupportedTxnTypes = SVAS.Operations;
                        //Toggle the buttons to match supported transaction types.
                        if (!AvailableTxnTypes(SupportedTxnTypes))
                        {
                            MessageBox.Show("This Service is not supported with CWS");
                            return;
                        }

                        blnMatch = true;

                        txtAboutTheService.Text = "ServiceId : " + SVAS.ServiceId
                                                  //Unique identifier for available services. These values change from sandbox to production.
                                                  + "\r\nService Name : " +
                                                  Helper.TranslateServiceIdToFriendlyName(SVAS)
                                                  //Operations Supported
                                                  + "\r\n\r\nOperations Supported:"
                                                  //Specifies which operations are supported for the service.
                                                  + "\r\n - Adjust : " + SVAS.Operations.Adjust
                                                  + "\r\n - AuthAndCapture : " + SVAS.Operations.AuthAndCapture
                                                  + "\r\n - Authorize : " + SVAS.Operations.Authorize
                                                  + "\r\n - Capture : " + SVAS.Operations.Capture
                                                  + "\r\n - CaptureAll : " + SVAS.Operations.CaptureAll
                                                  + "\r\n - CaptureSelective : " + SVAS.Operations.CaptureSelective
                                                  + "\r\n - CloseBatch : " + SVAS.Operations.CloseBatch
                                                  + "\r\n - QueryAccount : " + SVAS.Operations.QueryAccount
                                                  + "\r\n - ReturnById : " + SVAS.Operations.ReturnById
                                                  + "\r\n - ReturnUnlinked : " + SVAS.Operations.ReturnUnlinked
                                                  + "\r\n - Undo : " + SVAS.Operations.Undo
                                                  + "\r\n - Verify : " + SVAS.Operations.Verify
                                                  + "\r\n\r\nTender Information:"
                                                  //Contains specific information about the supported payment tenders.
                                                  + "\r\n - BatchAssignmentSupport : " +
                                                  SVAS.Tenders.BatchAssignmentSupport
                                                  //Indicates level of BatchAssignment supported by the service. Required.
                                                  + "\r\n - Credit : " + SVAS.Tenders.Credit
                                                  //Indicates support for the Credit Card tender. Required.
                                                  + "\r\n - CreditAuthorizeSupport : " +
                                                  SVAS.Tenders.CreditAuthorizeSupport
                                                  //Indicates the level of credit authorization support on a service. Required. Maps to the CreditAuthorizeSupportType enumeration.
                                                  + "\r\n - PINDebit : " + SVAS.Tenders.PINDebit
                                                  //Indicates support for the PIN Debit tender. Required.
                                                  + "\r\n - PINDebitReturnSupportType : " +
                                                  SVAS.Tenders.PINDebitReturnSupportType
                                                  //Indicates if Return is supported by the service for PIN Debit transactions. Required. Maps to the PinDebitReturnSupportType enumeration. Defaults to 'Standalone'.
                                                  + "\r\n - PinDebitUndoSupport : " + SVAS.Tenders.PinDebitUndoSupport
                                                  //Indicates if Undo is supported by the service for PIN Debit transactions. Required. Maps to the PINDebitUndoSupportType enumeration.
                                                  + "\r\n - PINDebitUndoTenderDataRequired : " +
                                                  SVAS.Tenders.PINDebitUndoTenderDataRequired
                                                  //Specifies whether a new card swipe is required when performing an Undo on a PIN Debit card. Required.
                                                  + "\r\n - PINlessDebit : " + SVAS.Tenders.PINlessDebit
                                                  //Indicates support for PINless Debit. Required.
                                                  + "\r\n - QueryRejectedSupport : " + SVAS.Tenders.QueryRejectedSupport
                            //Indicates if date ranges are supported for QueryRejected on a service. Required. Maps to the QueryRejectedSupportType enumeration.
                            ;

                        //show the arrow to indicate service information
                        picArrow.Visible = true;
                        tmrServiceArrow.Interval = 2000;
                        tmrServiceArrow.Start();
                        tmrServiceArrow.Enabled = true;
                        tmrServiceArrow.Tick += new EventHandler(OnTimedEvent);

                        //Some service providers require Username and Password.
                        if (Helper.CredentialRequired())
                        {
                            txtCredUserName.Enabled = true;
                            txtCredPassword.Enabled = true;
                        }
                        else
                        {
                            txtCredUserName.Enabled = false;
                            txtCredPassword.Enabled = false;
                        }

                        //We now have something to persiste
                        cmdPersistConfig.Enabled = true;
                        cmdDeletePersistCached.Enabled = true;

                        //List the current service selected
                        if (Helper.ServiceID.Length > 0)
                        {
                            lblCurrentService.Text = @"Processing as Service : " +
                                                     Helper.TranslateServiceIdToFriendlyName(SVAS) + @" - " +
                                                     SVAS.ServiceId;
                            lblCurrentServiceClass.Text = @"Service Class : SVA - Stored Value Processing";
                        }
                    }
                }
            }

            #endregion END StoredValueService

            #region WorkflowId
            foreach (Workflow WF in _si.Workflows)
            {
                if (WF.ServiceId == Helper.ServiceID)
                {
                    //CboWorkFlowIdsByServiceId
                    CboWorkFlowIdsByServiceId.Items.Add(new Item("["+ WF.WorkflowId + "] " + WF.Name, WF.WorkflowId, ""));
                    //CboWorkFlowIds.Items.Add(new item("[" + node["WorkflowId"].InnerText + "] " + node["Name"].InnerXml, node["WorkflowId"].InnerText));
                }
            }
            #endregion END WorkflowId


            //List the current service selected
            if (Helper.ServiceID.Length > 0 && _bcs != null)
            {
                lblCurrentService.Text = @"Processing as Service : " + _bcs.ServiceName + @" - " + _bcs.ServiceId;
            }
            if (Helper.ServiceID.Length > 0 && _ecks != null)
            {
                lblCurrentService.Text = @"Processing as Service : " + _ecks.ServiceName + @" - " + _ecks.ServiceId;
            }
            GetMerchantProfileIds();
            
            if (!blnMatch) MessageBox.Show(@"ServiceId did not contain a match");

        }

        private void CboWorkFlowIdsByServiceId_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item item = (Item)CboWorkFlowIdsByServiceId.SelectedItem;
            Helper.WorkflowID = item.Value1;
        }

        private void OnTimedEvent(Object myObject, EventArgs myEventArgs)
        {// Specify what you want to happen when the Elapsed event is raised.
            tmrServiceArrow.Stop();
            picArrow.Visible = false;
        }
        
        private void cboAvailableProfiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            Item item = (Item)cboAvailableProfiles.SelectedItem;
            Helper.MerchantProfileId = item.Value1;

            //We now have something to persiste
            cmdPersistConfig.Enabled = true;
            cmdDeletePersistCached.Enabled = true;
            txtPersistedAndCached.Text = "*** Updated Value Ready for Persistence ***\r\nApplicationProfileId : " + Helper.ApplicationProfileId + "\r\nServiceId : " + Helper.ServiceID + "\r\nWorkflowId : " + Helper.WorkflowID + "\r\nMerchantProfileId : " + Helper.MerchantProfileId;        }

        private void cmdPersistConfig_Click(object sender, EventArgs e)
        {
            /*NOTE
             * Special consideration is necessary for protecting the identity token. You are required to be familure with the following
             * information regarding the identity token.
             * http://docs.evosnap.com/TransactionProcessing/CWS/Implementation_Guidelines/2.0.17/ServiceInformationGuidelines/AuthenticationProcess/IdentityTokens.aspx
            */
            try
            {
                Cursor = Cursors.WaitCursor;

                DialogResult Result;
                //Verify values to persist
                if (Helper.ApplicationProfileId.Length < 1 | Helper.ServiceID.Length < 1 | Helper.MerchantProfileId.Length < 1)
                {
                    string sMessage = "Missing value(s) for\r\n";
                    if (Helper.ApplicationProfileId.Length < 1) sMessage = sMessage + " - ApplicationProfileId\r\n";
                    if (Helper.ServiceID.Length < 1) sMessage = sMessage + " - ServiceID\r\n";
                    if (Helper.MerchantProfileId.Length < 1) sMessage = sMessage + " - MerchantProfileId\r\n";
                    Result = MessageBox.Show(sMessage + "\r\nContinue?", "Verify Values to Save", MessageBoxButtons.YesNo);
                    if (Result != DialogResult.Yes)
                        return;
                }

                string strIdentityToken = "";
                bool blnEncryptedIdentityToken = false;
                if (!chkEncryptIdentityToken.Checked)
                {
                    MessageBox.Show("Since the identity token is not going to be encrypted, the " +
                                "appilcation must securly protect the identity token with administrator file based securty");
                    strIdentityToken = txtIdentityToken.Text; //The identity token will be stored as clear text
                }
                else
                {
                    strIdentityToken = Helper.Encrypt(txtIdentityToken.Text);
                    blnEncryptedIdentityToken = true;
                }
                _blnEncryptedIdentityToken = false;
                string strIdentityTokenMessage = "\r\n\tIdentity Token [NOT ENCRYPTED]";
                if (chkEncryptIdentityToken.Checked)
                {
                    strIdentityTokenMessage = "\r\n\tIdentity Token [ENCRYPTED]";
                    _blnEncryptedIdentityToken = true;
                }
                MessageBox.Show("The following values will be persisted\r\n\r\n\tApplicationProfielId : " + Helper.ApplicationProfileId + "\r\n\tServiceId : " + Helper.ServiceID + "\r\n\tWorkflowId : " + Helper.WorkflowID + "\r\n\tProfileId : " + Helper.MerchantProfileId + strIdentityTokenMessage);
                PersistAndCacheSettings PACS = new PersistAndCacheSettings(Helper.ApplicationProfileId, Helper.ServiceID, Helper.WorkflowID, Helper.MerchantProfileId, blnEncryptedIdentityToken, strIdentityToken);
                dg.SavePersistedConfig(PACS);
                dg.LoadPersistedConfig();

                Result = MessageBox.Show("Configuration values successfully persisted. Continue to Transaction Processing", "Continue to Transaction Processing", MessageBoxButtons.YesNo);
                if (Result == DialogResult.Yes)
                    tabControl1.SelectedTab = tbTransactionProcessing;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void cmdDeletePersistCached_Click(object sender, EventArgs e)
        {
            //Delete Persisted/Cached value
            /*NOTE
              * SECURITY CONSIDERATIONS
              * Stored on file system with read/write permission for only the application/service and IT Administration
              * Stored in DB with read/write permission for only the application/service and IT Administration
            */
            //Delete

            try
            {
                Cursor = Cursors.WaitCursor;

                if (File.Exists(Helper.ServiceKey + "_TransactionProcessing.config"))
                {
                    DialogResult Result;
                    Result = MessageBox.Show("Are you sure you want to delete " + Helper.ServiceKey + "_TransactionProcessing.config?", "Delete File?", MessageBoxButtons.YesNo);
                    if (Result != DialogResult.Yes)
                        return;

                    File.Delete(Helper.ServiceKey + "_TransactionProcessing.config");
                    MessageBox.Show("'" + Helper.ServiceKey + "_TransactionProcessing.config' has been deleted");

                    Helper.ApplicationProfileId = "";
                    Helper.ServiceID = "";
                    Helper.MerchantProfileId = "";
                    Helper.WorkflowID = "";
                    DefaultSetupValues();
                    cmdSignOnWithToken.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Unable to find or delete " + Helper.ServiceKey + "_TransactionProcessing.config'");
                }
                dg.LoadPersistedConfig();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strIdentityTokenMessage = "\r\nIdentity Token [NOT ENCRYPTED]";
            if (_blnEncryptedIdentityToken)
            {
                strIdentityTokenMessage = "\r\nIdentity Token [ENCRYPTED]";
                _blnEncryptedIdentityToken = true;
            }

            applicationProfileIdToolStripMenuItem.Text = "Application ProfileId : " + Helper.ApplicationProfileId;
            serviceIdToolStripMenuItem.Text = "Service Id : " + Helper.ServiceID;
            workflowIdToolStripMenuItem.Text = "Workflow Id : " + Helper.WorkflowID;
            merchantProfileIdToolStripMenuItem.Text = "MerchantProfileId : " + Helper.MerchantProfileId;
            identityTokenToolStripMenuItem.Text = strIdentityTokenMessage;
            ServiceKeyToolStripMenuItem.Text = "Service Key : " + Helper.ServiceKey;
        }

        private void txtIdentityToken_KeyDown(object sender, KeyEventArgs e)
        {
            // See if Ctrl-A is pressed... 
            if (e.Control && (e.KeyCode == Keys.A))
            {
                txtIdentityToken.SelectAll();
                e.Handled = true;
            }
        }

        private void txtIdentityToken_TextChanged(object sender, EventArgs e)
        {
            TxtServiceKey.Visible = false;
            TxtServiceKey.Text = "Service Key: ";

            if (txtIdentityToken.Text.Length > 1)
            {
                TxtServiceKey.Text = "Service Key: " + Helper.RetrieveServiceKeyFromIdentityToken(txtIdentityToken.Text.Trim());
                if (TxtServiceKey.Text.Length > 13)
                    TxtServiceKey.Visible = true;
            }
            
        }

        private void txtCredUserName_TextChanged(object sender, EventArgs e)
        {
            Helper.CredUserName = txtCredUserName.Text;
        }

        private void txtCredPassword_TextChanged(object sender, EventArgs e)
        {
            Helper.CredPassword = txtCredPassword.Text;
        }

        private void chkCaptureAllAndSelectiveAsync_CheckedChanged(object sender, EventArgs e)
        {
            if (chkCaptureAllAndSelectiveAsync.Checked)
                MessageBox.Show(
                    "By selecting CaptureAll Async or CaptureSelective Async, your application will need to use TMS to query for the results of the batch.");
        }
        
        private void ChkAcknowledge_Click(object sender, EventArgs e)
        {
            MessageBox.Show("The Acknowledge() operation is used to mark a transaction as “acknowledged” after receiving "
                + "a successful transaction response. This acknowledgement is useful when performing transaction management "
                + "functions, such as those provided by the Transaction Management Service (TMS) API.\r\n\r\n"
                + "Once a successful transaction processing response has been received, the Acknowledge() operation can be "
                + "called to set an IsAcknowledged flag to “true” in the transaction database for a specific transactionId. "
                + "In the event that a transaction response is not received, specific Transaction Management operations can be "
                + "called to query the transaction database for transactions that have the IsAcknowledged flag set to “false”. "
                + "This is helpful when troubleshooting the reason for a failed transaction.");
        }

        private void ChkUserWorkflowId_CheckedChanged(object sender, EventArgs e)
        {
            if (Helper.WorkflowID.Length < 1)
            {
                MessageBox.Show("Workflow Id must be selected");
                tabControl1.SelectedTab = tbPreparingApplicationToTransact;
                return;
            }
        }

        private void ckBoxDelegatedSignOn_CheckedChanged(object sender, EventArgs e)
        {
            if (ckBoxDelegatedSignOn.Checked)
            {
                txtDelegatedServiceKey.Visible = true;
            }
            else
            {
                txtDelegatedServiceKey.Visible = false;
                txtDelegatedServiceKey.Clear();
            }
        }

        private void txtDelegatedServiceKey_TextChanged(object sender, EventArgs e)
        {
            Helper.ServiceKey = txtDelegatedServiceKey.Text;
        }

        private void ChkCardNotPresent_Click(object sender, EventArgs e)
        {
            resetPaymentMethods();
            ChkCardNotPresent.Checked = true;
            dg.processAs = ProcessAs.Keyed;
        }

        private void ChkTokenization_Click(object sender, EventArgs e)
        {
            resetPaymentMethods();

            if(CboTokenizedCard.Items.Count < 1)
            {
                MessageBox.Show("A previously approved transaction is requied before you can send a tokenized transaction");
                ChkCardNotPresent.Checked = true;
                return;
            }

            ChkTokenization.Checked = true;
            dg.processAs = ProcessAs.PaymentAccountDataToken;
        }

        private void ChkTrackDataFromMSR_Click(object sender, EventArgs e)
        {
            resetPaymentMethods();
            ChkTrackDataFromMSR.Checked = true;
            dg.processAs = ProcessAs.TrackDataFromMSR;

            if (dg._ITV._IndustryType == IndustryType.Ecommerce | dg._ITV._IndustryType == IndustryType.MOTO)
                MessageBox.Show("Please note that your current industry type is " + dg._ITV._IndustryType + " which typically does not support card present.");
        }

        private void chkProcessAsPINDebitTxn_Click(object sender, EventArgs e)
        {
            resetPaymentMethods();
            chkProcessAsPINDebitTxn.Checked = true;
            dg.processAs = ProcessAs.PINDebit;
        }
        
        private void ChkProcessAsPINLessDebit_Click(object sender, EventArgs e)
        {
            resetPaymentMethods();
            ChkProcessAsPINLessDebit.Checked = true;
            dg.processAs = ProcessAs.PINLessDebit;
        }

        private void resetPaymentMethods()
        {
            ChkCardNotPresent.Checked = false;
            ChkTokenization.Checked = false;
            ChkTrackDataFromMSR.Checked = false;
            chkProcessAsPINDebitTxn.Checked = false;
            ChkProcessAsPINLessDebit.Checked = false;
            CboTokenizedCard.SelectedIndex = -1;
        }

        private void CmdClearTransactions_Click(object sender, EventArgs e)
        {
            ChkLstTransactionsProcessed.Items.Clear();
        }

        private void ChkLstTransactionsProcessed_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ChkOnClickDisplayTxnMessage.Checked)
                return;

            ResponseDetails rdoActivate = ((ResponseDetails)(ChkLstTransactionsProcessed.SelectedItem));
            MessageBox.Show(Helper.ProcessResponse(ref rdoActivate));//Pass as reference so we can extract more values from the response
        }

        private void CboindustryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CboindustryType.Text.Length > 0)
            {
                if (CboindustryType.Text == "Ecommerce")
                    dg.getIndustryType("Ecommerce");
                if (CboindustryType.Text == "MOTO")
                    dg.getIndustryType("MOTO");
                if (CboindustryType.Text == "Retail")
                    dg.getIndustryType("Retail");
                if (CboindustryType.Text == "Restaurant")
                    dg.getIndustryType("Restaurant");
            }
        }

        private void CboCardTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            dg.cardType = (TypeCardType)CboCardTypes.SelectedItem;
            dg.SandboxTestCardData();
            TxtPAN.Text = dg.pan;
            TxtExpirationDate.Text = dg.expirationDate;
            TxtTrackDataFromMSR.Text = dg.trackDataFromMSRVal;
        }

        private void CboTokenizedCard_SelectedIndexChanged(object sender, EventArgs e)
        {
            dg.tokenizedTransaction = (TokenizedTransaction)CboTokenizedCard.SelectedItem;
        }

        #endregion END FORM EVENTS

        #region Setup Help Links

        private void linkPreparingAppToTransact_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/");
        }
        private void lnkTxnProcessing_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/");
        } 
        private void lnkSignOnWithToken_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/#signonwithtoken-soap");
        }
        private void lnkLblDelegatedSignOn_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/TransactionProcessing/CWS/Developer_Guide/2.0.18/Implementation/PreparingTheAppToTransact/SignOnAuthentication/DelegatedSignOn.aspx");
        }
        private void lnkManageApplicationData_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/#getapplicationdata-soap");
        }
        private void lnkRetrieveServiceInformation_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/#getserviceinformation-soap");
        }
        private void lnkManageMerchantProfiles_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/#getmerchantprofile-soap");
        }
        private void lnkAuthorizeAndCapture_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#authorizeandcapture-soap");
        }
        private void lnkAuthorize_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#authorize-soap");
        }
        private void lnkAdjust_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#adjust-soap");
        }
        private void lnkUndo_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#undo-soap");
        }
        private void lnkCapture_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#capture-soap");
        }
        private void lnkCaptureAll_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#captureall-soap");
        }
        private void lnkCaptureSelective_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#captureselective-soap");
        }
        private void lnkReturnById_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#returnbyid-soap");
        }
        private void lnkReturnUnlinked_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#returnunlinked-soap");
        }
        private void lnkQueryAccount_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#queryaccount-soap");
        }
        private void lnkVerify_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#verify-soap");
        }
        private void lnkServiceKey_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/cwsdevelopersguide/#service-information-endpoints");
        }
        private void lnkAccessingWebServiceEndpoints_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/cwsdevelopersguide/");
        }
        private void lnkIdentityToken_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/preparing-the-application-to-transact-2/#signonwithtoken-soap");
        }
        private void lnkOnlineDocumentation_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support");
        }
        private void lnkAcknowledge_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#acknowledge-soap");
        }
        private void LnkRequestTransaction_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#requesttransaction-soap");
        }
        private void lnkManageAccount_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#manageaccount-soap");
        }
        private void lnkManageAccountById_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.evosnap.com/support/knowledgebase/transaction-processing/#manageaccount-soap");
        }
       
        #endregion Setup Help Links

        private void lnkOnlineDocumentation_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

    #endregion Helper Methods

        private void ChkAllowPartialApprovals_CheckedChanged(object sender, EventArgs e)
        {

        }

       

    }
}
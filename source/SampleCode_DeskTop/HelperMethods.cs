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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using SampleCode.CWSServiceInformation;
using SampleCode.CWSTransactionManagement;
using SampleCode.CwsTransactionProcessing;
using Response = SampleCode.CwsTransactionProcessing.Response;
using BankcardTransactionResponse = SampleCode.CwsTransactionProcessing.BankcardTransactionResponse;
using BankcardCaptureResponse = SampleCode.CwsTransactionProcessing.BankcardCaptureResponse;
using Capture = SampleCode.CwsTransactionProcessing.Capture;
using BankcardTransactionPro = SampleCode.CwsTransactionProcessing.BankcardTransactionPro;
using BankcardTransactionResponsePro = SampleCode.CwsTransactionProcessing.BankcardTransactionResponsePro;
using Addendum = SampleCode.CwsTransactionProcessing.Addendum;
using BankcardTransaction = SampleCode.CwsTransactionProcessing.BankcardTransaction;
using ElectronicCheckingCaptureResponse = SampleCode.CwsTransactionProcessing.ElectronicCheckingCaptureResponse;
using ElectronicCheckingTransaction = SampleCode.CwsTransactionProcessing.ElectronicCheckingTransaction;
using ElectronicCheckingTransactionResponse = SampleCode.CwsTransactionProcessing.ElectronicCheckingTransactionResponse;
using TypeCardType = SampleCode.CwsTransactionProcessing.TypeCardType;
using Unmanaged = SampleCode.CwsTransactionProcessing.Unmanaged;
using Status = SampleCode.CwsTransactionProcessing.Status;
using StoredValueTransaction = SampleCode.CwsTransactionProcessing.StoredValueTransaction;
using StoredValueTransactionResponse = SampleCode.CwsTransactionProcessing.StoredValueTransactionResponse;
using AlternativeMerchantData = SampleCode.CwsTransactionProcessing.AlternativeMerchantData;
using AddressInfo = SampleCode.CwsTransactionProcessing.AddressInfo;
using PrepaidCard = SampleCode.CwsTransactionProcessing.PrepaidCard;
using TypeISOCountryCodeA3 = SampleCode.CwsTransactionProcessing.TypeISOCountryCodeA3;


namespace SampleCode
{
    public class HelperMethods
    {
        #region Variable Declarations
        //Primary webservice objects
        private CWSServiceInformationClient _cwssic = new CWSServiceInformationClient();
        private CwsTransactionProcessingClient _cwsbc = new CwsTransactionProcessingClient();
        private TMSOperationsClient _tmsoc = new TMSOperationsClient();

        private string _applicationProfileId = ""; //Obtained after saving application data
        private string _serviceId = ""; //Unique to each service
        private string _workflowId = ""; //Unique to each bundle of module and service 
        private string _merchantProfileId = ""; //Your application will have one to many merchantProfileId's depending on how many merchant accounts you have.
        private string _sessionToken = ""; //The sessionToken lives for only 30 minutes. The CheckTokenExpire() method below rotates this value after 25 minutes as elapsed. 
        private string _delegatedSessionToken = ""; //The sessionToken lives for only 30 minutes. The CheckTokenExpire() method below rotates this value after 25 minutes as elapsed. 
        private string _identityToken = ""; //The identity token is the primary token used to gain access to CWS. This value expires every 3 year or if a security breach is detected. You application need to have a way to update this value at any time. 
        private string _serviceKey = "";//One identity token can have one to many serviceKey. ServiceKey's are tied to service available.
        private string _delegatedServiceKey = ""; // ServiceKey that has been delegated to the Initiator IdentityToken.
        private ServiceInformation _serviceInformation = new ServiceInformation();
        private FaultHandler.FaultHandler _FaultHandler = new FaultHandler.FaultHandler();
        public bool BlnDedicated;//Used to determine if this is a dynamic or dedicated solution.
        public DateTime DtSessionToken; //Used by CheckTokenExpire() to determine when the session token needs to be updated

        public string BaseSvcEndpointPrimary =  ConfigurationSettings.AppSettings["BaseSvcEndpointPrimary"]; //The following would likely come from a config value or database
        public string BaseSvcEndpointSecondary = ConfigurationSettings.AppSettings["BaseSvcEndpointSecondary"]; //The following would likely come from a config value or database
        public string BaseTxnEndpointPrimary = ConfigurationSettings.AppSettings["BaseTxnEndpointPrimary"]; //The following would likely come from a config value or database
        public string BaseTxnEndpointSecondary = ConfigurationSettings.AppSettings["BaseTxnEndpointSecondary"]; //The following would likely come from a config value or database
        public string BaseTMSEndpointPrimary = ConfigurationSettings.AppSettings["BaseDataServicesEndpointPrimary"]; //The following would likely come from a config value or database
        public string BaseTMSEndpointSecondary = ConfigurationSettings.AppSettings["BaseDataServicesEndpointSecondary"]; //The following would likely come from a config value or database

        //The following is used to track Primary versus Secondary Endpoint use
        private bool _blnSvcInfoPrimary = true; //Used to track if primary or secondary is currently being used
        private bool _blnTxnPrimary = true; //Used to track if primary or secondary is currently being used
        private bool _blnTMSPrimary = true; //Used to track if primary or secondary is currently being used

        //Enctyption based security key password
        private const string SKey = "204E3EB69155460F85A8F7AB0F609334"; //Using a random GUID for the key. Your application should replace this value with a new GUID for uniqueness. 
        
        //Some service providers require a username and user password with every transaction
        private string _credUserName = "";
        private string _credPassword= "";

        //The following are used to switch the URI for posting data
        private static object bankCardChannelLock = new object();
        private static object svcInfoChannelLock = new object();
        private static object TMSChannelLock = new object();

        //Send an acknowledge response for each transaction
        public bool BlnAcknowledge;

        //Message regarding processing. Likely instead of this your software would write the results directly to your database or a log file. 
        public string _message;
        
        #endregion Variable Declarations

        public void CheckTokenExpire()
        {
            _message = "";
            if (DateTime.UtcNow.Subtract(DtSessionToken).TotalMinutes > 25)//Use 25 as the baseline for renewing session tokens
            {
                try
                {
                    //if (_delegatedServiceKey != "")
                    //{
                    //    _delegatedSessionToken = Cwssic.DelegatedSignOn(_identityToken, _delegatedServiceKey, null);
                    //    _sessionToken = _delegatedSessionToken;
                    //}
                    //else
                    {
                        _sessionToken = Cwssic.SignOnWithToken(_identityToken);
                    }
                    DtSessionToken = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    string strErrorId;
                    string strErrorMessage;
                    _FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage);

                    Exception e = new Exception("Unable to Refresh a new Token\r\nError Message : " + ex.Message
                                                + "\r\n\r\nUnable to Refresh a new Token\r\nError Message : " + strErrorId + " : "
                                                + strErrorMessage);
                    throw e;
                }
            }
        }

        #region Data Encryption and Decryption

        public string Encrypt(string identityToken)
        {
            //Note : Encrypte the identity token so that it's safe from any non-administrative personel
            //Reference (String) : http://www.codeproject.com/KB/security/DotNetCrypto.aspx
            //Reference (File) : http://support.microsoft.com/kb/307010

            // First we need to turn the input string into a byte array. 
            byte[] clearBytes = System.Text.Encoding.Unicode.GetBytes(identityToken);

            // Then, we need to turn the password into Key and IV We are using salt to make it harder to guess our key
            // using a dictionary attack - trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(SKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the encryption using the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes. 
            byte[] encryptedData = Encrypt(clearBytes, pdb.GetBytes(32), pdb.GetBytes(16));


            // Now we need to turn the resulting byte array into a string. A common mistake would be to use an Encoding class for that.
            //It does not work because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is designed exactly for what we are trying to do. 
            return Convert.ToBase64String(encryptedData);
             
        }

        public string Decrypt(string identityToken)
        {
            // Since the identity token is encrypted you'll need to decrypt

            // First we need to turn the input string into a byte array. We presume that Base64 encoding was used 
            byte[] cipherBytes = Convert.FromBase64String(identityToken);

            // Then, we need to turn the password into Key and IV We are using salt to make it harder to guess our key
            // using a dictionary attack - trying to guess a password by enumerating all possible words. 
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(SKey,
                new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

            // Now get the key/IV and do the decryption using the function that accepts byte arrays. 
            // Using PasswordDeriveBytes object we are first getting 32 bytes for the Key 
            // (the default Rijndael key length is 256bit = 32bytes) and then 16 bytes for the IV. 
            // IV should always be the block size, which is by default 16 bytes (128 bit) for Rijndael. 
            // If you are using DES/TripleDES/RC2 the block size is 8 bytes and so should be the IV size. 
            // You can also read KeySize/BlockSize properties off the algorithm to find out the sizes. 
            byte[] decryptedData = Decrypt(cipherBytes, pdb.GetBytes(32), pdb.GetBytes(16));

            // Now we need to turn the resulting byte array into a string. A common mistake would be to use an Encoding class for that.
            // It does not work  because not all byte values can be represented by characters. 
            // We are going to be using Base64 encoding that is designed exactly for what we are trying to do. 
            return System.Text.Encoding.Unicode.GetString(decryptedData);
        }

        // Encrypt a byte array into a byte array using a key and an IV 
        private static byte[] Encrypt(byte[] clearData, byte[] key, byte[] IV)
        {
            // Create a MemoryStream to accept the encrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. We are going to use Rijndael because it is strong and
            // available on all platforms. You can use other algorithms, to do so substitute the
            // next line with something like TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. We need the IV (Initialization Vector) because
            // the algorithm is operating in its default mode called CBC (Cipher Block Chaining).
            // The IV is XORed with the first block (8 byte) of the data before it is encrypted, and then each
            // encrypted block is XORed with the following block of plaintext. This is done to make encryption more secure. 

            // There is also a mode called ECB which does not need an IV, but it is much less secure. 
            alg.Key = key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be pumping our data. 
            // CryptoStreamMode.Write means that we are going to be writing data to the stream and the output will be written
            // in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the encryption 
            cs.Write(clearData, 0, clearData.Length);

            // Close the crypto stream (or do FlushFinalBlock). This will tell it that we have done our encryption and
            // there is no more data coming in, and it is now a good time to apply the padding and finalize the encryption process. 
            cs.Close();

            // Now get the encrypted data from the MemoryStream. Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] encryptedData = ms.ToArray();
            ms.Close();

            return encryptedData;
        }

        // Decrypt a byte array into a byte array using a key and an IV 
        private static byte[] Decrypt(byte[] cipherData, byte[] key, byte[] IV)
        {
            // Create a MemoryStream that is going to accept the
            // decrypted bytes 
            MemoryStream ms = new MemoryStream();

            // Create a symmetric algorithm. We are going to use Rijndael because it is strong and
            // available on all platforms. You can use other algorithms, to do so substitute the next
            // line with something like TripleDES alg = TripleDES.Create(); 
            Rijndael alg = Rijndael.Create();

            // Now set the key and the IV. We need the IV (Initialization Vector) because the algorithm
            // is operating in its default mode called CBC (Cipher Block Chaining). The IV is XORed with
            // the first block (8 byte) of the data after it is decrypted, and then each decrypted
            // block is XORed with the previous cipher block. This is done to make encryption more secure. 
            // There is also a mode called ECB which does not need an IV, but it is much less secure. 
            alg.Key = key;
            alg.IV = IV;

            // Create a CryptoStream through which we are going to be pumping our data. CryptoStreamMode.Write means that we are going to be writing data to the stream 
            // and the output will be written in the MemoryStream we have provided. 
            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);

            // Write the data and make it do the decryption 
            cs.Write(cipherData, 0, cipherData.Length);

            // Close the crypto stream (or do FlushFinalBlock). This will tell it that we have done our decryption
            // and there is no more data coming in, and it is now a good time to remove the padding and finalize the decryption process. 
            cs.Close();

            // Now get the decrypted data from the MemoryStream. Some people make a mistake of using GetBuffer() here,
            // which is not the right way. 
            byte[] decryptedData = ms.ToArray();
            ms.Close();

            return decryptedData;
        }

        #endregion Data Encryption and Decryption

        #region Changing URI and Service Key

        public bool SetSvcEndpoint()
        {
            _message = "";

            string ServiceKey = "";
            if (BlnDedicated) { ServiceKey = "/" + _serviceKey; }//Switch to use Dedicated or Dynamic endpoints.

            try
            {
                if (_blnSvcInfoPrimary)
                {
                    _blnSvcInfoPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                    //Try the primary URI for CWSServiceInformationClient
                    Cwssic = GetServiceInfoChannel(ServiceKey, BaseSvcEndpointPrimary);
                }
                else
                {
                    _blnSvcInfoPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                    //Try the secondary URI for CWSServiceInformationClient
                    Cwssic = GetServiceInfoChannel(ServiceKey, BaseSvcEndpointSecondary);
                }
                return true;
            }
            catch (EndpointNotFoundException)
            {
                #region EndpointNotFoundException
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    if (_blnSvcInfoPrimary)
                    {
                        _blnSvcInfoPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                        //Try the primary URI for CWSServiceInformationClient
                        Cwssic = GetServiceInfoChannel(ServiceKey, BaseSvcEndpointPrimary);
                    }
                    else
                    {
                        _blnSvcInfoPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                        //Try the secondary URI for CWSServiceInformationClient
                        Cwssic = GetServiceInfoChannel(ServiceKey, BaseSvcEndpointSecondary);
                    }
                    return true;
                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                    return false;
                }
                catch (Exception ex)
                {
                    _message += "Unable to SigOn\r\nError Message : " + ex.Message;
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage))
                { _message += strErrorId + " : " + strErrorMessage; }
                else { _message += ex.Message; }
                return false;
            }
        }

        public bool SetTxnEndpoint()
        {
            _message = "";

            string ServiceKey = "";
            if (BlnDedicated) { ServiceKey = "/" + _serviceKey; }//Switch to use Dedicated or Dynamic endpoints

            try
            {
                if (_blnTxnPrimary)
                {
                    _blnTxnPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                    //Try the primary URI for CWSServiceInformationClient
                    Cwsbc = GetTxnChannel(ServiceKey, BaseTxnEndpointPrimary);
                }
                else
                {
                    _blnTxnPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                    //Try the secondary URI for CWSServiceInformationClient
                    Cwsbc = GetTxnChannel(ServiceKey, BaseTxnEndpointSecondary);
                }
                return true;
            }
            catch (EndpointNotFoundException)
            {
                #region EndpointNotFoundException
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    if (_blnTxnPrimary)
                    {
                        _blnTxnPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                        //Try the primary URI for CWSServiceInformationClient
                        Cwsbc = GetTxnChannel(ServiceKey, BaseTxnEndpointPrimary);
                    }
                    else
                    {
                        _blnTxnPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                        //Try the secondary URI for CWSServiceInformationClient
                        Cwsbc = GetTxnChannel(ServiceKey, BaseTxnEndpointSecondary);
                    }
                    return true;
                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                    return false;
                }
                catch (Exception ex)
                {
                    _message += "Unable to SigOn\r\nError Message : " + ex.Message;
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage))
                { _message += strErrorId + " : " + strErrorMessage; }
                else { _message += ex.Message; }
                return false;
            }
        }

        public bool SetTMSEndpoint()
        {
            _message = ""; 

            string ServiceKey = "";
            if (BlnDedicated) { ServiceKey = "/" + _serviceKey; }//Switch to use Dedicated or Shared endpoints

            try
            {
                if (_blnTMSPrimary)
                {
                    _blnTMSPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                    //Try the primary URI for CWSServiceInformationClient
                    Tmsoc = GetTMSChannel(ServiceKey, BaseTMSEndpointPrimary);
                }
                else
                {
                    _blnTMSPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                    //Try the secondary URI for CWSServiceInformationClient
                    Tmsoc = GetTMSChannel(ServiceKey, BaseTMSEndpointSecondary);
                }
                return true;
            }
            catch (EndpointNotFoundException)
            {
                #region EndpointNotFoundException
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    if (_blnTMSPrimary)
                    {
                        _blnTMSPrimary = false; //Flip the boolean so that next time the secondary Endpoint is used.
                        //Try the primary URI for CWSServiceInformationClient
                        Tmsoc = GetTMSChannel(ServiceKey, BaseTMSEndpointPrimary);
                    }
                    else
                    {
                        _blnTMSPrimary = true; //Flip the boolean so that next time the primary Endpoint is used.
                        //Try the secondary URI for CWSServiceInformationClient
                        Tmsoc = GetTMSChannel(ServiceKey, BaseTMSEndpointSecondary);
                    }
                    return true;
                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                    return false;
                }
                catch (Exception ex)
                {
                    _message += "Unable to SigOn\r\nError Message : " + ex.Message;
                    return false;
                }
                #endregion
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleSvcInfoFault(ex, out strErrorId, out strErrorMessage))
                { _message += strErrorId + " : " + strErrorMessage; }
                else { _message += ex.Message; }
                return false;
            }
        }

        public CWSServiceInformationClient GetServiceInfoChannel(string serviceActivationKey, string Uri)
        {
            lock (svcInfoChannelLock)
            {
                CWSServiceInformationClient client = new CWSServiceInformationClient();
                client.Endpoint.Address = new EndpointAddress(Uri + serviceActivationKey);
                client.Open();
                return client;
            }
        }

        public CwsTransactionProcessingClient GetTxnChannel(string serviceActivationKey, string Uri)
        {
            lock (bankCardChannelLock)
            {
                CwsTransactionProcessingClient client = new CwsTransactionProcessingClient();
                client.Endpoint.Address = new EndpointAddress(Uri + serviceActivationKey);
                client.Open();
                return client;
            }
        }

        public TMSOperationsClient GetTMSChannel(string serviceActivationKey, string Uri)
        {
            lock (TMSChannelLock)
            {
                TMSOperationsClient client = new TMSOperationsClient();
                client.Endpoint.Address = new EndpointAddress(Uri + serviceActivationKey);
                client.Open();
                return client;
            }
        }

        #endregion END Changing URI and Service Key

        #region BankCardTransaction Methods

        public Addendum CredentialsRequired(string serviceID, string username, string password)
        {
            //The following items have to be included in every transaction and is specific to each individual merchant. 
            //A UI has to be in place to support entry of these data elements
            if (username.Length > 1 && password.Length > 1)
            {
                List<string> _add = new List<string>();
                //if (serviceID == "35A7700001" || serviceID == "DF29D1300C")
                //{
                    _add.Add("<UserId>" + username + "</UserId>");
                    _add.Add("<Password>" + password + "</Password>");
                //}
                //else
                //{
                //    _add.Add("<Username>" + username + "</Username>");
                //    _add.Add("<Password>" + password + "</Password>");
                //}
                Addendum a = new Addendum();
                a.Unmanaged = new Unmanaged();
                a.Unmanaged.Any = _add;
                return a;
            }
            else
            {
                Exception e = new Exception("This ServiceId requires a Username and Password");
                throw e;
            }
        }

        public bool CredentialRequired()
        {

            //Affirmative ACH Now
            if (ServiceID == "35A7700001") return true; //Sandbox
            if (ServiceID == "DF29D1300C") return true; //Production
			
            return false;
        }

        public AlternativeMerchantData SetSoftDescriptors()
        {
            AlternativeMerchantData AMD = new AlternativeMerchantData();
            //Set Soft Descriptors
                                //BCP74                     BCP68                       BCP64         
            if (_serviceId == "D806000001" | _serviceId == "B447F00001" | _serviceId == "88D9300001" |  //Sandbox
                _serviceId == "DEDC61300C" | _serviceId == "1A8871300C" | _serviceId == "C43CC1300C")   //Production
            {
                AMD.Name = "ABC Merchant";
                AMD.Address = new AddressInfo();
                AMD.Address.City = "Denver";
                AMD.Address.CountryCode = TypeISOCountryCodeA3.USA;
                AMD.Address.StateProvince = "CO";
                AMD.Address.Street1 = "1234 Happy St";
                AMD.Address.PostalCode = "80023";
                AMD.CustomerServicePhone = "303 3575700";
                AMD.CustomerServiceInternet = "www.test.com";
            }
            else
            {//Not found so return null
                AMD = null;
            }

            return AMD;
        }

    
        public List<ResponseDetails> ProcessBCPTransaction(
            TransactionType _TT //Required
            , BankcardTransaction _BCtransaction //Conditional : Only used for an AuthorizeAndCapture, Authorize and ReturnUnlinked. Otherwise null
            , BankcardCapture _BCDifferenceData //Conditional : Only used for a Capture. Otherwise null
            , List<String> _BatchIds //Conditional : A list of one or more batch Ids to capture.
            , BankcardReturn _RDifferenceData //Conditional : Only used for a ReturnById. Otherwise null
            , Adjust _ADifferenceData //Conditional : Only used for an Adjust. Otherwise null
            , BankcardUndo _UDifferenceData //Conditional : Only used for an Undo. Otherwise null
            , List<string> _TransactionIds //Conditional : Only used for a CaptureSelective. Otherwise null
            , List<Capture> _CaptureDifferenceData //Conditional : Only used for CaptureAll and CaptureSelective. Otherwise null
            , bool _SendAcknowledge
            , bool _ForceClose)  
        {
            List<Response> _Response = new List<Response>();
            try
            {
                CheckTokenExpire();//Make sure the current token is valid

                string _serviceIdOrWorkflowId = _serviceId;

                if (_TT == TransactionType.AuthorizeAndCapture)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.AuthorizeAndCapture(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceIdOrWorkflowId));
                    //Always Verify that the requested amount and approved amount are the same. 
                    BankcardTransactionResponse BCR = new BankcardTransactionResponse();
                    BCR = (BankcardTransactionResponse)_Response[0];
                    if (_BCtransaction.TransactionData.Amount != BCR.Amount)
                        _message += "The transaction was approved for " + BCR.Amount
                            + " which is an amount not equal to the requested amount of " + _BCtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }

                if (_TT == TransactionType.Authorize)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Authorize(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceIdOrWorkflowId));
                    //Always Verify that the requested amount and approved amount are the same. 
                    BankcardTransactionResponse BCR = new BankcardTransactionResponse();
                    BCR = (BankcardTransactionResponse)_Response[0];
                    if (_BCtransaction.TransactionData.Amount != BCR.Amount)
                        _message += "The transaction was approved for " + BCR.Amount
                            + " which is an amount not equal to than the requested amount of " + _BCtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }
                if (_TT == TransactionType.Capture)
                {
                    if (CredentialRequired())
                        _BCDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Capture(_sessionToken, _BCDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.CaptureAll)
				{
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response = Cwsbc.CaptureAll(_sessionToken, _CaptureDifferenceData, _BatchIds, _applicationProfileId, _merchantProfileId, _serviceId, _ForceClose);
				}
                if (_TT == TransactionType.CaptureAllAsync)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.CaptureAllAsync(_sessionToken, _CaptureDifferenceData, _BatchIds, _applicationProfileId, _merchantProfileId, _serviceId, _ForceClose));
                }
                if (_TT == TransactionType.CaptureSelective)
				{
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);                 
					_Response = Cwsbc.CaptureSelective(_sessionToken, _TransactionIds, _CaptureDifferenceData, _applicationProfileId, _serviceId);
				}
                if (_TT == TransactionType.CaptureSelectiveAsync)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.CaptureSelectiveAsync(_sessionToken, _TransactionIds, _CaptureDifferenceData,
                                                              _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.ReturnById)
                {
                    if (CredentialRequired())
                        _RDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.ReturnById(_sessionToken, _RDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.Return)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.ReturnUnlinked(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                }
                if (_TT == TransactionType.Adjust)
                    _Response.Add(Cwsbc.Adjust(_sessionToken, _ADifferenceData, _applicationProfileId, _serviceId));
                if (_TT == TransactionType.Undo)
                {
                    if (CredentialRequired())
                        _UDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Undo(_sessionToken, _UDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.QueryAccount)
                    _Response.Add(Cwsbc.QueryAccount(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                if (_TT == TransactionType.Verify)
                    _Response.Add(Cwsbc.Verify(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceId));

                List<ResponseDetails> RD = new List<ResponseDetails>();//Convert the response to response details so that we can report on the UI
                if (_Response != null)
                {
                    foreach (Response r in _Response)
                    {
                        if (_SendAcknowledge && r.TransactionId.Length > 0)
                            Cwsbc.Acknowledge(_sessionToken, r.TransactionId, _applicationProfileId, _serviceId);

                        ResponseDetails RDN = new ResponseDetails(0.00M, r, _TT.ToString(), _serviceIdOrWorkflowId, _merchantProfileId, true, TypeCardType.NotSet, "");
                       _message += ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                        RD.Add(RDN);
                    }
                }

                return RD;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    SetTxnEndpoint();//Change the endpoint to use the backup.

                    //TODO : Add a copy of the code above once fully tested out.

                    return null;

                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                }
                catch (Exception ex)
                {
                    _message += "Unable to AuthorizeAndCapture\r\nError Message : " + ex.Message;
                }
            }
            catch (System.TimeoutException te)
            {
                //A timeout has occured. Prompt the user if they'd like to query for the last transaction submitted
                if (_BCtransaction != null)
                {
                    _message += "A timeout has occured. A call to 'RequestTransaction' was made to obtain the transactions that should have been returned. Your code will need to reconcile transactions.";
                    RequestTransaction(_BCtransaction.TenderData);
                }
                else { throw te; }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTxnFault(ex, out strErrorId, out strErrorMessage))
                { _message +=strErrorId + " : " + strErrorMessage; }
                else { _message +=ex.Message; }
            }

            return null;
        }

        public List<ResponseDetails> ProcessBCPTransactionPro(
            TransactionType _TT //Required
            , BankcardTransactionPro _BCtransaction //Conditional : Only used for an AuthorizeAndCapture, Authorize and ReturnUnlinked. Otherwise null
            , BankcardCapturePro _BCDifferenceData //Conditional : Only used for a Capture. Otherwise null
            , List<String> _BatchIds //Conditional : A list of one or more batch Ids to capture.
            , BankcardReturn _RDifferenceData //Conditional : Only used for a ReturnById. Otherwise null
            , Adjust _ADifferenceData //Conditional : Only used for an Adjust. Otherwise null
            , BankcardUndo _UDifferenceData //Conditional : Only used for an Undo. Otherwise null
            , List<string> _TransactionIds //Conditional : Only used for a CaptureSelective. Otherwise null
            , List<Capture> _CaptureDifferenceData //Conditional : Only used for CaptureAll and CaptureSelective. Otherwise null
            , bool _SendAcknowledge
            , bool _UseWorkflowId
            , bool _ForceClose)
        {
            List<Response> _Response = new List<Response>();
            try
            {
                CheckTokenExpire();//Make sure the current token is valid

                string _serviceIdOrWorkflowId = _serviceId;
                if (_UseWorkflowId)
                    _serviceIdOrWorkflowId = _workflowId;

                if (_TT == TransactionType.AuthorizeAndCapture)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.AuthorizeAndCapture(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceIdOrWorkflowId));
                    //Always Verify that the requested amount and approved amount are the same. 
                    BankcardTransactionResponsePro BCR = new BankcardTransactionResponsePro();
                    BCR = (BankcardTransactionResponsePro)_Response[0];
                    if (_BCtransaction.TransactionData.Amount != BCR.Amount)
                        _message += "The transaction was approved for " + BCR.Amount
                            + " which is an amount not equal to the requested amount of " + _BCtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }

                if (_TT == TransactionType.Authorize)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Authorize(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceIdOrWorkflowId));
                    //Always Verify that the requested amount and approved amount are the same. 
                    BankcardTransactionResponsePro BCR = new BankcardTransactionResponsePro();
                    BCR = (BankcardTransactionResponsePro)_Response[0];
                    if (_BCtransaction.TransactionData.Amount != BCR.Amount)
                        _message +="The transaction was approved for " + BCR.Amount
                            + " which is an amount not equal to than the requested amount of " + _BCtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }
                if (_TT == TransactionType.Capture)
                {
                    if (CredentialRequired())
                        _BCDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Capture(_sessionToken, _BCDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.CaptureAll)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response = Cwsbc.CaptureAll(_sessionToken, _CaptureDifferenceData, _BatchIds, _applicationProfileId,
                                                 _merchantProfileId, _serviceId, _ForceClose);
                }
                if (_TT == TransactionType.CaptureAllAsync)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.CaptureAllAsync(_sessionToken, _CaptureDifferenceData, _BatchIds,
                                                        _applicationProfileId, _merchantProfileId, _serviceId, _ForceClose));
                }
                if (_TT == TransactionType.CaptureSelective)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response = Cwsbc.CaptureSelective(_sessionToken, _TransactionIds, _CaptureDifferenceData,
                                                       _applicationProfileId, _serviceId);
                }
                if (_TT == TransactionType.CaptureSelectiveAsync)
                {
                    if (CredentialRequired())
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.CaptureSelectiveAsync(_sessionToken, _TransactionIds, _CaptureDifferenceData,
                                                              _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.ReturnById)
                {
                    if (CredentialRequired())
                        _RDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.ReturnById(_sessionToken, _RDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.Return)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.ReturnUnlinked(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                }
                if (_TT == TransactionType.Adjust)
                {
                    if (CredentialRequired())
                        _ADifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Adjust(_sessionToken, _ADifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.Undo)
                {
                    if (CredentialRequired())
                        _UDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Undo(_sessionToken, _UDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.QueryAccount)
                {
                    if (CredentialRequired())
                        _BCtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.QueryAccount(_sessionToken, _BCtransaction, _applicationProfileId,
                                                     _merchantProfileId, _serviceId));
                }
                if (_TT == TransactionType.Verify)
                    _Response.Add(Cwsbc.Verify(_sessionToken, _BCtransaction, _applicationProfileId, _merchantProfileId, _serviceId));

                List<ResponseDetails> RD = new List<ResponseDetails>();//Convert the response to response details so that we can report on the UI
                if (_Response != null)
                {   
                    foreach (Response r in _Response)
                    {
                        if (_SendAcknowledge && r.TransactionId.Length > 0)
                            Cwsbc.Acknowledge(_sessionToken, r.TransactionId, _applicationProfileId, _serviceId);

                        ResponseDetails RDN = new ResponseDetails(0.00M, r, _TT.ToString(), _serviceIdOrWorkflowId, _merchantProfileId, true, TypeCardType.NotSet, "");
                        _message += ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                        RD.Add(RDN);
                    }
                }

                return RD;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    SetTxnEndpoint();//Change the endpoint to use the backup.

                    //TODO : Add a copy of the code above once fully tested out.

                    return null;

                }
                catch (EndpointNotFoundException)
                {
                    _message +="Neither the primary or secondary endpoints are available. Unable to process.";
                }
                catch (Exception ex)
                {
                    _message +="Unable to AuthorizeAndCapture\r\nError Message : " + ex.Message;
                }
            }
            catch (System.TimeoutException te)
            {
                //A timeout has occured. Prompt the user if they'd like to query for the last transaction submitted               
                if(_BCtransaction != null)
                {
                    _message += "A timeout has occured. A call to 'RequestTransaction' was made to obtain the transactions that should have been returned. Your code will need to reconcile transactions.";
                    RequestTransaction(_BCtransaction.TenderData);
                }
                else{throw te;}
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTxnFault(ex, out strErrorId, out strErrorMessage))
                { _message +=strErrorId + " : " + strErrorMessage; }
                else { _message +=ex.Message; }
            }

            return null;
        }


        #region Card Validation Helper Methods

        public TrackFromMSRSwipe seperateTrackData(string _TrackFromMSR)
        {
            TrackFromMSRSwipe TFMSRS = new TrackFromMSRSwipe("", "");
            string[] Tracks = new string[3];
            char[] splitter = { '?' };

            Tracks = _TrackFromMSR.Split(splitter, StringSplitOptions.RemoveEmptyEntries);

            for (int x = 0; x < Tracks.Length; x++)
            {
                if (Tracks[x].Substring(0, 1) == @"%" & TFMSRS.Track1Data.Length < 1)//The following removes track 1 start sentinal
                    TFMSRS.Track1Data = Tracks[x].Substring(1, Tracks[x].Length - 1);
                if (Tracks[x].Substring(0, 1) == @";" & TFMSRS.Track2Data.Length < 1)//Removes start sentenal for Track 2
                    TFMSRS.Track2Data = Tracks[x].Substring(1, Tracks[x].Length - 1);
            }
            return TFMSRS;
        }

        public bool validateTrackData(ref BankcardTransactionPro _BCtransaction, string _SwipedTrackData)
        {
            //NOTE: The following assumes that you have already seperated track 1 from track 2 and selected which one to use
            Regex regTrackData1 = new Regex(@"^([B|b][\d ]{12,19}\^[ \S-[%\?]]{1,29}\^\d{2}((1[0-2])|(0\d))[ \S-[%\?]]{1,60})$");
            Regex regTrackData2 = new Regex(@"^([\d]{12,19}=\d{2}((1[0-2])|(0\d))[ \S-[;\?]]{1,22})$");
            if (regTrackData2.IsMatch(_SwipedTrackData))
            {//Track 2 Match - You'll have to remove the starting and ending sentinal
                _BCtransaction.TenderData.CardData.Track2Data = _SwipedTrackData;
            }
            else if (regTrackData1.IsMatch(_SwipedTrackData))
            {//Track 1 Match
                _BCtransaction.TenderData.CardData.Track1Data = _SwipedTrackData;
            }
            else
            {
                return false;
            }
            return true;
        }

        public TypeCardType CardTypeLookup(string strPAN)
        {
            string strCardType = "";

            //
            //             * NOTE : "NotSet" TypeCardType is not a valid card type to transmit in the BankcardTransaction Object. If returned the 
            //             * validation checks did not find a match. The software should log a message that it was unable to 
            //             * match as the CardTypeLookup may need additional validation logic added to it. Never log the PAN as this would 
            //             * be a PCI violation.
            //            

            if (Convert.ToInt16(strPAN.Substring(0, 1)) == 4)
            {
                return TypeCardType.Visa;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 1)) == 5)
            {
                return TypeCardType.MasterCard;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) == 34 | Convert.ToInt16(strPAN.Substring(0, 2)) == 37)
            {
                return TypeCardType.AmericanExpress;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) == 36)
            {
                return TypeCardType.Discover;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 4)) == 3095)
            {
                return TypeCardType.Discover;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) == 67)
            {
                return TypeCardType.Maestro;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 4)) > 2999 & Convert.ToInt16(strPAN.Substring(0, 4)) < 3096)
            {
                return TypeCardType.Discover;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) > 37 & Convert.ToInt16(strPAN.Substring(0, 2)) < 40)
            {
                return TypeCardType.Discover;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 4)) == 6011)
            {
                if (Convert.ToInt16(strPAN.Substring(4, 2)) == 4)
                {
                    //Paypal to be added to schema
                    //Return TypeCardType.Paypal

                    return TypeCardType.Discover;
                }
                else
                {
                    return TypeCardType.Discover;
                }
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) > 63 & Convert.ToInt16(strPAN.Substring(0, 2)) < 66)
            {
                if (Convert.ToInt16(strPAN.Substring(3, 3)) > 599 & Convert.ToInt16(strPAN.Substring(3, 3)) < 611)
                {
                    //Paypal to be added to schema
                    //Return TypeCardType.Paypal

                    return TypeCardType.Discover;
                }
                else
                {
                    return TypeCardType.Discover;
                }
            }

            if (Convert.ToInt32(strPAN.Substring(0, 6)) == 352800 & Convert.ToInt32(strPAN.Substring(0, 6)) < 358999)
            {
                return TypeCardType.JCB;
            }

            if (Convert.ToInt16(strPAN.Substring(0, 2)) == 62)
            {
                return TypeCardType.Discover;
            }

            return TypeCardType.NotSet;
            //No match was found
        }

        public static bool ValidateCreditCardNumber(string creditCardNumber)
        {
            //check to see if it's masked by the protector
            string pattern = @"^(X+)[\d]{4}$";
            Regex exp = new Regex(pattern);
            Match match = exp.Match(creditCardNumber);
            if (match.Success)
                return true;

            try
            {
                // Array to contain individual numbers
                ArrayList CheckNumbers = new ArrayList();

                // Get length of card
                int CardLength = creditCardNumber.Length;

                // Double the value of alternate digits, starting with the second digit
                // from the right, i.e. back to front.

                // Loop through starting at the end
                for (int i = CardLength - 2; i >= 0; i = i - 2)
                {
                    // Now read the contents at each index, this
                    // can then be stored as an array of integers

                    // Double the number returned
                    CheckNumbers.Add(Int32.Parse(creditCardNumber[i].ToString()) * 2);
                }

                int CheckSum = 0; // Will hold the total sum of all checksum digits

                // Second stage, add separate digits of all products
                for (int iCount = 0; iCount <= CheckNumbers.Count - 1; iCount++)
                {
                    int _count = 0; // will hold the sum of the digits

                    // determine if current number has more than one digit
                    if ((int)CheckNumbers[iCount] > 9)
                    {
                        int _numLength = ((int)CheckNumbers[iCount]).ToString().Length;
                        // add count to each digit
                        for (int x = 0; x < _numLength; x++)
                        {
                            _count = _count + Int32.Parse(((int)CheckNumbers[iCount]).ToString()[x].ToString());
                        }
                    }
                    else
                    {
                        _count = (int)CheckNumbers[iCount]; // single digit, just add it by itself
                    }

                    CheckSum = CheckSum + _count; // add sum to the total sum
                }

                // Stage 3, add the unaffected digits
                // Add all the digits that we didn't double still starting from the right
                // but this time we'll start from the rightmost number with alternating digits
                int OriginalSum = 0;
                for (int y = CardLength - 1; y >= 0; y = y - 2)
                {
                    OriginalSum = OriginalSum + Int32.Parse(creditCardNumber[y].ToString());
                }

                // Perform the final calculation, if the sum Mod 10 results in 0 then
                // it's valid, otherwise its false.
                bool isValid = (((OriginalSum + CheckSum) % 10) == 0);

                //Now that the number checks out, make sure it has the correct number of digits
                //for the card type
                if (isValid)
                {
                    // AMEX -- 34 or 37 -- 15 length
                    if (Regex.IsMatch(creditCardNumber, "^(34|37)"))
                    {
                        return (15 == creditCardNumber.Length);
                    }

                    // MasterCard -- 51 through 55 -- 16 length
                    if (Regex.IsMatch(creditCardNumber, "^(51|52|53|54|55)"))
                    {
                        return (16 == creditCardNumber.Length);
                    }

                    // VISA -- 4 -- 13 and 16 length
                    if (Regex.IsMatch(creditCardNumber, "^(4)"))
                    {
                        return (13 == creditCardNumber.Length || 16 == creditCardNumber.Length);
                    }

                    // Diners Club -- 300-305, 36 or 38 -- 14 length
                    if (Regex.IsMatch(creditCardNumber, "^(300|301|302|303|304|305|36|38)"))
                    {
                        return (14 == creditCardNumber.Length);
                    }

                    // enRoute -- 2014,2149 -- 15 length
                    if (Regex.IsMatch(creditCardNumber, "^(2014|2149)"))
                    {
                        return (15 == creditCardNumber.Length);
                    }

                    // Discover -- 6011 -- 16 length
                    if (Regex.IsMatch(creditCardNumber, "^(6011)"))
                    {
                        return (16 == creditCardNumber.Length);
                    }

                    // JCB -- 3 -- 16 length
                    if (Regex.IsMatch(creditCardNumber, "^(3)"))
                    {
                        return (16 == creditCardNumber.Length);
                    }

                    // JCB -- 2131, 1800 -- 15 length
                    if (Regex.IsMatch(creditCardNumber, "^(2131|1800)"))
                    {
                        return (15 == creditCardNumber.Length);
                    }
                    //Card type wasn't recognised but could be a private label.
                    //Since it isn't breaking any rules of known cards, it's OK to return true.
                    return true;

                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion Card Validation Helper Methods

        #endregion BankCardTransaction Methods

        #region ElectronicCheckTransaction Methods

        public string TranslateServiceIdToFriendlyName(ElectronicCheckingService electronicCheckingService)
        {
            //Sandbox
            if (electronicCheckingService.ServiceId == "35A7700001") return "Affirmative ACH Now";

            //Production
            if (electronicCheckingService.ServiceId == "DF29D1300C") return "Affirmative ACH Now";

            return electronicCheckingService.ServiceName;//Match was not found so pass back the name in the service.
        }

        public List<ResponseDetails> ProcessECKTransaction(
                        TransactionType _TT //Required
                        , ElectronicCheckingTransaction _ECKtransaction
                        , List<String> _BatchIds //Conditional : A list of one or more batch Ids to capture.
                        , List<Capture> _CaptureDifferenceData
                        , Undo _UDifferenceData //Conditional : Only used for an Undo. Otherwise null
                        , bool _SendAcknowledge
                        , bool _ForceClose)
        {
            List<Response> _Response = new List<Response>();
            try
            {
                CheckTokenExpire();//Make sure the current token is valid

                //if (_TT == TransactionType.AuthorizeAndCapture)
                if (_TT == TransactionType.Authorize)
                {
                    if (CredentialRequired())
                        _ECKtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Authorize(_sessionToken, _ECKtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                    //Always Verify that the requested amount and approved amount are the same. 

                    ElectronicCheckingTransactionResponse ECKR = new ElectronicCheckingTransactionResponse();
                    ECKR = (ElectronicCheckingTransactionResponse)_Response[0];
                    if (_ECKtransaction.TransactionData.Amount != ECKR.Amount)
                        _message +="The transaction was approved for " + ECKR.Amount
                            + " which is an amount not equal to than the requested amount of " + _ECKtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }
                //if (_TT == TransactionType.Capture)
                if (_TT == TransactionType.CaptureAll)
                {
                    _CaptureDifferenceData = new List<Capture>();
                    Capture cap = new Capture();
                    if (CredentialRequired())
                        cap.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    cap.TransactionId = "-1";
                    _CaptureDifferenceData.Add(cap);
                    _Response = Cwsbc.CaptureAll(_sessionToken, _CaptureDifferenceData, _BatchIds, _applicationProfileId,
                                                 _merchantProfileId, _serviceId, _ForceClose);
                }
                if (_TT == TransactionType.CaptureAllAsync)
                {
                    _CaptureDifferenceData = new List<Capture>();
                    Capture cap = new Capture();
                    if (CredentialRequired())
                        cap.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    cap.TransactionId = "-1";
                    _CaptureDifferenceData.Add(cap);
                    _Response.Add(Cwsbc.CaptureAllAsync(_sessionToken, _CaptureDifferenceData, _BatchIds, _applicationProfileId, _merchantProfileId, _serviceId, _ForceClose));
                }

                //if (_TT == TransactionType.CaptureSelective)
                //if (_TT == TransactionType.CaptureSelectiveAsync)
                //if (_TT == TransactionType.ReturnById)
                //if (_TT == TransactionType.Return)
                //if (_TT == TransactionType.Adjust)
                if (_TT == TransactionType.Undo)
                {
                    if (CredentialRequired())
                        _UDifferenceData.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.Undo(_sessionToken, _UDifferenceData, _applicationProfileId, _serviceId));
                }
                if (_TT == TransactionType.QueryAccount)
                {
                    if (CredentialRequired())
                        _ECKtransaction.Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                    _Response.Add(Cwsbc.QueryAccount(_sessionToken, _ECKtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                }

                //if (_TT == TransactionType.Verify)

                List<ResponseDetails> RD = new List<ResponseDetails>();//Convert the response to response details so that we can report on the UI
                if (_Response != null)
                {
                    foreach (Response r in _Response)
                    {
                        if (_SendAcknowledge && r.TransactionId.Length > 0)
                            Cwsbc.Acknowledge(_sessionToken, r.TransactionId, _applicationProfileId, _serviceId);

                        ResponseDetails RDN = new ResponseDetails(0.00M, r, _TT.ToString(), _serviceId, _merchantProfileId, true, TypeCardType.NotSet, "");
                        _message +=ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                        RD.Add(RDN);
                    }
                }

                return RD;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    SetTxnEndpoint();//Change the endpoint to use the backup.

                    //TODO : Add a copy of the code above once fully tested out.

                    return null;

                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                }
                catch (Exception ex)
                {
                    _message += "Unable to AuthorizeAndCapture\r\nError Message : " + ex.Message;
                }
            }
            catch (System.TimeoutException te)
            {
                //A timeout has occured. Prompt the user if they'd like to query for the last transaction submitted
                if (_ECKtransaction != null)
                {
                    _message += "A timeout has occured. A call to 'RequestTransaction' was made to obtain the transactions that should have been returned. Your code will need to reconcile transactions.";
                    RequestTransaction(_ECKtransaction.TenderData);
                }
                else { throw te; }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTxnFault(ex, out strErrorId, out strErrorMessage))
                { _message += strErrorId + " : " + strErrorMessage; }
                else { _message += ex.Message; }
            }

            return null;
        }
        public List<ResponseDetails> ProcessECKCaptureAll(
            TransactionType _TT //Required
            , List<Capture> _CaptureDifferenceData //Conditional : Only used for CaptureAll and CaptureSelective. Otherwise null
            , bool _SendAcknowledge
            , bool _UseWorkflowId)  
        {
            List<Response> _Response = new List<Response>();
            try
            {
                CheckTokenExpire();//Make sure the current token is valid

                string _serviceIdOrWorkflowId = _serviceId;
                if (_UseWorkflowId)
                    _serviceIdOrWorkflowId = _workflowId;

                if (_TT == TransactionType.CaptureAll)
				{
                    if (CredentialRequired())
                    {
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                        _CaptureDifferenceData[0].TransactionId = "-1";
                    }
                    
                    _Response = Cwsbc.CaptureAll(_sessionToken, _CaptureDifferenceData, null, _applicationProfileId, _merchantProfileId, _serviceId, false);
				}
                if (_TT == TransactionType.CaptureAllAsync)
                {
                    if (CredentialRequired())
                    {
                        _CaptureDifferenceData[0].Addendum = CredentialsRequired(_serviceId, _credUserName, _credPassword);
                        _CaptureDifferenceData[0].TransactionId = "-1";
                    }
                    _Response.Add(Cwsbc.CaptureAllAsync(_sessionToken, _CaptureDifferenceData, null, _applicationProfileId, _merchantProfileId, _serviceId, false));
                }
                

                List<ResponseDetails> RD = new List<ResponseDetails>();//Convert the response to response details so that we can report on the UI
                if (_Response != null)
                {
                    foreach (Response r in _Response)
                    {
                        if (_SendAcknowledge && r.TransactionId.Length > 0)
                            Cwsbc.Acknowledge(_sessionToken, r.TransactionId, _applicationProfileId, _serviceId);

                        ResponseDetails RDN = new ResponseDetails(0.00M, r, _TT.ToString(), _serviceIdOrWorkflowId, _merchantProfileId, true, TypeCardType.NotSet, "");
                       _message += ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                        RD.Add(RDN);
                    }
                }

                return RD;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    SetTxnEndpoint();//Change the endpoint to use the backup.

                    //TODO : Add a copy of the code above once fully tested out.

                    return null;

                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                }
                catch (Exception ex)
                {
                    _message += "Unable to AuthorizeAndCapture\r\nError Message : " + ex.Message;
                }
            }
            catch (System.TimeoutException te)
            {
                throw te;
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTxnFault(ex, out strErrorId, out strErrorMessage))
                { _message +=strErrorId + " : " + strErrorMessage; }
                else { _message +=ex.Message; }
            }

            return null;
        }
        #endregion ElectronicCheckTransaction Methods

        #region StoredValueTransaction Methods

        public string TranslateServiceIdToFriendlyName(StoredValueService storedValueService)
        {
            //Sandbox
            if (storedValueService.ServiceId == "C58FD00001") return "Stored Value Service";

            //Production
            if (storedValueService.ServiceId == "XXXXX1300C") return "Stored Value Service";

            return storedValueService.ServiceName;//Match was not found so pass back the name in the service.
        }


        public List<ResponseDetails> ProcessSVATransaction(
                         TransactionType _TT //Required
            , StoredValueTransaction _SVAtransaction //Conditional : Only used for an AuthorizeAndCapture, Authorize and ReturnUnlinked. Otherwise null
            , StoredValueManage _SVManage // Conditional : Only used to manage. Otherwise null
            , StoredValueCapture _SVDifferenceData //Conditional : Only used for a Capture. Otherwise null
            , StoredValueReturn _SVRDifferenceData //Conditional : Only used for a ReturnById. Otherwise null
            , Undo _UDifferenceData //Conditional : Only used for an Undo. Otherwise null
            , bool _SendAcknowledge)
        {
            List<Response> _Response = new List<Response>();
            try
            {
                CheckTokenExpire();//Make sure the current token is valid

                //if (_TT == TransactionType.AuthorizeAndCapture)
                if (_TT == TransactionType.Authorize)
                {
                    _Response.Add(Cwsbc.Authorize(_sessionToken, _SVAtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                    //Always Verify that the requested amount and approved amount are the same. 
                    StoredValueTransactionResponse SVR = new StoredValueTransactionResponse();
                    SVR = (StoredValueTransactionResponse)_Response[0];
                    if (_SVAtransaction.TransactionData.Amount != SVR.Amount)
                        _message += "The transaction was approved for " + SVR.Amount
                            + " which is an amount not equal to than the requested amount of " + _SVAtransaction.TransactionData.Amount
                            + ". Please provide alternate payment to complete transaction";
                }
                if (_TT == TransactionType.ManageAccountById)
                    _Response.Add(Cwsbc.ManageAccountById(_sessionToken, _SVManage, _applicationProfileId, _serviceId));
                if (_TT == TransactionType.ManageAccount)
                    _Response.Add(Cwsbc.ManageAccount(_sessionToken, _SVAtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                if (_TT == TransactionType.Capture)
                    _Response.Add(Cwsbc.Capture(_sessionToken, _SVDifferenceData, _applicationProfileId, _serviceId));
                if (_TT == TransactionType.ReturnById)
                    _Response.Add(Cwsbc.ReturnById(_sessionToken, _SVRDifferenceData, _applicationProfileId, _serviceId));
                if (_TT == TransactionType.Return)
                    _Response.Add(Cwsbc.ReturnUnlinked(_sessionToken, _SVAtransaction, _applicationProfileId, _merchantProfileId, _serviceId));
                if (_TT == TransactionType.Undo)
                    _Response.Add(Cwsbc.Undo(_sessionToken, _UDifferenceData, _applicationProfileId, _serviceId));
                if (_TT == TransactionType.QueryAccount)
                    _Response.Add(Cwsbc.QueryAccount(_sessionToken, _SVAtransaction, _applicationProfileId, _merchantProfileId, _serviceId));

                List<ResponseDetails> RD = new List<ResponseDetails>();//Convert the response to response details so that we can report on the UI
                if (_Response != null)
                {
                    foreach (Response r in _Response)
                    {
                        if (_SendAcknowledge && r.TransactionId.Length > 0)
                            Cwsbc.Acknowledge(_sessionToken, r.TransactionId, _applicationProfileId, _serviceId);

                        ResponseDetails RDN = new ResponseDetails(0.00M, r, _TT.ToString(), _serviceId, _merchantProfileId, true, TypeCardType.NotSet, "");
                        _message += ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                        RD.Add(RDN);
                    }
                }

                return RD;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    SetTxnEndpoint();//Change the endpoint to use the backup.

                    //TODO : Add a copy of the code above once fully tested out.

                    return null;

                }
                catch (EndpointNotFoundException)
                {
                    _message += "Neither the primary or secondary endpoints are available. Unable to process.";
                }
                catch (Exception ex)
                {
                    _message += "Unable to AuthorizeAndCapture\r\nError Message : " + ex.Message;
                }
            }
            catch (System.TimeoutException te)
            {
                //A timeout has occured. Prompt the user if they'd like to query for the last transaction submitted               
                if (_SVAtransaction != null)
                {
                    _message += "A timeout has occured. A call to 'RequestTransaction' was made to obtain the transactions that should have been returned. Your code will need to reconcile transactions.";
                    RequestTransaction(_SVAtransaction.TenderData);
                }
                else { throw te; }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTxnFault(ex, out strErrorId, out strErrorMessage))
                { _message += strErrorId + " : " + strErrorMessage; }
                else { _message += ex.Message; }
            }

            return null;
        }

        #endregion StoredValueTransaction Methods

        #region Process Response
        public void RequestTransaction(SampleCode.CwsTransactionProcessing.TransactionTenderData _transactionTenderData)
        {//The RequestTransaction() operation allows for the retrieval of any transaction matching the supplied tender data. This is useful in situations 
         //when the application does not receive a response from CWS indicating the TransactionState.

            List<Response> _Response = new List<Response>();
            CheckTokenExpire();//Make sure the current token is valid
            _Response = Cwsbc.RequestTransaction(_sessionToken, _merchantProfileId, _transactionTenderData);

            if (_Response != null)
            {
                foreach (Response r in _Response)
                {
                    ResponseDetails RDN = new ResponseDetails(0.00M, r, "", "", _merchantProfileId, true, TypeCardType.NotSet, "");
                    _message += ProcessResponse(ref RDN);//Pass as reference so we can extract more values from the response
                }
            }
        }

        public string ProcessResponse(ref ResponseDetails _Response)
        {
            if (_Response.Response is BankcardTransactionResponsePro)
            {//In the 1.17.11 release all response objects are BankcardTransactionResponsePro
                return ProcessBankcardTransactionResponsePro(ref _Response);
            }
            if (_Response.Response is BankcardTransactionResponse)
            {//Future functionality as a BankcardTransactionResponse is presently not returned
            }
            if (_Response.Response is BankcardCaptureResponse)
            {//BankcardCaptureResponse
                return ProcessBankcardCaptureResponse(ref _Response);
            }
            if (_Response.Response is ElectronicCheckingTransactionResponse)
            {
                return ProcessElectronicCheckingTransactionResponse(ref _Response);
            }
            if (_Response.Response is ElectronicCheckingCaptureResponse)
            {
                return ProcessElectronicCheckingCaptureResponse(ref _Response);
            }
            if (_Response.Response is StoredValueTransactionResponse)
            {
                return ProcessStoredValueTransactionResponse(ref _Response);
            }
           
            return "";
        }

        private string ProcessBankcardTransactionResponsePro(ref ResponseDetails _Response)
        {
            BankcardTransactionResponsePro _BCResponse = new BankcardTransactionResponsePro();
            _BCResponse = (BankcardTransactionResponsePro)_Response.Response;
            string strMessage = "";

            //Set the amount reference
            _Response.TxnAmount = _BCResponse.Amount;

            //Note : IMPORTANT Always verify the approved amount was the same as the requested approval amount for "AuthorizeAndCapture" as well as "Authorize" 
            if (_Response.TransactionType == "AuthorizeAndCapture" | _Response.TransactionType == "Authorize")

                if (!_Response.Verbose)
                {// In this case don't present to the user all of the data. 
                    if (_BCResponse.Status == Status.Successful)//The transaction was approved
                    {
                        //NOTE : Please reference the developers guide for a more complete explination of the return fields
                        strMessage = "Your '" + _Response.TransactionType + "' transaction was APPROVED"
                            //Note Highly recommended to save
                        + "\r\nTransactionId : " + _BCResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                        + "\r\nWorkflowId : " + _Response.WorkflowId
                        + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                            //Note Optional but recommended to save
                        + "\r\nStatus Code : " + _BCResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                        + "\r\nStatus Message : " + _BCResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                        + "\r\nApprovalCode : " + _BCResponse.ApprovalCode //A value returned when a transaction is approved. This value should be printed on the receipt, and also recorded for every off-line transaction, such as a voice authorization. This same data element and value must be provided during settlement. Required.
                        + "\r\nAmount : " + _BCResponse.Amount //Specifies the authorization amount of the transaction. This is the actual amount authorized.
                        ;
                    }
                    if (_BCResponse.Status == Status.Failure)//The transaction was declined
                    {
                        //NOTE : Please reference the developers guide for a more complete explination of the return fields
                        strMessage = "Your '" + _Response.TransactionType + "' transaction was DECLINED"
                            //Note Highly recommended to save
                        + "\r\nTransactionId : " + _BCResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                        + "\r\nWorkflowId : " + _Response.WorkflowId
                        + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                            //Note Optional but recommended to save
                        + "\r\nStatus Code : " + _BCResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                        + "\r\nStatus Message : " + _BCResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                        ;
                    }
                    return strMessage;
                }
            if (_BCResponse.Status == Status.Successful)//The transaction was approved
            {
                //NOTE : Please reference the developers guide for a more complete explination of the return fields
                strMessage = "Your '" + _Response.TransactionType + "' transaction was APPROVED"
                    //Note Highly recommended to save
                + "\r\nTransactionId : " + _BCResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                + "\r\nWorkflowId : " + _Response.WorkflowId
                + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                    //Note Highly recommended to save if Tokenization will be used
                    //+ "\r\nPaymentAccountDataToken : " + _BCResponse.PaymentAccountDataToken //If tokenization purchased this field represents the actual token returned in the transaction for future use.
                    //Note Optional but recommended to save
                + "\r\nStatus Code : " + _BCResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                + "\r\nStatus Message : " + _BCResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                + "\r\nApprovalCode : " + _BCResponse.ApprovalCode //A value returned when a transaction is approved. This value should be printed on the receipt, and also recorded for every off-line transaction, such as a voice authorization. This same data element and value must be provided during settlement. Required.
                + "\r\nAmount : " + _BCResponse.Amount //Specifies the authorization amount of the transaction. This is the actual amount authorized.
                    //Note Optional but recommended if AVS is supported
                    //+ "\r\nAVSResult ActualResult : " + _BCResponse.AVSResult.ActualResult //Specifies the actual result of AVS from the Service Provider.
                    //+ "\r\nAVSResult AddressResult : " + _BCResponse.AVSResult.AddressResult //Specifies the result of AVS as it pertains to Address matching
                    //+ "\r\nAVSResult PostalCodeResult : " + _BCResponse.AVSResult.PostalCodeResult //Specifies the result of AVS as it pertains to Postal Code matching
                    //Note Optional but recommended if CV data is supported
                + "\r\nCVResult : " + _BCResponse.CVResult //Response code returned by the card issuer indicating the result of Card Verification (CVV2/CVC2/CID).
                    //Note Optional
                + "\r\nPrepaidCard : " + _BCResponse.PrepaidCard // Enumeration of NotSet, Yes, No.  Indicates if this is a pre-paid card
                + "\r\nBatchId : " + _BCResponse.BatchId //A unique ID used to identify a specific batch settlement                
                + "\r\nDowngradeCode : " + _BCResponse.DowngradeCode //Indicates downgrade reason.
                + "\r\nFeeAmount : " + _BCResponse.FeeAmount //Fee amount charged for the transaction. 
                + "\r\nFinalBalance : " + _BCResponse.FinalBalance //Fee amount charged for the transaction.
                + "\r\nResubmit : " + _BCResponse.Resubmit //Specifies whether resubmission is supported for PIN Debit transactions.
                + "\r\nServiceTransactionId : " + _BCResponse.ServiceTransactionId
                    //+ "\r\nSettlementDate : " + _BCResponse.SettlementDate //Settlement date. Conditional, if present in the authorization response, this same data element and value must be provided during settlement
                ;
            }
            if (_BCResponse.Status == Status.Failure)//The transaction was declined
            {
                //NOTE : Please reference the developers guide for a more complete explination of the return fields
                strMessage = "Your '" + _Response.TransactionType + "' transaction was DECLINED"
                    //Note Highly recommended to save
                + "\r\nTransactionId : " + _BCResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                + "\r\nWorkflowId : " + _Response.WorkflowId
                + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                    //Note Optional but recommended to save
                + "\r\nStatus Code : " + _BCResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                + "\r\nStatus Message : " + _BCResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                    //Note Optional but recommended if CV data is supported
                + "\r\nCVResult : " + _BCResponse.CVResult //Response code returned by the card issuer indicating the result of Card Verification (CVV2/CVC2/CID).
                    //Note Optional
                + "\r\nServiceTransactionId : " + _BCResponse.ServiceTransactionId
                ;
            }
            return strMessage;
        }

        private string ProcessBankcardCaptureResponse(ref ResponseDetails _Response)
        {
            BankcardCaptureResponse _BCResponse = new BankcardCaptureResponse();
            _BCResponse = (BankcardCaptureResponse)_Response.Response;

            string strResponseMessage = "";

            if (!_Response.Verbose)
            {// In this case don't present to the user all of the data. 
                if (_BCResponse.Status == Status.Successful)//The transaction was approved
                {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                    //Note Highly recommended to save
                    if (_BCResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _BCResponse.TransactionId;
                    strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                    //Note Optional but recommended to save
                    if (_BCResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _BCResponse.StatusCode;
                    if (_BCResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _BCResponse.StatusMessage;

                    strResponseMessage = "Your transaction was APPROVED\r\n" + strResponseMessage;
                }
                if (_BCResponse.Status == Status.Failure)//The transaction was declined
                {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                    //Note Highly recommended to save
                    if (_BCResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _BCResponse.TransactionId;
                    strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                    //Note Optional but recommended to save
                    if (_BCResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _BCResponse.StatusCode;
                    if (_BCResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _BCResponse.StatusMessage;

                    strResponseMessage = "Your transaction was DECLINED\r\n" + strResponseMessage;
                }
                return strResponseMessage;
            }
            if (_BCResponse.Status == Status.Successful)//The transaction was approved
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_BCResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _BCResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_BCResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _BCResponse.StatusCode;
                if (_BCResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _BCResponse.StatusMessage;
                //Note Optional data about the batch
                if (_BCResponse.BatchId != null) strResponseMessage = strResponseMessage + "\r\nBatch Id : " + _BCResponse.BatchId;
                if (_BCResponse.PrepaidCard != PrepaidCard.NotSet) strResponseMessage = strResponseMessage + "\r\nPrepaidCard : " + _BCResponse.PrepaidCard; // Enumeration of NotSet, Yes, No.  Indicates if this is a pre-paid card
                if (_BCResponse.TransactionSummaryData != null)
                {
                    if (_BCResponse.TransactionSummaryData.CashBackTotals != null) strResponseMessage = strResponseMessage + "\r\nCash Back Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.CashBackTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.CashBackTotals.NetAmount;
                    if (_BCResponse.TransactionSummaryData.NetTotals != null)
                    {
                        strResponseMessage = strResponseMessage + "\r\nNet Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.NetTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.NetTotals.NetAmount;
                        //Set the amount reference
                        _Response.TxnAmount = _BCResponse.TransactionSummaryData.NetTotals.NetAmount;
                    }
                    if (_BCResponse.TransactionSummaryData.PINDebitReturnTotals != null) strResponseMessage = strResponseMessage + "\r\nPINDebit Return Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.PINDebitReturnTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.PINDebitReturnTotals.NetAmount;
                    if (_BCResponse.TransactionSummaryData.PINDebitSaleTotals != null) strResponseMessage = strResponseMessage + "\r\nPINDebit Sale Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.PINDebitSaleTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.PINDebitSaleTotals.NetAmount;
                    if (_BCResponse.TransactionSummaryData.ReturnTotals != null) strResponseMessage = strResponseMessage + "\r\nReturn Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.ReturnTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.ReturnTotals.NetAmount;
                    if (_BCResponse.TransactionSummaryData.SaleTotals != null) strResponseMessage = strResponseMessage + "\r\nSale Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.SaleTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.SaleTotals.NetAmount;
                    if (_BCResponse.TransactionSummaryData.VoidTotals != null) strResponseMessage = strResponseMessage + "\r\nVoid Totals \r\n  Count : " + _BCResponse.TransactionSummaryData.VoidTotals.Count + "\r\n  Net Amount : " + _BCResponse.TransactionSummaryData.VoidTotals.NetAmount;
                }
                strResponseMessage = "Your transaction was APPROVED\r\n" + strResponseMessage;
            }
            if (_BCResponse.Status == Status.Failure)//The transaction was declined
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_BCResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _BCResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_BCResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _BCResponse.StatusCode;
                if (_BCResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _BCResponse.StatusMessage;
                //Note Optional

                strResponseMessage = "Your transaction was DECLINED\r\n" + strResponseMessage;
            }
            if (_BCResponse.Status == Status.NotSet)//The transaction was declined
            {
                strResponseMessage = "No Transactions found for settlement";
            }
            return strResponseMessage;
        }

        private string ProcessElectronicCheckingTransactionResponse(ref ResponseDetails _Response)
        {
            ElectronicCheckingTransactionResponse _ECKResponse = new ElectronicCheckingTransactionResponse();
            _ECKResponse = (ElectronicCheckingTransactionResponse)_Response.Response;

            //Set the amount reference
            _Response.TxnAmount = _ECKResponse.Amount;

            string strResponseMessage = "";

            if (_ECKResponse.Status == Status.Successful)//The transaction was approved
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_ECKResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _ECKResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_ECKResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _ECKResponse.StatusCode;
                if (_ECKResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _ECKResponse.StatusMessage;
                //Note Optional data about the batch
                if (_ECKResponse.ACHCapable != null) strResponseMessage = strResponseMessage + "\r\nACHCapable : " + _ECKResponse.ACHCapable;//Indicates if the customer's account will accept ACH transactions. Returned only on QueryAccount() transactions.
                if (_ECKResponse.Amount != null) strResponseMessage = strResponseMessage + "\r\nAmount : " + _ECKResponse.Amount;
                if (_ECKResponse.ApprovalCode != null) strResponseMessage = strResponseMessage + "\r\nApprovalCode : " + _ECKResponse.ApprovalCode; //A code provided when a transaction is approved.
                if (_ECKResponse.ModifiedAccountNumber != null) strResponseMessage = strResponseMessage + "\r\nModifiedAccountNumber : " + _ECKResponse.ModifiedAccountNumber; //Indicates the corrected account number that should be used for electronic check processing
                if (_ECKResponse.ModifiedRoutingNumber != null) strResponseMessage = strResponseMessage + "\r\nModifiedRoutingNumber : " + _ECKResponse.ModifiedRoutingNumber; //Indicates the corrected 9-digit bank routing number that should be used for electronic check processing.
                if (_ECKResponse.ReturnInformation != null)
                {
                    if (_ECKResponse.ReturnInformation.ReturnCode != null) strResponseMessage = strResponseMessage + "\r\nReturnCode : " + _ECKResponse.ReturnInformation.ReturnCode;//Code that indicates why the transaction was returned.
                    if (_ECKResponse.ReturnInformation.ReturnDate != null) strResponseMessage = strResponseMessage + "\r\nReturnDate : " + _ECKResponse.ReturnInformation.ReturnDate; //The date the transaction was returned by the bank. 
                    if (_ECKResponse.ReturnInformation.ReturnReason != null) strResponseMessage = strResponseMessage + "\r\nReturnReason : " + _ECKResponse.ReturnInformation.ReturnReason; //Reason that indicates why the transaction was returned. 
                }
                if (_ECKResponse.SubmitDate != null) strResponseMessage = strResponseMessage + "\r\nSubmitDate : " + _ECKResponse.SubmitDate; //The date the transaction was submitted to the bank.

                strResponseMessage = "Your transaction was APPROVED\r\n" + strResponseMessage;
            }
            if (_ECKResponse.Status == Status.Failure)//The transaction was declined
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_ECKResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _ECKResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_ECKResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _ECKResponse.StatusCode;
                if (_ECKResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _ECKResponse.StatusMessage;
                //Note Optional
                if (_ECKResponse.ACHCapable != null) strResponseMessage = strResponseMessage + "\r\nACHCapable : " + _ECKResponse.ACHCapable;//Indicates if the customer's account will accept ACH transactions. Returned only on QueryAccount() transactions.
                if (_ECKResponse.Amount != null) strResponseMessage = strResponseMessage + "\r\nAmount : " + _ECKResponse.Amount;
                if (_ECKResponse.ApprovalCode != null) strResponseMessage = strResponseMessage + "\r\nApprovalCode : " + _ECKResponse.ApprovalCode; //A code provided when a transaction is approved.
                if (_ECKResponse.ModifiedAccountNumber != null) strResponseMessage = strResponseMessage + "\r\nModifiedAccountNumber : " + _ECKResponse.ModifiedAccountNumber; //Indicates the corrected account number that should be used for electronic check processing
                if (_ECKResponse.ModifiedRoutingNumber != null) strResponseMessage = strResponseMessage + "\r\nModifiedRoutingNumber : " + _ECKResponse.ModifiedRoutingNumber; //Indicates the corrected 9-digit bank routing number that should be used for electronic check processing.
                if (_ECKResponse.ReturnInformation != null)
                {
                    if (_ECKResponse.ReturnInformation.ReturnCode != null) strResponseMessage = strResponseMessage + "\r\nReturnCode : " + _ECKResponse.ReturnInformation.ReturnCode;//Code that indicates why the transaction was returned.
                    if (_ECKResponse.ReturnInformation.ReturnDate != null) strResponseMessage = strResponseMessage + "\r\nReturnDate : " + _ECKResponse.ReturnInformation.ReturnDate; //The date the transaction was returned by the bank. 
                    if (_ECKResponse.ReturnInformation.ReturnReason != null) strResponseMessage = strResponseMessage + "\r\nReturnReason : " + _ECKResponse.ReturnInformation.ReturnReason; //Reason that indicates why the transaction was returned. 
                }
                if (_ECKResponse.SubmitDate != null) strResponseMessage = strResponseMessage + "\r\nSubmitDate : " + _ECKResponse.SubmitDate; //The date the transaction was submitted to the bank.

                strResponseMessage = "Your transaction was DECLINED\r\n" + strResponseMessage;
            }
            if (_ECKResponse.Status == Status.NotSet)//The transaction was declined
            {
                strResponseMessage = "No Transactions found for settlement";
            }
            return strResponseMessage;
        }

        private string ProcessElectronicCheckingCaptureResponse(ref ResponseDetails _Response)
        {
            ElectronicCheckingCaptureResponse _ECKResponse = new ElectronicCheckingCaptureResponse();
            _ECKResponse = (ElectronicCheckingCaptureResponse)_Response.Response;

            //Set the amount reference
            _Response.TxnAmount = _ECKResponse.SummaryData.NetTotals.NetAmount;

            string strResponseMessage = "";

            if (_ECKResponse.Status == Status.Successful)//The transaction was approved
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_ECKResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _ECKResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_ECKResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _ECKResponse.StatusCode;
                if (_ECKResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _ECKResponse.StatusMessage;
                //Note Optional data about the batch
                if (_ECKResponse.SummaryData != null)
                {
                    if (_ECKResponse.SummaryData.CreditTotals != null) strResponseMessage = strResponseMessage + "\r\nCredit Totals \r\n  Count : " + _ECKResponse.SummaryData.CreditTotals.Count + "\r\n  Net Amount : " + _ECKResponse.SummaryData.CreditTotals.NetAmount;
                    if (_ECKResponse.SummaryData.DebitTotals != null) strResponseMessage = strResponseMessage + "\r\nDebit Totals \r\n  Count : " + _ECKResponse.SummaryData.DebitTotals.Count + "\r\n  Net Amount : " + _ECKResponse.SummaryData.DebitTotals.NetAmount;
                    if (_ECKResponse.SummaryData.NetTotals != null) strResponseMessage = strResponseMessage + "\r\nNet Totals \r\n  Count : " + _ECKResponse.SummaryData.NetTotals.Count + "\r\n  Net Amount : " + _ECKResponse.SummaryData.NetTotals.NetAmount;
                    if (_ECKResponse.SummaryData.VoidTotals != null) strResponseMessage = strResponseMessage + "\r\nVoid Totals \r\n  Count : " + _ECKResponse.SummaryData.VoidTotals.Count + "\r\n  Net Amount : " + _ECKResponse.SummaryData.VoidTotals.NetAmount;
                }
                strResponseMessage = "Your transaction was APPROVED\r\n" + strResponseMessage;
            }
            if (_ECKResponse.Status == Status.Failure)//The transaction was declined
            {//NOTE : Please reference the developers guide for a more complete explination of the return fields
                //Note Highly recommended to save
                if (_ECKResponse.TransactionId != null) strResponseMessage = strResponseMessage + "\r\nTransactionId : " + _ECKResponse.TransactionId;
                strResponseMessage = strResponseMessage + "\r\nMerchant Profile Id : " + _Response.MerchantProfileId;
                //Note Optional but recommended to save
                if (_ECKResponse.StatusCode != null) strResponseMessage = strResponseMessage + "\r\nStatus Code : " + _ECKResponse.StatusCode;
                if (_ECKResponse.StatusMessage != null) strResponseMessage = strResponseMessage + "\r\nStatus Message : " + _ECKResponse.StatusMessage;
                //Note Optional

                strResponseMessage = "Your transaction was DECLINED\r\n" + strResponseMessage;
            }
            if (_ECKResponse.Status == Status.NotSet)//The transaction was declined
            {
                strResponseMessage = "No Transactions found for settlement";
            }
            return strResponseMessage;
        }

        private string ProcessStoredValueTransactionResponse(ref ResponseDetails _Response)
        {
            StoredValueTransactionResponse _SVAResponse = new StoredValueTransactionResponse();
            _SVAResponse = (StoredValueTransactionResponse)_Response.Response;

            //Set the amount reference
            _Response.TxnAmount = _SVAResponse.Amount;

            string strMessage = "";

            //Note : IMPORTANT Always verify the approved amount was the same as the requested approval amount for "AuthorizeAndCapture" as well as "Authorize" 
            if (_Response.TransactionType == "AuthorizeAndCapture" | _Response.TransactionType == "Authorize")

                if (!_Response.Verbose)
                {// In this case don't present to the user all of the data. 
                    if (_SVAResponse.Status == Status.Successful)//The transaction was approved
                    {
                        //NOTE : Please reference the developers guide for a more complete explination of the return fields
                        strMessage = "Your '" + _Response.TransactionType + "' transaction was APPROVED"
                            //Note Highly recommended to save
                        + "\r\nTransactionId : " + _SVAResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                        + "\r\nWorkflowId : " + _Response.WorkflowId
                        + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                            //Note Optional but recommended to save
                        + "\r\nStatus Code : " + _SVAResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                        + "\r\nStatus Message : " + _SVAResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                        + "\r\nApprovalCode : " + _SVAResponse.ApprovalCode //A value returned when a transaction is approved. This value should be printed on the receipt, and also recorded for every off-line transaction, such as a voice authorization. This same data element and value must be provided during settlement. Required.
                        + "\r\nAmount : " + _SVAResponse.Amount //Specifies the authorization amount of the transaction. This is the actual amount authorized.
                        ;
                    }
                    if (_SVAResponse.Status == Status.Failure)//The transaction was declined
                    {
                        //NOTE : Please reference the developers guide for a more complete explination of the return fields
                        strMessage = "Your '" + _Response.TransactionType + "' transaction was DECLINED"
                            //Note Highly recommended to save
                        + "\r\nTransactionId : " + _SVAResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                        + "\r\nWorkflowId : " + _Response.WorkflowId
                        + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                            //Note Optional but recommended to save
                        + "\r\nStatus Code : " + _SVAResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                        + "\r\nStatus Message : " + _SVAResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                        ;
                    }
                    return strMessage;
                }
            if (_SVAResponse.Status == Status.Successful)//The transaction was approved
            {
                //NOTE : Please reference the developers guide for a more complete explination of the return fields
                strMessage = "Your '" + _Response.TransactionType + "' transaction was APPROVED"
                    //Note Highly recommended to save
                + "\r\nTransactionId : " + _SVAResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                + "\r\nWorkflowId : " + _Response.WorkflowId
                + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                    //Note Highly recommended to save if Tokenization will be used
                    //+ "\r\nPaymentAccountDataToken : " + _BCResponse.PaymentAccountDataToken //If tokenization purchased this field represents the actual token returned in the transaction for future use.
                    //Note Optional but recommended to save
                + "\r\nStatus Code : " + _SVAResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                + "\r\nStatus Message : " + _SVAResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                + "\r\nApprovalCode : " + _SVAResponse.ApprovalCode //A value returned when a transaction is approved. This value should be printed on the receipt, and also recorded for every off-line transaction, such as a voice authorization. This same data element and value must be provided during settlement. Required.
                + "\r\nAmount : " + _SVAResponse.Amount //Specifies the authorization amount of the transaction. This is the actual amount authorized.
                    //Note Optional but recommended if AVS is supported
                    //+ "\r\nAVSResult ActualResult : " + _BCResponse.AVSResult.ActualResult //Specifies the actual result of AVS from the Service Provider.
                    //+ "\r\nAVSResult AddressResult : " + _BCResponse.AVSResult.AddressResult //Specifies the result of AVS as it pertains to Address matching
                    //+ "\r\nAVSResult PostalCodeResult : " + _BCResponse.AVSResult.PostalCodeResult //Specifies the result of AVS as it pertains to Postal Code matching
                    //Note Optional but recommended if CV data is supported
                + "\r\nCVResult : " + _SVAResponse.CVResult //Response code returned by the card issuer indicating the result of Card Verification (CVV2/CVC2/CID).
                    //Note Optional              
                + "\r\nServiceTransactionId : " + _SVAResponse.ServiceTransactionId
                    //+ "\r\nSettlementDate : " + _BCResponse.SettlementDate //Settlement date. Conditional, if present in the authorization response, this same data element and value must be provided during settlement
                ;
            }
            if (_SVAResponse.Status == Status.Failure)//The transaction was declined
            {
                //NOTE : Please reference the developers guide for a more complete explination of the return fields
                strMessage = "Your '" + _Response.TransactionType + "' transaction was DECLINED"
                    //Note Highly recommended to save
                + "\r\nTransactionId : " + _SVAResponse.TransactionId //The unique id of the transaction. TransactionId is required for all subsequent transactions such as Return, Undo, etc.
                + "\r\nWorkflowId : " + _Response.WorkflowId
                + "\r\nMerchantProfileId : " + _Response.MerchantProfileId //Must be stored with the TransactionId in order to identify which merchant sent which transaction. Required to support multi-merchant.
                    //Note Optional but recommended to save
                + "\r\nStatus Code : " + _SVAResponse.StatusCode //Status code generated by the Service Provider. This code should be displayed to the user as verification of the transaction.
                + "\r\nStatus Message : " + _SVAResponse.StatusMessage //Explains the StatusCode which is generated by the Service Provider. This message should be displayed to the user as verification of the transaction.
                    //Note Optional but recommended if CV data is supported
                + "\r\nCVResult : " + _SVAResponse.CVResult //Response code returned by the card issuer indicating the result of Card Verification (CVV2/CVC2/CID).
                    //Note Optional
                + "\r\nServiceTransactionId : " + _SVAResponse.ServiceTransactionId
                ;
            }
            return strMessage;
        }

        #endregion Process Response

        #region Extras

        public Image ImageFromBase64String(string base64)
        {
            MemoryStream memory = new MemoryStream(Convert.FromBase64String(base64));
            Image result = Image.FromStream(memory);
            memory.Close();
            return result;
        }

        public string RetrieveServiceKeyFromIdentityToken(string identityToken)
        {
            try
            {
                String clearToken = Encoding.UTF8.GetString(Convert.FromBase64String(identityToken));

                //Now try and retrieve the Service Key from the XML
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(clearToken);
                XPathNavigator xnav = doc.CreateNavigator();

                XmlNamespaceManager manager = new XmlNamespaceManager(xnav.NameTable);
                manager.AddNamespace("SK", "urn:oasis:names:tc:SAML:1.0:assertion");

                XPathNavigator node = xnav.SelectSingleNode("//SK:Attribute[@AttributeName='SAK']", manager);
                return node.Value;
            }
            catch (Exception ex)
            {
                _message += ex.Message;
                return "";
            }
        }

        #endregion Extras

        #region Class Properties
        public string ApplicationProfileId
        {
            get { return _applicationProfileId; }
            set { _applicationProfileId = value.Trim(); }
        }
        public string ServiceID
        {
            get { return _serviceId; }
            set { _serviceId = value; }
        }
        public string WorkflowID
        {
            get { return _workflowId; }
            set { _workflowId = value; }
        }
        public string MerchantProfileId
        {
            get { return _merchantProfileId; }
            set { _merchantProfileId = value; }
        }
        public string SessionToken
        {
            get { return _sessionToken; }
            set { _sessionToken = value; }
        }
        public string DelegatedSessionToken
        {
            get { return _delegatedSessionToken; }
            set { _delegatedSessionToken = value; }
        }
        public string DelegatedServiceKey
        {
            get { return _delegatedServiceKey; }
            set { _delegatedServiceKey = value; }
        }
        public string IdentityToken
        {
            get { return _identityToken; }
            set { _identityToken = value.Trim(); }
        }
        public string ServiceKey
        {
            get { return _serviceKey; }
            set { _serviceKey = value; }
        }
        public string CredUserName
        {
            get { return _credUserName; }
            set { _credUserName = value; }
        }
        public ServiceInformation ServiceInformation
        {
            get { return _serviceInformation; }
            set { _serviceInformation = value; }
        }
        public string CredPassword
        {
            get { return _credPassword; }
            set { _credPassword = value; }
        }
        public CWSServiceInformationClient Cwssic
        {
            get { return _cwssic; }
            set { _cwssic = value; }
        }
        public CwsTransactionProcessingClient Cwsbc
        {
            get { return _cwsbc; }
            set { _cwsbc = value; }
        }
        public TMSOperationsClient Tmsoc
        {
            get { return _tmsoc; }
            set { _tmsoc = value; }
        }
        #endregion END Class Properties

    }

#region Extra Classes
    public class TrackFromMSRSwipe
    {
        public string Track1Data;
        public string Track2Data;

        public TrackFromMSRSwipe(string track1Data, string track2Data)
        {
            Track1Data = track1Data;
            Track2Data = track2Data;
        }
    }

    public class ResponseDetails
    {
        public decimal TxnAmount;
        public Response Response;
        public string TransactionType;
        public string WorkflowId;
        public string MerchantProfileId;
        public bool Verbose;
        public TypeCardType CardType;
        public string MaskedPan;

        public ResponseDetails(decimal txnAmount, Response response, string transactionType,
            string workflowId, string merchantProfileId, bool verbose, TypeCardType cardType, string maskedPan)
        {
            TxnAmount = txnAmount;
            Response = response;
            TransactionType = transactionType;
            WorkflowId = workflowId;
            MerchantProfileId = merchantProfileId;
            Verbose = verbose;
            CardType = TypeCardType.NotSet;
            MaskedPan = "";
        }
        public override string ToString()
        {// Generates the text shown in the List Checkbox
            return
                "<Orig Type:" + TransactionType + ">" 
                + TxnAmount + " " + Response.TransactionId
                 + " [Status : " + Response.Status + "]"
                //+ " [Status Code : " + Response.StatusCode + "]"
                //+ " [Status Message : " + Response.StatusMessage + "]"
                 + " [Capture State : " + Response.CaptureState + "]"
                 + " [Transaction State : " + Response.TransactionState + "] "
                 + DateTime.Now
                ;
        }
    }

    public enum TransactionType : int
    {
        [System.Runtime.Serialization.EnumMemberAttribute()]
        AuthorizeAndCapture = 0,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Authorize = 1,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Capture = 2,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CaptureAll = 3,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CaptureSelective = 4,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ReturnById = 5,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Return = 6,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Adjust = 7,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Undo = 8,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QueryAccount = 9,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Verify = 10,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CaptureAllAsync = 11,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        CaptureSelectiveAsync = 12,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ManageAccount = 13,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        ManageAccountById = 14,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        RequestTransaction = 15,
    }

    [Serializable]
    public class PersistAndCacheSettings
    {
        public string ApplicationProfileId;
        public string ServiceId;
        public string WorkflowId;
        public string MerchantProfileId;
        public bool EncryptedIdentityToken;
        public string IdentityToken;
        public PersistAndCacheSettings(string applicationProfileId, string serviceId, string workflowId, string merchantProfileId, bool encryptedIdentityToken, string identityToken)
        {
            ApplicationProfileId = applicationProfileId;
            ServiceId = serviceId;
            WorkflowId = workflowId;
            MerchantProfileId = merchantProfileId;
            EncryptedIdentityToken = encryptedIdentityToken;
            IdentityToken = identityToken;
        }
    }

#endregion Extra Classes
}

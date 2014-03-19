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
using System.ServiceModel;

namespace FaultHandler
{
    public class FaultHandler
    {
        public bool handleSvcInfoFault(Exception _ex, out string _strErrorID, out string _strErrorMessage)
        {//Note : that the boolean indicates if the fault was handled by this class

            _strErrorID = "";
            _strErrorMessage = "";

            #region AuthorizationFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion AuthorizationFault

            //#region ClaimNotFoundFault
            //try
            //{
            //    if (((FaultException<SampleCode.CWSServiceInformation.ClaimNotFoundFault>)(_ex)).Detail != null)
            //    {
            //        _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.ClaimNotFoundFault>)(_ex)).Detail.ErrorID.ToString();
            //        _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.ClaimNotFoundFault>)(_ex)).Detail.ProblemType;
            //        return true;//Positive Match
            //    }
            //}
            //catch { }
            //#endregion ClaimNotFoundFault

            #region CWSValidationResultFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.CWSValidationResultFault>)(_ex)).Detail != null)
                {
                    foreach (SampleCode.CWSServiceInformation.CWSValidationErrorFault error in ((FaultException<SampleCode.CWSServiceInformation.CWSValidationResultFault>)(_ex)).Detail.Errors)
                    {
                        _strErrorMessage = _strErrorMessage + error.RuleKey + " : " + error.RuleMessage + "\r\n\r\n";
                    }
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSValidationResultFault

            #region AuthenticationFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.AuthenticationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion AuthenticationFault

            #region STSUnavailableFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.STSUnavailableFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.STSUnavailableFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.STSUnavailableFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion STSUnavailableFault

            #region ExpiredTokenFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.ExpiredTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.ExpiredTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.ExpiredTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion AuthenticationFault

            #region InvalidTokenFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.InvalidTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.InvalidTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.InvalidTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion InvalidTokenFault

            #region CWSServiceInformationUnavailableFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.CWSServiceInformationUnavailableFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.CWSServiceInformationUnavailableFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.CWSServiceInformationUnavailableFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSServiceInformationUnavailableFault

//Additional Faults called out in the data contract

            #region CWSValidationErrorFault
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.CWSValidationErrorFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSServiceInformation.CWSValidationErrorFault>)(_ex)).Detail.RuleKey;
                    _strErrorMessage = ((FaultException<SampleCode.CWSServiceInformation.CWSValidationErrorFault>)(_ex)).Detail.RuleMessage;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSValidationErrorFault

            #region Presently Unsupported Faults
            try
            {
                if (((FaultException<SampleCode.CWSServiceInformation.BadAttemptThresholdExceededFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "BadAttemptThresholdExceededFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.GeneratePasswordFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "GeneratePasswordFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.InvalidEmailFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "InvalidEmailFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.LockedByAdminFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "LockedByAdminFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.OneTimePasswordFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "OneTimePasswordFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.PasswordExpiredFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "PasswordExpiredFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.PasswordInvalidFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "PasswordInvalidFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.SendEmailFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "SendEmailFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.STSUnavailableFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "STSUnavailableFault thrown however not handled by code"; 
                if (((FaultException<SampleCode.CWSServiceInformation.UserNotFoundFault>)(_ex)).Detail != null)  
                    _strErrorMessage = "UserNotFoundFault thrown however not handled by code"; 
            }
            catch { }
            #endregion Presently Unsupported Faults

            //As one last check look at the GeneralFaults
            if (GeneralFaults(_ex, out _strErrorID, out _strErrorMessage))
                return true;//Positive Match

            _strErrorMessage = "An unhandled fault was thrown \r\nMessage : " + _ex.Message;
            return false;//In this case unable to match fault so return false. 
        }

        public bool handleTxnFault(Exception _ex, out string _strErrorID, out string _strErrorMessage)
        {//Note : that the boolean indicates if the fault was handled by this class
            _strErrorID = "";
            _strErrorMessage = "";

            #region CWSConnectionFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSConnectionFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSConnectionFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSConnectionFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSConnectionFault

            #region CWSExtendedDataNotSupportedFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSExtendedDataNotSupportedFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSExtendedDataNotSupportedFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSExtendedDataNotSupportedFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSExtendedDataNotSupportedFault

            #region CWSInvalidOperationFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidOperationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidOperationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidOperationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSInvalidOperationFault

            #region CWSOperationNotSupportedFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSOperationNotSupportedFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSOperationNotSupportedFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSOperationNotSupportedFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSOperationNotSupportedFault

            #region CWSValidationResultFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSValidationResultFault>)(_ex)).Detail != null)
                {
                    foreach (SampleCode.CwsTransactionProcessing.CWSValidationErrorFault error in ((FaultException<SampleCode.CwsTransactionProcessing.CWSValidationResultFault>)(_ex)).Detail.Errors)
                    {
                        _strErrorMessage = _strErrorMessage + error.RuleKey + " : " + error.RuleMessage + "\r\n";
                    }
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSValidationResultFault

            #region AuthenticationFault

            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.AuthenticationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.AuthenticationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.AuthenticationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion AuthenticationFault

            #region ExpiredTokenFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.ExpiredTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.ExpiredTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.ExpiredTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion ExpiredTokenFault

            #region InvalidTokenFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.InvalidTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.InvalidTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.InvalidTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion InvalidTokenFault

            #region CWSTransactionServiceUnavailableFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionServiceUnavailableFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionServiceUnavailableFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionServiceUnavailableFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSTransactionServiceUnavailableFault

            #region CWSInvalidMessageFormatFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidMessageFormatFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidMessageFormatFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidMessageFormatFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSInvalidMessageFormatFault

            #region CWSInvalidServiceInformationFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidServiceInformationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidServiceInformationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSInvalidServiceInformationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSInvalidServiceInformationFault

            #region CWSTransactionAlreadySettledFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionAlreadySettledFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionAlreadySettledFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionAlreadySettledFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSTransactionAlreadySettledFault

            #region CWSTransactionFailedFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionFailedFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionFailedFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSTransactionFailedFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSTransactionFailedFault

//Additional Faults called out in the data contract
            #region CWSValidationErrorFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSValidationErrorFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSValidationErrorFault>)(_ex)).Detail.RuleKey;
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSValidationErrorFault>)(_ex)).Detail.RuleMessage;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion CWSValidationErrorFault

            #region CWSDeserializationFault
            try
            {
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSDeserializationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSDeserializationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSDeserializationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }

            #endregion CWSDeserializationFault

            //As one last check look at the GeneralFaults
            if (GeneralFaults(_ex, out _strErrorID, out _strErrorMessage))
                return true;//Positive Match

            _strErrorMessage = "An unhandled fault was thrown \r\nMessage : " + _ex.Message;
            return false;//In this case unable to match fault so return false. 
        }

        public bool handleTMSFault(Exception _ex, out string _strErrorID, out string _strErrorMessage)
        {//Note : that the boolean indicates if the fault was handled by this class
            _strErrorID = "";
            _strErrorMessage = "";

            #region AuthenticationFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.AuthenticationFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.AuthenticationFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.AuthenticationFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion AuthenticationFault

            #region TMSOperationNotSupportedFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.TMSOperationNotSupportedFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.TMSOperationNotSupportedFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.TMSOperationNotSupportedFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion TMSOperationNotSupportedFault

            #region TMSTransactionFailedFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.TMSTransactionFailedFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.TMSTransactionFailedFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.TMSTransactionFailedFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion TMSTransactionFailedFault

            #region TMSUnknownServiceKeyFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.TMSUnknownServiceKeyFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.TMSUnknownServiceKeyFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.TMSUnknownServiceKeyFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion TMSUnknownServiceKeyFault

            #region ExpiredTokenFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.ExpiredTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.ExpiredTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.ExpiredTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion ExpiredTokenFault

            #region InvalidTokenFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.InvalidTokenFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.InvalidTokenFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.InvalidTokenFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion InvalidTokenFault

            #region TMSUnavailableFault
            try
            {
                if (((FaultException<SampleCode.CWSTransactionManagement.TMSUnavailableFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CWSTransactionManagement.TMSUnavailableFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CWSTransactionManagement.TMSUnavailableFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }
            catch { }
            #endregion TMSUnavailableFault


            //As one last check look at the GeneralFaults
            if (GeneralFaults(_ex, out _strErrorID, out _strErrorMessage))
                return true;//Positive Match

            _strErrorMessage = "An unhandled fault was thrown \r\nMessage : " + _ex.Message;
            return false;//In this case unable to match fault so return false. 
        }

        public bool GeneralFaults(Exception _ex, out string _strErrorID, out string _strErrorMessage)
        {//Note : that the boolean indicates if the fault was handled by this class
            
            _strErrorID = "";
            _strErrorMessage = "";

            try{
                if (((FaultException<SampleCode.CwsTransactionProcessing.CWSFault>)(_ex)).Detail != null)
                {
                    _strErrorID = ((FaultException<SampleCode.CwsTransactionProcessing.CWSFault>)(_ex)).Detail.ErrorID.ToString();
                    _strErrorMessage = ((FaultException<SampleCode.CwsTransactionProcessing.CWSFault>)(_ex)).Detail.ProblemType;
                    return true;//Positive Match
                }
            }catch{}

            //No General faults found
            return false;
        }
    }
}

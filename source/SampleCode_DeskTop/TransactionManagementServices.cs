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
using System.Drawing;
using System.Net;
using System.ServiceModel;
using System.Windows.Forms;
using SampleCode.CWSTransactionManagement;

namespace SampleCode
{
    public partial class TransactionManagementServices : Form
    {
        private FaultHandler.FaultHandler _FaultHandler = new FaultHandler.FaultHandler();
        private int _intCurrentPage = 0; //Zero based so 0 is page 1
        private int _intResultsPerPage = 50; //The upperlimit is 50.

        private int _intCurrentPageResults = 0; //Zero based so 0 is page 1
        private int _intResultsPerPageResults = 50; //The upperlimit is 50.

        private LastSearchType _lastSearch;

        public TransactionManagementServices()
        {
            InitializeComponent();

            cboQTP_CaptureStates.Sorted = true;
            cboQTP_CaptureStates.DataSource = Enum.GetValues(typeof(CaptureState));
            cboQTP_CaptureStates.SelectedIndex = -1;

            cboQTP_CardTypes.Sorted = true;
            cboQTP_CardTypes.DataSource = Enum.GetValues(typeof(TypeCardType));
            cboQTP_CardTypes.SelectedIndex = -1;

            //Populate drop downs with enumerated values
            cboQTP_IsAcknowledged.Items.Add(true);
            cboQTP_IsAcknowledged.Items.Add(false);

            cboQTP_QueryType.Sorted = true;
            cboQTP_QueryType.DataSource = Enum.GetValues(typeof(QueryType));
            cboQTP_QueryType.SelectedIndex = -1;

            cboQTP_TransactionStates.Sorted = true;
            cboQTP_TransactionStates.DataSource = Enum.GetValues(typeof(TransactionState));
            cboQTP_TransactionStates.SelectedIndex = -1;

            //Format the dateTimePicker for TMS queries
            dtpStartTimeTMS.Format = DateTimePickerFormat.Custom;
            dtpStartTimeTMS.CustomFormat = "dddd MM'/'dd'/'yyyy hh':'mm tt";
            dtpStartTimeTMS.Value = DateTime.Now.AddHours(-2);

            dtpEndTimeTMS.Format = DateTimePickerFormat.Custom;
            dtpEndTimeTMS.CustomFormat = "dddd MM'/'dd'/'yyyy hh':'mm tt";
            dtpEndTimeTMS.Value = DateTime.Now;

            CboTransactionDetailFormat.Sorted = true;
            CboTransactionDetailFormat.DataSource = Enum.GetValues(typeof(TransactionDetailFormat));
            
        }

        private void TransactionManagementServices_Load(object sender, EventArgs e)
        {
            //Load Service and Merchant values from ServiceInformation
            if (((SampleCode_DeskTop)(Owner)).cboAvailableServices.Items.Count > 0)
            {
                foreach (var item in ((SampleCode_DeskTop)(Owner)).cboAvailableServices.Items)
                {
                    CboQTP_ServiceIds.Items.Add(((SampleCode.Item)(item)).Value1);
                }
                
            }
            if (((SampleCode_DeskTop)(Owner)).cboAvailableProfiles.Items.Count > 0)
            {
                foreach (var item in ((SampleCode_DeskTop)(Owner)).cboAvailableProfiles.Items)
                {
                    CboQTP_MerchantProfileIds.Items.Add(item);
                    CboQBP_MercProfileIds.Items.Add(item);
                }
            }
        }

        private void cmdQueryBatch_Click(object sender, EventArgs e)
        {
            ResetPreviousNext();
            if (_lastSearch != LastSearchType.QueryBatch) { ResetPreviousNext(); _lastSearch = LastSearchType.QueryBatch; }
            QueryBatch();
        }

        private void cmdQueryTransactionsSummary_Click(object sender, EventArgs e)
        {
            ResetPreviousNext();
            if (_lastSearch != LastSearchType.QueryTransactionsSummary) { ResetPreviousNext(); _lastSearch = LastSearchType.QueryTransactionsSummary; }
            QueryTransactionsSummary();
        }

        private void cmdQueryTransactionFamilies_Click(object sender, EventArgs e)
        {
            if (txtQTP_TransactionIds.Text.Length < 1 && !chkUseTransactionIdSelected.Checked) { MessageBox.Show("At Lease one TransactionId is necessary in Query Transaction Parameters"); Cursor = Cursors.Default; return; }
            ResetPreviousNext();
            if (_lastSearch != LastSearchType.QueryTransactionFamilies) { ResetPreviousNext(); _lastSearch = LastSearchType.QueryTransactionFamilies; }
            QueryTransactionFamilies();
        }

        private void cmdQueryTransactionsDetail_Click(object sender, EventArgs e)
        {
            if (txtQTP_TransactionIds.Text.Length < 1 && !chkUseTransactionIdSelected.Checked) { MessageBox.Show("At Lease one TransactionId is necessary in Query Transaction Parameters"); Cursor = Cursors.Default; return; }
            ResetPreviousNext();
            if (_lastSearch != LastSearchType.QueryTransactionsDetail) { ResetPreviousNext(); _lastSearch = LastSearchType.QueryTransactionsDetail; }
            QueryTransactionsDetail();
        }

        private void QueryBatch()
        {
            //http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryBatch.aspx

            try
            {
                ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();//Make sure the current token is valid
                RtxtTMSResults.Clear();

                Cursor = Cursors.WaitCursor;
                ProcessQueryBatchResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryBatch(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QBP(), PP()));

                if (RtxtTMSResults.TextLength < 1)
                {
                    RtxtTMSResults.SelectionColor = Color.Blue;
                    RtxtTMSResults.AppendText("No Query Batch Results : " + DateTime.Now);
                }
                Cursor = Cursors.Default;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetTMSEndpoint();//Change the endpoint to use the backup.

                    ProcessQueryBatchResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryBatch(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QBP(), PP()));
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(
                        "Neither the primary or secondary TMS endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to QueryBatch\r\nError Message : " + ex.Message, "QueryBatch Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (WebException eW)
            {
                MessageBox.Show(eW.Message);
                //e.Status;
                //((HttpWebResponse) e.Response).StatusCode;
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTMSFault(ex, out strErrorId, out strErrorMessage))
                    MessageBox.Show(strErrorId + " : " + strErrorMessage);
                else
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }
 
        private void QueryTransactionsSummary()
        {//http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionsSummary.aspx

            bool BlnIncludeRelated = chkIncludeRelated.Checked;
            
            try
            {
                ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();//Make sure the current token is valid
                RtxtTMSResults.Clear();

                Cursor = Cursors.WaitCursor;
                ProcessQueryTransactionSummaryResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionsSummary(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), BlnIncludeRelated, PP())); ;

                RtxtTMSResults.SelectionColor = Color.Blue;
                RtxtTMSResults.AppendText("Last Transaction Summary Search : " + DateTime.Now);

                Cursor = Cursors.Default;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetTMSEndpoint();//Change the endpoint to use the backup.

                    ProcessQueryTransactionSummaryResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionsSummary(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), BlnIncludeRelated, PP())); ;
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(
                        "Neither the primary or secondary TMS endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to Query Transactions Summary\r\nError Message : " + ex.Message, "Query Transactions Summary Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTMSFault(ex, out strErrorId, out strErrorMessage))
                    MessageBox.Show(strErrorId + " : " + strErrorMessage);
                else
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void QueryTransactionFamilies()
        {
            //http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionFamilies.aspx

            try
            {
                ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();//Make sure the current token is valid
                RtxtTMSResults.Clear();

                Cursor = Cursors.WaitCursor;
                ProcessQueryTransactionFamiliesResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionFamilies(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), PP()));

                Cursor = Cursors.Default;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetTMSEndpoint();//Change the endpoint to use the backup.

                    ProcessQueryTransactionFamiliesResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionFamilies(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), PP()));
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(
                        "Neither the primary or secondary TMS endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to Query Transaction Families\r\nError Message : " + ex.Message, "Query Transaction Families Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTMSFault(ex, out strErrorId, out strErrorMessage))
                    MessageBox.Show(strErrorId + " : " + strErrorMessage);
                else
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void QueryTransactionsDetail()
        {
            //http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionsDetail.aspx

            bool BlnIncludeRelated = chkIncludeRelated.Checked;
            try
            {
                ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();//Make sure the current token is valid
                RtxtTMSResults.Clear();

                Cursor = Cursors.WaitCursor;

                TransactionDetailFormat TDF = (TransactionDetailFormat)CboTransactionDetailFormat.SelectedItem;
                
                ProcessQueryTransactionsDetailResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionsDetail(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), TDF, BlnIncludeRelated, PP())); ;

                if (RtxtTMSResults.Text.Length < 1)
                {
                    RtxtTMSResults.SelectionColor = Color.Blue;
                    RtxtTMSResults.AppendText("No Query Transaction Detail Results : " + DateTime.Now);
                }
                Cursor = Cursors.Default;
            }
            catch (EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetTMSEndpoint();//Change the endpoint to use the backup.

                    TransactionDetailFormat TDF = (TransactionDetailFormat)CboTransactionDetailFormat.SelectedItem;

                    ProcessQueryTransactionsDetailResponse(((SampleCode_DeskTop)(Owner)).Helper.Tmsoc.QueryTransactionsDetail(((SampleCode_DeskTop)(Owner)).Helper.SessionToken, QTP(), TDF, BlnIncludeRelated, PP()));
                }
                catch (EndpointNotFoundException)
                {
                    MessageBox.Show(
                        "Neither the primary or secondary TMS endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to Query Transaction Families\r\nError Message : " + ex.Message, "Query Transaction Families Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                string strErrorId;
                string strErrorMessage;
                if (_FaultHandler.handleTMSFault(ex, out strErrorId, out strErrorMessage))
                    MessageBox.Show(strErrorId + " : " + strErrorMessage);
                else
                    MessageBox.Show(ex.Message);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void ResetPreviousNext()
        {
            LnkPrevious.Visible = false;
            LnkNext.Visible = false;
            lblPageNumber.Visible = false;
            _intCurrentPage = 0; //Zero based so 0 is page 1
        }

        private void LnkNext_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _intCurrentPage++;
            if(_lastSearch == LastSearchType.QueryTransactionsSummary)QueryTransactionsSummary();
            //Since data is saved to the results textbox the following do not apply
            //if (_lst == LastSearchType.QueryTransactionFamilies) QueryTransactionFamilies();
            //if (_lst == LastSearchType.QueryTransactionsDetail) QueryTransactionsDetail();
            //if (_lst == LastSearchType.QueryBatch) QueryBatch();
        }

        private void LnkPrevious_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _intCurrentPage--;
            if (_lastSearch == LastSearchType.QueryTransactionsSummary) QueryTransactionsSummary();
            //Since data is saved to the results textbox the following do not apply
            //if (_lst == LastSearchType.QueryTransactionFamilies) QueryTransactionFamilies();
            //if (_lst == LastSearchType.QueryTransactionsDetail) QueryTransactionsDetail();
            //if (_lst == LastSearchType.QueryBatch) QueryBatch();
        }

        private QueryBatchParameters QBP()
        {
            QueryBatchParameters QBP = new QueryBatchParameters();
            char[] splitter = { ',' };
            ////Specify batch paramaters
            QBP.BatchDateRange = new DateRange();
            QBP.BatchDateRange.StartDateTime = dtpStartTimeTMS.Value.ToUniversalTime();
            QBP.BatchDateRange.EndDateTime = dtpEndTimeTMS.Value.ToUniversalTime();

            if (txtQBP_BatchIds.Text.Length > 0)
                QBP.BatchIds = new List<string>(txtQBP_BatchIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (CboQBP_MercProfileIds.Text.Length > 0)
                QBP.MerchantProfileIds = new List<string>(CboQBP_MercProfileIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (txtQBP_ServiceKeys.Text.Length > 0)
                QBP.ServiceKeys = new List<string>(txtQBP_ServiceKeys.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

            List<string> txnIds = new List<string>();
            if (txtQBP_TransactionIds.Text.Length > 0)
            {
                //First Process any transactionId's in the text box
                txnIds = new List<string>(txtQBP_TransactionIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            }
            //Now check to see if any are listed in the Check List.
            if (chkUseTransactionIdSelected.Checked && chklstTMSResults.CheckedItems.Count > 0)
            {
                //Iterate through the checked list box and add transactionId(s)
                foreach (object itemChecked in chklstTMSResults.CheckedItems)
                {
                    if (itemChecked.GetType().ToString() == "SampleCode.SummaryDetailVal")
                    {
                        txtQBP_TransactionIds.Text = txtQBP_TransactionIds.Text + ((SummaryDetailVal)(itemChecked)).SD.TransactionInformation.TransactionId + ", ";
                        txnIds.Add(((SummaryDetailVal)(itemChecked)).SD.TransactionInformation.TransactionId);
                    }

                    if (itemChecked.GetType().ToString() == "SampleCode.TransactionDetailVal")
                    {
                        txtQBP_TransactionIds.Text = txtQBP_TransactionIds.Text + ((TransactionDetailVal)(itemChecked)).TD.TransactionInformation.TransactionId + ", ";
                        txnIds.Add(((TransactionDetailVal)(itemChecked)).TD.TransactionInformation.TransactionId);
                    }                 
                }
            }
            //Note : Only set transactionId's if one or more actually exist. Otherwise the result set will be empty
            if(txnIds.Count >0)
                QBP.TransactionIds = txnIds;

            return QBP;
        }

        private QueryTransactionsParameters QTP()
        {
            QueryTransactionsParameters QTP = new QueryTransactionsParameters();
            char[] splitter = { ',' };

            if (txtQTP_Amounts.Text.Length > 0)
            {
                List<decimal> Amt = new List<decimal>();
                string[] values = txtQTP_Amounts.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
                foreach (string s in values)
                {
                    try
                    {
                        decimal d = Convert.ToDecimal(s);
                        d = decimal.Parse(d.ToString("0.00"));//Amounts must be in format N.NN
                        Amt.Add(d);
                    }
                    catch { }
                }
                QTP.Amounts = Amt;
            }

            if (txtQTP_ApprovalCodes.Text.Length > 0)
                QTP.ApprovalCodes = new List<string>(txtQTP_ApprovalCodes.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (txtQTP_BatchIds.Text.Length > 0)
                QTP.BatchIds = new List<string>(txtQTP_BatchIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            if (txtQTP_OrderNumber.Text.Length > 0)
                QTP.OrderNumbers = new List<string>(txtQTP_OrderNumber.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

            //ToDo : add logic
            //QTP.CaptureDateRange = new DateRange();
            //QTP.CaptureDateRange.StartDateTime = dtpStartTimeTMS.Value.ToUniversalTime();
            //QTP.CaptureDateRange.EndDateTime = dtpEndTimeTMS.Value.ToUniversalTime();

            if (cboQTP_CaptureStates.Text.Length > 0)
            {
                List<CaptureState> TS = new List<CaptureState>();
                TS.Add((CaptureState)cboQTP_CaptureStates.SelectedItem);
                QTP.CaptureStates = TS;
            }

            if (cboQTP_CardTypes.Text.Length > 0)
            {
                List<TypeCardType> TS = new List<TypeCardType>();
                TS.Add((TypeCardType)cboQTP_CardTypes.SelectedItem);
                QTP.CardTypes = TS;
            }

            if (cboQTP_IsAcknowledged.Text.Length > 0)
            {
                if ((bool)cboQTP_IsAcknowledged.SelectedItem)
                    QTP.IsAcknowledged = BooleanParameter.True;
                else
                    QTP.IsAcknowledged = BooleanParameter.False;
            }

            if (CboQTP_MerchantProfileIds.Text.Length > 0)
                QTP.MerchantProfileIds = new List<string>(CboQTP_MerchantProfileIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

            if (cboQTP_QueryType.Text.Length > 0)
                QTP.QueryType = (QueryType)cboQTP_QueryType.SelectedItem;

            if (CboQTP_ServiceIds.Text.Length > 0)
                QTP.ServiceIds = new List<string>(CboQTP_ServiceIds.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

            if (txtQTP_ServiceKeys.Text.Length > 0)
                QTP.ServiceKeys = new List<string>(txtQTP_ServiceKeys.Text.Split(splitter, StringSplitOptions.RemoveEmptyEntries));

            //ToDo : add logic
            if (txtQTP_TransactionClassTypePairs.Text.Length > 0)
            {
                List<TransactionClassTypePair> TCTP = new List<TransactionClassTypePair>();
                TCTP.Add(new TransactionClassTypePair());
                QTP.TransactionClassTypePairs = TCTP;
            }

            ////Specify batch paramaters
            QTP.TransactionDateRange = new DateRange();
            QTP.TransactionDateRange.StartDateTime = dtpStartTimeTMS.Value.ToUniversalTime();
            QTP.TransactionDateRange.EndDateTime = dtpEndTimeTMS.Value.ToUniversalTime();


            List<string> txnIds = new List<string>();
            if (txtQTP_TransactionIds.Text.Length > 0)
            {
                //First Process any transactionId's in the text box
                txnIds = new List<string>(txtQTP_TransactionIds.Text.Replace(" ", "").Split(splitter, StringSplitOptions.RemoveEmptyEntries));
            }
            //Now check to see if any are listed in the Check List.
            if(chkUseTransactionIdSelected.Checked && chklstTMSResults.CheckedItems.Count > 0)
            {
                //Iterate through the checked list box and add transactionId(s)
                foreach (object itemChecked in chklstTMSResults.CheckedItems)
                {
                    if (itemChecked.GetType().ToString() == "SampleCode.SummaryDetailVal")
                    {
                        txtQTP_TransactionIds.Text = txtQTP_TransactionIds.Text + ((SummaryDetailVal)(itemChecked)).SD.TransactionInformation.TransactionId + ", ";
                        txnIds.Add(((SummaryDetailVal)(itemChecked)).SD.TransactionInformation.TransactionId);
                    }
                    
                    if (itemChecked.GetType().ToString() == "SampleCode.TransactionDetailVal")
                    {
                        txtQTP_TransactionIds.Text = txtQTP_TransactionIds.Text + ((TransactionDetailVal)(itemChecked)).TD.TransactionInformation.TransactionId + ", ";
                        txnIds.Add(((TransactionDetailVal)(itemChecked)).TD.TransactionInformation.TransactionId);
                    }

                    //if (itemChecked.GetType().ToString() == "SampleCode.FamilyDetailVal")
                    //    txnIds.Add(((FamilyDetailVal)(itemChecked)).FD.TransactionIds);
                    
                    //if (itemChecked.GetType().ToString() == "SampleCode.BatchDetailDataVal")
                    //    txnIds.Add(((BatchDetailDataVal)(itemChecked)).BDD.TransactionIds);                   
                }
            }
            //Note : Only set transactionId's if one or more actually exist. Otherwise the result set will be empty
            if (txnIds.Count > 0)
                QTP.TransactionIds = txnIds;
            
            if (cboQTP_TransactionStates.Text.Length > 0)
            {
                List<TransactionState> TS = new List<TransactionState>();
                TS.Add((TransactionState)cboQTP_TransactionStates.SelectedItem);
                QTP.TransactionStates = TS;
            }

            return QTP;
        }

        private PagingParameters PP()
        {
            //Specify Paging Parameters
            PagingParameters PP = new PagingParameters();

            if (_lastSearch == LastSearchType.QueryTransactionsSummary)
            {
                PP.Page = _intCurrentPage;
                PP.PageSize = _intResultsPerPage;
            }

            if (_lastSearch == LastSearchType.QueryTransactionFamilies 
                | _lastSearch == LastSearchType.QueryTransactionsDetail 
                | _lastSearch == LastSearchType.QueryBatch) 
            {
                PP.Page = _intCurrentPageResults;
                PP.PageSize = _intResultsPerPageResults;
            }

            return PP;
        }

        private void chklstTMSResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            RtxtTMSResults.Clear();

            //Process SummaryDetail response
            if (_lastSearch == LastSearchType.QueryTransactionsSummary)
            {
                if (chklstTMSResults.SelectedItem == null) return;
                SummaryDetail s = ((SummaryDetailVal)(chklstTMSResults.SelectedItem)).SD;
                SummaryDetailString(s);
            }
            //Process FamilyDetail response
            if (_lastSearch == LastSearchType.QueryTransactionFamilies)
            {
                if (chklstTMSResults.SelectedItem == null) return;
                FamilyDetail f = ((FamilyDetailVal)(chklstTMSResults.SelectedItem)).FD;
                FamilyDetailString(f);
            }
            //Process BatchDetailData response
            if (_lastSearch == LastSearchType.QueryBatch)
            {
                if (chklstTMSResults.SelectedItem == null) return;
                BatchDetailData b = ((BatchDetailDataVal)(chklstTMSResults.SelectedItem)).BDD;
                BatchDetailDataString(b);
            }
            //Process TransactionDetail response
            if (_lastSearch == LastSearchType.QueryTransactionsDetail)
            {
                if (chklstTMSResults.SelectedItem == null) return;
                TransactionDetail t = ((TransactionDetailVal)(chklstTMSResults.SelectedItem)).TD;
                TransactionDetailString(t);
            }
        }

        #region process TMS response

        private void ProcessQueryBatchResponse(List<BatchDetailData> _BDD)
        {
            chklstTMSResults.Items.Clear();

            //Check for the need of paging
            if (_BDD.Count > _intResultsPerPage - 1)
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                lblPageNumber.Visible = true;
                LnkNext.Visible = true;
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }
            else
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                LnkNext.Visible = false;//End of the list so disable the Next link
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }
            
            if (_BDD.Count > 0)
            {
                foreach (BatchDetailData b in _BDD)
                {
                    BatchDetailData BDD = new BatchDetailDataVal(b);
                    chklstTMSResults.Items.Add(BDD);
                }
            }
        }

        private void ProcessQueryTransactionSummaryResponse(List<SummaryDetail> _SD)
        {
            chklstTMSResults.Items.Clear();

            //Check for the need of paging
            if (_SD.Count > _intResultsPerPage-1)
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage+1);
                lblPageNumber.Visible = true;
                LnkNext.Visible = true;
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }
            else
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                LnkNext.Visible = false;//End of the list so disable the Next link
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }

            if (_SD.Count > 0)
            {
                foreach (SummaryDetail s in _SD)
                {
                    SummaryDetailVal SDV = new SummaryDetailVal(s);
                    chklstTMSResults.Items.Add(SDV);
                }
            }
        }

        private void ProcessQueryTransactionFamiliesResponse(List<FamilyDetail> _FD)
        {
            chklstTMSResults.Items.Clear();

            //Check for the need of paging
            if (_FD.Count > _intResultsPerPage - 1)
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                lblPageNumber.Visible = true;
                LnkNext.Visible = true;
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }
            else
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                LnkNext.Visible = false;//End of the list so disable the Next link
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }

            if (_FD.Count > 0)
            {
                foreach (FamilyDetail f in _FD)
                {
                    FamilyDetailVal SDV = new FamilyDetailVal(f);
                    chklstTMSResults.Items.Add(SDV);
                }
            }
        }

        private void ProcessQueryTransactionsDetailResponse(List<TransactionDetail> _TD)
        {
            chklstTMSResults.Items.Clear();

            //Check for the need of paging
            if (_TD.Count > _intResultsPerPage - 1)
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                lblPageNumber.Visible = true;
                LnkNext.Visible = true;
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }
            else
            {
                lblPageNumber.Text = @"Page : " + (_intCurrentPage + 1);
                LnkNext.Visible = false;//End of the list so disable the Next link
                LnkPrevious.Visible = (_intCurrentPage > 0 ? true : false);//Enable previous on page 2
            }

            if (_TD.Count > 0)
            {
                foreach (TransactionDetail t in _TD)
                {
                    TransactionDetailVal TDV = new TransactionDetailVal(t);
                    chklstTMSResults.Items.Add(TDV);
                }
            }
        }

        private void BatchDetailDataString(BatchDetailData b)
        {
            RtxtTMSResults.SelectionColor = Color.Black;
            //Batch Summary
            RtxtTMSResults.AppendText("BatchCaptureDate : " + b.BatchCaptureDate + " (UTC)\r\n");
            RtxtTMSResults.AppendText("BatchId : " + b.BatchId + "\r\n");
            RtxtTMSResults.AppendText("Capture State : " + b.CaptureState + "\r\n");
            RtxtTMSResults.AppendText("Description : " + b.Description + "\r\n");
            //Batch Summary Data
            if (b.SummaryData != null)
            {
                RtxtTMSResults.AppendText("Batch Summary Data");
                if (b.SummaryData.CashBackTotals != null) RtxtTMSResults.AppendText("\r\nCash Back Totals \r\n  Count : " + b.SummaryData.CashBackTotals.Count + "\r\n  Net Amount : " + b.SummaryData.CashBackTotals.NetAmount);
                if (b.SummaryData.CreditReturnTotals != null) RtxtTMSResults.AppendText("\r\nCredit Return Totals \r\n  Count : " + b.SummaryData.CreditReturnTotals.Count + "\r\n  Net Amount : " + b.SummaryData.CreditReturnTotals.NetAmount);
                if (b.SummaryData.CreditTotals != null) RtxtTMSResults.AppendText("\r\nCredit Totals \r\n  Count : " + b.SummaryData.CreditTotals.Count + "\r\n  Net Amount : " + b.SummaryData.CreditTotals.NetAmount);
                if (b.SummaryData.DebitReturnTotals != null) RtxtTMSResults.AppendText("\r\nDebit Return Totals \r\n  Count : " + b.SummaryData.DebitReturnTotals.Count + "\r\n  Net Amount : " + b.SummaryData.DebitReturnTotals.NetAmount);
                if (b.SummaryData.DebitTotals != null) RtxtTMSResults.AppendText("\r\nDebit Totals \r\n  Count : " + b.SummaryData.DebitTotals.Count + "\r\n  Net Amount : " + b.SummaryData.DebitTotals.NetAmount);
                if (b.SummaryData.NetTotals != null) RtxtTMSResults.AppendText("\r\nNet Totals \r\n  Count : " + b.SummaryData.NetTotals.Count + "\r\n  Net Amount : " + b.SummaryData.NetTotals.NetAmount);
                if (b.SummaryData.VoidTotals != null) RtxtTMSResults.AppendText("\r\nVoid Totals \r\n  Count : " + b.SummaryData.VoidTotals.Count + "\r\n  Net Amount : " + b.SummaryData.VoidTotals.NetAmount);
            }
            //TransactionId
            RtxtTMSResults.AppendText("\r\nList of TransactionIds\r\n");
            if (b.TransactionIds != null)
            {
                foreach (string txnid in b.TransactionIds)
                {
                    RtxtTMSResults.AppendText(txnid + "\r\n");
                }
            }
            RtxtTMSResults.AppendText("\r\n");
        }

        private void SummaryDetailString(SummaryDetail s)
        {
            RtxtTMSResults.SelectionColor = Color.Black;

            ColorText("TransactionState : " + s.TransactionInformation.TransactionState + "\r\n", s.TransactionInformation.TransactionState);
            ColorText("TransactionStatusCode : " + s.TransactionInformation.TransactionStatusCode + "\r\n", s.TransactionInformation.TransactionState);

            //FamilyInformation
            RtxtTMSResults.AppendText("Family Information\r\n");
            RtxtTMSResults.AppendText("FamilyId : " + s.FamilyInformation.FamilyId + "\r\n");
            RtxtTMSResults.AppendText("FamilySequenceCount : " + s.FamilyInformation.FamilySequenceCount + "\r\n");
            RtxtTMSResults.AppendText("FamilySequenceNumber : " + s.FamilyInformation.FamilySequenceNumber + "\r\n");
            RtxtTMSResults.AppendText("FamilyState : " + s.FamilyInformation.FamilyState + "\r\n");
            RtxtTMSResults.AppendText("NetAmount : " + s.FamilyInformation.NetAmount + "\r\n");
            //TransactionInformation
            RtxtTMSResults.AppendText("Transaction Information\r\n");
            RtxtTMSResults.AppendText("Amount : " + s.TransactionInformation.Amount + "\r\n");
            RtxtTMSResults.AppendText("ApprovalCode : " + s.TransactionInformation.ApprovalCode + "\r\n");
            
             //TransactionInformation.BankcardData
            if (s.TransactionInformation.BankcardData != null)
            {
                if (s.TransactionInformation.BankcardData.AVSResult != null)
                {
                    //TransactionInformation.BankcardData.AVSResult
                    RtxtTMSResults.AppendText("AVSResult Summary\r\n");
                    RtxtTMSResults.AppendText(" - ActualResult : " + s.TransactionInformation.BankcardData.AVSResult.ActualResult + "\r\n");
                    RtxtTMSResults.AppendText(" - AddressResult : " + s.TransactionInformation.BankcardData.AVSResult.AddressResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CardholderNameResult : " + s.TransactionInformation.BankcardData.AVSResult.CardholderNameResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CityResult : " + s.TransactionInformation.BankcardData.AVSResult.CityResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CountryResult : " + s.TransactionInformation.BankcardData.AVSResult.CountryResult + "\r\n");
                    RtxtTMSResults.AppendText(" - PhoneResult : " + s.TransactionInformation.BankcardData.AVSResult.PhoneResult + "\r\n");
                    RtxtTMSResults.AppendText(" - PostalCodeResult : " + s.TransactionInformation.BankcardData.AVSResult.PostalCodeResult + "\r\n");
                    RtxtTMSResults.AppendText(" - StateResult : " + s.TransactionInformation.BankcardData.AVSResult.StateResult + "\r\n");
                }
                //TransactionInformation.BankcardData
                RtxtTMSResults.AppendText("CardType : " + s.TransactionInformation.BankcardData.CardType + "\r\n");//The card type used on the transaction. Expected.
                RtxtTMSResults.AppendText("CVResult : " + s.TransactionInformation.BankcardData.CVResult + "\r\n");//Response code returned by the card issuer indicating the result of Card Verification (CVV2/CVC2/CID) returned by the service provider. Optional.
                RtxtTMSResults.AppendText("MaskedPAN : " + s.TransactionInformation.BankcardData.MaskedPAN + "\r\n");//The cardholder's PAN in masked format. Expected.
                RtxtTMSResults.AppendText("OrderId : " + s.TransactionInformation.BankcardData.OrderId + "\r\n"); //The order id generated by CWS. This value is often used by service providers for transaction correlation. Expected.
            }
            //TransactionInformation
            RtxtTMSResults.AppendText("BatchId : " + s.TransactionInformation.BatchId + "\r\n");
            RtxtTMSResults.AppendText("CapturedAmount : " + s.TransactionInformation.CapturedAmount + "\r\n");
            RtxtTMSResults.AppendText("CaptureDateTime : " + s.TransactionInformation.CaptureDateTime + "\r\n");
            RtxtTMSResults.AppendText("CaptureState : " + s.TransactionInformation.CaptureState + "\r\n");
            RtxtTMSResults.AppendText("CaptureStatusMessage : " + s.TransactionInformation.CaptureStatusMessage + "\r\n");
            RtxtTMSResults.AppendText("CustomerId : " + s.TransactionInformation.CustomerId + "\r\n");
            //TransactionInformation.ElectronicCheckData
            if (s.TransactionInformation.ElectronicCheckData != null)
            {
                RtxtTMSResults.AppendText("Electronic Check Data (if applicable)\r\n");
                RtxtTMSResults.AppendText("- CheckNumber : " + s.TransactionInformation.ElectronicCheckData.CheckNumber + "\r\n");
                RtxtTMSResults.AppendText("- MaskedAccountNumber : " + s.TransactionInformation.ElectronicCheckData.MaskedAccountNumber + "\r\n");
                RtxtTMSResults.AppendText("- TransactionType : " + s.TransactionInformation.ElectronicCheckData.TransactionType + "\r\n");
            }
            RtxtTMSResults.AppendText("IsAcknowledged : " + s.TransactionInformation.IsAcknowledged + "\r\n");
            RtxtTMSResults.AppendText("MaskedPAN : " + s.TransactionInformation.MaskedPAN + "\r\n");
            RtxtTMSResults.AppendText("MerchantProfileId : " + s.TransactionInformation.MerchantProfileId + "\r\n");
            RtxtTMSResults.AppendText("OriginatorTransactionId : " + s.TransactionInformation.OriginatorTransactionId + "\r\n");
            RtxtTMSResults.AppendText("Reference : " + s.TransactionInformation.Reference + "\r\n");
            RtxtTMSResults.AppendText("ServiceId : " + s.TransactionInformation.ServiceId + "\r\n");
            RtxtTMSResults.AppendText("ServiceKey : " + s.TransactionInformation.ServiceKey + "\r\n");
            RtxtTMSResults.AppendText("ServiceTransactionId : " + s.TransactionInformation.ServiceTransactionId + "\r\n");
            RtxtTMSResults.AppendText("Status : " + s.TransactionInformation.Status + "\r\n");
            //TransactionInformation.StoredValueData
            if (s.TransactionInformation.StoredValueData != null)
            {
                RtxtTMSResults.AppendText("Stored Value Data (if applicable)\r\n");
                RtxtTMSResults.AppendText("- CVResult : " + s.TransactionInformation.StoredValueData.CVResult + "\r\n");
                RtxtTMSResults.AppendText("- CardRestrictionValue : " + s.TransactionInformation.StoredValueData.CardRestrictionValue + "\r\n");
                RtxtTMSResults.AppendText("- CardStatus : " + s.TransactionInformation.StoredValueData.CardStatus + "\r\n");
                RtxtTMSResults.AppendText("- NewBalance : " + s.TransactionInformation.StoredValueData.NewBalance + "\r\n");
                RtxtTMSResults.AppendText("- OrderId : " + s.TransactionInformation.StoredValueData.OrderId + "\r\n");
                RtxtTMSResults.AppendText("- PreviousBalance : " + s.TransactionInformation.StoredValueData.PreviousBalance + "\r\n");
            }
            RtxtTMSResults.AppendText("TransactionClass : " + s.TransactionInformation.TransactionClassTypePair.TransactionClass + "    ");
            RtxtTMSResults.AppendText("TransactionType : " + s.TransactionInformation.TransactionClassTypePair.TransactionType + "\r\n");
            RtxtTMSResults.AppendText("TransactionId : " + s.TransactionInformation.TransactionId + "\r\n");
            RtxtTMSResults.AppendText("TransactionTimestamp : " + s.TransactionInformation.TransactionTimestamp + "\r\n");
            RtxtTMSResults.AppendText("\r\n");
        }

        private void FamilyDetailString(FamilyDetail f)
        {
            RtxtTMSResults.SelectionColor = Color.Black;


            //Family Summary
            RtxtTMSResults.AppendText("BatchId : " + f.BatchId + "\r\n");
            RtxtTMSResults.AppendText("CaptureDateTime : " + f.CaptureDateTime + "\r\n");
            RtxtTMSResults.AppendText("CapturedAmount : " + f.CapturedAmount + "\r\n");
            RtxtTMSResults.AppendText("CustomerId : " + f.CustomerId + "\r\n");
            RtxtTMSResults.AppendText("FamilyId : " + f.FamilyId + "\r\n");
            RtxtTMSResults.AppendText("FamilyState : " + f.FamilyState + "\r\n");
            RtxtTMSResults.AppendText("LastAuthorizedAmount : " + f.LastAuthorizedAmount + "\r\n");
            RtxtTMSResults.AppendText("MerchantProfileId : " + f.MerchantProfileId + "\r\n");
            RtxtTMSResults.AppendText("NetAmount : " + f.NetAmount + "\r\n");
            RtxtTMSResults.AppendText("ServiceKey : " + f.ServiceKey + "\r\n");

            //TransactionId
            RtxtTMSResults.AppendText("List of TransactionIds\r\n");
            foreach (string txnid in f.TransactionIds)
            {
                RtxtTMSResults.AppendText(txnid + "\r\n");
            }
            RtxtTMSResults.AppendText("\r\nTransaction Meta Data\r\n");
            foreach (TransactionMetaData TMD in f.TransactionMetaData)
            {
                RtxtTMSResults.AppendText("TransactionId : " + TMD.TransactionId + "\r\n");
                RtxtTMSResults.AppendText("* Amount : " + TMD.Amount + "\r\n");
                RtxtTMSResults.AppendText("* CardType : " + TMD.CardType + "\r\n");
                RtxtTMSResults.AppendText("* MaskedPAN : " + TMD.MaskedPAN + "\r\n");
                RtxtTMSResults.AppendText("* SequenceNumber : " + TMD.SequenceNumber + "\r\n");
                RtxtTMSResults.AppendText("* ServiceId : " + TMD.ServiceId + "\r\n");
                RtxtTMSResults.AppendText("* TransactionClass : " + TMD.TransactionClassTypePair.TransactionClass + "    ");
                RtxtTMSResults.AppendText("* TransactionType : " + TMD.TransactionClassTypePair.TransactionType + "\r\n");
                RtxtTMSResults.AppendText("* TransactionDateTime : " + TMD.TransactionDateTime + "\r\n");
                RtxtTMSResults.AppendText("* TransactionState : " + TMD.TransactionState + "\r\n");
                RtxtTMSResults.AppendText("* WorkflowId : " + TMD.WorkflowId + "\r\n");
                RtxtTMSResults.AppendText("\r\n");
            }

            RtxtTMSResults.AppendText("\r\n");
        }

        private void TransactionDetailString(TransactionDetail t)
        {
            RtxtTMSResults.SelectionColor = Color.Black;

            ColorText("TransactionState : " + t.TransactionInformation.TransactionState + "\r\n", t.TransactionInformation.TransactionState);
            ColorText("TransactionStatusCode : " + t.TransactionInformation.TransactionStatusCode + "\r\n", t.TransactionInformation.TransactionState);

            //CompleteTransaction
            RtxtTMSResults.AppendText("CompleteTransaction CWS object : " + "OBJECT\r\n");
            if ((TransactionDetailFormat)CboTransactionDetailFormat.SelectedItem == TransactionDetailFormat.SerializedCWS)
                TxtTransactionDetailFormat.Text = t.CompleteTransaction.SerializedTransaction;
                //RtxtTMSResults.AppendText((t.CompleteTransaction.SerializedTransaction == null ? "CompleteTransaction Serialized : NOT AVAILABLE\r\n" : "CompleteTransaction Serialized : " + t.CompleteTransaction.SerializedTransaction + "\r\n"));
            //Family Information
            RtxtTMSResults.AppendText("Family Information \r\n");
            RtxtTMSResults.AppendText("FamilyId : " + t.FamilyInformation.FamilyId + "\r\n");
            RtxtTMSResults.AppendText("FamilySequenceCount : " + t.FamilyInformation.FamilySequenceCount + "\r\n");
            RtxtTMSResults.AppendText("FamilySequenceNumber : " + t.FamilyInformation.FamilySequenceNumber + "\r\n");
            RtxtTMSResults.AppendText("FamilyState : " + t.FamilyInformation.FamilyState + "\r\n");
            RtxtTMSResults.AppendText("NetAmount : " + t.FamilyInformation.NetAmount + "\r\n");
            //Transaction Information
            RtxtTMSResults.AppendText("Transaction Information\r\n");
            RtxtTMSResults.AppendText("Amount : " + t.TransactionInformation.Amount + "\r\n");
            RtxtTMSResults.AppendText("ApprovalCode : " + t.TransactionInformation.ApprovalCode + "\r\n");

            //TransactionInformation.BankcardData
            if (t.TransactionInformation.BankcardData != null)
            {
                //TransactionInformation.BankcardData.AVSResult
                if (t.TransactionInformation.BankcardData.AVSResult != null)
                {
                    RtxtTMSResults.AppendText("AVSResult Summary");
                    RtxtTMSResults.AppendText(" - ActualResult : " + t.TransactionInformation.BankcardData.AVSResult.ActualResult + "\r\n");
                    RtxtTMSResults.AppendText(" - AddressResult : " + t.TransactionInformation.BankcardData.AVSResult.AddressResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CardholderNameResult : " + t.TransactionInformation.BankcardData.AVSResult.CardholderNameResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CityResult : " + t.TransactionInformation.BankcardData.AVSResult.CityResult + "\r\n");
                    RtxtTMSResults.AppendText(" - CountryResult : " + t.TransactionInformation.BankcardData.AVSResult.CountryResult + "\r\n");
                    RtxtTMSResults.AppendText(" - PhoneResult : " + t.TransactionInformation.BankcardData.AVSResult.PhoneResult + "\r\n");
                    RtxtTMSResults.AppendText(" - PostalCodeResult : " + t.TransactionInformation.BankcardData.AVSResult.PostalCodeResult + "\r\n");
                    RtxtTMSResults.AppendText(" - StateResult : " + t.TransactionInformation.BankcardData.AVSResult.StateResult + "\r\n");
                }
                //TransactionInformation.BankcardData
                RtxtTMSResults.AppendText("CardType : " + t.TransactionInformation.BankcardData.CardType + "\r\n");
                RtxtTMSResults.AppendText("CVResult : " + t.TransactionInformation.BankcardData.CVResult + "\r\n");
                RtxtTMSResults.AppendText("MaskedPAN : " + t.TransactionInformation.BankcardData.MaskedPAN + "\r\n");
                RtxtTMSResults.AppendText("OrderId : " + t.TransactionInformation.BankcardData.OrderId + "\r\n"); //The order id generated by CWS. This value is often used by service providers for transaction correlation. Expected.
            }
            RtxtTMSResults.AppendText("BatchId : " + t.TransactionInformation.BatchId + "\r\n");
            RtxtTMSResults.AppendText("CapturedAmount : " + t.TransactionInformation.CapturedAmount + "\r\n");
            RtxtTMSResults.AppendText("CaptureDateTime : " + t.TransactionInformation.CaptureDateTime + "\r\n");
            RtxtTMSResults.AppendText("CaptureState : " + t.TransactionInformation.CaptureState + "\r\n");
            RtxtTMSResults.AppendText("CaptureStatusMessage : " + t.TransactionInformation.CaptureStatusMessage + "\r\n");
            RtxtTMSResults.AppendText("CustomerId : " + t.TransactionInformation.CustomerId + "\r\n");
            if (t.TransactionInformation.ElectronicCheckData != null)
            {
				//TransactionInformation.ElectronicCheckData
				RtxtTMSResults.AppendText("Electronic Check Data");
				RtxtTMSResults.AppendText(" - CheckNumber : " + t.TransactionInformation.ElectronicCheckData.CheckNumber + "\r\n");
				RtxtTMSResults.AppendText(" - MaskedAccountNumber : " + t.TransactionInformation.ElectronicCheckData.MaskedAccountNumber + "\r\n");
				RtxtTMSResults.AppendText(" - TransactionType : " + t.TransactionInformation.ElectronicCheckData.TransactionType + "\r\n");
			}
            RtxtTMSResults.AppendText("IsAcknowledged : " + t.TransactionInformation.IsAcknowledged + "\r\n");
            RtxtTMSResults.AppendText("MaskedPAN : " + t.TransactionInformation.MaskedPAN + "\r\n");
            RtxtTMSResults.AppendText("MerchantProfileId : " + t.TransactionInformation.MerchantProfileId + "\r\n");
            RtxtTMSResults.AppendText("OriginatorTransactionId : " + t.TransactionInformation.OriginatorTransactionId + "\r\n");
            RtxtTMSResults.AppendText("Reference : " + t.TransactionInformation.Reference + "\r\n");
            RtxtTMSResults.AppendText("ServiceId : " + t.TransactionInformation.ServiceId + "\r\n");
            RtxtTMSResults.AppendText("ServiceKey : " + t.TransactionInformation.ServiceKey + "\r\n");
            RtxtTMSResults.AppendText("ServiceTransactionId : " + t.TransactionInformation.ServiceTransactionId + "\r\n");
            RtxtTMSResults.AppendText("Status : " + t.TransactionInformation.Status + "\r\n");
            if (t.TransactionInformation.StoredValueData != null)
            {
                //TransactionInformation.ElectronicCheckData
                RtxtTMSResults.AppendText("Electronic Check Data");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.CVResult + "\r\n");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.CardRestrictionValue + "\r\n");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.CardStatus + "\r\n");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.NewBalance + "\r\n");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.OrderId + "\r\n");
                RtxtTMSResults.AppendText(" - CVResult : " + t.TransactionInformation.StoredValueData.PreviousBalance + "\r\n");
            }
            RtxtTMSResults.AppendText("TransactionClass : " + t.TransactionInformation.TransactionClassTypePair.TransactionClass + "    ");
            RtxtTMSResults.AppendText("TransactionType : " + t.TransactionInformation.TransactionClassTypePair.TransactionType + "\r\n");
            RtxtTMSResults.AppendText("TransactionId : " + t.TransactionInformation.TransactionId + "\r\n");
            RtxtTMSResults.SelectionColor = Color.Black;
            RtxtTMSResults.AppendText("TransactionTimestamp : " + t.TransactionInformation.TransactionTimestamp + "\r\n");
            RtxtTMSResults.AppendText("\r\n");

            RtxtTMSResults.AppendText("\r\n");
        }

        private void ColorText(string _string, TransactionState _ts)
        {
            if (_ts == TransactionState.ErrorConnecting | _ts == TransactionState.ErrorUnknown | _ts == TransactionState.ErrorValidation)
                RtxtTMSResults.SelectionColor = Color.Red;
            else if (_ts == TransactionState.Declined)
                RtxtTMSResults.SelectionColor = Color.Purple;
            else
                RtxtTMSResults.SelectionColor = Color.DarkGreen;

            RtxtTMSResults.AppendText(_string);
        }

        #endregion END process TMS response

        #region Setup Help Links
        private void lnkQueryBatch_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryBatch.aspx");
        }
        private void lnkQueryTransactions_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionsSummary.aspx");
        }
        private void lnkQueryTransactionFamilies_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionFamilies.aspx");
        }
        private void lnkQueryTransactionDetails_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/DataServices/TMS_Developer_Guide/2.0.17/Implementation/SOAP/QueryTransactionsDetail.aspx");
        }
        #endregion END Setup Help Links

        private void TxtClearTransactionIds_Click(object sender, EventArgs e)
        {
            txtQTP_TransactionIds.Text = "";
        }

        private void TxtClearBatchTransactionIds_Click(object sender, EventArgs e)
        {
            txtQBP_TransactionIds.Text = "";
        }

        private void CboTransactionDetailFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if((TransactionDetailFormat)CboTransactionDetailFormat.SelectedItem == TransactionDetailFormat.SerializedCWS)
            {
                TxtTransactionDetailFormat.Enabled = true;
            }
            else
            {
                TxtTransactionDetailFormat.Text = "";
                TxtTransactionDetailFormat.Enabled = false;
            }
        }

        private void TxtTransactionDetailFormat_KeyDown(object sender, KeyEventArgs e)
        {
            // See if Ctrl-A is pressed... 
            if (e.Control && (e.KeyCode == Keys.A))
            {
                TxtTransactionDetailFormat.SelectAll();
                e.Handled = true;
            }
        }

        private void TmrUTCTIme_Tick(object sender, EventArgs e)
        {
            LblCurrentUTCTime.Text = "Current Time [Local : " +DateTime.Now + "] - [UTC : " + DateTime.UtcNow +"]";
        }

    }

    #region Local Classes

    public class SummaryDetailVal : SummaryDetail
    {
        public SummaryDetail SD;
        public SummaryDetailVal(SummaryDetail sd)
        {
            SD = sd;
        }
        public override string ToString()
        {// Generates the text shown in the combo box
            return SD.TransactionInformation.Amount + " | " + SD.TransactionInformation.TransactionId + " (" + SD.TransactionInformation.TransactionTimestamp + ") UTC";
        }
    }

    public class FamilyDetailVal : FamilyDetail
    {
        public FamilyDetail FD;
        public FamilyDetailVal(FamilyDetail fd)
        {
            FD = fd;
        }
        public override string ToString()
        {// Generates the text shown in the combo box
            return "Net Amount : " + FD.NetAmount + " | Family State : " + FD.FamilyState;
        }
    }

    public class BatchDetailDataVal : BatchDetailData
    {
        public BatchDetailData BDD;
        public BatchDetailDataVal(BatchDetailData bdd)
        {
            BDD = bdd;
        }
        public override string ToString()
        {// Generates the text shown in the combo box
            return "BatchId : " + BDD.BatchId + " | BatchCaptureDate : " + BDD.BatchCaptureDate;
        }
    }

    public class TransactionDetailVal : TransactionDetail
    {
        public TransactionDetail TD;
        public TransactionDetailVal(TransactionDetail td)
        {
            TD = td;
        }
        public override string ToString()
        {// Generates the text shown in the combo box
            return TD.TransactionInformation.Amount + " | " + TD.TransactionInformation.TransactionId + " (" + TD.TransactionInformation.TransactionTimestamp + ") UTC";
        }
    }

    public enum LastSearchType : int
    {
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QueryTransactionsSummary = 0,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QueryTransactionFamilies = 1,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QueryTransactionsDetail = 2,
        [System.Runtime.Serialization.EnumMemberAttribute()]
        QueryBatch = 3,

    }

    #endregion END Local Classes
}
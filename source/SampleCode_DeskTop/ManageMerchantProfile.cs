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
using System.Windows.Forms;
using SampleCode.CWSServiceInformation;

namespace SampleCode
{
    public partial class ManageMerchantProfile : Form
    {
        private TypeISOLanguageCodeA3 _Language;
        private TypeISOCountryCodeA3 _CountryCode;
        private TypeISOCurrencyCodeA3 _CurrencyCode;
        private CustomerPresent _CustomerPresent;
        private RequestACI _RequestACI;
        private IndustryType _MerchantIndustryType;
        private EntryMode _EntryMode ;
        public bool _Dirty;
        public HelperMethods Helper = new HelperMethods();//The following class performs many of the different operations needs for service information and trasaction processing
        private BankcardService _bcs = new BankcardService();
        private ElectronicCheckingService _ecks = new ElectronicCheckingService();
        private StoredValueService _svas = new StoredValueService();
        private string _strServiceID;
        private static DataGenerator _dg;


        protected FaultHandler.FaultHandler _FaultHandler = new FaultHandler.FaultHandler();

        public ManageMerchantProfile()
        {
            InitializeComponent();

            //Actions for Application Data - Typically only performed upon initial installation of software
            //Note : Resultant variable to be stored : ApplicationProfileId
            cboMerchantDataAction.Items.Add(new Item("[Select Action]", "0", ""));
            cboMerchantDataAction.Items.Add(new Item("Is Merchant Profile Initialized", "1", ""));
            cboMerchantDataAction.Items.Add(new Item("Get Merchant Profile", "2", ""));
            cboMerchantDataAction.Items.Add(new Item("Get Merchant Profile Ids", "3", ""));
            cboMerchantDataAction.Items.Add(new Item("Get Merchant Profiles", "4", ""));
            cboMerchantDataAction.Items.Add(new Item("Get Merchant Profiles By ProfileId", "5", ""));
            cboMerchantDataAction.Items.Add(new Item("Save Merchant Profiles", "6", ""));
            cboMerchantDataAction.Items.Add(new Item("Delete Merchant Profile", "7", ""));
            cboMerchantDataAction.SelectedIndex = 0;
        }

        private void ManageMerchantProfile_Load(object sender, EventArgs e)
        {
            foreach (var item in ((SampleCode_DeskTop)(Owner)).cboAvailableProfiles.Items)
            {
                cboAvailableProfiles.Items.Add(item);
            }
            cboAvailableProfiles.SelectedItem = ((SampleCode_DeskTop) (Owner)).cboAvailableProfiles.SelectedItem;
        }

        private void showECKFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = false;
            txtCustomerServicePhone.ReadOnly = false;
            txtMerchantId.ReadOnly = false;
            lblMerchantId.Text = "OriginatorId";
            txtName.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtTaxId.ReadOnly = true;
            //MerchantData.Address
            txtCity.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtStateProvince.ReadOnly = false;
            txtStreetAddress1.ReadOnly = false;
            txtStreetAddress2.ReadOnly = false;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = true;
            txtAcquirerBIN.ReadOnly = true;
            txtAgentBank.ReadOnly = true;
            txtAgentChain.ReadOnly = true;
            txtClientNum.ReadOnly = true;
            txtLocation.ReadOnly = true;
            txtSecondaryTerminalId.ReadOnly = true;
            txtSettlementAgent.ReadOnly = true;
            txtSharingGroup.ReadOnly = true;
            txtSIC.ReadOnly = false;
            txtStoreId.ReadOnly = false;
            lblStoreId.Text = "SiteId";
            txtSocketNum.ReadOnly = false;
            lblTerminalId.Text = "ProductId";
            txtTimeZoneDifferential.ReadOnly = true;
            txtReimbursementAttribute.ReadOnly = true;
            txtClientNum.Text = "";

        }

        private void showSVAFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = false;
            txtCustomerServicePhone.ReadOnly = false;
            txtMerchantId.ReadOnly = false;
            txtName.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtTaxId.ReadOnly = false;
            //MerchantData.Address
            txtCity.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtStateProvince.ReadOnly = false;
            txtStreetAddress1.ReadOnly = false;
            txtStreetAddress2.ReadOnly = false;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = true;
            txtAcquirerBIN.ReadOnly = true;
            txtAgentBank.ReadOnly = true;
            txtAgentChain.ReadOnly = false;
            txtAgentChain.Text = "124423";
            txtClientNum.ReadOnly = false;
            txtLocation.ReadOnly = true;
            txtSecondaryTerminalId.ReadOnly = true;
            txtSettlementAgent.ReadOnly = true;
            txtSharingGroup.ReadOnly = true;
            txtSIC.ReadOnly = false;
            txtStoreId.ReadOnly = false;
            txtSocketNum.ReadOnly = false;
            txtTimeZoneDifferential.ReadOnly = true;
            txtReimbursementAttribute.ReadOnly = true;
            
        }

        private void showBCPFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = false;
            txtCustomerServicePhone.ReadOnly = false;
            txtMerchantId.ReadOnly = false;
            txtName.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtTaxId.ReadOnly = false;
            //MerchantData.Address
            txtCity.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtStateProvince.ReadOnly = false;
            txtStreetAddress1.ReadOnly = false;
            txtStreetAddress2.ReadOnly = false;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = true;
            txtAcquirerBIN.ReadOnly = true;
            txtAgentBank.ReadOnly = true;
            txtAgentChain.ReadOnly = true;
            txtClientNum.ReadOnly = true;
            txtLocation.ReadOnly = true;
            txtSecondaryTerminalId.ReadOnly = true;
            txtSettlementAgent.ReadOnly = true;
            txtSharingGroup.ReadOnly = true;
            txtSIC.ReadOnly = false;
            txtStoreId.ReadOnly = true;
            txtSocketNum.ReadOnly = false;
            txtTimeZoneDifferential.ReadOnly = true;
            txtReimbursementAttribute.ReadOnly = true;
            
        }

        private void showBCPExpandedFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = false;
            txtCustomerServicePhone.ReadOnly = false;
            txtMerchantId.ReadOnly = false;
            txtName.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtTaxId.ReadOnly = false;
            //MerchantData.Address
            txtCity.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtStateProvince.ReadOnly = false;
            txtStreetAddress1.ReadOnly = false;
            txtStreetAddress2.ReadOnly = false;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = false;
            txtAcquirerBIN.ReadOnly = false;
            txtAgentBank.ReadOnly = false;
            txtAgentChain.ReadOnly = false;
            txtClientNum.ReadOnly = false;
            txtLocation.ReadOnly = false;
            txtSecondaryTerminalId.ReadOnly = false;
            txtSettlementAgent.ReadOnly = false;
            txtSharingGroup.ReadOnly = false;
            txtSIC.ReadOnly = false;
            txtStoreId.ReadOnly = false;
            txtSocketNum.ReadOnly = false;
            txtTimeZoneDifferential.ReadOnly = false;
            txtReimbursementAttribute.ReadOnly = false;
        }
        
        private void hideAllFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = true;
            txtCustomerServicePhone.ReadOnly = true;
            txtMerchantId.ReadOnly = true;
            lblMerchantId.Text = "MerchantId";
            txtName.ReadOnly = true;
            txtPhone.ReadOnly = true;
            txtTaxId.ReadOnly = true;
            //MerchantData.Address
            txtCity.ReadOnly = true;
            txtPostalCode.ReadOnly = true;
            txtStateProvince.ReadOnly = true;
            txtStreetAddress1.ReadOnly = true;
            txtStreetAddress2.ReadOnly = true;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = true;
            txtAcquirerBIN.ReadOnly = true;
            txtAgentBank.ReadOnly = true;
            txtAgentChain.ReadOnly = true;
            txtClientNum.ReadOnly = true;
            txtLocation.ReadOnly = true;
            txtSecondaryTerminalId.ReadOnly = true;
            txtSettlementAgent.ReadOnly = true;
            txtSharingGroup.ReadOnly = true;
            txtSIC.ReadOnly = true;
            txtStoreId.ReadOnly = true;
            lblStoreId.Text = "StoreId";
            txtSocketNum.ReadOnly = true;
            lblTerminalId.Text = "TerminalId";
            txtTimeZoneDifferential.ReadOnly = true;
            txtReimbursementAttribute.ReadOnly = true;
        }

        private void showAllFields()
        {
            //MerchantData
            txtCustomerServiceInternet.ReadOnly = false;
            txtCustomerServicePhone.ReadOnly = false;
            txtMerchantId.ReadOnly = false;
            lblMerchantId.Text = "MerchantId";
            txtName.ReadOnly = false;
            txtPhone.ReadOnly = false;
            txtTaxId.ReadOnly = false;
            //MerchantData.Address
            txtCity.ReadOnly = false;
            txtPostalCode.ReadOnly = false;
            txtStateProvince.ReadOnly = false;
            txtStreetAddress1.ReadOnly = false;
            txtStreetAddress2.ReadOnly = false;
            //MerchantData.BankcardMerchantData
            txtABANumber.ReadOnly = false;
            txtAcquirerBIN.ReadOnly = false;
            txtAgentBank.ReadOnly = false;
            txtAgentChain.ReadOnly = false;
            txtClientNum.ReadOnly = false;
            txtLocation.ReadOnly = false;
            txtSecondaryTerminalId.ReadOnly = false;
            txtSettlementAgent.ReadOnly = false;
            txtSharingGroup.ReadOnly = false;
            txtSIC.ReadOnly = false;
            txtStoreId.ReadOnly = false;
            lblStoreId.Text = "StoreId";
            txtSocketNum.ReadOnly = false;
            lblTerminalId.Text = "TerminalId";
            txtTimeZoneDifferential.ReadOnly = false;
            txtReimbursementAttribute.ReadOnly = false;
        }

        public void CallingForm(MerchantProfile merchantProfile, bool blnNewProfile, BankcardService bcs, ElectronicCheckingService ecks, StoredValueService svas, string serviceId, DataGenerator dg)
        {
            _bcs = bcs;
            _ecks = ecks;
            _svas = svas;
            _strServiceID = serviceId;
            _dg = dg;

            hideAllFields();
            //Since MerchantProfile is saved at the serviceId level, display serviceId.
            //txtMerchantProfileServiceId.Text = merchantProfile.ServiceId;

            if (serviceId.Length < 1)
                txtMerchantProfileServiceId.Text = "ServiceId not selected in calling form";
            else
                txtMerchantProfileServiceId.Text = serviceId;

            //Populate combo boxes with the Enumeration
            cboCountryCode.Sorted = true;
            cboCountryCode.DataSource = Enum.GetValues(typeof(TypeISOCountryCodeA3));
            cboCountryCode.SelectedItem = TypeISOCountryCodeA3.NotSet;

            cboLanguage.Sorted = true;
            cboLanguage.DataSource = Enum.GetValues(typeof(TypeISOLanguageCodeA3));
            cboLanguage.SelectedItem = TypeISOLanguageCodeA3.NotSet;

            cboCurrencyCode.Sorted = true;
            cboCurrencyCode.DataSource = Enum.GetValues(typeof(TypeISOCurrencyCodeA3));
            cboCurrencyCode.SelectedItem = TypeISOCurrencyCodeA3.NotSet;


            cboCustomerPresent.Sorted = true;
            cboCustomerPresent.DataSource = Enum.GetValues(typeof(CustomerPresent));
            cboCustomerPresent.SelectedItem = CustomerPresent.NotSet;

            cboRequestACI.Sorted = true;
            cboRequestACI.DataSource = Enum.GetValues(typeof(RequestACI));
            cboRequestACI.SelectedItem = RequestACI.IsCPSMeritCapable;

            cboEntryMode.Sorted = true;
            cboEntryMode.DataSource = Enum.GetValues(typeof(EntryMode));
            cboEntryMode.SelectedItem = EntryMode.NotSet;

            cboMerchantIndustryType.Sorted = true;
            cboMerchantIndustryType.DataSource = Enum.GetValues(typeof(IndustryType));
            cboMerchantIndustryType.SelectedItem = IndustryType.NotSet;
        
            if (_bcs != null)
            {
                if (_strServiceID == "C82ED00001" || _strServiceID == "71C8700001" ||
                    _strServiceID == "88D9300001" || _strServiceID == "B447F00001" ||
                    _strServiceID == "D806000001" || _strServiceID == "E88FD00001")
                    showBCPExpandedFields();
                else if (_strServiceID == "168511300C" || _strServiceID == "9999999999")
                    showBCPExpandedFields();
                else
                {
                    showBCPFields();
                }
            }

            if (_ecks != null)
            {
                showECKFields();
            }
            if (_svas != null)
            {
                showSVAFields();
            }
        }
        
        private void SaveMerchantProfiles()
        {
            try
            {
                //Unique MerchantProfileId should be used for all new MerchantProfiles added
                if (cboAvailableProfiles.Text.Length < 1)
                {
                    MessageBox.Show("Merchant ProfileId required and cannot be empty");
                    cboAvailableProfiles.Focus();
                    return;
                }
                SaveMerchantInformation();
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetSvcEndpoint();//Change the endpoint to use the backup.
                    SaveMerchantInformation();
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    MessageBox.Show("Neither the primary or secondary endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        private void DeleteMerchantProfile()
        {
            try
            {
                //Unique MerchantProfileId should be used for all new MerchantProfiles added
                if (cboAvailableProfiles.Text.Length < 1)
                {
                    MessageBox.Show("Merchant ProfileId required and cannot be empty");
                    cboAvailableProfiles.Focus();
                    return;
                }

                DeleteMerchProfile();                
            }
            catch (System.ServiceModel.EndpointNotFoundException)
            {
                //In this case the SvcEndpoint was not available. Try the same logic again with the alternate Endpoint
                try
                {
                    ((SampleCode_DeskTop)(Owner)).Helper.SetSvcEndpoint();//Change the endpoint to use the backup.
                    DeleteMerchProfile();
                }
                catch (System.ServiceModel.EndpointNotFoundException)
                {
                    MessageBox.Show("Neither the primary or secondary endpoints are available. Unable to process.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
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

        #region WebService Calls

        private void SaveMerchantInformation()
        {
            List<MerchantProfile> MPList = new List<MerchantProfile>();
            MerchantProfile MerP = new MerchantProfile();

            MerP.ProfileId = cboAvailableProfiles.Text;
            MerP.MerchantData = new MerchantProfileMerchantData();
            MerP.MerchantData.Address = new AddressInfo();
            //MerP.MerchantData.BankcardMerchantData = new BankcardMerchantData();

            MerP.TransactionData = new MerchantProfileTransactionData();


            MerP.MerchantData.Address.Street1 = txtStreetAddress1.Text;
            MerP.MerchantData.Address.Street2 = txtStreetAddress2.Text;
            MerP.MerchantData.Address.City = txtCity.Text;
            try { MerP.MerchantData.Address.StateProvince = (TypeStateProvince)Enum.Parse(typeof(TypeStateProvince), txtStateProvince.Text); }
            catch { }
            MerP.MerchantData.Address.PostalCode = txtPostalCode.Text;
            MerP.MerchantData.CustomerServicePhone = txtCustomerServicePhone.Text;
            MerP.MerchantData.CustomerServiceInternet = txtCustomerServiceInternet.Text;
            MerP.MerchantData.MerchantId = txtMerchantId.Text;
            MerP.MerchantData.Name = txtName.Text;
            MerP.MerchantData.Phone = txtPhone.Text;
            MerP.MerchantData.TaxId = txtTaxId.Text;
            MerP.MerchantData.Language = _Language;
            MerP.MerchantData.Address.CountryCode = _CountryCode;

            MerP.MerchantData.BankcardMerchantData = new BankcardMerchantData();

            if (_bcs != null)
            {
                MerP.TransactionData.BankcardTransactionDataDefaults = new BankcardTransactionDataDefaults();
                MerP.MerchantData.BankcardMerchantData = new BankcardMerchantData();
                MerP.MerchantData.BankcardMerchantData.ClientNumber = txtClientNum.Text;
                MerP.MerchantData.BankcardMerchantData.SIC = txtSIC.Text;
                MerP.MerchantData.BankcardMerchantData.TerminalId = txtSocketNum.Text;
                MerP.MerchantData.BankcardMerchantData.StoreId = txtStoreId.Text;
                MerP.TransactionData.BankcardTransactionDataDefaults.CurrencyCode = _CurrencyCode;
                MerP.TransactionData.BankcardTransactionDataDefaults.CustomerPresent = _CustomerPresent;
                MerP.TransactionData.BankcardTransactionDataDefaults.RequestACI = _RequestACI;
                MerP.TransactionData.BankcardTransactionDataDefaults.RequestAdvice = RequestAdvice.Capable;
                MerP.TransactionData.BankcardTransactionDataDefaults.EntryMode = _EntryMode;

                // Terminal Capture other providers
                MerP.MerchantData.BankcardMerchantData.ABANumber = txtABANumber.Text;
                MerP.MerchantData.BankcardMerchantData.AcquirerBIN = txtAcquirerBIN.Text;
                MerP.MerchantData.BankcardMerchantData.AgentBank = txtAgentBank.Text;
                MerP.MerchantData.BankcardMerchantData.AgentChain = txtAgentChain.Text;
                MerP.MerchantData.BankcardMerchantData.Location = txtLocation.Text;
                MerP.MerchantData.BankcardMerchantData.SecondaryTerminalId = txtSecondaryTerminalId.Text;
                MerP.MerchantData.BankcardMerchantData.SettlementAgent = txtSettlementAgent.Text;
                MerP.MerchantData.BankcardMerchantData.SharingGroup = txtSharingGroup.Text;
                MerP.MerchantData.BankcardMerchantData.TimeZoneDifferential = txtTimeZoneDifferential.Text;
                MerP.MerchantData.BankcardMerchantData.ReimbursementAttribute = txtReimbursementAttribute.Text;
                MerP.MerchantData.BankcardMerchantData.IndustryType = _MerchantIndustryType;
            }

            if (_ecks != null)
            {
                MerP.MerchantData.ElectronicCheckingMerchantData = new ElectronicCheckingMerchantData();
                MerP.MerchantData.ElectronicCheckingMerchantData.OrginatorId = txtMerchantId.Text;
                MerP.MerchantData.ElectronicCheckingMerchantData.ProductId = txtSocketNum.Text;
                MerP.MerchantData.ElectronicCheckingMerchantData.SiteId = txtStoreId.Text;
            }
            if (_svas != null)
            {
                MerP.MerchantData.StoredValueMerchantData = new StoredValueMerchantData();
                MerP.MerchantData.StoredValueMerchantData.AgentChain = txtAgentChain.Text;
                MerP.MerchantData.StoredValueMerchantData.ClientNumber = txtClientNum.Text;
                MerP.MerchantData.StoredValueMerchantData.SIC = txtSIC.Text;
                MerP.MerchantData.StoredValueMerchantData.StoreId = txtStoreId.Text;
                MerP.MerchantData.StoredValueMerchantData.TerminalId = txtSocketNum.Text;
                MerP.MerchantData.StoredValueMerchantData.IndustryType = _MerchantIndustryType;

            }

            //Add the profile to a list of profiles. This is necessary to save the profile
            MPList.Add(MerP);
            //From the calling form
            ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();
            string _strServiceID = ((SampleCode_DeskTop)(Owner)).Helper.ServiceID;
            string _strSessionToken = ((SampleCode_DeskTop)(Owner)).Helper.SessionToken;

            ((SampleCode_DeskTop)(Owner)).Helper.Cwssic.SaveMerchantProfiles(_strSessionToken, _strServiceID, TenderType.Credit, MPList);

            _Dirty = true; //When control is returned to calling form the form will re-generate service information. 

            MessageBox.Show("Successfully Saved a Merchant Profile");

            //Reset the cboAvailableProfiles dropdown.
            cboAvailableProfiles.Items.Clear();
            cboAvailableProfiles.Text = "";
            //Now sync the new list
            ((SampleCode_DeskTop)(Owner)).GetMerchantProfileIds();
            foreach (var item in ((SampleCode_DeskTop)(Owner)).cboAvailableProfiles.Items)
            {
                cboAvailableProfiles.Items.Add(item);
            }
        }

        private void DeleteMerchProfile()
        {
            DialogResult Result;
            string profileId = cboAvailableProfiles.Text;
            if (profileId.Trim() == "") profileId = "<default>";
            Result = MessageBox.Show(
                "The action will attempt to delete the profile \r\n\r\n     '" + profileId + "' \r\n\r\nDo you want to continue?",
                "Overwrite", MessageBoxButtons.OKCancel);
            if (Result == DialogResult.Cancel) return;

            //From the calling form
            ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();
            string _strServiceID = ((SampleCode_DeskTop)(Owner)).Helper.ServiceID;
            string _strSessionToken = ((SampleCode_DeskTop)(Owner)).Helper.SessionToken;

            ((SampleCode_DeskTop)(Owner)).Helper.Cwssic.DeleteMerchantProfile(_strSessionToken, cboAvailableProfiles.Text, _strServiceID, TenderType.Credit);
            MessageBox.Show("Successfully deleted merchant ProfileId : " + cboAvailableProfiles.Text);
           
            _Dirty = true; //When control is returned to calling form the form will re-generate service information. 
            
            //Reset the cboAvailableProfiles dropdown.
            cboAvailableProfiles.Items.Clear();
            cboAvailableProfiles.Text = "";
            //Now sync the new list
            ((SampleCode_DeskTop)(Owner)).GetMerchantProfileIds();
            foreach (var item in ((SampleCode_DeskTop)(Owner)).cboAvailableProfiles.Items)
            {
                cboAvailableProfiles.Items.Add(item);
            }
        }

        private void IsMerchantProfileInitialized()
        {
            if (cboAvailableProfiles.Text.Length <1)
            {
                MessageBox.Show("Please select a merchant profileId");
                cboAvailableProfiles.Focus();
                return;
            }
            //From the calling form
            ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();
            string _strServiceID = ((SampleCode_DeskTop)(Owner)).Helper.ServiceID;
            string _strSessionToken = ((SampleCode_DeskTop)(Owner)).Helper.SessionToken;

            if(((SampleCode_DeskTop)(Owner)).Helper.Cwssic.IsMerchantProfileInitialized(_strSessionToken, _strServiceID, cboAvailableProfiles.Text, TenderType.Credit))
            {
                MessageBox.Show("Merchant profileId : " + cboAvailableProfiles.Text + " is initialized.");
            }
            else
            {
                MessageBox.Show("Merchant profileId : " + cboAvailableProfiles.Text + " is NOT initialized.");
            }
            
        }

        private void GetMerchantProfile()
        {
            if (cboAvailableProfiles.Text.Length < 1)
            {
                MessageBox.Show("Please select a merchant profileId");
                cboAvailableProfiles.Focus();
                return;
            }

             ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();
            string _strServiceID = ((SampleCode_DeskTop)(Owner)).Helper.ServiceID;
            string _strSessionToken = ((SampleCode_DeskTop)(Owner)).Helper.SessionToken;


            MerchantProfile merchantProfile =
                ((SampleCode_DeskTop)(Owner)).Helper.Cwssic.GetMerchantProfile(_strSessionToken, cboAvailableProfiles.Text, _strServiceID, TenderType.Credit);

            //Note : items commented out are not use so no need to wire up a text box as well as add to 'SaveMerchantInformation()'
            lblLastUpdated.Text = "Last Updated : " + merchantProfile.LastUpdated;
            //MerchantData
            //MerchantData.Address
            txtCity.Text = merchantProfile.MerchantData.Address.City;
            txtPostalCode.Text = merchantProfile.MerchantData.Address.PostalCode;
            txtStateProvince.Text = merchantProfile.MerchantData.Address.StateProvince.ToString();
            txtStreetAddress1.Text = merchantProfile.MerchantData.Address.Street1;
            txtStreetAddress2.Text = merchantProfile.MerchantData.Address.Street2;

            txtCustomerServiceInternet.Text = merchantProfile.MerchantData.CustomerServiceInternet;
            txtCustomerServicePhone.Text = merchantProfile.MerchantData.CustomerServicePhone;
            txtMerchantId.Text = merchantProfile.MerchantData.MerchantId;
            txtName.Text = merchantProfile.MerchantData.Name;
            txtPhone.Text = merchantProfile.MerchantData.Phone;
            txtTaxId.Text = merchantProfile.MerchantData.TaxId;
            if (_bcs != null)
            {
                //MerchantData.BankcardMerchantData
                txtABANumber.Text = merchantProfile.MerchantData.BankcardMerchantData.ABANumber;
                txtAcquirerBIN.Text = merchantProfile.MerchantData.BankcardMerchantData.AcquirerBIN;
                txtAgentBank.Text = merchantProfile.MerchantData.BankcardMerchantData.AgentBank;
                txtAgentChain.Text = merchantProfile.MerchantData.BankcardMerchantData.AgentChain;
                txtClientNum.Text = merchantProfile.MerchantData.BankcardMerchantData.ClientNumber;
                txtLocation.Text = merchantProfile.MerchantData.BankcardMerchantData.Location;
                //txtTBD.Text = _MerchantProfile.MerchantData.BankcardMerchantData.PrintCustomerServicePhone == "";
                txtSecondaryTerminalId.Text = merchantProfile.MerchantData.BankcardMerchantData.SecondaryTerminalId;
                txtSettlementAgent.Text = merchantProfile.MerchantData.BankcardMerchantData.SettlementAgent;
                txtSharingGroup.Text = merchantProfile.MerchantData.BankcardMerchantData.SharingGroup;
                txtSIC.Text = merchantProfile.MerchantData.BankcardMerchantData.SIC;
                txtStoreId.Text = merchantProfile.MerchantData.BankcardMerchantData.StoreId;
                txtSocketNum.Text = merchantProfile.MerchantData.BankcardMerchantData.TerminalId;
                txtTimeZoneDifferential.Text = merchantProfile.MerchantData.BankcardMerchantData.TimeZoneDifferential;
                txtReimbursementAttribute.Text = merchantProfile.MerchantData.BankcardMerchantData.ReimbursementAttribute;
            }
            if (_svas != null)
            {
                //MerchantData.StoredValueMerchantData
                txtAgentChain.Text = merchantProfile.MerchantData.StoredValueMerchantData.AgentChain;
                txtClientNum.Text = merchantProfile.MerchantData.StoredValueMerchantData.ClientNumber;
                txtSIC.Text = merchantProfile.MerchantData.StoredValueMerchantData.SIC;
                txtStoreId.Text = merchantProfile.MerchantData.StoredValueMerchantData.StoreId;
                txtSocketNum.Text = merchantProfile.MerchantData.StoredValueMerchantData.TerminalId;
                _MerchantIndustryType = merchantProfile.MerchantData.StoredValueMerchantData.IndustryType;
            }
            if (_ecks != null)
            {
                //MerchantData.ElectronicCheckingMerchantData
                txtMerchantId.Text = merchantProfile.MerchantData.ElectronicCheckingMerchantData.OrginatorId;
                txtStoreId.Text = merchantProfile.MerchantData.ElectronicCheckingMerchantData.SiteId;
                txtSocketNum.Text = merchantProfile.MerchantData.ElectronicCheckingMerchantData.ProductId;
            }

            //First Populate with the Enumeration
            cboCountryCode.DataSource = Enum.GetValues(typeof(TypeISOCountryCodeA3));
            //Now select the index that matches
            if (merchantProfile.MerchantData.Address.CountryCode.ToString().Length > 0)
            {
                cboCountryCode.SelectedItem = merchantProfile.MerchantData.Address.CountryCode;
                _CountryCode = (TypeISOCountryCodeA3)cboCountryCode.SelectedItem;
            }
            //First Populate with the Enumeration
            cboLanguage.DataSource = Enum.GetValues(typeof(TypeISOLanguageCodeA3));
            //Now select the index that matches
            if (merchantProfile.MerchantData.Language.ToString().Length > 0)
            {
                cboLanguage.SelectedItem = merchantProfile.MerchantData.Language;
                _Language = (TypeISOLanguageCodeA3)cboLanguage.SelectedItem;
            }
            //First Populate with the Enumeration
            cboCurrencyCode.DataSource = Enum.GetValues(typeof(TypeISOCurrencyCodeA3));
            //Now select the index that matches
            if (merchantProfile.MerchantData.Language.ToString().Length > 0)
            {
                cboCurrencyCode.SelectedItem = merchantProfile.TransactionData.BankcardTransactionDataDefaults.CurrencyCode;
                _CurrencyCode = (TypeISOCurrencyCodeA3)cboCurrencyCode.SelectedItem;
            }

            //First Populate with the Enumeration
            cboCustomerPresent.DataSource = Enum.GetValues(typeof(CustomerPresent));
            //Now select the index that matches
            if (merchantProfile.TransactionData.BankcardTransactionDataDefaults.CustomerPresent.ToString().Length > 0)
            {
                cboCustomerPresent.SelectedItem = merchantProfile.TransactionData.BankcardTransactionDataDefaults.CustomerPresent;
                _CustomerPresent = (CustomerPresent)cboCustomerPresent.SelectedItem;
            }

            //First Populate with the Enumeration
            cboRequestACI.DataSource = Enum.GetValues(typeof(RequestACI));
            //Now select the index that matches
            if (merchantProfile.TransactionData.BankcardTransactionDataDefaults.RequestACI.ToString().Length > 0)
            {
                cboRequestACI.SelectedItem = merchantProfile.TransactionData.BankcardTransactionDataDefaults.RequestACI;
                _RequestACI = (RequestACI)cboRequestACI.SelectedItem;
            }

            //First Populate with the Enumeration
            cboMerchantIndustryType.DataSource = Enum.GetValues(typeof(IndustryType));
            if (merchantProfile.MerchantData.BankcardMerchantData.IndustryType.ToString().Length > 0)
            {
                cboMerchantIndustryType.SelectedItem = merchantProfile.MerchantData.BankcardMerchantData.IndustryType;
                _MerchantIndustryType = (IndustryType)cboMerchantIndustryType.SelectedItem;
            }

            //First Populate with the Enumeration
            cboEntryMode.DataSource = Enum.GetValues(typeof(EntryMode));
            if (merchantProfile.TransactionData.BankcardTransactionDataDefaults.EntryMode.ToString().Length > 0)
            {
                cboEntryMode.SelectedItem = merchantProfile.TransactionData.BankcardTransactionDataDefaults.EntryMode;
                _EntryMode = (EntryMode)cboEntryMode.SelectedItem;
            }
        }

        private void GetMerchantProfileIds()
        {
            ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();
            string _strServiceID = ((SampleCode_DeskTop)(Owner)).Helper.ServiceID;
            string _strSessionToken = ((SampleCode_DeskTop)(Owner)).Helper.SessionToken;

            List<string> merchantProfileIds = ((SampleCode_DeskTop)(Owner)).Helper.Cwssic.GetMerchantProfileIds(_strSessionToken, _strServiceID, TenderType.Credit);

            string msgbox = "";
            foreach (string merchantProfileId in merchantProfileIds)
            {
                msgbox += merchantProfileId + "\r\n";
            }
            MessageBox.Show("The following is a list of saved merchant profileId(s)\r\n\r\n" + msgbox);
        }

        #endregion WebService Calls

        #region Form Events

        private void CmdPerformWebRequest_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                ((SampleCode_DeskTop)(Owner)).Helper.CheckTokenExpire();

                Item item = (Item)cboMerchantDataAction.SelectedItem;
                if (item.Value1 == "0")
                {
                    MessageBox.Show("Please select an action");
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "1")
                {//IsMerchantProfileInitialized
                    IsMerchantProfileInitialized();
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "2")
                {//GetMerchantProfile
                    GetMerchantProfile();
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "3")
                {//GetMerchantProfileIds
                    GetMerchantProfileIds();
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "4")
                {//GetMerchantProfiles
                    MessageBox.Show("In general it's better to use the 'GetMerchantProfileIds' first and then to use 'GetMerchantProfile' to manage a merchant account.");
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "5")
                {//GetMerchantProfilesByProfileId
                    MessageBox.Show("The GetMerchantProfilesByProfileId returns a list of profiles which this page cannot currently disply. Please use GetMerchantProfile instead.");
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "6")
                {//SaveMerchantProfiles
                    SaveMerchantProfiles();
                    Cursor = Cursors.Default;
                    return;
                }
                if (item.Value1 == "7")
                {//DeleteMerchantProfile
                    DeleteMerchantProfile();
                    Cursor = Cursors.Default;
                    return;
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

        private void cmdPopulateTestValues_Click(object sender, EventArgs e)
        {//Online Reference http://www.evosnap.com/support/knowledgebase/service-information-data-elements/#bankcardmerchantdata
            MessageBox.Show(
                "Please note that the following values are generic. Depending on the scope of your integration the following values may change. Please contact your solution consultant with any questions.");

            MerchantProfile MP = _dg.CreateMerchantProfile();

            //The following are typical settings please ask your solution consultant if you have any questions
            txtStreetAddress1.Text = MP.MerchantData.Address.Street1;
            txtStreetAddress2.Text = MP.MerchantData.Address.Street2;
            txtCity.Text = MP.MerchantData.Address.City;
            txtStateProvince.Text = MP.MerchantData.Address.StateProvince.ToString();
            txtPostalCode.Text = MP.MerchantData.Address.PostalCode;
            txtCustomerServicePhone.Text = MP.MerchantData.CustomerServicePhone; // Must be of format “NNN NNNNNNN”
            txtMerchantId.Text = MP.MerchantData.MerchantId;
            txtName.Text = MP.MerchantData.Name;
            txtPhone.Text = MP.MerchantData.Phone; // Must be of format “NNN NNNNNNN”
            txtTaxId.Text = MP.MerchantData.TaxId;
            txtCustomerServiceInternet.Text = MP.MerchantData.CustomerServiceInternet;

            //BankCard
            txtClientNum.Text = MP.MerchantData.BankcardMerchantData.ClientNumber;
            txtSIC.Text = MP.MerchantData.BankcardMerchantData.SIC;
            txtStoreId.Text = MP.MerchantData.BankcardMerchantData.StoreId;
            txtSocketNum.Text = MP.MerchantData.BankcardMerchantData.TerminalId;
            
            //The following fields are used by Other Terminal Capture Providers and are part of the additional items
            if (_strServiceID == "C82ED00001" || _strServiceID == "71C8700001" ||
                    _strServiceID == "88D9300001" || _strServiceID == "B447F00001" ||
                    _strServiceID == "D806000001" || _strServiceID == "E88FD00001")
            {
                txtABANumber.Text = MP.MerchantData.BankcardMerchantData.ABANumber;
                txtAcquirerBIN.Text = MP.MerchantData.BankcardMerchantData.AcquirerBIN;
                txtAgentBank.Text = MP.MerchantData.BankcardMerchantData.AgentBank;
                txtAgentChain.Text = MP.MerchantData.BankcardMerchantData.AgentChain;
                txtLocation.Text = MP.MerchantData.BankcardMerchantData.Location;
                txtSecondaryTerminalId.Text = MP.MerchantData.BankcardMerchantData.SecondaryTerminalId;
                txtSettlementAgent.Text = MP.MerchantData.BankcardMerchantData.SettlementAgent;
                txtSharingGroup.Text = MP.MerchantData.BankcardMerchantData.SharingGroup;
                txtTimeZoneDifferential.Text = MP.MerchantData.BankcardMerchantData.TimeZoneDifferential;
                txtReimbursementAttribute.Text = MP.MerchantData.BankcardMerchantData.ReimbursementAttribute;
                txtSocketNum.Text = MP.MerchantData.BankcardMerchantData.TerminalId;
            }

            try
            {
                try { cboMerchantIndustryType.SelectedItem = MP.MerchantData.BankcardMerchantData.IndustryType; }
                catch { }
                cboLanguage.SelectedItem = MP.MerchantData.Language;
                cboCountryCode.SelectedItem = MP.MerchantData.Address.CountryCode;
                cboCurrencyCode.SelectedItem = MP.TransactionData.BankcardTransactionDataDefaults.CurrencyCode;
                try { cboCustomerPresent.SelectedItem = MP.TransactionData.BankcardTransactionDataDefaults.CustomerPresent; }// [Ecommerce : Ecommerce] [MOTO : MOTO] [Retail/Restaurant : Present]
                catch { }
                try { cboRequestACI.SelectedItem = MP.TransactionData.BankcardTransactionDataDefaults.RequestACI; }//In general default to "IsCPSMeritCapable"
                catch { }
                try { cboEntryMode.SelectedItem = MP.TransactionData.BankcardTransactionDataDefaults.EntryMode; }//[Keyed : TrackDataFromMSR ]
                catch { }
            }
            catch
            {
            }
            if (_bcs != null)
            {
                if (_strServiceID == "C82ED00001" || _strServiceID == "71C8700001" ||
                    _strServiceID == "88D9300001" || _strServiceID == "B447F00001" ||
                    _strServiceID == "D806000001" || _strServiceID == "E88FD00001")
                    showBCPExpandedFields();
                else if (_strServiceID == "168511300C" || _strServiceID == "9999999999")
                    showBCPExpandedFields();
                else
                {
                    showBCPFields();
                }
            }

            if (_ecks != null)
            {
                showECKFields();
            }
            if (_svas != null)
            {
                showSVAFields();
            }
        }

        private void ChkEnableAllMerchantFields_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkEnableAllMerchantFields.Checked)
            {
                showAllFields();
            }
            else
            {
                hideAllFields();
            }
        }

        #region ComboBox Events
        private void cboCountryCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CountryCode = (TypeISOCountryCodeA3)cboCountryCode.SelectedItem;
        }

        private void cboLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            _Language = (TypeISOLanguageCodeA3)cboLanguage.SelectedItem;
        }

        private void cboCurrencyCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CurrencyCode = (TypeISOCurrencyCodeA3)cboCurrencyCode.SelectedItem;
        }

        private void cboCustomerPresent_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CustomerPresent = (CustomerPresent)cboCustomerPresent.SelectedItem;
        }

        private void cboRequestACI_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RequestACI = (RequestACI)cboRequestACI.SelectedItem;
        }

        private void cboMerchantIndustryType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _MerchantIndustryType = (IndustryType)cboMerchantIndustryType.SelectedItem;
        }

        private void cboEntryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            _EntryMode = (EntryMode)cboEntryMode.SelectedItem;
        }

        #endregion ComboBox Events

        private void cmdClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion END Form Events

        private void lnkManageMerchantData_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://docs.evosnap.com/TransactionProcessing/CWS/Developer_Guide/2.0.18/Implementation/PreparingTheAppToTransact/ManagingMerchantProfiles/index.aspx");
        }
   
    }
}
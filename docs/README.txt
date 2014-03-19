/* Copyright (c) 2013 EVO Payments International - All Rights Reserved.
 *
 * This software and documentation is subject to and made
 * available only pursuant to the terms of an executed license
 * agreement, and may be used only in accordance with the terms
 * of said agreement. This software may not, in whole or in part,
 * be copied, photocopied, reproduced, translated, or reduced to
 * any electronic medium or machine-readable form without
 * prior consent, in writing, from EVO Payments International
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

____________________________________________________________________________________________________________________

NOTE: You will need to obtain an IdentityToken from your Solutions Consultant in order to run this project successfully.

**Executable**

Step 1: Edit the SampleCode.exe.config file and enter the IdentityToken given to you by your Solutions Consultant. 

Step 2: The Endpoint Management section of the SampleCode.exe.config file define the endpoints the application will use.
		Be sure these are set to the version of CWS you are connecting to. 
		
Step 3: Launch SampleCode.exe application. Selecting the Populate With My Test Values button will pull the values for the
		IdentityToken and endpoints from the config file and fill in the form. Follow the online documentation and links
		for further information about CWS while using this tool. 

**Source**

Step 1: Edit the app.config file and enter the IdentityToken given to you by your Solutions Consultant. 

Step 2: The Endpoint Management section of the app.config file define the endpoints the application will use.
		Be sure these are set to the version of CWS you are connecting to. 
		
Step 3: The .NET solution is for Visual Studio 2010. If you are using a more recent version, you'll need to run the 
		wizard to upgrade the solution. 
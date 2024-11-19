<!-- default badges list -->
![](https://img.shields.io/endpoint?url=https://codecentral.devexpress.com/api/v1/VersionRange/217485549/23.2.2%2B)
[![](https://img.shields.io/badge/Open_in_DevExpress_Support_Center-FF7200?style=flat-square&logo=DevExpress&logoColor=white)](https://supportcenter.devexpress.com/ticket/details/T827714)
[![](https://img.shields.io/badge/📖_How_to_use_DevExpress_Examples-e9f6fc?style=flat-square)](https://docs.devexpress.com/GeneralInformation/403183)
[![](https://img.shields.io/badge/💬_Leave_Feedback-feecdd?style=flat-square)](#does-this-example-address-your-development-requirementsobjectives)
<!-- default badges end -->

# Dashboard for ASP.NET Core - How to create new JSON data sources at runtime

You can provide end users with the capability to create a new data connection for a [JSON Data Source](https://docs.devexpress.com/Dashboard/DevExpress.DashboardCommon.DashboardJsonDataSource) at runtime.

## Example Overview

### Server

On the server side, implement the [IDataSourceWizardConnectionStringsStorage](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.IDataSourceWizardConnectionStringsStorage) interface to provide an end user with the capability to save the created JSON data connections. Use the created class instance as the [ASPxDashboard.SetConnectionStringsProvider](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.ASPxDashboard.SetConnectionStringsProvider(DevExpress.DashboardWeb.IDataSourceWizardConnectionStringsStorage)) / [DashboardConfigurator.SetConnectionStringsProvider](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.DashboardConfigurator.SetConnectionStringsProvider(DevExpress.DashboardWeb.IDataSourceWizardConnectionStringsStorage)) method's parameter.

### Client

To enable the capability for end users, set the **allowCreateNewJsonConnection** property to **true**:

Platform | API
-----|------
 HTML JavaScript | [DataSourceWizardExtensionOptions.allowCreateNewJsonConnection](https://docs.devexpress.com/Dashboard/js-DevExpress.Dashboard.Designer.DataSourceWizardExtensionOptions#js_DevExpress_Dashboard_Designer_DataSourceWizardExtensionOptions_allowCreateNewJsonConnection)
 ASP.NET Core | [DataSourceWizardOptionBuilder.AllowCreateNewJsonConnection(Boolean)](https://docs.devexpress.com/Dashboard/DevExpress.DashboardAspNetCore.DataSourceWizardOptionBuilder.AllowCreateNewJsonConnection(System.Boolean))
 MVC | [DashboardExtensionSettings.AllowCreateNewJsonConnection](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.Mvc.DashboardExtensionSettings.AllowCreateNewJsonConnection) 
 Web Forms | [ASPxDashboard.AllowCreateNewJsonConnection](https://docs.devexpress.com/Dashboard/DevExpress.DashboardWeb.ASPxDashboard.AllowCreateNewJsonConnection) 

After that, the "Choose Connection (JSON)" page allows end users to create new data connections:

![Can create new data connections](img.png)

You can use our test JSON files to try this approach: [DataSources](https://github.com/DevExpress-Examples/DataSources/tree/master/JSON).

## Files to Review

* [Program.cs](./CS/SaveJsonConnectionTutorial/Program.cs)
* [DashboardUtils.cs](./CS/SaveJsonConnectionTutorial/Code/DashboardUtils.cs)
* [Index.cshtml](./CS/SaveJsonConnectionTutorial/Pages/Index.cshtml)

## Documentation

- [Dashboard Data Source Wizard](https://docs.devexpress.com/Dashboard/117680/)
- [Customize the Dashboard Data Source Wizard](https://docs.devexpress.com/Dashboard/401330/)
<!-- feedback -->
## Does this example address your development requirements/objectives?

[<img src="https://www.devexpress.com/support/examples/i/yes-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-dashboard-create-json-connections&~~~was_helpful=yes) [<img src="https://www.devexpress.com/support/examples/i/no-button.svg"/>](https://www.devexpress.com/support/examples/survey.xml?utm_source=github&utm_campaign=asp-net-core-dashboard-create-json-connections&~~~was_helpful=no)

(you will be redirected to DevExpress.com to submit your response)
<!-- feedback end -->

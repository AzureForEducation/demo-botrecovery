# Demo - Proactively Engaging Students

## What is this is about?

This demo does simulate a "course evaluation process" and its possible engagement triggered automatically by the system based on the responses received from the student's side, once they're the ones in charge for answering that.

This repository groups a multi-faceted demo which was designed to show to EDU's customers one (among various others) feasible way to proactively engage both K-12 and HigherEd institutions with its students using Microsoft Cloud and enclosed technologies for that.

This demo is a connected puzzle, meaning that we are using different technologies available on Microsoft Cloud (Azure and Office 365) to build it. Ideally, it would be best if you presented all the pieces together. However, you should be able to perform parts of it separately as well.

## Why is this demo so cool?

This demo shows an automated way to engage with students using technologies that differentiate Microsoft's cloud and services from its competitors: Cognitive Services, Azure Functions, Office 365 Forms, Office 365 Flow, SQL Server PaaS among others.

By doing this, you would be able to demonstrate how O365 and Azure can be used together to complement themselves in a real scenario. Besides that, this demo presents different aspects and technologies separately, giving an idea about what is possible by using each service individually.

## When you should present this out?

This demo is ideal if you're targeting to present something real on the following scenarios:

* How to engage students using AI and cloud services.
* How to integrate O365 and Azure services.
* How to use O365 Flow to automatize both existing and new routines.
* How to use Azure Functions integrated with Logic Apps.
* How to use Sentiment Analysis API to drive system behavior.
* How to combine O365, Azure Services, and Bot in a single ecosystem.

## How does it works?

<img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/recoverybot-arch.png" width="800" />

* School/University to create a new form to request student's feedback regarding some specific course/training recently by them.
* Students to answer that form once providing its input on that regard.
* Answers are automatically sent to an existing SQL Server database (in this demo this database is running on Azure, but it would be an on-prem environment as well).
* Each new response is automatically pulled out from the database and is analyzed by a Function which implements an integration with Text Analytics API to detect sentiment on that responses.
* If a lousy feeling is detected, a mail is dropped to that student asking few more details on that regard in exchange for some benefit.
* By accepting on being collaborative and clicking in a specific link, the student is directed to a University's web page where a Bot is waiting to collect that details and feed up the database with more information and suggestions.

## Internal view

Overall the demo is comprised of five key and distinct parts:

1) **Office 365 Forms Service**: Forms is undoubtedly one of the most useful services available on the portfolio of O365. It is because you can easily create high-end and responsible forms with no development skills. In this demo, we're using Forms to create the assessment form. The one whereby students will send their thoughts to our fictitious Americas University. The image below shows the form already up and running.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form.PNG" width="800" />

2) **Office 365 Flow Service**: Flow is an integration service available on Office 365. By using it, you will be able to create behaviors based in events happening along with some running process under your O365 tenant or event outside of it. In this demo, we're using Flow to direct every single response lending from that assessment form to an existing SQL Database on Azure.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/flow.PNG" width="800" />

3) **Azure SQL Database**: SQL Databases is a high-level and high-scalable service offered by Azure to host SQL Server databases on PaaS model. For the context of this demo, we're going to use an existent SQL database as a data repository for the answers sent by Americas University's students. 

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/sqldatabase.PNG" width="800" />

4) **Azure Logic Apps**: Azure Logic Apps is a cloud service that helps you automate and orchestrate tasks, business processes, and workflows when you need to integrate apps, data, systems, and services across enterprises or organizations. On the context of this demo, we are using one logic app to "hear" a specific table in a target database. As soon as a new registry lands on that table, the logic app calls an Azure Function to do something for us.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/logicapp.PNG" width="800">

5) **Azure Functions**: Azure Functions is a serverless compute service that enables you to run code-on-demand without having to explicitly provision or manage infrastructure. Use Azure Functions to run a script or piece of code in response to a variety of events. For the context of this demo, we have a function which gets the newest answer sent by the student (actually, Logic App is in charge for that HTTP call), analyses that answer's items and, depending on the result of that analysis, either drop off a mail to that student inviting him to collaborate with Americas University giving a bit more of details or do nothing.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/function.PNG" width="800" />

## How to build it?

There are a couple of steps you need to follow up to get this demo up and running in your Azure subscription. Let's go for it.

### Pre-requisites

To get it done, you will need to have available in your local machine:

* Azure Subscription with enough credit.
* Office 365 account.
* Azure SDK for .NET ([donwload](https://www.microsoft.com/en-us/download/details.aspx?id=54917)).
* Either Visual Studio 2017 ([download](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio/?sku=Community&rel=15#)) or Visual Studio Code ([download](https://code.visualstudio.com/download)).
* .NET Core 2.1+ ([download](https://www.microsoft.com/net/learn/dotnet/hello-world-tutorial)).
* Bot Builder SDK v4 template for C# ([download](https://botbuilder.myget.org/feed/aitemplates/package/vsix/BotBuilderV4.fbe0fc50-a6f1-4500-82a2-189314b7bea2)).
* Bot Framework Emulator ([download]

### Creating a new SQL database on Azure

Let's start creating the data repository for our demo, I mean, the SQL Database. We're going to use Azure SQL Databases for that. This process is already well described by Azure documentation, so we're just referencing it [here](https://docs.microsoft.com/en-us/azure/sql-database/sql-database-design-first-database#create-a-blank-sql-database) and assuming that you've followed all those steps carefully to get it done. If everything went well, you should be seeing a new Azure SQL Database through the Azure portal, as shown by the image below.

<img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/sqldatabase.PNG" width="800" />

### Adding a new table into the database

Next step consists on the creation of a new table into the database we just built. There are several ways to accomplish that, however, for the context of this demo, I will use the "Query Editor" feature, provided by the Azure portal.

All you need to do to surpass this step is:

* Once in the blade of your Azure database on Azure portal, click on "Query editor (preview)".
* Add user and password to get yourself authenticated on the database's context.
* On the tool context, click at "+ New query". It is going to create a new tab with a code editor under the button you just clicked at.
* Copy and paste the code below on that area and click at "Run". It is going to create a new table called "AnnualCourseEvaluation" into your database.

    ```sql
    CREATE TABLE [dbo].[AnnualCourseEvaluations](
    [Id] [bigint] IDENTITY(1,1) NOT NULL,
    [SubmissionDate] [datetime] NOT NULL,
    [Name] [varchar](100) NOT NULL,
    [Email] [varchar](100) NOT NULL,
    [AnswerQuestion1] [text] NOT NULL,
    [AnswerQuestion2] [text] NOT NULL,
    [AnswerQuestion3] [text] NOT NULL,
    [AnswerQuestion4] [text] NOT NULL,
    [AnswerQuestion5] [text] NOT NULL,
    [AnswerQuestion6] [text] NOT NULL,
    [AnswerQuestion7] [text] NOT NULL,
    [AnswerQuestion8] [text] NOT NULL,
    [AnswerQuestion9] [text] NOT NULL,
    [AnswerQuestion10] [text] NOT NULL)
    ```
That's all.

### Creating a new Form

Time to create our assessment form. We'll do that by navigating to O365 portal and selecting the Forms service. Once in there, click to create a new form. Then, add the following configuration:

* **Title**: Course's Evaluation - Americas University
* **Description**: Dear student, this is the annual "Course's Evaluation" initiative promoted by Americas University. We have only one goal with that: do offer the under-graduation courses. Because of this, we ask you to answer this 10 quick questions. It will be extremely valuable for us and will take only 5 min of you time. Thanks!
* **Qustions**: Add 12 questions respecting the configuration below.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question1.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question2.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question3.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question4.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question5.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question6.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question7.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question8.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question9.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question10.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question11.PNG" width="500" />

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-question12.PNG" width="500" />

Finally, add the following configuration to the Form itself.

<img src="" width="500" />
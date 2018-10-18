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

Time to create our assessment form. We'll do that by navigating to the O365 portal and selecting the Forms service. Once in there, click to create a new form. Then, add the following configuration:

* **Title**: Course's Evaluation - Americas University

* **Description**: Dear student, this is the annual "Course's Evaluation" initiative promoted by Americas University. We have only one goal with that: do offer the under-graduation courses. Because of this, we ask you to answer this ten quick questions. It will be precious for us and will take only 5 min of your time. Thanks!

* **Questions**: Add 12 questions respecting the configuration below.

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

Finally, apply out the following configuration to the Form itself.

<img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form-config.PNG" width="250" />

Done. Our assessment form is up and running. Now, we need to create some mechanism to direct answers sent through that form to our existing database. For that, we're going to create a new Office 365 Flow.

### Creating the Integration Flow

Time to create our integration Flow. Remember: it will play as a "responses replicator", once it creates a new copy of each answer into the existing database. We'll do that by navigating to the O365 portal and selecting the Flow service. Once in there, click at "+ New" > "Create from Blank". It will throw out a new blank space for you to create a new Flow. 

Then, add the following configuration:

* **Title:**: Receives an assessment and ad it into a SQL database

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/flow-detailed.PNG" width="800" />

There we have four different elements:

1) A Form action triggered "When a new response is submitted". Just select the form you just created.
2) Then, add an "Apply to each" action. It is because we need to iterate over each response arriving from the form and create a single record at the target database. The output will be the "List of responses".
3) Then, for each response, we need to get the details to create the record at the database. That is why we're collecting the "response details". Just select the Form and, the list of columns in there.
4) Finally, send that "payload" you've created and sent it to a specific table into the database. To do that, we'll be using the "Insert row" action, which belongs to SQL Server flow integration. 

    To get access to your database and table, click at ". . ." button on top of the action and select the option "Add new connection". By doing this, will see a screen like that one presented by the image below. Just add the connection information (available at Azure Portal) to each specific configuration field and then, click on save.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/flow-db-setting.PNG" width="400" />

    Now you should see the connection you just created. Select it to get access the table to where you would like to send the students' answers (in our case, "AnnualCourseEvaluation").

    Next, you'll need to match each Form answer with each table column. Go ahead and do that. On end, you should see something like that image below.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/flow-db-binding.PNG" width="400" />

    Done. Now our database should receive the answers sent by the students. Next step? Send Grid account.

### Creating a Send Grid account

Send Grid is a high-scalable service to SMTP messages built by Send Grid on top of Azure. We need to have a Send Grid account set up, once we're going to send SMTP messages to our students through it.

To see how to create a Send Grid account and get User, Password and Keys (we will need both that info later on), please, follow the [steps described in this tutorial](https://docs.microsoft.com/en-us/azure/sendgrid-dotnet-how-to-send-email#create-a-sendgrid-account).

### Creating an Azure Function to evaluate responses and send emails

The next consists of the automatic evaluation of the responses arriving in our database. The same way, there are several different ways to accomplish that, however, to show the toolset offered by Azure with O365, we're going to create an Azure Function to do that evaluation.

Two different implementations have been made:

* **Static (hard-coded)**: we iterate over the answers sent by the students and manually do a verification about the sentiment present in there.

* **Text Analytics API (Cognitive Services)**: for each response received, we call the text analytics API (part of Azure Cognitive Services) to identify sentiments automatically.

For both cases, if a lousy sentiment is identified, our function is going to send a mail to that student asking for more details. If there's nothing wrong in there, nothing happens.

We called our function "SendingMail". Its code is fully available [here](https://github.com/AzureForEducation/demo-botrecovery/tree/master/SendingEmail). To publish the function, you should following the steps below.

1) To get that up and running on Azure, the first thing to do is either download or clone this repo to gain access to the function's code, once we will need to a quick adjustment on top of it. You can do that either clicking at that green "Clone or download" button on top of this page or downloading the zip file in which this repo files are packaged off.

2) Once you have it in your machine, open up the file "SendingEmail.sln", placed at the root of the downloaded directory. By doing this, Visual Studio will open up and load up function's files on the right side of the IDE (Solution Explorer).

3) Now we need to update a small portion of code with Send Grid information; otherwise, the function will not be able to send the messages back. To get there, double-click at "SendingEmail.cs". On the code being shown, look for the portion of code presented below.

    ```csharp
    string smtpHost = "{smtp endpoint here}"; // your smtp host
    string smtpUser = "{smtp user here}"; // your smtp user
    string smtpPass = "{smtp password here}"; // your smtp password
    ```

    Where you see the placeholders, you must add real information from the Send Grid account created earlier in this tutorial. Host, User, and Password are required for the method. Once you have appropriately updated the file,  save (Ctrl + S) and close it.

4) Now, the only thing we need to do is publish that function out into Azure. We could do that in so many different ways; however, we're going to use Visual Studio itself to get there. Please, follow [this link](https://blogs.msdn.microsoft.com/benjaminperkins/2018/04/05/deploy-an-azure-function-created-from-visual-studio/) to see an excellent tutorial explaining how to publish your function.

Done. This is all with Functions. Next step? Logic App.

### Automatizing Function calls with Logic App

The very last thing we need to do to complete our demo flow is create a Logic App. It will act as listener/caller for our database/function. As soon as a  new answer lands into our database, that Logic App will going to call our function passing the new answer to it through a HTTP request. The the function will do what needs to be done.

To get there:

1) To create a new Logic App, please, follow up the steps specificly related to it described on [this tutorial](https://docs.microsoft.com/en-us/azure/azure-functions/functions-twitter-email#create-a-logic-app).

2) Once inside the Logic App just created, click at "Logic App Designer". A blank space will be shown for you to start your integration. First add e new action called "When a new item is created" and add the configuration below.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/logic-app-block1.PNG" width="500" />

    To get access to your "AnnualCourseEvaluations" table, just click at "..." button on top-right corner and select the connection already inserted when we've created our Flow on O365.

3) As next step, add a Azure Function call as 2nd action and select the Function you've published recently. At the "Request body" field add the following information.

    ```json
        { 
            "fromEmail": "no-reply@americasuniversity.com",
            "isImportant": true,
            "mail": "@{triggerBody()?['Email']}",
            "message": "<h2 style=\"color: #2e6c80;\">Hi @{triggerBody()?['Name']},</h2><hr style=\"border: 1px solid #efefef;\" /><p>We're reaching out because we've identified that you seems to be insatisfied with one or more aspects of one of the Americas University's courses and as you know, we are always trying to improve ourselves to offer to our students the best learning conditions.</p><p>This way, we're writing  to ask you for 3 minutes of your time to give us a bit more of context on that regard. Could you please help us out? We're willing to give something in exchange for your valuable help.&nbsp;&nbsp;</p><p>Click the&nbsp;<span style=\"background-color: #2b2301; color: #fff; display: inline-block; padding: 3px 10px; font-weight: bold; border-radius: 3px;\"><a style=\"color: #ffffff;\" href=\"https://{your url}/digitalassistent.html\">Provide more details</a></span>&nbsp;to talk directly with our digital assistent.</p><p><strong>We much appreciate you attention and williness to help!</strong></p><p>Best,<br />Americas University<br />Quality of Courses Department</p><p><strong>&nbsp;</strong></p>",
            "name": "@{triggerBody()?['Name']}",
            "question1": "@{triggerBody()?['AnswerQuestion1']}",
            "question10": "@{triggerBody()?['AnswerQuestion10']}",
            "question2": "@{triggerBody()?['AnswerQuestion2']}",
            "question3": "@{triggerBody()?['AnswerQuestion3']}",
            "question4": "@{triggerBody()?['AnswerQuestion4']}",
            "question5": "@{triggerBody()?['AnswerQuestion5']}",
            "question6": "@{triggerBody()?['AnswerQuestion6']}",
            "question7": "@{triggerBody()?['AnswerQuestion7']}",
            "question8": "@{triggerBody()?['AnswerQuestion8']}",
            "question9": "@{triggerBody()?['AnswerQuestion9']}",
            "subject": "@{triggerBody()?['Name']}, may we have 3 minutes of your time?",
            "toEmail": "@{triggerBody()?['Email']}"
            }
    ```

    On the end, your call to the Azure Function inside the Logic App should looks like the image below.

    <img src="" width="" />
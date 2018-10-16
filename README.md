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

## How it works

* School/University to create a new form to request student's feedback regarding some specific course/training recently by them.
* Students to answer that form once providing its input on that regard.
* Answers are automatically sent to an existing SQL Server database (in this demo this database is running on Azure, but it would be an on-prem environment as well).
* Each new response is automatically pulled out from the database and is analyzed by a Function which implements an integration with Text Analytics API to detect sentiment on that responses.
* If a lousy feeling is detected, a mail is dropped to that student asking few more details on that regard in exchange for some benefit.
* By accepting on being collaborative and clicking in a specific link, the student is directed to a University's web page where a Bot is waiting to collect that details and feed up the database with more information and suggestions.

## Internal view

Overall the demo is comprised of five key and distinct parts:

1) **Office 365 Forms Service**: Forms is undoubtedly one of the most useful services available on the portfolio of O365. It is because you can easily create high-end and responsible forms with no development skills. In this demo, we're using Forms to create the assessment form. The one whereby students will send their thoughts to our fictitious Americas University. The image below shows the form already up and running.

    <img src="https://raw.githubusercontent.com/AzureForEducation/demo-botrecovery/master/Img/form.PNG" width="600" />

2) **Office 365 Flow Service**:

3) **Azure SQL Database**:

4) **Azure Logic Apps**:

5) **Azure Functions**:
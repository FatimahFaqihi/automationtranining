Feature: MalafiHome

Move to a page DocumentsTypes

Background: Login to Internal Portal and click on Malafi Link
	Given Entered 'blue2'as a username
	And Enterd 'Ksmc@1234' as password
	When I cilck on login button
	And Click on Malafi link


Scenario: Click on Document Types link
	When Clicked DocumentsTypes link
	Then I should be navigated to Document Types Page


	
Scenario: Open Page Employees Search
	When Clicked Employees Search link
	Then I should be navigated to Employees Search Page



Scenario: open View Document Approval LVI History
	When Clicked Registered Employees link
	Then I should be navigated to Registered Employees Page




Scenario: Click on Document Group link
	When Clicked Documents Group link
	Then I should be navigated to Documents Group Page



Scenario: Check Executive Dashboard
	Given I click on Dashboard
	Then I shoed see Executive Dashboard



Scenario: Click on MyFiles link
	When Clicked My files link
	Then I should be navigated to My files Page



	
Scenario: Click on inbox link
When Click on inbox link
Then I should be navigated to My inbox Page


	

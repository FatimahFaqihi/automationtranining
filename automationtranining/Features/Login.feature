Feature: Login Feature

Login scenarios for the malfi project 

@loginTest


Scenario: Login using valid username and password should be succeeded


	Given Entered 'blue2'as a username
	And Enterd 'Ksmc@1234' as password
	When I cilck on login button
	Then I should be able to view my home page



Scenario: Login using invalid username or  unvalid password should not be succeeded
	Given Entered 'blue'as a username
	And Enterd 'Ksmc@1234' as password
	When I cilck on login button
	Then I should not be able to view my home page



Scenario Outline: I want to click on link
	Given Click on '<LinkText>' link
	Then I should be navigated to the '<PageURL>'

Examples:
	| LinkText              | PageURL      |
	| Self Services         | selfservices |
	| Forgot your password? | selfservices |


Scenario Outline: the regex code for the alert messge should be matched to the selected language.
	Given I change the language '<language>'
	And Entered 'bule'as a username
	And Enterd 'ksmc@1234' as password
	When I cilck on login button
	Then The received error message regex should be releted to the selected language '<Regex>'

Examples:

	| language | Regex                         |
	| ar       | ^[\\u0621-\\u064A]+           |
	| en       | ^[a-zA-Z0-9$@$!%*?&#^-_. +]+$ |
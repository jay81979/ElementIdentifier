Feature: Element Highlight
	In order to see what element is being identified
	As a tester
	I want to the element to be highlighted

@mytag
Scenario: Test paragraph hover
	Given I enter the url "./html/NoUniqueClasses.html"
	When I click "Selector On"
	And I hover mouse at x "20" and y "34"
	Then the result should be 120 on the screen

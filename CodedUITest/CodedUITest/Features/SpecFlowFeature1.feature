Feature: SpecFlowFeature1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Add two numbers
	Given I have entered 5 into the calculator
	And I have entered 6 into the calculator
	When I press add
	Then the result should be 11 on the screen

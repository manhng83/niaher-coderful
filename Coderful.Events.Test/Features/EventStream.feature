Feature: EventStream

Scenario: Subscribe
	Given an action has been subscribed to an event stream
	When an event is published
	Then the callback must be invoked

Scenario Outline: Publish with argument
	Given an action has been subscribed to an event stream
	When an event is published with argument <"x">
	Then the callback must be invoked with argument <"x">
	Examples: 
	| x     |
	| 1     |
	| asdf2 |

Scenario: Unsubscribe
	Given an action has been subscribed to an event stream
	When action is unsubscribed
	And an event is published
	Then the action must not be invoked

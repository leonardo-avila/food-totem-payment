Feature: Update payment
    As a user
    I want to be able to update the payments
    So that I can approve or cancel the payments

Scenario: Approve payment
    Given I have a payment with status Pending
    When I update the payment with status Paid
    Then the payment status should be Paid

Scenario: Cancel payment
    Given I have a payment with status Pending
    When I update the payment with status Cancelled
    Then the payment status should be Cancelled

Feature: Get payment
    As a user
    I want to be able to get the payments
    So that I can see the payments

Scenario: Get payments
    Given I have a payment
    When I get the payments
    Then I should see the payments

Scenario: Get payments with a internal error
    Given I have a payment
    And there is a internal error
    When I get the payments
    Then I should receive a internal error for payments

Scenario: Get payment by order reference
    Given I have a payment
    When I get the payment by order reference
    Then I should see the payment

Scenario: Get payment by unknown order reference
    Given I have a payment
    When I get the payment by unknown order reference
    Then I should receive a domain error

Scenario: Get single payment with a internal error
    Given I have a payment
    And there is a internal error
    When I get the payment by order reference
    Then I should receive a internal error for payment
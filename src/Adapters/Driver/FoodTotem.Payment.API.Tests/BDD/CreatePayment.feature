Feature: CreatePayment
    As a user
    I want to be able to create a payment
    So that I can complete my order

Scenario: Successful payment creation
    Given I have an order
    When I create a payment for the order
    Then the payment should be created successfully

Scenario: Failed payment creation due to domain exception
    Given I have an payment that will cause a domain exception
    When I create a payment for the order
    Then I should receive a bad request response

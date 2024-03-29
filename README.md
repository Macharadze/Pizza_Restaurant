Pizza Application

This web API project is designed to serve the needs of a pizza restaurant. It provides CRUD functionality for managing both Pizza and User entities. Below are the technical requirements, overall architecture, data structures, validations, and acceptance criteria for this application.
Technical Infrastructure Requirements

    Web API built on .NET 6
    Layered architecture with separate layers for Web/Presentation, Core/Services, and Persistence/DataAccess
    Middleware for culture, global exception handling, and request-response logging
    Swagger documentation with examples

Overall Architecture

    Web/Presentation Layer: Controllers, DTOs, and application setup.
    Core/Service Layer: Main business logic.
    Persistence/DataAccess Layer: In-memory collections of data structures, entities, custom DbContext, repositories.


Validations

    All Ids should be incremental.
    Pizza:
        Name: Should not be empty with length between 3 and 20 characters.
        Price: Mandatory and should be greater than 0.
        Description: Optional with length up to 100 characters.
        Calory count: Mandatory and should be greater than 0.
    User:
        Firstname: Should not be empty with length between 2 and 20 characters.
        Lastname: Should not be empty with length between 2 and 30 characters.
        Email: Should not be empty with regex validation.
        PhoneNumber: Should not be empty with regex validation.
        Address: Optional.
    Address:
        UserId: Should exist in DB and not be marked as deleted.
        City: Should not be empty with length up to 15 characters.
        Country: Should not be empty with length up to 15 characters.
        Region: Optional with length up to 15 characters.
        Description: Optional with length up to 100 characters.
    Order:
        UserId and PizzaId should exist in DB and not be marked as deleted.
        List of Pizzas should not be empty.
    RankHistory:
        UserId and PizzaId should exist in DB and not be marked as deleted.
        Rank should be between 1 and 10.

Acceptance Criteria

    CRUD operations for Pizza, User, and Address entities.
    When deleting User, its addresses should be marked as Deleted (Optional).
    Update and Delete operations are forbidden for Ranks and Orders.
    Actions on deleted items are not permitted.
    Proper responses are returned for non-existent item ids and invalid requests.
    Users can create orders using their addresses and a list of pizzas.
    Users who ordered the pizza can rank it on a 1-10 scale.
    Average ranking of a pizza is displayed; if no evaluation is available, -1 is shown.
    OPTIONAL: Users can upload pictures (restricted extensions and size) and link them to the pizza.

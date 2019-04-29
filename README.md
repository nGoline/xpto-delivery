# xpto-delivery

XPTO Delivery Service

## Delivery Service

Company XPTO has a ecommerce platform built under SOA and currently with 62 services/applications built within. This platform enables the final users to search by products, buy and ship them. However, the shipping process it’s a manual process to find out which route should be used.
This process is painful, and the company wants to have a solution so they can infer the route to be used to ship certain product from point A to B.
Some rules must be accomplished within the solution:

- The route must not perform a direct delivery between the origin and the destination, for example, if a delivery must be made from point A to point B, it must choose the routes that pass through intermediate points, as for example, A - C - B (see figure below)
- Each route have a cost and time associated so it is possible to retrieve the best option depending on the cost our time (in the figure below, it exists multiple options to deliver from A to B, however the route A-C-B has a less time to deliver).

### Rules

Please, develop a service with an REST API (don’t need to implement an UI interface for this) where it will be possible
to:

- Retrieve the routes within a defined origin and destination points;
- Manage points and routes (implement the CRUD);
- Guarantee a minimum amount of tests so you feel confident with any developments in the future;
- Please inject some example data to your solution database.

## Installation

Clone this repository:

```bash
git clone https://github.com/nGoline/xpto-delivery.git
cd xpto-delivery
```

## Run

To run the application just:

```bash
dotnet run Xpto.Application
```

And access ```http://localhost:5000/swagger``` to view the documentation.

You can test the application calls using the swagger page and its examples.

### Flow

You must first authenticate using the ```GET /api/User/Authenticate``` route and the combination:

- User: admin@xpto.com
- Password: 123456

Copy the token string from the response and use it on the pop-up that shows when you click on the ```Authorize``` button on the top left.

Now you can check all the other API routes.

To test it just like the documentation of the test, fetch all the MapPoints from ```GET /api/MapPoints``` and copy the ```Id``` from point ```A``` and ```B``` and paste it on the ```GET /api/KnownRoutes/{point A}/{point B}```.

The result should bring the smaller route ```A -> C -> B```.

Feel free to add new Users, MapPoints and Routes.

## Test

To run the tests:

```bash
dotnet test Xpto.Tests
```

### Test Completion

Not all the tests are writen, just a few of them were made to show my approach on how to test the application.
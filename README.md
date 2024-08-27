# DigitalHub_Task

# Task Management API

This RESTful API allows users to manage tasks with functionalities for creating, reading, updating, and deleting tasks. Additionally, the API includes advanced features for searching, filtering, sorting, and pagination.

## API Endpoints

### Create Task
- **Endpoint:** `POST /api/tasks`
- **Request Body:**
  ```json
  {
    "title": "Complete Documentation",
    "description": "Write and review the project documentation before submission.",
    "status": "NotStarted",
    "dueDate": "2024-09-15T23:59:59"
  }

### Get All Tasks

-   **Endpoint:** `GET /api/tasks`
-   **Query Parameters (Optional):**
    -   `isCompleted` (bool): Filter tasks by completion status.
    -   `dueDate` (DateTime): Filter tasks by due date.
    -   `title` (string): Search tasks by title.
    -   `sortBy` (string): Sort tasks by `CreatedAt` or `DueDate`.
    -   `descending` (bool): Sort in descending order.
    -   `page` (int): Page number for pagination.
    -   `pageSize` (int): Number of items per page.
-   **Description:** Retrieves all tasks with optional filtering, sorting, and pagination.

### Get Task by Id

-   **Endpoint:** `GET /api/tasks/{id}`
-   **Description:** Retrieves a task by its unique identifier.

### Update Task

-   **Endpoint:** `PUT /api/tasks/{id}`
-   **Request Body:**
  ```json
{
    "title": "Complete Documentation",
    "description": "Write and review the project documentation before submission.",
    "status": "NotStarted",
    "dueDate": "2024-09-15T23:59:59"
  }

```

-   **Description:** Updates an existing task identified by its unique identifier. The request body should include the updated `Title`, `Description`, `Status`, and `DueDate`.

### Delete Task

-   **Endpoint:** `DELETE /api/tasks/{id}`
-   **Description:** Deletes a task by its unique identifier.

Validation Rules
----------------

-   **Title**: Required, cannot be empty, and must not exceed 100 characters.
-   **Description**: Optional, up to 500 characters.
-   **DueDate**: Optional, if provided, cannot be set in the past.

Persistence
-----------

-   **Database**: Microsoft SQL Server

Error Handling
--------------

-   **400 Bad Request**: Returned for validation errors.
-   **404 Not Found**: Returned if a task is not found.

Bonus Requirements
------------------

### Search & Filtering

-   **Filter tasks** by `isCompleted` and `dueDate` using query parameters.
-   **Search tasks** by `title`.

### Sorting

-   **Sort tasks** by `CreatedAt` or `DueDate` in ascending or descending order.

### Pagination

-   **Paginate tasks** using query parameters for `page` and `pageSize`.

Testing
-------

The API has been tested using Postman.


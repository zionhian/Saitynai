# SaitynaiSmoke

Steam tipo internetinė žaidimų parduotuvė

Publisher > Game > Review

Rolės: Administratorius - valdo viską
Publisher: Gali užregistruoti naują žaidimą
User: Gali pirkti žaidimus, registruoti reviews
Backend: .net web api
Frontend: Angular
Db: Postgresql

# SaitynaiBackend
## Version: 1

### /api/register

#### POST
##### Summary:

Register a new user

##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | User successfully created |
| 400 | Bad Request - User already exists or validation error |

### /api/login

#### POST
##### Summary:

Login a user

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Login successful |
| 400 | Invalid username or password |

### /api/accessToken

#### POST
##### Summary:

Refresh access token using refresh token

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Token successfully refreshed |
| 422 | Unprocessable Entity - Invalid or missing refresh token |

### /api/publishers/{publisherId}/games

#### GET
##### Summary:

Get all games for a publisher

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | List of games for the publisher |
| 404 | Publisher not found |

#### POST
##### Summary:

Create a game for a publisher

##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Game created successfully |
| 403 | Forbidden - User is not authorized |
| 404 | Publisher not found |

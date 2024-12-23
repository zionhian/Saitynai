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
## Version: 1.0

### /api/register

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 400 | Bad Request - Invalid input |

### /api/login

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 400 | Bad Request - Invalid credentials |

### /api/accessToken

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 401 | Unauthorized - Invalid refresh token |

### /api/publishers/{publisherId}/games

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Not Found - Publisher not found |

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Created - Game created successfully |
| 400 | Bad Request - Invalid input |
| 404 | Not Found - Publisher not found |

### /api/publishers/{publisherId}/games/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Not Found - Game not found |

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | No Content - Game updated successfully |
| 400 | Bad Request - Invalid input |
| 404 | Not Found - Game not found |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | No Content - Game deleted successfully |
| 404 | Not Found - Game not found |
### /api/publishers

#### GET
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 500 | Internal server error |

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Publisher created successfully |
| 400 | Bad request - invalid input |
| 500 | Internal server error |

### /api/publishers/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Publisher not found |
| 500 | Internal server error |

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | Publisher updated successfully |
| 400 | Bad request - invalid input |
| 404 | Publisher not found |
| 500 | Internal server error |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | Publisher deleted successfully |
| 404 | Publisher not found |
| 500 | Internal server error |

### /api/publishers/{publisherId}/games/{gameId}/reviews

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Game or Publisher not found |
| 500 | Internal server error |

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 201 | Review created successfully |
| 400 | Bad request - invalid input |
| 404 | Game or Publisher not found |
| 500 | Internal server error |

### /api/publishers/{publisherId}/games/{gameId}/reviews/{id}

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
| 404 | Review, Game, or Publisher not found |
| 500 | Internal server error |

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | Review updated successfully |
| 400 | Bad request - invalid input |
| 404 | Review, Game, or Publisher not found |
| 500 | Internal server error |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 204 | Review deleted successfully |
| 404 | Review, Game, or Publisher not found |
| 500 | Internal server error |


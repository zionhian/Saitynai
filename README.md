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

### /api/login

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/accessToken

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

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

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

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

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/publishers/{publisherId}/games/{id}/buy

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |
| publisherId | path |  | Yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/publishers

#### GET
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

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

#### PUT
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| id | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

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

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| publisherId | path |  | Yes | integer |
| gameId | path |  | Yes | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

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
| 200 | Success |

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
| 200 | Success |

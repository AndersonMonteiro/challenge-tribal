# Project

> Credit Rating Service (Tribal Challenge)

## Requirements
Make sure you've met the following requirements:
* You have installed:: \
    - `docker`
    - `docker-compose`
    - `make`
* You have a `Linux` or `Mac` environment

## Installing 

Using Docker

```bash
docker-compose up --build
```

Using Makefile

```bash
make run
```

## Usage
Use the following endpoint to determine the credit line:

url: POST "http://localhost:5001/v1/credit-line-requests"

Payload:

```json
{
    "foundingType": "SME",
    "cashBalance": 900,
    "monthlyRevenue": 1000,
    "requestedCreditLine": 200,
    "requestedDate": "2022-02-20T21:32:59.860Z"
}
```

Or try using curl:

```bash
curl -XPOST -H "Content-type: application/json" -d '{
    "foundingType": "SME",
    "cashBalance": 900,
    "monthlyRevenue": 1000,
    "requestedCreditLine": 200,
    "requestedDate": "2022-02-27T21:32:59.860Z"
}' 'http://localhost:5001/v1/credit-line-requests'
```

## Documentation

You can check the document here:
"http://localhost:5001/swagger/index.html"

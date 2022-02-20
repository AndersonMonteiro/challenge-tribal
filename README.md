# Project

> Credit Rating Service (Tribal Challenge)

## Requirements
make sure you've met the following requirements:
* You have installed:: \
    - `docker`
    - `docker-compose`
    - `make`
* You have a `Linux` or `Mac` environment

## Instalando 

Using Docker

```
docker-compose up --build
```

Using Makefile

```
make run
```

## Using the Credit Line API
Use the following endpoint to determine the credit line:

url: POST "http://localhost:5001/v1/credit-line-requests"

Payload:

```
{
    "foundingType": "SME",
    "cashBalance": 900,
    "monthlyRevenue": 1000,
    "requestedCreditLine": 200,
    "requestedDate": "2022-02-20T21:32:59.860Z"
}
```

Or try using curl:

```
curl -XPOST -H "Content-type: application/json" -d '{
    "foundingType": "SME",
    "cashBalance": 900,
    "monthlyRevenue": 1000,
    "requestedCreditLine": 200,
    "requestedDate": "2022-02-20T21:32:59.860Z"
}' 'http://localhost:5001/v1/credit-line-requests'
```

## Documentation

Document is avaliable here:
"http://localhost:5001/swagger/index.html"

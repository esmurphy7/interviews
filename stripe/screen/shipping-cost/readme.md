## Shipping Cost

- https://www.interviewdb.io/question/stripe?page=2&name=shipping-cost

### Part 1

Given an order and shipping cost, calculate the total price. For example:

Order:
```json
{
    "country": "US", // or "CA" for the CA order
    "items": [
        {"product": "mouse", "quantity": 20},
        {"product": "laptop", "quantity": 5}
    ]
}
```

Shipping Cost:
```json
{
    "US": [
        {"product": "mouse", "cost": 550},
        {"product": "laptop", "cost": 1000}
    ],
    "CA": [
        {"product": "mouse", "cost": 750},
        {"product": "laptop", "cost": 1100}
    ]
}
```
```
calculate_shipping_cost(order_us, shipping_cost) == 16000
calculate_shipping_cost(order_ca, shipping_cost) == 20500
```

### Part 2

The price is not fixed, (i.e. decreases with the quantity). For example:

Shipping Cost:
```json
{
    "US": [
        {
            "product": "mouse",
            "costs": [
                {
                    "minQuantity": 0,
                    "maxQuantity": null,
                    "cost": 550
                }
            ]
        },
        {
            "product": "laptop",
            "costs": [
                {
                    "minQuantity": 0,
                    "maxQuantity": 2,
                    "cost": 1000
                },
                {
                    "minQuantity": 3,
                    "maxQuantity": null,
                    "cost": 900
                }
            ]
        }
    ],
    "CA": [
        {
            "product": "mouse",
            "costs": [
                {
                    "minQuantity": 0,
                    "maxQuantity": null,
                    "cost": 750
                }
            ]
        },
        {
            "product": "laptop",
            "costs": [
                {
                    "minQuantity": 0,
                    "maxQuantity": 2,
                    "cost": 1100
                },
                {
                    "minQuantity": 3,
                    "maxQuantity": null,
                    "cost": 1000
                }
            ]
        }
    ]
}
```
```
calculate_shipping_cost(order_us, shipping_cost) == 15700
calculate_shipping_cost(order_ca, shipping_cost) == 20200
```

### Part 3

There are two cost calculation methods: incremental (same as Question 2) and fixed (the total price is the same within a range, regardless of the quantity). For example:

Shipping Cost:
```json
{
    "US": [
        {
            "product": "mouse",
            "costs": [
                {
                    "type": "incremental",
                    "minQuantity": 0,
                    "maxQuantity": null,
                    "cost": 550
                }
            ]
        },
        {
            "product": "laptop",
            "costs": [
                {
                    "type": "fixed",
                    "minQuantity": 0,
                    "maxQuantity": 2,
                    "cost": 1000
                },
                {
                    "type": "incremental",
                    "minQuantity": 3,
                    "maxQuantity": null,
                    "cost": 900
                }
            ]
        }
    ],
    "CA": [
        {
            "product": "mouse",
            "costs": [
                {
                    "type": "incremental",
                    "minQuantity": 0,
                    "maxQuantity": null,
                    "cost": 750
                }
            ]
        },
        {
            "product": "laptop",
      ‍‍‍‍‍‍‌‌‌‌‌‍‌‍‍‌‌‍‍      "costs": [
                {
                    "type": "fixed",
                    "minQuantity": 0,
                    "maxQuantity": 2,
                    "cost": 1100
                },
                {
                    "type": "incremental",
                    "minQuantity": 3,
                    "maxQuantity": null,
                    "cost": 1000
                }
            ]
        }
    ]
}
```
```
calculate_shipping_cost(order_us, shipping_cost) == 14700
calculate_shipping_cost(order_ca, shipping_cost) == 19100
```
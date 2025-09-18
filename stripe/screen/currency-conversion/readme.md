## Currency Conversion
- https://www.interviewdb.io/question/stripe?page=1&name=currency-conversion 

You are given a string that represents currency conversion rates in the format `CURRENCY1:CURRENCY2:RATE`, where each pair provides the exchange rate from `CURRENCY1` to `CURRENCY2`.

Input example: `"AUD:USD:0.75,CAD:USD:0.8"`

Your task is to implement the following features:

### Part 1

Direct Conversion: Given two currency codes (e.g., AUD and USD), if a direct exchange rate between them exists in the input, return that conversion rate.

### Part 2

Single Intermediate Conversion: If a direct conversion doesn't exist, allow for a conversion through a single intermediate currency (e.g., convert CAD to AUD via USD). Return the calculated exchange rate if this conversion is possible.

### Part 3

Best Conversion Rate: When performing conversions with an intermediate currency, return the "best" exchange rate available. You may discuss with the interviewer what "best" means (e.g., highest or lowest conversion rate depending on context).

### Part 4

Multiple Conversions: Without restricting the number of conversions, determine if a conversion is possible between any two currencies.
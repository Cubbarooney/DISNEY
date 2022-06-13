# DISNEY

By: Jacob "Cubby" Rubenstein

Disney Interview Software -- New Employee (YAY!)

Not the best back-ronym I've come up with... but it works, I think!

## Problem 1

Amazon Client Tester

Opens a browser (Firefox), navigates to https://www.amazon.com, and does automated tests at the website.

Implemented Tests:
- Open the website (written as an analog to "Hello, World!")
- Search for a term and verify if expected results appear
    - Uses a config file that contains the URL and the expected results
    - Some of the results in the config file are designed to fail
        - This was done partially because I noticed that the searches are not consistent. (I assume this is because of the cache)
- Input Invalid Password
    - **N.B.**: Sometimes Amazon requires the user to input a captcha after inputing the password. To handle that, the test will pause for a minute to let the user input the captcha. If it isn't inputted in time, it won't work.
- Item to Cart Test
    - Searches for an item, adds it the the cart, and verifies that the item is in the cart

**N.B.**: This project requires that GeckoDriver is installed on the PATH. GeckoDriver can be downloaded here: https://github.com/mozilla/geckodriver/releases

On a completely unrelated note, I just need to share this random comment I found in Amazon's DOM:
```
    _
.__(.)< (MEOW)
 \___)
```

## Problem 2

Bunny Hopper

New method uses math to determine the value for any n. (Runs in linear time, so the run is a LOT faster.)

Basic concept is that, given the set of all answers, the following appears to be true:
answers[n] == answers[n - 1] + answers[n - 2] + answers[n - 3]

Using this, we can quickly calculate from the base case of [4, 2, 1] (the values of n == 3, n == 2, and n == 1)

Ran against the older recursive method and this produced the same outputs at all values up to (and including) n == 25 (the highest value tested. Much higher, and the recursive method takes too long.)

Usage: 
```
python BunnyHopper.py N
```
Where N is the distance the bunny should travel
(Assumes 'python' is on the path)

*Alternatively*, the function bunnyHops(n) can be imported and run seperately. It takes in an integer, much like the script.
Running it this way just returns the result of bunnyHops(n) instead of printing it to the console.
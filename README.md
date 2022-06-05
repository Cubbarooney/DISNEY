# DISNEY
Disney Interview Software (rest of acronym to come later)

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
**N.B.**: This project requires that GeckoDriver is installed on the PATH. GeckoDriver can be downloaded here: https://github.com/mozilla/geckodriver/releases

To-Do: Add Item to Cart test
To-Do (if time permits): Add a Try/Catch arround using the GeckoDriver. Possibly use other browsers, or just straight-up fail the tests requiring GeckoDriver. 

## Problem 2
Bunny Hopper

Uses recursion to determine the paths a hopping bunny could take to go N distance.

I did create an alternate version which uses loops, but it is MUCH slower. (And far less readible.)
I will be removing in a future commit.

Usage: 
```
python BunnyHopper.py N
```
Where N is the distance the bunny should travel
(Assumes 'python' is on the path)
# DISNEY
Disney Interview Software (rest of acronym to come later)

## Problem 1
Amazon Client Tester

Opens a browser (Firefox), navigates to https://www.amazon.com, and does automated tests at the website.

Currently the only test is navigating to the website.
**N.B.**: This project requires that GeckoDriver is installed on the PATH. GeckoDriver can be downloaded here: https://github.com/mozilla/geckodriver/releases

To-Do: All of the prescribed tests
To-Do (if time permits): Add a Try/Catch arround using the GeckoDriver. Possibly use other browsers, or just straight-up fail the tests requiring GeckoDriver. 

## Problem 2
Bunny Hopper

Uses recursion to determine the paths a hopping bunny could take to go N distance.

Usage: 
```
python BunnyHopper.py
```
(Assumes 'python' is on the path)

To-Do: Pass in value instead of just using hardcoded 1 - 6
To-Do: Per email w/ Poojita, the function should return the number of possible paths